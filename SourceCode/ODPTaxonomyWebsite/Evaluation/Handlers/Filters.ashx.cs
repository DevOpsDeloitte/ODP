using System;
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
                          (/*h.AbstractStatusID >= (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N
                            &&*/
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

            var reviewdata = from a in db.Abstracts
                        join h in db.AbstractStatusChangeHistories on a.AbstractID equals h.AbstractID
                        join s in db.AbstractStatus on h.AbstractStatusID equals s.AbstractStatusID
                        join rv in db.AbstractReviewLists on a.AbstractID equals rv.AbstractID
                        where (
                           //h.AbstractStatusID >= (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N &&
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

            var reportexcludedata = from a in db.Abstracts
                       //join h in db.AbstractStatusChangeHistories on a.AbstractID equals h.AbstractID
                       //join s in db.AbstractStatus on h.AbstractStatusID equals s.AbstractStatusID
                       join rv in db.Report_AbstractExcludedLists on a.AbstractID equals rv.AbstractID
                       //where (
                          //h.AbstractStatusID >= (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N &&
                      //    h.AbstractStatusChangeHistoryID == db.AbstractStatusChangeHistories
                      //     .Where(h2 => h2.AbstractID == a.AbstractID)
                       //    .Select(h2 => h2.AbstractStatusChangeHistoryID).Max()
                      //     )
                       select new AbstractListRow
                       {

                           AbstractID = a.AbstractID,
                           //ProjectTitle = a.ProjectTitle + " (" + s.AbstractStatusCode + ")",
                           PIProjectLeader = a.PIProjectLeader,
                           ApplicationID = a.ApplicationID,
                           AbstractStatusID = 14, //s.AbstractStatusID,
                           AbstractStatusCode = "", //s.AbstractStatusCode,
                           //StatusDate = h.CreatedDate,
                           LastExportDate = a.LastExportDate,
                           //EvaluationID = h.EvaluationId,
                           KappaType = KappaTypeEnum.K1,
                           IsParent = true
                       };

            List<AbstractListRow> reportexcludeabstracts = reportexcludedata.ToList();

            List<AbstractListRow> reviewabstracts = reviewdata.ToList();

            List<AbstractListRow> abstracts = data.ToList();

            switch(roleRequested){
                    // the 1st filter entry will be defaulted by the client code.
                case "Admin":
                    FV.opts.Add(new FilterOpts() { option = "uncoded", text = "View Uncoded Abstracts" + " (" + abstracts.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.OPEN_0).Select(s => s).ToList().Count.ToString() + ")" });
                    FV.opts.Add(new FilterOpts() { option = "default", text = "All Abstracts" + " (" + abstracts.Where(q => q.AbstractStatusID >= (int)AbstractStatusEnum.RETRIEVED_FOR_CODING_1).Select(s => s).ToList().Count.ToString() + ")" });
                    FV.opts.Add(new FilterOpts() { option = "codercompleted", text = "Coder Completed" + " (" + abstracts.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N).Select(s => s).ToList().Count.ToString() + ")" });
                    FV.opts.Add(new FilterOpts() { option = "odpcompleted", text = "ODP Completed" + " (" + abstracts.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.ODP_CONSENSUS_WITH_NOTES_2N).Select(s => s).ToList().Count.ToString() + ")" });
                    FV.opts.Add(new FilterOpts() { option = "closed", text = "Closed" + " (" + abstracts.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.CLOSED_3).Select(s => s).ToList().Count.ToString() + ")" });
                    FV.opts.Add(new FilterOpts() { option = "exported", text = "Exported" + " (" + abstracts.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.DATA_EXPORTED_4).Select(s => s).ToList().Count.ToString() + ")" });

                    //FV.opts.Add(new FilterOpts() { option = "review", text = "In Review List" + " (" + reviewabstracts.Where(q => q.AbstractStatusID >= (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N).ToList().Count.ToString() + ")" });
                    //FV.opts.Add(new FilterOpts() { option = "uncoded", text = "In Review List - Uncoded" });
                    //FV.opts.Add(new FilterOpts() { option = "activeabstracts", text = "Active Abstracts" + " (" + abstracts.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.CODED_BY_ODP_STAFF_2A || q.AbstractStatusID == (int)AbstractStatusEnum.RETRIEVED_FOR_ODP_CODING_2).Select(s => s).ToList().Count.ToString() + ")" });
                    //FV.opts.Add(new FilterOpts() { option = "odpcompletedwonotes", text = "ODP Completed without notes" + " (" + abstracts.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.ODP_STAFF_AND_CODER_CONSENSUS_2C).Select(s => s).ToList().Count.ToString() + ")" });
                  
                    break;

                case "ODPSupervisor" :
                    FV.opts.Add(new FilterOpts() { option = "odpcompleted", text = "ODP Completed" + " (" + abstracts.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.ODP_CONSENSUS_WITH_NOTES_2N).Select(s => s).ToList().Count.ToString() + ")" });  
                    // all abstracts being set to all instead of "default".
                    FV.opts.Add(new FilterOpts() { option = "all", text = "All Abstracts" + " (" + abstracts.Where(q => q.AbstractStatusID >= (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N).Select(s => s).ToList().Count.ToString() + ")" });
                    FV.opts.Add(new FilterOpts() { option = "review", text = "In Review List" + " (" + reviewabstracts.Where(q => q.AbstractStatusID >= (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N).ToList().Count.ToString() + ")" });
                    FV.opts.Add(new FilterOpts() { option = "reviewuncoded", text = "In Review List Uncoded" + " (" + reviewabstracts.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N).ToList().Count.ToString() + ")" });
                    //FV.opts.Add(new FilterOpts() { option = "reviewuncoded", text = "In Review List Uncoded" + " (" + reviewabstracts.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.RETRIEVED_FOR_ODP_CODING_2 || q.AbstractStatusID == (int)AbstractStatusEnum.CODED_BY_ODP_STAFF_2A || q.AbstractStatusID == (int)AbstractStatusEnum.ODP_STAFF_CONSENSUS_2B).ToList().Count.ToString() + ")" });         
                    FV.opts.Add(new FilterOpts() { option = "codercompleted", text = "Coder Completed" + " (" + abstracts.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N).Select(s => s).ToList().Count.ToString() + ")" });
                    FV.opts.Add(new FilterOpts() { option = "activeabstracts", text = "Active Abstracts" + " (" + abstracts.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.CODED_BY_ODP_STAFF_2A || q.AbstractStatusID == (int)AbstractStatusEnum.RETRIEVED_FOR_ODP_CODING_2).Select(s => s).ToList().Count.ToString() + ")" });
                    FV.opts.Add(new FilterOpts() { option = "odpcompletedwonotes", text = "ODP Completed without notes" + " (" + abstracts.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.ODP_STAFF_AND_CODER_CONSENSUS_2C).Select(s => s).ToList().Count.ToString() + ")" });
                    FV.opts.Add(new FilterOpts() { option = "closed", text = "Closed" + " (" + abstracts.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.CLOSED_3).Select(s => s).ToList().Count.ToString() + ")" });
                    FV.opts.Add(new FilterOpts() { option = "exported", text = "Exported" + " (" + abstracts.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.DATA_EXPORTED_4).Select(s => s).ToList().Count.ToString() + ")" });
                    FV.opts.Add(new FilterOpts() { option = "reportexclude", text = "Report Exclude List" + " (" + reportexcludeabstracts.Where(q => q.AbstractStatusID >= (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N).Select(s => s).ToList().Count.ToString() + ")" });
                    break;

                case "ODPStaff":
                FV.opts.Add(new FilterOpts()
                {
                    option = "default",
                    text = "View All" + " (" + abstracts.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N || q.AbstractStatusID == (int)AbstractStatusEnum.ODP_CONSENSUS_WITH_NOTES_2N ||
                        q.AbstractStatusID == (int)AbstractStatusEnum.CLOSED_3 || q.AbstractStatusID == (int)AbstractStatusEnum.DATA_EXPORTED_4 ||
                        q.AbstractStatusID == (int)AbstractStatusEnum.RETRIEVED_FOR_ODP_CODING_2 || q.AbstractStatusID == (int)AbstractStatusEnum.CODED_BY_ODP_STAFF_2A 
                        || q.AbstractStatusID == (int)AbstractStatusEnum.ODP_STAFF_CONSENSUS_2B || q.AbstractStatusID == (int)AbstractStatusEnum.ODP_STAFF_AND_CODER_CONSENSUS_2C).Select(s => s).ToList().Count.ToString() + ")"
                });
                FV.opts.Add(new FilterOpts()
                {
                    option = "review",
                    text = "In Review List" + " (" + reviewabstracts.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.RETRIEVED_FOR_ODP_CODING_2 || q.AbstractStatusID == (int)AbstractStatusEnum.CODED_BY_ODP_STAFF_2A || q.AbstractStatusID == (int)AbstractStatusEnum.ODP_STAFF_CONSENSUS_2B || q.AbstractStatusID == (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N || q.AbstractStatusID == (int)AbstractStatusEnum.ODP_CONSENSUS_WITH_NOTES_2N || q.AbstractStatusID == (int)AbstractStatusEnum.CLOSED_3
                        || q.AbstractStatusID == (int)AbstractStatusEnum.ODP_STAFF_AND_CODER_CONSENSUS_2C || q.AbstractStatusID == (int)AbstractStatusEnum.DATA_EXPORTED_4).ToList().Count.ToString() + ")"
                });
                FV.opts.Add(new FilterOpts() { option = "reviewuncoded", text = "In Review List Uncoded" + " (" + reviewabstracts.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N).ToList().Count.ToString() + ")" });
                //FV.opts.Add(new FilterOpts() { option = "reviewuncoded", text = "In Review List Uncoded" + " (" + reviewabstracts.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.RETRIEVED_FOR_ODP_CODING_2 || q.AbstractStatusID == (int)AbstractStatusEnum.CODED_BY_ODP_STAFF_2A || q.AbstractStatusID == (int)AbstractStatusEnum.ODP_STAFF_CONSENSUS_2B).ToList().Count.ToString() + ")" });
                    
                //FV.opts.Add(new FilterOpts() { option = "uncoded", text = "In Review List - Uncoded" });
                FV.opts.Add(new FilterOpts() { option = "codercompleted", text = "Coder Completed" + " (" + abstracts.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N).Select(s => s).ToList().Count.ToString() + ")" });
                FV.opts.Add(new FilterOpts() { option = "odpcompleted", text = "ODP Completed" + " (" + abstracts.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.ODP_CONSENSUS_WITH_NOTES_2N).Select(s => s).ToList().Count.ToString() + ")" });
                FV.opts.Add(new FilterOpts() { option = "closed", text = "Closed" + " (" + abstracts.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.CLOSED_3).Select(s => s).ToList().Count.ToString() + ")" });
                FV.opts.Add(new FilterOpts() { option = "exported", text = "Exported" + " (" + abstracts.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.DATA_EXPORTED_4).Select(s => s).ToList().Count.ToString() + ")" });
                break;

                case "CoderSupervisor":
                FV.opts.Add(new FilterOpts()
                {
                    option = "default",
                    text = "View All" + " (" + abstracts.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N || q.AbstractStatusID == (int)AbstractStatusEnum.CONSENSUS_COMPLETE_1B ||
                        q.AbstractStatusID == (int)AbstractStatusEnum.ODP_CONSENSUS_WITH_NOTES_2N || q.AbstractStatusID == (int)AbstractStatusEnum.CLOSED_3 || q.AbstractStatusID == (int)AbstractStatusEnum.DATA_EXPORTED_4).Select(s => s).ToList().Count.ToString() + ")"
                });
                
                FV.opts.Add(new FilterOpts() { option = "codercompleted", text = "Coder Completed" + " (" + abstracts.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N).Select(s => s).ToList().Count.ToString() + ")" });
                FV.opts.Add(new FilterOpts() { option = "codercompletedwonotes", text = "Coder Completed without notes" + " (" + abstracts.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.CONSENSUS_COMPLETE_1B).Select(s => s).ToList().Count.ToString() + ")" });
                FV.opts.Add(new FilterOpts() { option = "activeabstracts", text = "Active Abstracts" + " (" + abstracts.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.RETRIEVED_FOR_CODING_1 || q.AbstractStatusID == (int)AbstractStatusEnum.CODED_BY_CODER_1A).Select(s => s).ToList().Count.ToString() + ")" });    
                FV.opts.Add(new FilterOpts() { option = "odpcompleted", text = "ODP Completed" + " (" + abstracts.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.ODP_CONSENSUS_WITH_NOTES_2N).Select(s => s).ToList().Count.ToString() + ")" });
                FV.opts.Add(new FilterOpts() { option = "closed", text = "Closed" + " (" + abstracts.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.CLOSED_3).Select(s => s).ToList().Count.ToString() + ")" });
                FV.opts.Add(new FilterOpts() { option = "exported", text = "Exported" + " (" + abstracts.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.DATA_EXPORTED_4).Select(s => s).ToList().Count.ToString() + ")" });
                break;


          }


           
            



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