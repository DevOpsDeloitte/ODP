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
        public string action = "";
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
            action = context.Request["action"] ?? "";

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
                            abstractData = GetParentAbstractsODPSupervisorReportExclude(filter, action, searchString, start, length, sortCol, sortDir);
                            parentAbstracts = abstractData.data;

                            ALR = AbstractListViewHelper.ProcessAbstracts2(parentAbstracts, AbstractViewRole.ODPSupervisor);
                            abstractData.data = ALR;
                            abstractData.draw = draw;

                            serializeResponse(context, abstractData);
                            break;

                        case "review":
                        case "reviewuncoded":
                            abstractData = GetParentAbstractsODPSupervisorReview(filter, action, searchString, start, length, sortCol, sortDir);
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
                            abstractData = GetParentAbstractsODPSupervisor(filter, action, searchString, start, length, sortCol, sortDir); // we are passing the filter as the query.
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
                    action = string.Empty;
                    abstractData = this.GetParentAbstractsCoderSupervisor(filter, action, searchString, start, length, sortCol, sortDir);
                    parentAbstracts = abstractData.data;

                    ALR = AbstractListViewHelper.ProcessAbstracts2(parentAbstracts, AbstractViewRole.CoderSupervisor);
                    abstractData.draw = draw;
                    abstractData.data = ALR;
                    serializeResponse(context, abstractData);

                    break;

                case "ODPStaff":
                    action = string.Empty;
                    switch (filter)
                    {
                        case "": // first call default is review list.

                            abstractData = this.GetParentAbstractsODPStaffMemberReview("review", action, searchString, start, length, sortCol, sortDir);
                            parentAbstracts = abstractData.data;

                            ALR = AbstractListViewHelper.ProcessAbstracts2(parentAbstracts, AbstractViewRole.ODPStaff);
                            abstractData.data = ALR;
                            abstractData.draw = draw;

                            serializeResponse(context, abstractData);
                            break;

                        case "review":
                        case "reviewuncoded":
                            abstractData = this.GetParentAbstractsODPStaffMemberReview(filter, action, searchString, start, length, sortCol, sortDir);
                            parentAbstracts = abstractData.data;

                            ALR = AbstractListViewHelper.ProcessAbstracts2(parentAbstracts, AbstractViewRole.ODPStaff);
                            abstractData.data = ALR;
                            abstractData.draw = draw;

                            serializeResponse(context, abstractData);
                            break;

                        default: // return default
                            abstractData = this.GetParentAbstractsODPStaffMember(filter, action, searchString, start, length, sortCol, sortDir);
                            parentAbstracts = abstractData.data;

                            ALR = AbstractListViewHelper.ProcessAbstracts2(parentAbstracts, AbstractViewRole.ODPStaff);
                            abstractData.data = ALR;
                            abstractData.draw = draw;

                            serializeResponse(context, abstractData);

                            break;
                    }
                    break;
                case "Admin":
                    action = string.Empty;
                    abstractData = this.GetParentAbstractsAdmin(filter, action, searchString, start, length, sortCol, sortDir);
                    parentAbstracts = abstractData.data;

                    ALR = AbstractListViewHelper.ProcessAbstracts2(parentAbstracts, AbstractViewRole.Admin);
                    abstractData.draw = draw;
                    abstractData.data = ALR;

                    serializeResponse(context, abstractData);
                    break;
                default:
                    break;
            }

            return;
        }

        protected AbstractData GetParentAbstractsAdmin(string filter = "", string action = "", string search = "", int start = 0, int length = 10, string sort = "", SortDirection direction = SortDirection.Ascending)
        {
            string connString = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString;
            DataJYDataContext db = new DataJYDataContext(connString);

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

            switch (filter)
            {
                case "uncoded":
                    query = query.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.OPEN_0);
                    break;
                case "default":
                    query = query.Where(q => q.AbstractStatusID >= (int)AbstractStatusEnum.RETRIEVED_FOR_CODING_1);
                    break;
                case "codercompleted":
                    query = query.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N);
                    break;

                case "odpcompleted":
                    query = query.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.ODP_CONSENSUS_WITH_NOTES_2N);
                    break;

                case "closed":
                    query = query.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.CLOSED_3);
                    break;
                case "exported":
                    query = query.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.DATA_EXPORTED_4);
                    break;
                default: // Note the default here is uncoded!
                    query = query.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.OPEN_0);
                    break;
            }

            return ProcessAbstracts(query.ToList(), action, search, start, length, sort, direction);
        }

        protected AbstractData GetParentAbstractsODPSupervisor(string filter = "", string action = "", string search = "", int start = 0, int length = 10, string sort = "", SortDirection direction = SortDirection.Ascending)
        {
            string connStr = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString;
            DataJYDataContext db = new DataJYDataContext(connStr);

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

            return ProcessAbstracts(query.ToList(), action, search, start, length, sort, direction);
        }

        protected AbstractData GetParentAbstractsODPSupervisorReview(string filter = "", string action = "", string search = "", int start = 0, int length = 10, string sort = "", SortDirection direction = SortDirection.Ascending)
        {
            string connString = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString;
            DataJYDataContext db = new DataJYDataContext(connString);

            var query = from a in db.Abstracts
                        join h in db.AbstractStatusChangeHistories on a.AbstractID equals h.AbstractID
                        join s in db.AbstractStatus on h.AbstractStatusID equals s.AbstractStatusID
                        join rv in db.AbstractReviewLists on a.AbstractID equals rv.AbstractID
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

            switch (filter)
            {
                case "review":
                    query = query.Where(q => q.AbstractStatusID >= (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N);
                    break;
                case "reviewuncoded":
                    query = query.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N);
                    break;
            }

            return ProcessAbstracts(query.ToList(), action, search, start, length, sort, direction);
        }

        protected AbstractData GetParentAbstractsODPSupervisorReportExclude(string filter = "", string action = "", string search = "", int start = 0, int length = 10, string sort = "", SortDirection direction = SortDirection.Ascending)
        {
            string connString = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString;
            DataJYDataContext db = new DataJYDataContext(connString);

            var query = from a in db.Abstracts
                        join h in db.AbstractStatusChangeHistories on a.AbstractID equals h.AbstractID
                        join s in db.AbstractStatus on h.AbstractStatusID equals s.AbstractStatusID
                        join rv in db.Report_AbstractExcludedLists on a.AbstractID equals rv.AbstractID
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

            switch (filter)
            {
                case "reportexclude":
                    query = query.Where(q => q.AbstractStatusID >= (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N);
                    break;

            }

            return ProcessAbstracts(query.ToList(), action, search, start, length, sort, direction);
        }

        protected AbstractData GetParentAbstractsODPStaffMember(string filter = "", string action = "", string search = "", int start = 0, int length = 10, string sort = "", SortDirection direction = SortDirection.Ascending)
        {
            string connString = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString;
            DataJYDataContext db = new DataJYDataContext(connString);

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
                            KappaType = KappaTypeEnum.K1,
                            IsParent = true


                        };

            switch (filter)
            {
                case "default":
                    query = query.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N || q.AbstractStatusID == (int)AbstractStatusEnum.ODP_CONSENSUS_WITH_NOTES_2N ||
                        q.AbstractStatusID == (int)AbstractStatusEnum.CLOSED_3 || q.AbstractStatusID == (int)AbstractStatusEnum.DATA_EXPORTED_4 ||
                        q.AbstractStatusID == (int)AbstractStatusEnum.RETRIEVED_FOR_ODP_CODING_2 || q.AbstractStatusID == (int)AbstractStatusEnum.CODED_BY_ODP_STAFF_2A || q.AbstractStatusID == (int)AbstractStatusEnum.ODP_STAFF_CONSENSUS_2B || q.AbstractStatusID == (int)AbstractStatusEnum.ODP_STAFF_AND_CODER_CONSENSUS_2C);
                    break;
                case "codercompleted":
                    query = query.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N);
                    break;
                case "odpcompleted":
                    query = query.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.ODP_CONSENSUS_WITH_NOTES_2N);
                    break;
                case "closed":
                    query = query.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.CLOSED_3);
                    break;
                case "exported":
                    query = query.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.DATA_EXPORTED_4);
                    break;
                default:
                    query = query.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N || q.AbstractStatusID == (int)AbstractStatusEnum.ODP_CONSENSUS_WITH_NOTES_2N ||
                        q.AbstractStatusID == (int)AbstractStatusEnum.CLOSED_3 || q.AbstractStatusID == (int)AbstractStatusEnum.DATA_EXPORTED_4);
                    break;
            }

            return ProcessAbstracts(query.ToList(), action, search, start, length, sort, direction);
        }

        protected AbstractData GetParentAbstractsODPStaffMemberReview(string filter = "", string action = "", string search = "", int start = 0, int length = 10, string sort = "", SortDirection direction = SortDirection.Ascending)
        {
            string connString = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString;
            DataJYDataContext db = new DataJYDataContext(connString);

            var query = from a in db.Abstracts
                        join h in db.AbstractStatusChangeHistories on a.AbstractID equals h.AbstractID
                        join s in db.AbstractStatus on h.AbstractStatusID equals s.AbstractStatusID
                        join rv in db.AbstractReviewLists on a.AbstractID equals rv.AbstractID
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

            switch (filter)
            {
                case "review":
                    query = query.Where(q =>
                        q.AbstractStatusID == (int)AbstractStatusEnum.RETRIEVED_FOR_ODP_CODING_2 ||
                        q.AbstractStatusID == (int)AbstractStatusEnum.CODED_BY_ODP_STAFF_2A ||
                        q.AbstractStatusID == (int)AbstractStatusEnum.ODP_STAFF_CONSENSUS_2B ||
                        q.AbstractStatusID == (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N ||
                        q.AbstractStatusID == (int)AbstractStatusEnum.ODP_CONSENSUS_WITH_NOTES_2N ||
                        q.AbstractStatusID == (int)AbstractStatusEnum.CLOSED_3 ||
                        q.AbstractStatusID == (int)AbstractStatusEnum.ODP_STAFF_AND_CODER_CONSENSUS_2C ||
                        q.AbstractStatusID == (int)AbstractStatusEnum.DATA_EXPORTED_4
                        );
                    break;
                case "reviewuncoded":
                    query = query.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N);
                    break;

            }

            return ProcessAbstracts(query.ToList(), action, search, start, length, sort, direction);
        }

        protected AbstractData GetParentAbstractsCoderSupervisor(string filter = "", string action = "", string search = "", int start = 0, int length = 10, string sort = "", SortDirection direction = SortDirection.Ascending)
        {
            string connString = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString;
            DataJYDataContext db = new DataJYDataContext(connString);

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
                            KappaType = KappaTypeEnum.K1,
                            IsParent = true
                        };

            switch (filter)
            {
                case "default":
                    query = query.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N || q.AbstractStatusID == (int)AbstractStatusEnum.CONSENSUS_COMPLETE_1B ||
                        q.AbstractStatusID == (int)AbstractStatusEnum.ODP_CONSENSUS_WITH_NOTES_2N || q.AbstractStatusID == (int)AbstractStatusEnum.CLOSED_3 || q.AbstractStatusID == (int)AbstractStatusEnum.DATA_EXPORTED_4);
                    break;
                case "codercompleted":
                    query = query.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N);
                    break;
                case "activeabstracts":
                    query = query.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.RETRIEVED_FOR_CODING_1 || q.AbstractStatusID == (int)AbstractStatusEnum.CODED_BY_CODER_1A);
                    break;
                case "odpcompleted":
                    query = query.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.ODP_CONSENSUS_WITH_NOTES_2N);
                    break;
                case "codercompletedwonotes":
                    query = query.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.CONSENSUS_COMPLETE_1B);
                    break;
                case "closed":
                    query = query.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.CLOSED_3);
                    break;
                case "exported":
                    query = query.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.DATA_EXPORTED_4);
                    break;
                default:
                    query = query.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N || q.AbstractStatusID == (int)AbstractStatusEnum.CONSENSUS_COMPLETE_1B ||
                        q.AbstractStatusID == (int)AbstractStatusEnum.ODP_CONSENSUS_WITH_NOTES_2N || q.AbstractStatusID == (int)AbstractStatusEnum.CLOSED_3 || q.AbstractStatusID == (int)AbstractStatusEnum.DATA_EXPORTED_4);
                    break;
            }

            return ProcessAbstracts(query.ToList(), action, search, start, length, sort, direction);
        }

        private AbstractData ProcessAbstracts(List<AbstractListRow> Abstracts, string action = "", string search = "", int start = 0, int length = 10, string sort = "", SortDirection direction = SortDirection.Ascending)
        {
            List<int> actionAbstracts = GetActionAbstracts(action);
            if (actionAbstracts.Count > 0)
            {
                Abstracts = Abstracts.Where(a => !actionAbstracts.Contains(a.AbstractID)).ToList();
            }

            int total = Abstracts.Count;
            if (!string.IsNullOrEmpty(search))
            {
                Abstracts = (from a in Abstracts
                             where
                                 a.AbstractID.ToString().Contains(search) ||
                                 a.ApplicationID.ToString().Contains(search) ||
                                 a.PIProjectLeader.ToLower().Contains(search.ToLower()) ||
                                 a.ProjectTitle.ToLower().Contains(search.ToLower())
                             select a).ToList();
            }

            Abstracts = AbstractListViewHelper.SortAbstracts(Abstracts, sort, direction);

            int totalFilter = Abstracts.Count();

            return new AbstractData()
            {
                draw = 1,
                recordsTotal = total,
                recordsFiltered = totalFilter,
                data = length == -1 ? Abstracts.Skip(start).ToList() : Abstracts.Skip(start).Take(length).ToList()
            };
        }

        private List<int> GetActionAbstracts(string action)
        {
            List<int> abstracts = new List<int>();
            string connStr = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString;

            using (DataJYDataContext db = new DataJYDataContext(connStr))
            {
                switch (action)
                {
                    case "addreview":
                        abstracts = db.AbstractReviewLists.Select(q => q.AbstractID).ToList();
                        break;
                    case "addreportexclude":
                        abstracts = db.Report_AbstractExcludedLists.Select(q => q.AbstractID).ToList();
                        break;
                    case "closeabstract":
                    case "exportabstracts":
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

                        if (action == "closeabstract")
                        {
                            abstracts = data.Where(q =>
                                q.AbstractStatusID != (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N &&
                                q.AbstractStatusID != (int)AbstractStatusEnum.ODP_CONSENSUS_WITH_NOTES_2N
                                ).Select(s => s.AbstractID).ToList();
                        }
                        else
                        {
                            abstracts = data.Where(q =>
                                q.AbstractStatusID != (int)AbstractStatusEnum.CLOSED_3
                                && q.AbstractStatusID != (int)AbstractStatusEnum.DATA_EXPORTED_4
                                ).Select(s => s.AbstractID).ToList();
                        }

                        break;
                    case "removereview":
                    default:
                        break;
                }
            }

            return abstracts;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        #region legacy

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

        #endregion
    }
}