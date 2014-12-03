using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using ODPTaxonomyUtility_TT;
using ODPTaxonomyDAL_TT;
using System.Configuration;

namespace ODPTaxonomyWebsite.Evaluation.Handlers
{
    
    //this handler returns Abstracts
    //that could NOT be re-opened
    public class AbstractsReopen : IHttpHandler
    {
        #region Fields

        private string connString = null;
        private List<int> abstracts = null;
        private string userguid = "";

        #endregion        

        public void ProcessRequest(HttpContext context)
        {
            connString = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ToString();            
            userguid = context.Request["guid"] ?? "";
            Guid ug;
            if (!Guid.TryParse(userguid, out ug))
            {
                context.Response.Write(JsonConvert.SerializeObject(new { success = false, invalidguid = true }));
                return;
            }

            try
            {
                abstracts = Common.GetAbstractsNotToReopen(connString, ug);                
                context.Response.Write(JsonConvert.SerializeObject(new { nottoreopen = abstracts, success = true }));
            }
            catch (Exception ex)
            {
                context.Response.Write(JsonConvert.SerializeObject(new { success = false, message = ex.Message }));
            }
            
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}