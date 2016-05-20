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


        protected void serializeResponse(HttpContext context, List<AbstractListRow> ALR)
        {
            IEnumerable<AbstractListRow> ALRX = ALR.Where(x => x.IsParent == true);
            context.Response.Write("{  \"data\" : " + JsonConvert.SerializeObject(ALRX.ToList()) + "  }");
            return;
        }

        protected void serializeResponse(HttpContext context, AbstractData data)
        {
            context.Response.Write(JsonConvert.SerializeObject(data));
        }

        public void ProcessRequest(HttpContext context)
        {
            List<AbstractListRow> parentAbstracts;
            List<AbstractListRow> ALR;
            AbstractData abstractData;

            roleRequested = context.Request["role"] ?? "";
            filter = context.Request["filter"] ?? "";
            int draw = context.Request["draw"] != null ? Convert.ToInt32(context.Request["draw"]) : 0;
            int start = context.Request["start"] != null ? Convert.ToInt32(context.Request["start"]) : 0;
            int length = context.Request["length"] != null ? Convert.ToInt32(context.Request["length"]) : 10;
            string searchString = context.Request["search[value]"];
            var sortColumnIndex = Convert.ToInt32(context.Request["order[0][column]"]);
            string sortCol = context.Request["columns[" + sortColumnIndex + "][data]"] ?? "StatusDate";
            SortDirection sortDir = context.Request["order[0][dir]"] == "desc" ? SortDirection.Descending : SortDirection.Ascending;

            AbstractListViewData data = new AbstractListViewData();
            switch (roleRequested)
            {

                case "ODPSupervisor":

                    switch (filter)
                    {
                        case "reportexclude":
                            abstractData = GetParentAbstractsODPSupervisorReportExclude(filter, searchString, start, length, sortCol, sortDir);
                            parentAbstracts = abstractData.data;

                            ALR = AbstractListViewHelper.ProcessAbstracts2(parentAbstracts, AbstractViewRole.ODPSupervisor);
                            abstractData.data = ALR;
                            abstractData.draw = draw;

                            serializeResponse(context, abstractData);
                            break;

                        case "review":
                        case "reviewuncoded":
                            abstractData = GetParentAbstractsODPSupervisorReview(filter, searchString, start, length, sortCol, sortDir);
                            parentAbstracts = abstractData.data;

                            foreach (var abs in parentAbstracts)
                            {

                                abs.InReview = true;

                            }
                            ALR = AbstractListViewHelper.ProcessAbstracts2(parentAbstracts, AbstractViewRole.ODPSupervisor);
                            abstractData.data = ALR;
                            abstractData.draw = draw;

                            serializeResponse(context, abstractData);

                            break;


                        default: // return default
                            abstractData = GetParentAbstractsODPSupervisor(filter, searchString, start, length, sortCol, sortDir); // we are passing the filter as the query.
                            parentAbstracts = abstractData.data;

                            var RL = data.GetAllAbstractsInReview();
                            foreach (var abs in parentAbstracts)
                            {
                                if (data.IsAbstractInReviewCache(abs.AbstractID, RL))
                                {
                                    abs.InReview = true;
                                }
                                else
                                {
                                    abs.InReview = false;
                                }
                            }
                            ALR = AbstractListViewHelper.ProcessAbstracts2(parentAbstracts, AbstractViewRole.ODPSupervisor);
                            abstractData.data = ALR;
                            abstractData.draw = draw;

                            serializeResponse(context, abstractData);
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



                        case "": // first call default is review list.

                            parentAbstracts = this.GetParentAbstractsODPStaffMemberReview("review");
                            ALR = AbstractListViewHelper.ProcessAbstracts2(parentAbstracts, AbstractViewRole.ODPStaff);
                            serializeResponse(context, ALR);
                            break;

                        case "review":
                        case "reviewuncoded":
                            parentAbstracts = this.GetParentAbstractsODPStaffMemberReview(filter);
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
                case "Admin":
                    parentAbstracts = this.GetParentAbstractsAdmin(filter);
                    ALR = AbstractListViewHelper.ProcessAbstracts2(parentAbstracts, AbstractViewRole.Admin);
                    serializeResponse(context, ALR);

                    break;

                case null:

                    break;

                default:

                    break;



            }


            return;
        }

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
                           ApplicationID = a.ChrApplicationID,
                           AbstractStatusID = s.AbstractStatusID,
                           AbstractStatusCode = s.AbstractStatusCode,
                           StatusDate = h.CreatedDate,
                           LastExportDate = a.LastExportDate,
                           EvaluationID = h.EvaluationId,
                           IsParent = true
                       };

            List<AbstractListRow> abstracts = data.ToList();
            List<AbstractListRow> finalabstracts = null;

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
                           ApplicationID = a.ChrApplicationID,
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
                           ApplicationID = a.ChrApplicationID,
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


        protected AbstractData GetParentAbstractsODPSupervisor(string filter = "", string search = "", int start = 0, int length = 10, string sort = "", SortDirection direction = SortDirection.Ascending)
        {
            string connStr = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString;
            DataJYDataContext db = new DataJYDataContext(connStr);
            int total = 0;

            var query = from a in db.Abstracts
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

            if (!string.IsNullOrEmpty(search))
            {
                query = from q in query
                        where q.AbstractID.ToString().Contains(search) ||
                        q.ApplicationID.ToString().Contains(search) ||
                        q.PIProjectLeader.Contains(search) ||
                        q.ProjectTitle.Contains(search)
                        select q;
            }

            query = AbstractListViewHelper.SortAbstracts(query, sort, direction);

            switch (filter)
            {
                case "default":
                    query = query.Where(q => q.AbstractStatusID >= (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N);
                    break;
                case "all":
                    query = query.Where(q => q.AbstractStatusID >= (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N);
                    break;
                case "codercompleted":
                    query = query.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N);
                    break;
                case "activeabstracts":
                    query = query.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.CODED_BY_ODP_STAFF_2A || q.AbstractStatusID == (int)AbstractStatusEnum.RETRIEVED_FOR_ODP_CODING_2);
                    break;
                case "odpcompleted":
                    query = query.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.ODP_CONSENSUS_WITH_NOTES_2N);
                    break;
                case "odpcompletedwonotes":
                    query = query.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.ODP_STAFF_AND_CODER_CONSENSUS_2C);
                    break;
                case "closed":
                    query = query.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.CLOSED_3);
                    break;
                case "exported":
                    query = query.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.DATA_EXPORTED_4);
                    break;
                default:
                    query = query.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.ODP_CONSENSUS_WITH_NOTES_2N);
                    break;
            }
            total = query.Count();

            return new AbstractData()
            {
                draw = 1,
                recordsTotal = total,
                recordsFiltered = total,
                data = query.Skip(start).Take(length).ToList()
            };
        }
        protected AbstractData GetParentAbstractsODPSupervisorReview(string filter = "", string search = "", int start = 0, int length = 10, string sort = "", SortDirection direction = SortDirection.Ascending)
        {
            string connString = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString;
            DataJYDataContext db = new DataJYDataContext(connString);
            int total = 0;

            var query = from a in db.Abstracts
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
                           PIProjectLeader = a.PIProjectLeader,
                           ApplicationID = a.ChrApplicationID,
                           AbstractStatusID = s.AbstractStatusID,
                           AbstractStatusCode = s.AbstractStatusCode,
                           StatusDate = h.CreatedDate,
                           LastExportDate = a.LastExportDate,
                           EvaluationID = h.EvaluationId,
                           KappaType = KappaTypeEnum.K1,
                           IsParent = true
                       };

            if (!string.IsNullOrEmpty(search))
            {
                query = from q in query
                        where q.AbstractID.ToString().Contains(search) ||
                        q.ApplicationID.ToString().Contains(search) ||
                        q.PIProjectLeader.Contains(search) ||
                        q.ProjectTitle.Contains(search)
                        select q;
            }

            query = AbstractListViewHelper.SortAbstracts(query, sort, direction);

            switch (filter)
            {
                case "review":
                    query = query.Where(q => q.AbstractStatusID >= (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N);
                    break;
                case "reviewuncoded":
                    query = query.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N);
                    break;
            }
            total = query.Count();

            return new AbstractData()
            {
                draw = 1,
                recordsTotal = total,
                recordsFiltered = total,
                data = query.Skip(start).Take(length).ToList()
            };
        }
        protected AbstractData GetParentAbstractsODPSupervisorReportExclude(string filter = "", string search = "", int start = 0, int length = 10, string sort = "", SortDirection direction = SortDirection.Ascending)
        {
            string connString = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString;
            DataJYDataContext db = new DataJYDataContext(connString);
            int total = 0;

            var query = from a in db.Abstracts
                       join h in db.AbstractStatusChangeHistories on a.AbstractID equals h.AbstractID
                       join s in db.AbstractStatus on h.AbstractStatusID equals s.AbstractStatusID
                       join rv in db.Report_AbstractExcludedLists on a.AbstractID equals rv.AbstractID
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
                           PIProjectLeader = a.PIProjectLeader,
                           ApplicationID = a.ChrApplicationID,
                           AbstractStatusID = s.AbstractStatusID,
                           AbstractStatusCode = s.AbstractStatusCode,
                           StatusDate = h.CreatedDate,
                           LastExportDate = a.LastExportDate,
                           EvaluationID = h.EvaluationId,
                           KappaType = KappaTypeEnum.K1,
                           IsParent = true
                       };

            if (!string.IsNullOrEmpty(search))
            {
                query = from q in query
                        where q.AbstractID.ToString().Contains(search) ||
                        q.ApplicationID.ToString().Contains(search) ||
                        q.PIProjectLeader.Contains(search) ||
                        q.ProjectTitle.Contains(search)
                        select q;
            }

            query = AbstractListViewHelper.SortAbstracts(query, sort, direction);

            switch (filter)
            {
                case "reportexclude":
                    query = query.Where(q => q.AbstractStatusID >= (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N);
                    break;

            }

            total = query.Count();

            return new AbstractData()
            {
                draw = 1,
                recordsTotal = total,
                recordsFiltered = total,
                data = query.Skip(start).Take(length).ToList()
            };
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
                           ApplicationID = a.ChrApplicationID,
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
                    finalabstracts = abstracts.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N || q.AbstractStatusID == (int)AbstractStatusEnum.ODP_CONSENSUS_WITH_NOTES_2N ||
                        q.AbstractStatusID == (int)AbstractStatusEnum.CLOSED_3 || q.AbstractStatusID == (int)AbstractStatusEnum.DATA_EXPORTED_4 ||
                        q.AbstractStatusID == (int)AbstractStatusEnum.RETRIEVED_FOR_ODP_CODING_2 || q.AbstractStatusID == (int)AbstractStatusEnum.CODED_BY_ODP_STAFF_2A || q.AbstractStatusID == (int)AbstractStatusEnum.ODP_STAFF_CONSENSUS_2B || q.AbstractStatusID == (int)AbstractStatusEnum.ODP_STAFF_AND_CODER_CONSENSUS_2C).Select(s => s).ToList();
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
        protected List<AbstractListRow> GetParentAbstractsODPStaffMemberReview(string query = "", string sort = "", SortDirection direction = SortDirection.Ascending)
        {
            string connString = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString;
            DataJYDataContext db = new DataJYDataContext(connString);

            var data = from a in db.Abstracts
                       join h in db.AbstractStatusChangeHistories on a.AbstractID equals h.AbstractID
                       join s in db.AbstractStatus on h.AbstractStatusID equals s.AbstractStatusID
                       join rv in db.AbstractReviewLists on a.AbstractID equals rv.AbstractID
                       where (
                          // (h.AbstractStatusID == (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N || h.AbstractStatusID == (int)AbstractStatusEnum.ODP_CONSENSUS_WITH_NOTES_2N ||  h.AbstractStatusID == (int)AbstractStatusEnum.CLOSED_3
                          // || h.AbstractStatusID == (int)AbstractStatusEnum.ODP_STAFF_AND_CODER_CONSENSUS_2C ||  h.AbstractStatusID == (int)AbstractStatusEnum.DATA_EXPORTED_4) 
                          // &&
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
                case "review":
                    finalabstracts = abstracts.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.RETRIEVED_FOR_ODP_CODING_2 || q.AbstractStatusID == (int)AbstractStatusEnum.CODED_BY_ODP_STAFF_2A || q.AbstractStatusID == (int)AbstractStatusEnum.ODP_STAFF_CONSENSUS_2B || q.AbstractStatusID == (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N || q.AbstractStatusID == (int)AbstractStatusEnum.ODP_CONSENSUS_WITH_NOTES_2N || q.AbstractStatusID == (int)AbstractStatusEnum.CLOSED_3
                          || q.AbstractStatusID == (int)AbstractStatusEnum.ODP_STAFF_AND_CODER_CONSENSUS_2C || q.AbstractStatusID == (int)AbstractStatusEnum.DATA_EXPORTED_4).Select(s => s).ToList();
                    break;
                case "reviewuncoded":
                    finalabstracts = abstracts.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N).Select(s => s).ToList();
                    //finalabstracts = abstracts.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.RETRIEVED_FOR_ODP_CODING_2 || q.AbstractStatusID == (int)AbstractStatusEnum.CODED_BY_ODP_STAFF_2A || q.AbstractStatusID == (int)AbstractStatusEnum.ODP_STAFF_CONSENSUS_2B).Select(s => s).ToList();
                    break;

            }


            return AbstractListViewHelper.SortAbstracts(finalabstracts, sort, direction);
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
                           ApplicationID = a.ChrApplicationID,
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
                           ApplicationID = a.ChrApplicationID,
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
                           ApplicationID = a.ChrApplicationID,
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
                           ApplicationID = a.ChrApplicationID,
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