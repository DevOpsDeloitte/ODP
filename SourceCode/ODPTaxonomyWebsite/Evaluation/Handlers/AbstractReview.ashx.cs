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
        public string function = "";
        public string userguid = "";
        public List<string> abstractIDs;

        public void ProcessRequest(HttpContext context)
        {
            abstracts = context.Request["abstracts"] ?? "";
            function = context.Request["function"] ?? "";
            userguid = context.Request["function"] ?? "";
            Guid ug;
            if (!Guid.TryParse(userguid, out ug))
            {
                context.Response.Write(JsonConvert.SerializeObject(new { success = false, invalidguid = true }));
                return;
            }
            abstractIDs = abstracts.Split(',').ToList();
            AbstractListViewData data = new AbstractListViewData();
            switch (function)
            {

                case "add" :

                    try
                    {
                        foreach (var abs in abstractIDs)
                        {
                            if (!data.IsAbstractInReview(Convert.ToInt16(abs)))
                            {
                                data.AddAbstractToReview(Convert.ToInt16(abs), (Guid)ug);
                            }
                        }

                        context.Response.Write(JsonConvert.SerializeObject(new { success = true }));
                    }
                    catch(Exception ex)
                    {
                        context.Response.Write(JsonConvert.SerializeObject(new { success = false, message = ex.Message }));
                    }

                    break;

                case "remove" :

                    
                    foreach (var abs in abstractIDs)
                    {
                        if (data.IsAbstractInReview(Convert.ToInt16(abs)))
                        {
                            data.RemoveAbstractFromReview(Convert.ToInt16(abs));
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