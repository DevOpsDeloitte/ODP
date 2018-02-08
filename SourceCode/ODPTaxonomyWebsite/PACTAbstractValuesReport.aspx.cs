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
    public partial class PACTAbstractValuesReport : System.Web.UI.Page
    {
        #region Fields
        private string filenameBase = "PACT Abstract Values-";
        private string filename = "PACT Abstract Values-MM-DD-YYYY.xlsx";
        private string connString = null;
        private string excelFileName = "";
        private string filePath = "";
        DateTime m_year;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {

                // process if valid parameters
                if (isValidParams())
                {
                    string l_year = m_year.ToString("yyyy");
                    string format = "MM-dd-yyyy-h-mm-ss";
                    connString = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ToString();
                    filename = filenameBase + DateTime.Now.ToString(format) + ".xlsx";
                    excelFileName = HttpContext.Current.Request.PhysicalApplicationPath + "Reports\\" + filename;
                    filePath = "/Reports/" + filename;
                    List<Report_SelectionDatapulling_PIVOTResult> rptList = null;
                    string noDataMsg = "No data found for " + l_year.ToString() ;

                        using (ReportDataLinqDataContext db = new ReportDataLinqDataContext(ReportDAL.connString))
                        {
                            rptList = db.Report_SelectionDatapulling_PIVOT(l_year).ToList<Report_SelectionDatapulling_PIVOTResult>();
                        }

                    if (rptList.Count() == 0)
                    {
                        lbl_errmsg.Text = noDataMsg;
                    }
                    else
                    {
                        CreateExcelFile.CreateExcelDocument<Report_SelectionDatapulling_PIVOTResult>(rptList, excelFileName);
                        Response.Redirect(filePath);
                    }

                }

            }
            catch (Exception ex)
            {
                Utils.LogError(ex);
                lbl_errmsg.Text = "Error: " + ex.ToString();
            }

        }

        protected bool isValidParams()
        {
            string l_year;

            DateTime l_outyear;


            if (string.IsNullOrEmpty(Request.QueryString["FY"])) 
            {
                lbl_errmsg.Text = "Missing Parameters: FY.";
                return false;
            }
   
            l_year = Request.QueryString["FY"].ToString();
          
            if (!DateTime.TryParseExact(l_year, "yyyy", null, System.Globalization.DateTimeStyles.None, out l_outyear))
            {
                lbl_errmsg.Text = "FY parameter has wrong format.  Needs to be yyyy.";
                return false;
            }

            m_year = l_outyear.Date;

            return true;
        }
    }
}