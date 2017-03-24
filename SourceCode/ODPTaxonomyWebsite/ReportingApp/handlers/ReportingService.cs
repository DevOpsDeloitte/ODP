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
    public class ReportingService : IHttpHandler
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
            //write your handler implementation here.
            switch (type)
            {
                case "dates" :
                    context.Response.Write(getTimePeriods());
                    break;
                case "mechanismtypes" :
                    context.Response.Write(getMechanismTypes());
                    break;
                default:
                    //context.Response.Write(getTimePeriods());
                    break;
            }
            

        }

        private string getTimePeriods()
        {
            string connString = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString;        
            using (ReportingAppDataContext db = new ReportingAppDataContext(connString))
            {
                var  qcWeeks = db.Report_QC_Weeks.ToList();
                return JsonConvert.SerializeObject(qcWeeks);
            }
        }

        private string getMechanismTypes()
        {
            string connString = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString;
            using (ReportingAppDataContext db = new ReportingAppDataContext(connString))
            {
                var mechanismTypes = db.Report_Mechanism_Types.Where(q => q.StatusID == 1).OrderBy(q => q.Sorting).ToList();
                return JsonConvert.SerializeObject(mechanismTypes);
            }
        }

        #endregion
    }
}
