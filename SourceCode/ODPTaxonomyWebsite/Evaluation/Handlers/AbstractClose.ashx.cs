using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using ODPTaxonomyUtility_TT;
using ODPTaxonomyDAL_TT;
using System.Configuration;
using ODPTaxonomyDAL_JY;

namespace ODPTaxonomyWebsite.Evaluation.Handlers
{

    public class AbstractClose : IHttpHandler
    {
        #region Fields
        private string connString = null;
        #endregion

        public void ProcessRequest(HttpContext context)
        {
            connString = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ToString();
            AbstractActionParams param = new AbstractActionParams(context);

            if (param.userGuid == null)
            {
                context.Response.Write(JsonConvert.SerializeObject(new { success = false, invalidguid = true }));
                return;
            }

            switch (param.type)
            {
                case "close":
                    try
                    {
                        foreach (var abstractId in param.Abstracts)
                        {
                            Common.CloseAbstract(connString, abstractId, param.userGuid);
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
                        foreach (var abstractId in param.Abstracts)
                        {
                            Common.OpenClosedAbstract(connString, abstractId, param.userGuid);
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