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
        private List<AbstractListRow> finalabstracts = null;
        //private List<AbstractListRow> finalabstracts2 = null;
        private string userguid = "";
        private string action = "";

        #endregion

        private void loadAbstracts()
        {
            using (DataJYDataContext db = new DataJYDataContext(connString))
            {
                var data = from a in db.Abstracts
                           join h in db.AbstractStatusChangeHistories on a.AbstractID equals h.AbstractID
                           join s in db.AbstractStatus on h.AbstractStatusID equals s.AbstractStatusID
                           where (
                              h.AbstractStatusChangeHistoryID == db.AbstractStatusChangeHistories
                               .Where(h2 => h2.AbstractID == a.AbstractID)
                               .Select(h2 => h2.AbstractStatusChangeHistoryID).Max()
                               )
                           select new AbstractListRow
                           {
                               AbstractID = a.AbstractID,
                               ProjectTitle = a.ProjectTitle + " (" + s.AbstractStatusCode + ")",
                               PIProjectLeader = a.PIProjectLeader,
                               ApplicationID = a.ChrApplicationID,
                               AbstractStatusID = s.AbstractStatusID,
                               AbstractStatusCode = s.AbstractStatusCode,
                               StatusDate = h.CreatedDate,
                               LastExportDate = a.LastExportDate,
                               EvaluationID = h.EvaluationId,
                               IsParent = true
                           };
                finalabstracts = data.ToList();
            }

        }

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
                        case "addreportexclude":
                            abstracts = db.Report_AbstractExcludedLists.Select(q => q.AbstractID).ToList<int>();
                            context.Response.Write(JsonConvert.SerializeObject(new { hideboxes = abstracts, success = true }));
                            break;
                        case "removereview":
                             //abstracts = db.AbstractReviewLists.Select(q => q.AbstractID).ToList<int>();
                             context.Response.Write(JsonConvert.SerializeObject(new { hideboxes = new List<int>(), success = true }));
                            break;
                        case "closeabstract":
                            //var data = from a in db.Abstracts
                            //           join h in db.AbstractStatusChangeHistories on a.AbstractID equals h.AbstractID
                            //           join s in db.AbstractStatus on h.AbstractStatusID equals s.AbstractStatusID
                            //           where (
                            //              h.AbstractStatusChangeHistoryID == db.AbstractStatusChangeHistories
                            //               .Where(h2 => h2.AbstractID == a.AbstractID)
                            //               .Select(h2 => h2.AbstractStatusChangeHistoryID).Max()
                            //               )
                            //           select new AbstractListRow
                            //           {
                            //               AbstractID = a.AbstractID,
                            //               ProjectTitle = a.ProjectTitle + " (" + s.AbstractStatusCode + ")",
                            //               PIProjectLeader = a.PIProjectLeader,
                            //               ApplicationID = a.ApplicationID,
                            //               AbstractStatusID = s.AbstractStatusID,
                            //               AbstractStatusCode = s.AbstractStatusCode,
                            //               StatusDate = h.CreatedDate,
                            //               LastExportDate = a.LastExportDate,
                            //               EvaluationID = h.EvaluationId,
                            //               IsParent = true
                            //           };
                            //finalabstracts = data.ToList();
                            loadAbstracts();
                            //finalabstracts2 = finalabstracts.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.CLOSED_3 && ( q.AbstractStatusID != (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N || q.AbstractStatusID != (int)AbstractStatusEnum.ODP_CONSENSUS_WITH_NOTES_2N) ).Select(s => s.AbstractID).ToList();
                            abstracts = finalabstracts.Where(q => q.AbstractStatusID != (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N && q.AbstractStatusID != (int)AbstractStatusEnum.ODP_CONSENSUS_WITH_NOTES_2N).Select(s => s.AbstractID).ToList();
                            context.Response.Write(JsonConvert.SerializeObject(new { hideboxes = abstracts, success = true }));
                            break;
                        case "exportabstracts":
                            
                            loadAbstracts();
                            abstracts = finalabstracts.Where(q => q.AbstractStatusID != (int)AbstractStatusEnum.CLOSED_3 && q.AbstractStatusID != (int)AbstractStatusEnum.DATA_EXPORTED_4).Select(s => s.AbstractID).ToList();
                            context.Response.Write(JsonConvert.SerializeObject(new { hideboxes = abstracts, success = true }));
                            break;
                        default:
                            context.Response.Write(JsonConvert.SerializeObject(new { hideboxes = new List<int>(), success = true }));;
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