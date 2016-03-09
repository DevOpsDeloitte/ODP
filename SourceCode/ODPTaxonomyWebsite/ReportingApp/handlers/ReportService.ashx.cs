using System;
using System.Web;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web.UI.WebControls;
using System.Configuration;
using ODPTaxonomyDAL_ST;

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
            context.Response.Write(getTimePeriods());

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