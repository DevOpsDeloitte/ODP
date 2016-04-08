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
using ODPTaxonomyUtility_TT;

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
                case "run":
                    //string csvPath = context.Server.MapPath("Book1.csv");
                    //context.Response.Clear();
                    //context.Response.ContentType = "application/csv";
                    //context.Response.AppendHeader("content-disposition",
                    //        "attachment; filename=" + csvPath);
                    //context.Response.TransmitFile(csvPath);
                    //context.Response.End();

                    getReport(context);


                    //context.Response.Write(getReport(context));
                    break;
                case "avgreport":
                    getAvgReport(context);
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
                string start = context.Request["start"] ?? "";
                string end = context.Request["end"] ?? "";
                string ktype = context.Request["ktype"] ?? "";
                List<Report_KappaAvg_ByQCWeeksResult> reportvals = db.Report_KappaAvg_ByQCWeeks(start, end, ktype).ToList();
                List<Report_KappaAvg_DataDetail_ByQCWeeksResult> reportvalsdetail = db.Report_KappaAvg_DataDetail_ByQCWeeks(start, end, ktype).ToList();

                DataSet ds = new DataSet();
                CreateExcelFile.CreateExcelDocumentPrecision<Report_KappaAvg_ByQCWeeksResult>(reportvals, context.Response, "KappaAvg-"+ktype, ds);
                CreateExcelFile.CreateExcelDocumentPrecision<Report_KappaAvg_DataDetail_ByQCWeeksResult>(reportvalsdetail, context.Response, "KappaAvgDetail-" + ktype, ds);
                CreateExcelFile.CreateExcelDocumentAsStream(ds, "KappaAvg-"+start+"-"+end+"-"+ktype+".xlsx", context.Response);
                //return JsonConvert.SerializeObject(reportvals);
            }
        }

        private void getAvgReport(HttpContext context)
        {
            string connString = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString;
            using (ReportingAppDataContext db = new ReportingAppDataContext(connString))
            {

                List<Report_AbstractSummaryResult> reportvals = db.Report_AbstractSummary().ToList();
                DataSet ds = new DataSet();
                CreateExcelFile.CreateExcelDocumentPrecision<Report_AbstractSummaryResult>(reportvals, context.Response, "AbstractSummary" , ds);
                //CreateExcelFile.CreateExcelDocumentPrecision<Report_AbstractSummaryResult>(reportvals, context.Response, "AbstractSummary2", ds);
                string format = "-dd_MM_yyyy_h_mm_ss_tt";
                CreateExcelFile.CreateExcelDocumentAsStream(ds, "AbstractSummary" + DateTime.Now.ToString(format) + ".xlsx", context.Response);
 
            }
        }

        private string getTimePeriods()
        {
            string connString = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString;
            using (ReportingAppDataContext db = new ReportingAppDataContext(connString))
            {
                var qcWeeks = db.Report_QC_Weeks.ToList();
                return JsonConvert.SerializeObject(qcWeeks);
            }
        }

        #endregion
    }
}