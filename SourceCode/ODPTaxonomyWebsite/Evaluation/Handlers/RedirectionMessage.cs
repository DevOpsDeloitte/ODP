using System;
using System.Web;
using Newtonsoft.Json;
using ODPTaxonomyDAL_TT;
using System.Configuration;

namespace ODPTaxonomyWebsite.Evaluation.Handlers
{
    public class RedirectionMessage : IHttpHandler
    {
        /// <summary>
        /// You will need to configure this handler in the web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpHandler Members

        public bool IsReusable
        {
            // Return false in case your Managed Handler cannot be reused for another request.
            // Usually this would be false in case you have some state information preserved per request.
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            //context.Response.ContentType = "application/json";
            //context.Response.Write(JsonConvert.SerializeObject(new { ShowRedirectionMessage = true }));
            if (context.Request.QueryString["userName"] != null)
            {
                string userName = context.Request.QueryString["userName"].ToString().Trim();
                string connString = null;
                connString = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ToString();
                string abstractId = Common.GetAbstarctIdForTeamMember(connString, userName);
                
                context.Response.ContentType = "text/plain";
                context.Response.Write(abstractId);
            }
            else
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write("N/A");
            }
            
            
        }

        #endregion
    }
}
