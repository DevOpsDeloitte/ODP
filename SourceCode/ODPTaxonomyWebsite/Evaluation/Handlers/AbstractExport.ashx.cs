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
    
    public class AbstractExport : IHttpHandler
    {
        #region Fields

        private string connString = null;
        private string abstracts = "";
        private string userguid = "";
        private List<string> abstractIDs;

        #endregion

        public void ProcessRequest(HttpContext context)
        {
            int abstractId = -1;
            connString = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ToString();
            abstracts = context.Request["abstracts"] ?? "";
            userguid = context.Request["guid"] ?? "";
            Guid ug;
            if (!Guid.TryParse(userguid, out ug))
            {
                context.Response.Write(JsonConvert.SerializeObject(new { success = false, invalidguid = true }));
                return;
            }
            abstractIDs = abstracts.Split(',').ToList();

            try
            {
                //Update Abstract's Status
                foreach (string abs in abstractIDs)
                {
                    if (Int32.TryParse(abs, out abstractId))
                    {
                        Common.ExportAbstract(connString, abstractId, ug);
                    }
                    else
                    {
                        throw new Exception("abstractID '" + abs  + "' is incorrect");                        
                    }
                }
                //Generate Excel Reports

                //Generate message
                context.Response.Write(JsonConvert.SerializeObject(new { success = true }));
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