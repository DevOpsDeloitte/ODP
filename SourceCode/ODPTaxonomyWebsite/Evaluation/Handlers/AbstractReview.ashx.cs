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
        public string abstracts = "";
        public string type = "";
        public string userguid = "";
        public List<string> abstractIDs;

        public void ProcessRequest(HttpContext context)
        {
            AbstractReviewParams param = new AbstractReviewParams(context);

            if (param.userGuid==null)
            {
                context.Response.Write(JsonConvert.SerializeObject(new { success = false, invalidguid = true }));
                return;
            }

            if(!param.all && param.includeList.Count == 0)
            {
                context.Response.Write(JsonConvert.SerializeObject(new { success = false, error = "No abstract selected" }));
            }

            abstractIDs = abstracts.Split(',').ToList();
            AbstractListViewData data = new AbstractListViewData();
            switch (type)
            {

                case "add":

                    try
                    {
                        foreach (var abs in abstractIDs)
                        {
                            if (!data.IsAbstractInReview(Convert.ToInt32(abs)))
                            {
                                data.AddAbstractToReview(Convert.ToInt32(abs), param.userGuid);
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


                    foreach (var abs in abstractIDs)
                    {
                        if (data.IsAbstractInReview(Convert.ToInt32(abs)))
                        {
                            data.RemoveAbstractFromReview(Convert.ToInt32(abs));
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