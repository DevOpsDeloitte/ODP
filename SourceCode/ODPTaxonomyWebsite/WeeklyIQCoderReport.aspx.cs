using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ODPTaxonomyUtility_TT;
using ODPTaxonomyDAL_TT;
using ODPTaxonomyReportDAL;
using System.Configuration;
using System.Data;
using System.Text;

namespace ODPTaxonomyWebsite
{
    public partial class WeeklyIQCoderReport : System.Web.UI.Page
    {
        #region Fields
        private string filenameBase = "PACT-Coded Abstract List-";
        private string filename = "PACT-Coded Abstract List-MM-DD-YYYY.xlsx";
        private string connString = null;
        private string excelFileName = "";
        private string filePath = "";
        DateTime m_startdate;
        DateTime m_enddate;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            try
            {

                // process if valid parameters
                if (isValidParams())
                {
                    string l_startdate;
                    string l_tabname;
                    bool isData = false;
                    string format = "MM-dd-yyyy-h-mm-ss";
                    connString = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ToString();
                    filename = filenameBase + DateTime.Now.ToString(format) + ".xlsx";
                    excelFileName = HttpContext.Current.Request.PhysicalApplicationPath + "Reports\\" + filename;
                    filePath = "/Reports/" + filename;
                    List<rpt_IQCode_Abstracts_ByDateResult> rptList = null;
                    string noDataMsg = "No data found for " + m_startdate.ToString() + " to " + m_enddate.ToString();

                    // loop through
                    while (m_startdate <= m_enddate)
                    {
                        l_tabname = m_startdate.ToString("ddd MMM dd");
                        l_startdate = m_startdate.ToString();

                        using (ReportDataLinqDataContext db = new ReportDataLinqDataContext(ReportDAL.connString))
                        {
                            rptList = db.rpt_IQCode_Abstracts_ByDate(l_startdate, l_startdate).ToList<rpt_IQCode_Abstracts_ByDateResult>();
                        }

                        if (rptList.Count() != 0)
                        {
                            isData = true;
                            CreateExcelFile.CreateExcelDocument<rpt_IQCode_Abstracts_ByDateResult>(rptList, null, l_tabname, ds);                            
                        }

                        rptList.Clear();
                        m_startdate = m_startdate.AddDays(1);

                    }

                    if (isData)
                    {
                        CreateExcelFile.CreateExcelDocument(ds, excelFileName);
                        Response.Redirect(filePath);
                    }
                    else
                    {
                        lbl_errmsg.Text = noDataMsg;
                    }
                    
                    /*
                    using (ReportDataLinqDataContext db = new ReportDataLinqDataContext(ReportDAL.connString))
                    {
                        rptList = db.rpt_IQCode_Abstracts_ByDate(l_startdate, l_enddate).ToList<rpt_IQCode_Abstracts_ByDateResult>();
                    }

                    // check count
                    if (rptList.Count() == 0)
                    {
                        lbl_errmsg.Text = "No data found for " + l_startdate + " - " + l_enddate + ".";
                    }
                    else
                    {
                        CreateExcelFile.CreateExcelDocument<rpt_IQCode_Abstracts_ByDateResult>(rptList, excelFileName);
                        Response.Redirect(filePath);
                    }
                    */

                }

            }
            catch (Exception ex)
            {
                Utils.LogError(ex);
                lbl_errmsg.Text = "Error: " + ex.ToString();
            }
            finally
            {
                ds.Dispose();
                ds = null;
            }
        }

        protected bool isValidParams()
        {
            string l_start;
            string l_end;

            DateTime l_outstart;
            DateTime l_outend;

            if ((string.IsNullOrEmpty(Request.QueryString["StartDate"])) || (string.IsNullOrEmpty(Request.QueryString["EndDate"])))
            {
                lbl_errmsg.Text = "Missing Parameters: StartDate and/or EndDate.";
                return false;
            }

            l_start = Request.QueryString["StartDate"].ToString();
            l_end = Request.QueryString["EndDate"].ToString();

            if (!DateTime.TryParseExact(l_start, "MMddyyyy", null, System.Globalization.DateTimeStyles.None, out l_outstart)){
                lbl_errmsg.Text = "StartDate parameter has wrong format.  Needs to be mmddyyy.";
                return false;
            }

            if (!DateTime.TryParseExact(l_end, "MMddyyyy", null, System.Globalization.DateTimeStyles.None, out l_outend))
            {
                lbl_errmsg.Text = "EndDate parameter has wrong format.  Needs to be mmddyyy.";
                return false;
            }

            if (l_outstart > l_outend)
            {
                lbl_errmsg.Text = "EndDate value is earlier than StartDate value.";
                return false;
            }

            m_startdate = l_outstart.Date;
            m_enddate = l_outend.Date;


            return true;
        }
    }
}