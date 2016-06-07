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

    public class AbstractExport : IHttpHandler
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

            try
            {
                //Update Abstract's Status
                foreach (var abstractId in param.Abstracts)
                {
                    Common.ExportAbstract(connString, abstractId, param.userGuid);
                }

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