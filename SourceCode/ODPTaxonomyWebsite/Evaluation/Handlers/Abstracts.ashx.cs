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
    /// Summary description for Abstracts
    /// </summary>
    public class Abstracts : IHttpHandler
    {
        public string roleRequested = "";
        public string filter = "";
        public short? submissiontypeID = 0;


        protected void serializeResponse(HttpContext context, List<AbstractListRow> ALR){
              IEnumerable<AbstractListRow> ALRX = ALR.Where(x => x.IsParent == true);                                      
              context.Response.Write("{  \"data\" : " +JsonConvert.SerializeObject(ALRX.ToList())+"  }");
               return;
        }
        
        public void ProcessRequest(HttpContext context)
        {
            List<AbstractListRow> parentAbstracts;
            List<AbstractListRow> ALR;
            roleRequested = context.Request["role"] ?? "";
            filter = context.Request["filter"] ?? "";
            AbstractListViewData data = new AbstractListViewData();
            switch (roleRequested)
            {

                case "ODPSupervisor":

                    switch (filter)
                    {
                        //case "open":
                        //    parentAbstracts = this.GetParentAbstractsODPSupervisorOpen();   
                        //    ALR = AbstractListViewHelper.ProcessAbstracts2(parentAbstracts, AbstractViewRole.ODPSupervisor);
                        //    serializeResponse(context, ALR);
                            
                        //    break;

                        //case "uncoded":
                        //    parentAbstracts = this.GetParentAbstractsODPSupervisorReviewUnCoded();  
                        //    ALR = AbstractListViewHelper.ProcessAbstracts2(parentAbstracts, AbstractViewRole.ODPSupervisor);
                        //    serializeResponse(context, ALR);

                        //    break;

                        case "review" :
                            parentAbstracts = this.GetParentAbstractsODPSupervisorReview();
                            foreach (var abs in parentAbstracts)
                            {
                                
                                    abs.InReview  = true;
                                
                            }
                            ALR = AbstractListViewHelper.ProcessAbstracts2(parentAbstracts, AbstractViewRole.ODPSupervisor);
                            serializeResponse(context, ALR);
                            
                            break;


                        default: // return default
                            //parentAbstracts = GetParentAbstractsODPSupervisorDefault();
                            parentAbstracts = GetParentAbstractsODPSupervisor(filter); // we are passing the filter as the query.
                            System.Diagnostics.Stopwatch objStopWatch = new System.Diagnostics.Stopwatch();
                            objStopWatch.Start();
                            foreach (var abs in parentAbstracts)
                            {
                                if (data.IsAbstractInReview(abs.AbstractID))
                                {
                                    abs.InReview  = true;
                                }
                                else
                                {
                                    abs.InReview = false;
                                }
                            }
                            ALR = AbstractListViewHelper.ProcessAbstracts2(parentAbstracts, AbstractViewRole.ODPSupervisor);
                            objStopWatch.Stop();
                            serializeResponse(context, ALR);
                            System.Diagnostics.Trace.WriteLine("The time taken to execute ProcessAbstracts is : " +
                            objStopWatch.ElapsedMilliseconds.ToString() + " MillionSeconds<br>");
                            objStopWatch.Reset();
                            break;

                    }
                   
                    break;
                case "CoderSupervisor":

                    switch (filter)
                    {
                        //case "open" :
                        //     parentAbstracts = this.GetParentAbstractsCoderSupervisorOpen();
                        //     ALR = AbstractListViewHelper.ProcessAbstracts2(parentAbstracts, AbstractViewRole.CoderSupervisor);
                        //     serializeResponse(context, ALR);

                        //    break;

                        //case "coded" :
                        //    parentAbstracts = this.GetParentAbstractsCoderSupervisorCoded();
                        //     ALR = AbstractListViewHelper.ProcessAbstracts2(parentAbstracts, AbstractViewRole.CoderSupervisor);
                        //     serializeResponse(context, ALR);

                        //    break;

                        default: // return open
                              parentAbstracts = this.GetParentAbstractsCoderSupervisor(filter);
                              ALR = AbstractListViewHelper.ProcessAbstracts2(parentAbstracts, AbstractViewRole.CoderSupervisor);
                              serializeResponse(context, ALR);
                            break;

                    }

                  
                    break;

                case "ODPStaff":
                    


                     switch (filter)
                    {
                        //case "uncoded":
                        //    parentAbstracts = this.GetParentAbstractsODPStaffMemberUncoded();
                        //    ALR = AbstractListViewHelper.ProcessAbstracts2(parentAbstracts, AbstractViewRole.ODPStaff);
                        //    serializeResponse(context, ALR);
                            
                        //    break;

                        //case "coded":
                        //    parentAbstracts = this.GetParentAbstractsODPStaffMemberDefault();
                        //    ALR = AbstractListViewHelper.ProcessAbstracts2(parentAbstracts, AbstractViewRole.ODPStaff);
                        //    serializeResponse(context, ALR);
                        //    break;


                        case "": // first call default is review list.
                            parentAbstracts = this.GetParentAbstractsODPStaffMemberReview();
                            ALR = AbstractListViewHelper.ProcessAbstracts2(parentAbstracts, AbstractViewRole.ODPStaff);
                            serializeResponse(context, ALR);
                            break;

                        case "review":
                            parentAbstracts = this.GetParentAbstractsODPStaffMemberReview();
                            ALR = AbstractListViewHelper.ProcessAbstracts2(parentAbstracts, AbstractViewRole.ODPStaff);
                            serializeResponse(context, ALR);
                            break;

                        default: // return default
                           parentAbstracts = this.GetParentAbstractsODPStaffMember(filter);
                            ALR = AbstractListViewHelper.ProcessAbstracts2(parentAbstracts, AbstractViewRole.ODPStaff);
                            serializeResponse(context, ALR);
                            break;

                    }
                   
                    
                    break;
                case "Admin" :
                    parentAbstracts = this.GetParentAbstractsAdmin(filter);
                    ALR = AbstractListViewHelper.ProcessAbstracts2(parentAbstracts, AbstractViewRole.Admin);
                    serializeResponse(context, ALR);

                    break;

                case null :

                    break;

                default:

                    break;



            }

            

           


          
            //context.Response.Write(JsonConvert.SerializeObject(new { success = false, supervisorauthfailed = true }));
            return;
        }
        //protected List<AbstractListRow> GetParentAbstractsAdmin(string sort = "", SortDirection direction = SortDirection.Ascending)
        //{
        //    string connString = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString;
        //    DataJYDataContext db = new DataJYDataContext(connString);

        //    var data = from a in db.Abstracts
        //               join h in db.AbstractStatusChangeHistories on a.AbstractID equals h.AbstractID
        //               join s in db.AbstractStatus on h.AbstractStatusID equals s.AbstractStatusID
        //               where (
        //                  h.AbstractStatusChangeHistoryID == db.AbstractStatusChangeHistories
        //                   .Where(h2 => h2.AbstractID == a.AbstractID)
        //                   .Select(h2 => h2.AbstractStatusChangeHistoryID).Max()
        //                   )
        //               select new AbstractListRow
        //               {
        //                   AbstractID = a.AbstractID,
        //                   ProjectTitle = a.ProjectTitle + " (" + s.AbstractStatusCode + ")",
        //                   ApplicationID = a.ApplicationID,
        //                   AbstractStatusID = s.AbstractStatusID,
        //                   AbstractStatusCode = s.AbstractStatusCode,
        //                   StatusDate = h.CreatedDate,
        //                   EvaluationID = h.EvaluationId,
        //                   KappaType = KappaTypeEnum.K1,
        //                   IsParent = true
        //               };

        //    List<AbstractListRow> abstracts = data.ToList();

        //    return AbstractListViewHelper.SortAbstracts(abstracts, sort, direction);
        //}
        protected List<AbstractListRow> GetParentAbstractsAdmin(string query = "", string sort = "", SortDirection direction = SortDirection.Ascending)
        {
            string connString = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString;
            DataJYDataContext db = new DataJYDataContext(connString);

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
                           ApplicationID = a.ApplicationID,
                           AbstractStatusID = s.AbstractStatusID,
                           AbstractStatusCode = s.AbstractStatusCode,
                           StatusDate = h.CreatedDate,
                           LastExportDate = a.LastExportDate,
                           EvaluationID = h.EvaluationId,
                           IsParent = true
                       };

            List<AbstractListRow> abstracts = data.ToList();
            List<AbstractListRow> finalabstracts = null;
            //public enum AbstractStatusEnum
            //      {
            //          OPEN_0 = 1,
            //          RETRIEVED_FOR_CODING_1 = 2,
            //          CODED_BY_CODER_1A = 3,
            //          CONSENSUS_COMPLETE_1B = 4,
            //          CONSENSUS_COMPLETE_WITH_NOTES_1N = 6,
            //          RETRIEVED_FOR_ODP_CODING_2 = 7,
            //          CODED_BY_ODP_STAFF_2A = 8,
            //          ODP_STAFF_CONSENSUS_2B = 9,
            //          ODP_STAFF_AND_CODER_CONSENSUS_2C = 10,
            //          ODP_CONSENSUS_WITH_NOTES_2N = 12,
            //          CLOSED_3 = 13,
            //          DATA_EXPORTED_4 = 14,
            //          REOPEN_FOR_REVIEW_BY_ODP = 15
            //      }
            switch (query)
            {
                case "uncoded":
                    finalabstracts = abstracts.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.OPEN_0).Select(s => s).ToList();
                    break;
                case "default":
                    finalabstracts = abstracts.Where(q => q.AbstractStatusID >= (int)AbstractStatusEnum.RETRIEVED_FOR_CODING_1).Select(s => s).ToList();
                    break;
                case "codercompleted":
                    finalabstracts = abstracts.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N).Select(s => s).ToList();
                    break;
                
                case "odpcompleted":
                    finalabstracts = abstracts.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.ODP_CONSENSUS_WITH_NOTES_2N).Select(s => s).ToList();
                    break;

                case "closed":
                    finalabstracts = abstracts.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.CLOSED_3).Select(s => s).ToList();
                    break;
                case "exported":
                    finalabstracts = abstracts.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.DATA_EXPORTED_4).Select(s => s).ToList();
                    break;
                default: // Note the default here is uncoded!
                    finalabstracts = abstracts.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.OPEN_0).Select(s => s).ToList();
                    break;


            }


            return AbstractListViewHelper.SortAbstracts(finalabstracts, sort, direction);
        }
        protected List<AbstractListRow> GetParentAbstractsCoderSupervisorCoded(string sort = "", SortDirection direction = SortDirection.Ascending)
        {
            string connString = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString;
            DataJYDataContext db = new DataJYDataContext(connString);

            var data = from a in db.Abstracts
                       join h in db.AbstractStatusChangeHistories on a.AbstractID equals h.AbstractID
                       join s in db.AbstractStatus on h.AbstractStatusID equals s.AbstractStatusID
                       where (
                          (
                          (h.AbstractStatusID >= (int)AbstractStatusEnum.CONSENSUS_COMPLETE_1B &&
                           h.AbstractStatusID <= (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N) ||
                          (h.AbstractStatusID >= (int)AbstractStatusEnum.ODP_STAFF_AND_CODER_CONSENSUS_2C)
                          ) &&
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

            List<AbstractListRow> abstracts = data.ToList();

            return AbstractListViewHelper.SortAbstracts(abstracts, sort, direction);
        }
        protected List<AbstractListRow> GetParentAbstractsCoderSupervisorOpen(string sort = "", SortDirection direction = SortDirection.Ascending)
        {
            string connString = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString;
            DataJYDataContext db = new DataJYDataContext(connString);

            var data = from a in db.Abstracts
                       join h in db.AbstractStatusChangeHistories on a.AbstractID equals h.AbstractID
                       join s in db.AbstractStatus on h.AbstractStatusID equals s.AbstractStatusID
                       where (
                          (h.AbstractStatusID == (int)AbstractStatusEnum.RETRIEVED_FOR_CODING_1 ||
                          h.AbstractStatusID == (int)AbstractStatusEnum.CODED_BY_CODER_1A) &&
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
                           KappaType = KappaTypeEnum.K1,
                           IsParent = true
                       };

            List<AbstractListRow> abstracts = data.ToList();

            //if (AbstractViewGridView.Attributes["CurrentSortExp"] != null)
            //{
            //    sort = AbstractViewGridView.Attributes["CurrentSortExp"];
            //    direction = AbstractViewGridView.Attributes["CurrentSortDir"] == "ASC" ? SortDirection.Ascending : SortDirection.Descending;
            //}

            return AbstractListViewHelper.SortAbstracts(abstracts, sort, direction);
        }


        protected List<AbstractListRow> GetParentAbstractsODPSupervisor(string query = "", string sort = "", SortDirection direction = SortDirection.Ascending)
        {
            string connStr = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString;
            DataJYDataContext db = new DataJYDataContext(connStr);


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
                           ApplicationID = a.ApplicationID,
                           AbstractStatusID = s.AbstractStatusID,
                           AbstractStatusCode = s.AbstractStatusCode,
                           StatusDate = h.CreatedDate,
                           LastExportDate = a.LastExportDate,
                           EvaluationID = h.EvaluationId,
                           IsParent = true
                       };



            List<AbstractListRow> abstracts = data.ToList();
            List<AbstractListRow> finalabstracts = null;
              //public enum AbstractStatusEnum
              //      {
              //          OPEN_0 = 1,
              //          RETRIEVED_FOR_CODING_1 = 2,
              //          CODED_BY_CODER_1A = 3,
              //          CONSENSUS_COMPLETE_1B = 4,
              //          CONSENSUS_COMPLETE_WITH_NOTES_1N = 6,
              //          RETRIEVED_FOR_ODP_CODING_2 = 7,
              //          CODED_BY_ODP_STAFF_2A = 8,
              //          ODP_STAFF_CONSENSUS_2B = 9,
              //          ODP_STAFF_AND_CODER_CONSENSUS_2C = 10,
              //          ODP_CONSENSUS_WITH_NOTES_2N = 12,
              //          CLOSED_3 = 13,
              //          DATA_EXPORTED_4 = 14,
              //          REOPEN_FOR_REVIEW_BY_ODP = 15
              //      }
            switch (query)
            {
                case "default" :
                    finalabstracts = abstracts.Where(q => q.AbstractStatusID >= (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N).Select(s => s).ToList();
                    break;
                case "codercompleted":
                    finalabstracts = abstracts.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N).Select(s => s).ToList();
                    break;
                case "activeabstracts":
                    finalabstracts = abstracts.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.CODED_BY_ODP_STAFF_2A || q.AbstractStatusID == (int)AbstractStatusEnum.RETRIEVED_FOR_ODP_CODING_2).Select(s => s).ToList();
                    break;
                case "odpcompleted":
                    finalabstracts = abstracts.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.ODP_CONSENSUS_WITH_NOTES_2N).Select(s => s).ToList();
                    break;
                case "odpcompletedwonotes":
                    finalabstracts = abstracts.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.ODP_STAFF_AND_CODER_CONSENSUS_2C).Select(s => s).ToList();
                    break;
                case "closed":
                    finalabstracts = abstracts.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.CLOSED_3).Select(s => s).ToList();
                    break;
                case "exported":
                    finalabstracts = abstracts.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.DATA_EXPORTED_4).Select(s => s).ToList();
                    break;
                default:
                    finalabstracts = abstracts.Where(q => q.AbstractStatusID >= (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N).Select(s => s).ToList();
                    break;


            }
            //List<AbstractListRow> finalabstracts = abstracts.Where(q => q.AbstractStatusID >= (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N).Select(s => s).ToList();


            return AbstractListViewHelper.SortAbstracts(finalabstracts, sort, direction);
        }
        protected List<AbstractListRow> GetParentAbstractsODPSupervisorReview(string sort = "", SortDirection direction = SortDirection.Ascending)
        {
            string connString = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString;
            DataJYDataContext db = new DataJYDataContext(connString);

            var data = from a in db.Abstracts
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
                           PIProjectLeader = a.PIProjectLeader,
                           ApplicationID = a.ApplicationID,
                           AbstractStatusID = s.AbstractStatusID,
                           AbstractStatusCode = s.AbstractStatusCode,
                           StatusDate = h.CreatedDate,
                           LastExportDate = a.LastExportDate,
                           EvaluationID = h.EvaluationId,
                           KappaType = KappaTypeEnum.K1,
                           IsParent = true
                       };

            List<AbstractListRow> abstracts = data.ToList();

            //if (AbstractViewGridView.Attributes["CurrentSortExp"] != null)
            //{
            //    sort = AbstractViewGridView.Attributes["CurrentSortExp"];
            //    direction = AbstractViewGridView.Attributes["CurrentSortDir"] == "ASC" ? SortDirection.Ascending : SortDirection.Descending;
            //}

            return AbstractListViewHelper.SortAbstracts(abstracts, sort, direction);
        }


        protected List<AbstractListRow> GetParentAbstractsODPStaffMember(string query = "", string sort = "", SortDirection direction = SortDirection.Ascending)
        {
            string connString = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString;
            DataJYDataContext db = new DataJYDataContext(connString);

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
                           ApplicationID = a.ApplicationID,
                           AbstractStatusID = s.AbstractStatusID,
                           AbstractStatusCode = s.AbstractStatusCode,
                           StatusDate = h.CreatedDate,
                           LastExportDate = a.LastExportDate,
                           EvaluationID = h.EvaluationId,
                           KappaType = KappaTypeEnum.K1,
                           IsParent = true

                           
                       };

            List<AbstractListRow> abstracts = data.ToList();
            List<AbstractListRow> finalabstracts = null;
              
            switch (query)
            {
                case "default":
                    finalabstracts = abstracts.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N || q.AbstractStatusID == (int) AbstractStatusEnum.ODP_CONSENSUS_WITH_NOTES_2N ||
                        q.AbstractStatusID == (int)AbstractStatusEnum.CLOSED_3 || q.AbstractStatusID == (int)AbstractStatusEnum.DATA_EXPORTED_4).Select(s => s).ToList();
                    break;
                case "codercompleted":
                    finalabstracts = abstracts.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N).Select(s => s).ToList();
                    break;
                case "odpcompleted":
                    finalabstracts = abstracts.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.ODP_CONSENSUS_WITH_NOTES_2N).Select(s => s).ToList();
                    break;
                case "closed":
                    finalabstracts = abstracts.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.CLOSED_3).Select(s => s).ToList();
                    break;
                case "exported":
                    finalabstracts = abstracts.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.DATA_EXPORTED_4).Select(s => s).ToList();
                    break;
                default:
                    finalabstracts = abstracts.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N || q.AbstractStatusID == (int)AbstractStatusEnum.ODP_CONSENSUS_WITH_NOTES_2N ||
                        q.AbstractStatusID == (int)AbstractStatusEnum.CLOSED_3 || q.AbstractStatusID == (int)AbstractStatusEnum.DATA_EXPORTED_4).Select(s => s).ToList();
                    break;
            }



            return AbstractListViewHelper.SortAbstracts(finalabstracts, sort, direction);
        }
        protected List<AbstractListRow> GetParentAbstractsODPStaffMemberReview(string sort = "", SortDirection direction = SortDirection.Ascending)
        {
            string connString = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString;
            DataJYDataContext db = new DataJYDataContext(connString);

            var data = from a in db.Abstracts
                       join h in db.AbstractStatusChangeHistories on a.AbstractID equals h.AbstractID
                       join s in db.AbstractStatus on h.AbstractStatusID equals s.AbstractStatusID
                       join rv in db.AbstractReviewLists on a.AbstractID equals rv.AbstractID
                       where (
                          (h.AbstractStatusID == (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N || h.AbstractStatusID == (int)AbstractStatusEnum.ODP_CONSENSUS_WITH_NOTES_2N ||  h.AbstractStatusID == (int)AbstractStatusEnum.CLOSED_3
                          || h.AbstractStatusID == (int)AbstractStatusEnum.ODP_STAFF_AND_CODER_CONSENSUS_2C ||  h.AbstractStatusID == (int)AbstractStatusEnum.DATA_EXPORTED_4) 
                          &&
                          h.AbstractStatusChangeHistoryID == db.AbstractStatusChangeHistories
                           .Where(h2 => h2.AbstractID == a.AbstractID)
                           .Select(h2 => h2.AbstractStatusChangeHistoryID).Max()
                           )
                       select new AbstractListRow
                       {
                           AbstractID = a.AbstractID,
                           ProjectTitle = a.ProjectTitle + " (" + s.AbstractStatusCode + ")",
                           PIProjectLeader = a.PIProjectLeader,
                           ApplicationID = a.ApplicationID,
                           AbstractStatusID = s.AbstractStatusID,
                           AbstractStatusCode = s.AbstractStatusCode,
                           StatusDate = h.CreatedDate,
                           LastExportDate = a.LastExportDate,
                           EvaluationID = h.EvaluationId,
                           KappaType = KappaTypeEnum.K1,
                           IsParent = true
                       };

            List<AbstractListRow> abstracts = data.ToList();


            return AbstractListViewHelper.SortAbstracts(abstracts, sort, direction);
        }

        protected List<AbstractListRow> GetParentAbstractsCoderSupervisor(string query = "", string sort = "", SortDirection direction = SortDirection.Ascending)
        {
            string connString = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString;
            DataJYDataContext db = new DataJYDataContext(connString);

            var data = from a in db.Abstracts
                       join h in db.AbstractStatusChangeHistories on a.AbstractID equals h.AbstractID
                       join s in db.AbstractStatus on h.AbstractStatusID equals s.AbstractStatusID
                       where (
                          //(h.AbstractStatusID == (int)AbstractStatusEnum.RETRIEVED_FOR_CODING_1 ||
                          //h.AbstractStatusID == (int)AbstractStatusEnum.CODED_BY_CODER_1A) &&
                          h.AbstractStatusChangeHistoryID == db.AbstractStatusChangeHistories
                           .Where(h2 => h2.AbstractID == a.AbstractID)
                           .Select(h2 => h2.AbstractStatusChangeHistoryID).Max()
                           )
                       select new AbstractListRow
                       {
                           

                           AbstractID = a.AbstractID,
                           ProjectTitle = a.ProjectTitle + " (" + s.AbstractStatusCode + ")",
                           PIProjectLeader = a.PIProjectLeader,
                           ApplicationID = a.ApplicationID,
                           AbstractStatusID = s.AbstractStatusID,
                           AbstractStatusCode = s.AbstractStatusCode,
                           StatusDate = h.CreatedDate,
                           LastExportDate = a.LastExportDate,
                           EvaluationID = h.EvaluationId,
                           KappaType = KappaTypeEnum.K1,
                           IsParent = true
                       };

            List<AbstractListRow> abstracts = data.ToList();
            List<AbstractListRow> finalabstracts = null;
            //public enum AbstractStatusEnum
            //      {
            //          OPEN_0 = 1,
            //          RETRIEVED_FOR_CODING_1 = 2,
            //          CODED_BY_CODER_1A = 3,
            //          CONSENSUS_COMPLETE_1B = 4,
            //          CONSENSUS_COMPLETE_WITH_NOTES_1N = 6,
            //          RETRIEVED_FOR_ODP_CODING_2 = 7,
            //          CODED_BY_ODP_STAFF_2A = 8,
            //          ODP_STAFF_CONSENSUS_2B = 9,
            //          ODP_STAFF_AND_CODER_CONSENSUS_2C = 10,
            //          ODP_CONSENSUS_WITH_NOTES_2N = 12,
            //          CLOSED_3 = 13,
            //          DATA_EXPORTED_4 = 14,
            //          REOPEN_FOR_REVIEW_BY_ODP = 15
            //      }
            switch (query)
            {
                case "default":
                    finalabstracts = abstracts.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N || q.AbstractStatusID == (int)AbstractStatusEnum.CONSENSUS_COMPLETE_1B ||
                        q.AbstractStatusID == (int)AbstractStatusEnum.ODP_CONSENSUS_WITH_NOTES_2N || q.AbstractStatusID == (int)AbstractStatusEnum.CLOSED_3 || q.AbstractStatusID == (int)AbstractStatusEnum.DATA_EXPORTED_4).Select(s => s).ToList();
                    break;
                case "codercompleted":
                    finalabstracts = abstracts.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N).Select(s => s).ToList();
                    break;
                case "activeabstracts":
                    finalabstracts = abstracts.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.RETRIEVED_FOR_CODING_1 || q.AbstractStatusID == (int)AbstractStatusEnum.CODED_BY_CODER_1A).Select(s => s).ToList();
                    break;
                case "odpcompleted":
                    finalabstracts = abstracts.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.ODP_CONSENSUS_WITH_NOTES_2N).Select(s => s).ToList();
                    break;
                case "codercompletedwonotes":
                    finalabstracts = abstracts.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.CONSENSUS_COMPLETE_1B).Select(s => s).ToList();
                    break;
                case "closed":
                    finalabstracts = abstracts.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.CLOSED_3).Select(s => s).ToList();
                    break;
                case "exported":
                    finalabstracts = abstracts.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.DATA_EXPORTED_4).Select(s => s).ToList();
                    break;
                default:
                    finalabstracts = abstracts.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N || q.AbstractStatusID == (int)AbstractStatusEnum.CONSENSUS_COMPLETE_1B ||
                        q.AbstractStatusID == (int)AbstractStatusEnum.ODP_CONSENSUS_WITH_NOTES_2N || q.AbstractStatusID == (int)AbstractStatusEnum.CLOSED_3 || q.AbstractStatusID == (int)AbstractStatusEnum.DATA_EXPORTED_4).Select(s => s).ToList();
                    break;


            }


            return AbstractListViewHelper.SortAbstracts(finalabstracts, sort, direction);
        }



        //protected List<AbstractListRow> GetParentAbstractsODPSupervisorDefault(string sort = "", SortDirection direction = SortDirection.Ascending)
        //{
        //    string connStr = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString;
        //    DataJYDataContext db = new DataJYDataContext(connStr);

        //    //System.Diagnostics.Stopwatch objStopWatch = new System.Diagnostics.Stopwatch();
        //    //objStopWatch.Start();

        //    var data = from a in db.Abstracts
        //               join h in db.AbstractStatusChangeHistories on a.AbstractID equals h.AbstractID
        //               join s in db.AbstractStatus on h.AbstractStatusID equals s.AbstractStatusID
        //               where (
        //                  h.AbstractStatusID >= (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N &&
        //                  h.AbstractStatusChangeHistoryID == db.AbstractStatusChangeHistories
        //                   .Where(h2 => h2.AbstractID == a.AbstractID)
        //                   .Select(h2 => h2.AbstractStatusChangeHistoryID).Max()
        //                   )
        //               select new AbstractListRow
        //               {
        //                   AbstractID = a.AbstractID,
        //                   ProjectTitle = a.ProjectTitle + " (" + s.AbstractStatusCode + ")",
        //                   ApplicationID = a.ApplicationID,
        //                   AbstractStatusID = s.AbstractStatusID,
        //                   AbstractStatusCode = s.AbstractStatusCode,
        //                   StatusDate = h.CreatedDate,
        //                   EvaluationID = h.EvaluationId,
        //                   IsParent = true
        //               };

        //    //var genSQL = db.GetCommand(data).CommandText;
        //    //System.Diagnostics.Trace.WriteLine(genSQL);

           

        //    List<AbstractListRow> abstracts = data.ToList();

        //    //objStopWatch.Stop();
        //    //System.Diagnostics.Trace.Write("The time taken to execute query without compilation is : " +
        //    //objStopWatch.ElapsedMilliseconds.ToString() + " MillionSeconds<br>");
        //    //objStopWatch.Reset();
           
        //    //db.Log = Console.Out;

        //    //if (AbstractViewGridView.Attributes["CurrentSortExp"] != null)
        //    //{
        //    //    sort = AbstractViewGridView.Attributes["CurrentSortExp"];
        //    //    direction = AbstractViewGridView.Attributes["CurrentSortDir"] == "ASC" ? SortDirection.Ascending : SortDirection.Descending;
        //    //}

        //    return AbstractListViewHelper.SortAbstracts(abstracts, sort, direction);
        //}
        
        //protected List<AbstractListRow> GetParentAbstractsODPSupervisorOpen(string sort = "", SortDirection direction = SortDirection.Ascending)
        //{
        //    string connStr = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString;
        //    DataJYDataContext db = new DataJYDataContext(connStr);

        //    var data = from a in db.Abstracts
        //               join h in db.AbstractStatusChangeHistories on a.AbstractID equals h.AbstractID
        //               join s in db.AbstractStatus on h.AbstractStatusID equals s.AbstractStatusID
        //               where (
        //                  (h.AbstractStatusID == (int)AbstractStatusEnum.RETRIEVED_FOR_ODP_CODING_2 ||
        //                  h.AbstractStatusID == (int)AbstractStatusEnum.CODED_BY_ODP_STAFF_2A) &&
        //                  h.AbstractStatusChangeHistoryID == db.AbstractStatusChangeHistories
        //                   .Where(h2 => h2.AbstractID == a.AbstractID)
        //                   .Select(h2 => h2.AbstractStatusChangeHistoryID).Max()
        //                   )
        //               select new AbstractListRow
        //               {
        //                   AbstractID = a.AbstractID,
        //                   ProjectTitle = a.ProjectTitle + " (" + s.AbstractStatusCode + ")",
        //                   ApplicationID = a.ApplicationID,
        //                   AbstractStatusID = s.AbstractStatusID,
        //                   AbstractStatusCode = s.AbstractStatusCode,
        //                   StatusDate = h.CreatedDate,
        //                   EvaluationID = h.EvaluationId,
        //                   KappaType = KappaTypeEnum.K1,
        //                   IsParent = true
        //               };

        //    List<AbstractListRow> abstracts = data.ToList();

        //    //if (AbstractViewGridView.Attributes["CurrentSortExp"] != null)
        //    //{
        //    //    sort = AbstractViewGridView.Attributes["CurrentSortExp"];
        //    //    direction = AbstractViewGridView.Attributes["CurrentSortDir"] == "ASC" ? SortDirection.Ascending : SortDirection.Descending;
        //    //}

        //    return AbstractListViewHelper.SortAbstracts(abstracts, sort, direction);
        //}
        //protected List<AbstractListRow> GetParentAbstractsODPSupervisorReviewUnCoded(string sort = "", SortDirection direction = SortDirection.Ascending)
        //{
        //    string connStr = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString;
        //    DataJYDataContext db = new DataJYDataContext(connStr);

        //    var data = from a in db.Abstracts
        //               join h in db.AbstractStatusChangeHistories on a.AbstractID equals h.AbstractID
        //               join s in db.AbstractStatus on h.AbstractStatusID equals s.AbstractStatusID
        //               join rv in db.AbstractReviewLists on a.AbstractID equals rv.AbstractID
        //               where (
        //                  h.AbstractStatusID == (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N &&
        //                  h.AbstractStatusChangeHistoryID == db.AbstractStatusChangeHistories
        //                   .Where(h2 => h2.AbstractID == a.AbstractID)
        //                   .Select(h2 => h2.AbstractStatusChangeHistoryID).Max()
        //                   )
        //               select new AbstractListRow
        //               {
        //                   AbstractID = a.AbstractID,
        //                   ProjectTitle = a.ProjectTitle + " (" + s.AbstractStatusCode + ")",
        //                   ApplicationID = a.ApplicationID,
        //                   AbstractStatusID = s.AbstractStatusID,
        //                   AbstractStatusCode = s.AbstractStatusCode,
        //                   StatusDate = h.CreatedDate,
        //                   EvaluationID = h.EvaluationId,
        //                   KappaType = KappaTypeEnum.K1,
        //                   IsParent = true
        //               };

        //    List<AbstractListRow> abstracts = data.ToList();

        //    //if (AbstractViewGridView.Attributes["CurrentSortExp"] != null)
        //    //{
        //    //    sort = AbstractViewGridView.Attributes["CurrentSortExp"];
        //    //    direction = AbstractViewGridView.Attributes["CurrentSortDir"] == "ASC" ? SortDirection.Ascending : SortDirection.Descending;
        //    //}

        //    return AbstractListViewHelper.SortAbstracts(abstracts, sort, direction);
        //}

        protected List<AbstractListRow> GetParentAbstractsODPStaffMemberDefaultOLD(string sort = "", SortDirection direction = SortDirection.Ascending)
        {
            string connString = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString;
            DataJYDataContext db = new DataJYDataContext(connString);

            var data = from a in db.Abstracts
                       join h in db.AbstractStatusChangeHistories on a.AbstractID equals h.AbstractID
                       join s in db.AbstractStatus on h.AbstractStatusID equals s.AbstractStatusID
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

            List<AbstractListRow> abstracts = data.ToList();

            //if (AbstractViewGridView.Attributes["CurrentSortExp"] != null)
            //{
            //    sort = AbstractViewGridView.Attributes["CurrentSortExp"];
            //    direction = AbstractViewGridView.Attributes["CurrentSortDir"] == "ASC" ? SortDirection.Ascending : SortDirection.Descending;
            //}

            return AbstractListViewHelper.SortAbstracts(abstracts, sort, direction);
        }
        protected List<AbstractListRow> GetParentAbstractsODPStaffMemberReviewOLD(string sort = "", SortDirection direction = SortDirection.Ascending)
        {
            string connString = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString;
            DataJYDataContext db = new DataJYDataContext(connString);

            var data = from a in db.Abstracts
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

            List<AbstractListRow> abstracts = data.ToList();

            
            return AbstractListViewHelper.SortAbstracts(abstracts, sort, direction);
        }
        protected List<AbstractListRow> GetParentAbstractsODPStaffMemberUncodedOLD(string sort = "", SortDirection direction = SortDirection.Ascending)
        {
            string connStr = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString;
            DataJYDataContext db = new DataJYDataContext(connStr);

            var data = from a in db.Abstracts
                       join h in db.AbstractStatusChangeHistories on a.AbstractID equals h.AbstractID
                       join s in db.AbstractStatus on h.AbstractStatusID equals s.AbstractStatusID
                       join rv in db.AbstractReviewLists on a.AbstractID equals rv.AbstractID
                       where (
                          h.AbstractStatusID == (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N &&
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

            List<AbstractListRow> abstracts = data.ToList();


            return AbstractListViewHelper.SortAbstracts(abstracts, sort, direction);
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