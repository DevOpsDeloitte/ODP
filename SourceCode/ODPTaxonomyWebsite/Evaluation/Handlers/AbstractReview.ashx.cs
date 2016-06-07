using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using ODPTaxonomyDAL_JY;

namespace ODPTaxonomyWebsite.Evaluation.Handlers
{
    /// <summary>
    /// Summary description for AbstractReview
    /// </summary>
    public class AbstractReview : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            AbstractActionParams param = new AbstractActionParams(context);

            if (param.userGuid==null)
            {
                context.Response.Write(JsonConvert.SerializeObject(new { success = false, invalidguid = true }));
                return;
            }

            if(!param.all && param.includeList.Count == 0)
            {
                context.Response.Write(JsonConvert.SerializeObject(new { success = false, error = "No abstract selected" }));
            }

            AbstractListViewData data = new AbstractListViewData();

            switch (param.type)
            {
                case "add":
                    try
                    {
                        foreach (var abs in param.Abstracts)
                        {
                            if (!data.IsAbstractInReview(abs))
                            {
                                data.AddAbstractToReview(abs, param.userGuid);
                            }
                        }

                        context.Response.Write(JsonConvert.SerializeObject(new { success = true }));
                    }
                    catch (Exception ex)
                    {
                        context.Response.Write(JsonConvert.SerializeObject(new { success = false, message = ex.Message }));
                    }

                    break;

                case "remove":
                    foreach (var abs in param.Abstracts)
                    {
                        if (data.IsAbstractInReview(abs))
                        {
                            data.RemoveAbstractFromReview(abs);
                        }
                    }

                    context.Response.Write(JsonConvert.SerializeObject(new { success = true }));
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