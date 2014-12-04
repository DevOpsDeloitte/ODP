using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using ODPTaxonomyDAL_TT;
using ODPTaxonomyDAL_JY;
using ODPTaxonomyUtility_TT;
using System.Configuration;

namespace ODPTaxonomyWebsite.Evaluation.Handlers
{

    //this handler returns Abstracts
    //that could NOT be re-opened
    public class AbstractsListCheck : IHttpHandler
    {
        #region Fields

        private string connString = null;
        private List<int> abstracts = null;
        private string userguid = "";
        private string action = "";

        #endregion

        public void ProcessRequest(HttpContext context)
        {
            connString = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString;
            //DataJYDataContext db = new DataJYDataContext(connString);
            userguid = context.Request["guid"] ?? "";
            action = context.Request["action"] ?? "";
            Guid ug;
            if (!Guid.TryParse(userguid, out ug))
            {
                context.Response.Write(JsonConvert.SerializeObject(new { success = false, invalidguid = true }));
                return;
            }

            //List<int> abstractIds = null;
            using (DataJYDataContext db = new DataJYDataContext(connString))

            {
                try
                {
                    switch (action)
                    {

                        case "addreview" :
                            abstracts = db.AbstractReviewLists.Select(q => q.AbstractID).ToList<int>();
                            context.Response.Write(JsonConvert.SerializeObject(new { hideboxes = abstracts, success = true }));
                    break;
                        case "removereview":
                             abstracts = db.AbstractReviewLists.Select(q => q.AbstractID).ToList<int>();
                             context.Response.Write(JsonConvert.SerializeObject(new { hideboxes = new List<int>(), success = true }));
                    break;



                    }


                    //abstracts = db.AbstractReviewLists.Select(q => q.AbstractID).ToList<int>();
                    //context.Response.Write(JsonConvert.SerializeObject(new { hideboxes = abstracts, success = true }));
                }
                catch (Exception ex)
                {
                    context.Response.Write(JsonConvert.SerializeObject(new { success = false, message = ex.Message }));
                }




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