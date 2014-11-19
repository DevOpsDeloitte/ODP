﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using System.Configuration;
using ODPTaxonomyDAL_JY;
using ODPTaxonomyUtility_TT;
using System.Web.Security;
using Newtonsoft.Json;

namespace ODPTaxonomyWebsite.Evaluation.Handlers
{
    /// <summary>
    /// Summary description for Filters
    /// </summary>
        // select asch.AbstractStatusID, COUNT(*)
        //from Abstract a 
        //inner join AbstractStatusChangeHistory asch on a.AbstractID = asch.AbstractID
        //inner join AbstractStatus stat on stat.AbstractStatusID = asch.AbstractStatusID
        //where asch.AbstractStatusChangeHistoryID = ( select max(a2.AbstractStatusChangeHistoryID) from AbstractStatusChangeHistory a2 where a2.AbstractID = a.AbstractID)
        //group by asch.AbstractStatusID
    public class Filters : IHttpHandler
    {
        private FilterVals FV = new FilterVals();
        //public string filter = "";
        public string roleRequested = "";

        public void ProcessRequest(HttpContext context)
        {
            //context.Response.ContentType = "";
            roleRequested = context.Request["role"] ?? "";       
            buildFilters();

            context.Response.Write(JsonConvert.SerializeObject(FV));
            return;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public void buildFilters(){

            string connString = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString;
            DataJYDataContext db = new DataJYDataContext(connString);

            var data = from a in db.Abstracts
                       join h in db.AbstractStatusChangeHistories on a.AbstractID equals h.AbstractID
                       join s in db.AbstractStatus on h.AbstractStatusID equals s.AbstractStatusID
                       where (
                          (h.AbstractStatusID >= (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N
                            &&
                          h.AbstractStatusChangeHistoryID == db.AbstractStatusChangeHistories
                           .Where(h2 => h2.AbstractID == a.AbstractID)
                           .Select(h2 => h2.AbstractStatusChangeHistoryID).Max()
                           ))
                       select new AbstractListRow
                       {
                           AbstractID = a.AbstractID,
                           ProjectTitle = a.ProjectTitle + " (" + s.AbstractStatusCode + ")",
                           ApplicationID = a.ApplicationID,
                           AbstractStatusID = s.AbstractStatusID,
                           AbstractStatusCode = s.AbstractStatusCode,
                           StatusDate = h.CreatedDate,
                           EvaluationID = h.EvaluationId,
                           KappaType = KappaTypeEnum.K1,
                           IsParent = true
                       };

            List<AbstractListRow> abstracts = data.ToList();
            FV.opts.Add(new FilterOpts() { option = "default", text = "All Abstracts" + " (" + abstracts.Count.ToString() + ")" });
            //FV.opts.Add(new FilterOpts() { option = "open", text = "Open Abstracts" });



            //string connString = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString;
            //DataJYDataContext db = new DataJYDataContext(connString);

            var data2 = from a in db.Abstracts
                       join h in db.AbstractStatusChangeHistories on a.AbstractID equals h.AbstractID
                       join s in db.AbstractStatus on h.AbstractStatusID equals s.AbstractStatusID
                       join rv in db.AbstractReviewLists on a.AbstractID equals rv.AbstractID
                       where (
                          h.AbstractStatusID >= (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N &&
                          h.AbstractStatusChangeHistoryID == db.AbstractStatusChangeHistories
                           .Where(h2 => h2.AbstractID == a.AbstractID)
                           .Select(h2 => h2.AbstractStatusChangeHistoryID).Max()
                           )
                       select new AbstractListRow
                       {
                           AbstractID = a.AbstractID,
                           ProjectTitle = a.ProjectTitle + " (" + s.AbstractStatusCode + ")",
                           ApplicationID = a.ApplicationID,
                           AbstractStatusID = s.AbstractStatusID,
                           AbstractStatusCode = s.AbstractStatusCode,
                           StatusDate = h.CreatedDate,
                           EvaluationID = h.EvaluationId,
                           KappaType = KappaTypeEnum.K1,
                           IsParent = true
                       };

            List<AbstractListRow> reviewabstracts = data2.ToList();
            FV.opts.Add(new FilterOpts() { option = "review", text = "In Review List"+ " (" + reviewabstracts.Count.ToString()+")" });
            //FV.opts.Add(new FilterOpts() { option = "uncoded", text = "In Review List - Uncoded" });

            FV.opts.Add(new FilterOpts() { option = "codercompleted", text = "Coder Completed" });
            FV.opts.Add(new FilterOpts() { option = "activeabstracts", text = "Active Abstracts" });
            FV.opts.Add(new FilterOpts() { option = "odpcompleted", text = "ODP Completed" });
            FV.opts.Add(new FilterOpts() { option = "odpcompletedwonotes", text = "ODP Completed without notes" });
            FV.opts.Add(new FilterOpts() { option = "closed", text = "Closed" });
            FV.opts.Add(new FilterOpts() { option = "exported", text = "Exported" });



        }
    }

    

    public class FilterVals
    {
        public List<FilterOpts> opts = new List<FilterOpts>();

    }

    public class FilterOpts
    {
        public string option { get; set; }
        public string text { get; set; }
    }
}