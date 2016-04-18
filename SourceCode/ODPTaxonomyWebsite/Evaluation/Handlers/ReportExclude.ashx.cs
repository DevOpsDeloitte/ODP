using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using ODPTaxonomyDAL_JY;

namespace ODPTaxonomyWebsite.Evaluation.Handlers
{
    /// <summary>
    /// Summary description for ReportExclude
    /// </summary>
    public class ReportExclude : IHttpHandler
    {

        public string abstracts = "";
        public string type = "";
        public string userguid = "";
        public List<string> abstractIDs;

        public void ProcessRequest(HttpContext context)
        {
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
                                data.AddAbstractToReview(Convert.ToInt32(abs), (Guid)ug);
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