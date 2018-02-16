using System;
using System.Web;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web.UI.WebControls;
using System.Configuration;
using ODPTaxonomyDAL_ST;
using ODPTaxonomyDAL_TT;
using ODPTaxonomyReportDAL;
using ODPTaxonomyUtility_TT;
using System.Globalization;

namespace ODPTaxonomyWebsite.ReportingApp.handlers
{
    /// <summary>
    /// Summary description for ReportService
    /// </summary>
    public class ReportService : IHttpHandler
    {

        public string type = "";

        #region IHttpHandler Members

        public bool IsReusable
        {
            // Return false in case your Managed Handler cannot be reused for another request.
            // Usually this would be false in case you have some state information preserved per request.
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            type = context.Request["type"] ?? "";
            switch (type)
            {
                case "dates":
                    context.Response.Write(getTimePeriods());
                    break;
                case "mechanismtypes":
                    context.Response.Write(getMechanismTypes());
                    break;
                case "run":
                    getReport(context);


                    //context.Response.Write(getReport(context));
                    break;
                case "avgreport":
                    getAvgReport(context);
                    break;

                case "valuesreport":
                    getAbsValuesReport(context);
                    break;

                default:
                    context.Response.Write("NA");
                    break;
            }


        }

        private void getReport(HttpContext context)
        {
            string connString = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString;
            using (ReportingAppDataContext db = new ReportingAppDataContext(connString))
            {
                db.CommandTimeout = 0;
                string start = context.Request["start"] ?? "";
                string end = context.Request["end"] ?? "";
                string ktype = context.Request["ktype"] ?? "";
                string mechanisms = context.Request["mechanisms"] ?? "";
                string[] allmechanisms = mechanisms.Split(',');
                List<int> selectedMechanisms = new List<int>();
                DataSet ds = new DataSet();
                int mechCount = allmechanisms.Length;
                List<List<Report_KappaAvg_ByQCWeeks_NewResult>> reports = new List<List<Report_KappaAvg_ByQCWeeks_NewResult>>();
                foreach (string mechanism in allmechanisms)
                {
                    var m = mechanism.Split('-');
                    int? mechanism_id = Convert.ToInt32(m[0]);
                    var mechanism_name = m[1];
                    selectedMechanisms.Add(mechanism_id ?? 0);
                    List<Report_KappaAvg_ByQCWeeks_NewResult> reportvals = db.Report_KappaAvg_ByQCWeeks_New(start, end, ktype, mechanism_id).ToList();
                    CreateExcelFile.CreateExcelDocumentPrecision<Report_KappaAvg_ByQCWeeks_NewResult>(reportvals, context.Response, mechanism_name + "-" + ktype, ds);


                }

                List<Report_KappaAvg_DataDetail_ByQCWeeks_NewResult> reportvalsdetail = db.Report_KappaAvg_DataDetail_ByQCWeeks_New(start, end, ktype, String.Join(",", selectedMechanisms.ToArray())).ToList();
                CreateExcelFile.CreateExcelDocumentPrecision<Report_KappaAvg_DataDetail_ByQCWeeks_NewResult>(reportvalsdetail, context.Response, "KappaAvgDetail-" + ktype, ds);
                CreateExcelFile.CreateExcelDocumentAsStreamSpecialHeaders(ds, "KappaAvg-" + start + "-" + end + "-" + ktype + ".xlsx", context.Response, start, end);

                //List<Report_KappaAvg_ByQCWeeksResult> reportvals = db.Report_KappaAvg_ByQCWeeks(start, end, ktype).ToList();
                //CreateExcelFile.CreateExcelDocumentPrecision<Report_KappaAvg_ByQCWeeksResult>(reportvals, context.Response, "KappaAvg-"+ktype, ds);
                //CreateExcelFile.CreateExcelDocumentPrecision<Report_KappaAvg_ByQCWeeksResult>(reportvals, context.Response, "KappaAvg-2" + ktype, ds);
                //CreateExcelFile.CreateExcelDocumentPrecision<Report_KappaAvg_ByQCWeeksResult>(reportvals, context.Response, "KappaAvg-3" + ktype, ds);
                //CreateExcelFile.CreateExcelDocumentPrecision<Report_KappaAvg_DataDetail_ByQCWeeksResult>(reportvalsdetail, context.Response, "KappaAvgDetail-" + ktype, ds);
                //CreateExcelFile.CreateExcelDocumentAsStreamSpecialHeaders(ds, "KappaAvg-"+start+"-"+end+"-"+ktype+".xlsx", context.Response, start, end);
                //return JsonConvert.SerializeObject(reportvals);
            }
        }

        private void getAvgReport(HttpContext context)
        {
            string connString = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString;
            using (ReportingAppDataContext db = new ReportingAppDataContext(connString))
            {

                List<Report_AbstractSummaryResult> reportvals = db.Report_AbstractSummary().ToList();
                List<Report_AbstractSummary_MergeResult> reportvalsAB = db.Report_AbstractSummary_Merge().ToList();
                DataSet ds = new DataSet();
                CreateExcelFile.CreateExcelDocumentPrecision<Report_AbstractSummary_MergeResult>(reportvalsAB, context.Response, "Abstract Summary", ds);
                CreateExcelFile.CreateExcelDocumentPrecision<Report_AbstractSummaryResult>(reportvals, context.Response, "Abstract Summary Details", ds);
                string format = "-dd_MM_yyyy_h_mm_ss_tt";
                CreateExcelFile.CreateExcelDocumentAsStream(ds, "AbstractSummary" + DateTime.Now.ToString(format) + ".xlsx", context.Response);

            }
        }


        private string getMechanismTypes()
        {
            string connString = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString;
            using (ReportingAppDataContext db = new ReportingAppDataContext(connString))
            {
                var mechanismTypes = db.Mechanism_Types.Where(q => q.StatusID == 1).OrderBy(q => q.Sorting).ToList();
                return JsonConvert.SerializeObject(mechanismTypes);
            }
        }

        private string getTimePeriods()
        {
            string connString = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString;
            using (ReportingAppDataContext db = new ReportingAppDataContext(connString))
            {
                var qcWeeks = db.Report_QC_Weeks.ToList();

                qcWeeks.RemoveRange(0, 9);
                CultureInfo cul = CultureInfo.CurrentCulture;
                int weekNum = cul.Calendar.GetWeekOfYear(
                        DateTime.Now,
                        CalendarWeekRule.FirstDay,
                        DayOfWeek.Monday);
                var cyear = DateTime.Now.Year.ToString();
                var cweek = weekNum.ToString();
                var maxid = qcWeeks.Max(x => x.QC_ID);
                try
                {
                    //var getcurrentIdx = qcWeeks.FindIndex(q => q.QC_week == cyear + '-' + cweek);
                    var getcurrentId = qcWeeks.Where(q => q.QC_week == cyear + '-' + cweek).Select(q => q.QC_ID).FirstOrDefault();
                    if (getcurrentId.Value > 0)
                    {
                        //qcWeeks.RemoveRange(getcurrentIdx, qcWeeks.Count - getcurrentIdx);
                        // or
                        qcWeeks = qcWeeks.Where(qc => qc.QC_ID.Value < getcurrentId).OrderByDescending(qc => qc.QC_ID).ToList();
                    }
                }
                catch
                {
                    // do nothing, keep the list.
                }

                return JsonConvert.SerializeObject(qcWeeks);
            }
        }

        private void getAbsValuesReport(HttpContext context)
        {
            string connString = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString;
            using (ReportDataLinqDataContext db = new ReportDataLinqDataContext(connString))
            {
                db.CommandTimeout = 0;
                string l_year = context.Request["fy"] ?? "";
                List<Report_SelectionDatapulling_PIVOTResult> reportvals = db.Report_SelectionDatapulling_PIVOT(l_year).ToList<Report_SelectionDatapulling_PIVOTResult>();
                DataSet ds = new DataSet();
                CreateExcelFile.CreateExcelDocumentPrecision<Report_SelectionDatapulling_PIVOTResult>(reportvals, context.Response, "Abstract Values", ds);
                // CreateExcelFile.CreateExcelDocumentPrecision<Report_AbstractSummaryResult>(reportvals, context.Response, "Abstract Summary Details", ds);
                string format = "-dd_MM_yyyy_h_mm_ss_tt";
                CreateExcelFile.CreateExcelDocumentAsStream(ds, "PACT Abstract Values" + DateTime.Now.ToString(format) + ".xlsx", context.Response);
            }
        }

        #endregion
    }
}