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
        private string type = "";
        private string abstractStatusIDs = "";
        private List<int> abstracts = null;
        private string userguid = "";

        #endregion

        protected void serializeResponse(HttpContext context, List<int> abstracts)
        {
            context.Response.Write(JsonConvert.SerializeObject(abstracts));
            return;
        }

        public void ProcessRequest(HttpContext context)
        {
            connString = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ToString();            
            type = context.Request["type"] ?? "";
            userguid = context.Request["guid"] ?? "";
            Guid ug;
            if (!Guid.TryParse(userguid, out ug))
            {
                context.Response.Write(JsonConvert.SerializeObject(new { success = false, invalidguid = true }));
                return;
            }

            switch (type)
            {
                case "closed":
                    try
                    {
                        abstractStatusIDs += (int)AbstractStatusID._3;
                        abstracts = Common.GetAbstractsNotToReopen(connString, abstractStatusIDs, ug);
                        serializeResponse(context, abstracts);
                    }
                    catch (Exception ex)
                    {
                        context.Response.Write(JsonConvert.SerializeObject(new { success = false, message = ex.Message }));
                    }
                    break;

                case "exported":
                    try
                    {
                        abstractStatusIDs += (int)AbstractStatusID._4;
                        abstracts = Common.GetAbstractsNotToReopen(connString, abstractStatusIDs, ug);
                        serializeResponse(context, abstracts);
                    }
                    catch (Exception ex)
                    {
                        context.Response.Write(JsonConvert.SerializeObject(new { success = false, message = ex.Message }));
                    }
                    break;

                default:
                    context.Response.Write(JsonConvert.SerializeObject(new { success = false, message = "incorrect URL parameters" }));
                    break;
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