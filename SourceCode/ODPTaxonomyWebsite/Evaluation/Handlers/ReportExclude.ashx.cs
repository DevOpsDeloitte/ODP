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
        public void ProcessRequest(HttpContext context)
        {
            AbstractActionParams param = new AbstractActionParams(context);

            if (param.userGuid==null)
            {
                context.Response.Write(JsonConvert.SerializeObject(new { success = false, invalidguid = true }));
                return;
            }

            AbstractListViewData data = new AbstractListViewData();
            switch (param.type)
            {
                case "add":
                    try
                    {
                        foreach (var abs in param.Abstracts)
                        {
                            if (!data.IsAbstractInReportExclude(abs))
                            {
                                data.AddAbstractToReportExclude(abs, param.userGuid);
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
                        if (data.IsAbstractInReportExclude(abs))
                        {
                            data.RemoveAbstractFromReportExclude(abs);
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