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
    
    public class AbstractClose : IHttpHandler
    {
        #region Fields

        private string connString = null;
        private string abstracts = "";
        private string type = "";
        private string userguid = "";
        private List<string> abstractIDs;

        #endregion

        public void ProcessRequest(HttpContext context)
        {
            int abstractId = -1;
            connString = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ToString();
            abstracts = context.Request["abstracts"] ?? "";
            type = context.Request["type"] ?? "";
            userguid = context.Request["guid"] ?? "";
            Guid ug;
            if (!Guid.TryParse(userguid, out ug))
            {
                context.Response.Write(JsonConvert.SerializeObject(new { success = false, invalidguid = true }));
                return;
            }
            abstractIDs = abstracts.Split(',').ToList();

            
            switch (type)
            {

                case "close":

                    try
                    {
                        foreach (string abs in abstractIDs)
                        {
                            if (Int32.TryParse(abs, out abstractId))
                            {
                                Common.CloseAbstract(connString, abstractId, ug);
                            }
                            else
                            {
                                throw new Exception("abstractID '" + abs + "' is incorrect");
                            }
                        }

                        context.Response.Write(JsonConvert.SerializeObject(new { success = true }));
                    }
                    catch (Exception ex)
                    {
                        context.Response.Write(JsonConvert.SerializeObject(new { success = false, message = ex.Message }));
                    }

                    break;

                case "open":

                    try
                    {
                        foreach (var abs in abstractIDs)
                        {
                            if (Int32.TryParse(abs, out abstractId))
                            {
                                Common.OpenClosedAbstract(connString, abstractId, ug);
                            }
                            else
                            {
                                throw new Exception("abstractID '" + abs + "' is incorrect");
                            }
                        }
                        context.Response.Write(JsonConvert.SerializeObject(new { success = true }));                        
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