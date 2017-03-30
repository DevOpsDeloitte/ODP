using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace ODPTaxonomyDAL_JY
{
    public static class AbstractHelper
    {
        public static AbstractData GetAbstracts(AbstractParams param)
        {
            List<AbstractListRow> parentAbstracts;
            List<AbstractListRow> ALR;
            AbstractListViewData data = new AbstractListViewData();
            AbstractData abstractData = null;

            switch (param.role)
            {
                case "ODPSupervisor":
                    switch (param.filter)
                    {
                        case "reportexclude":
                            abstractData = GetParentAbstractsODPSupervisorReportExclude(param);
                            parentAbstracts = abstractData.data;

                            ALR = AbstractListViewHelper.ProcessAbstracts2(parentAbstracts, AbstractViewRole.ODPSupervisor);
                            abstractData.data = ALR;
                            abstractData.draw = param.draw;
                            break;

                        case "review":
                        case "reviewuncoded":
                            abstractData = GetParentAbstractsODPSupervisorReview(param);
                            parentAbstracts = abstractData.data;

                            foreach (var abs in parentAbstracts)
                            {

                                abs.InReview = true;

                            }
                            ALR = AbstractListViewHelper.ProcessAbstracts2(parentAbstracts, AbstractViewRole.ODPSupervisor);
                            abstractData.data = ALR;
                            abstractData.draw = param.draw;
                            break;


                        default: // return default
                            abstractData = GetParentAbstractsODPSupervisor(param); // we are passing the filter as the query.
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
                            abstractData.draw = param.draw;
                            break;

                    }

                    break;
                case "CoderSupervisor":
                    param.action = string.Empty;
                    abstractData = GetParentAbstractsCoderSupervisor(param);
                    parentAbstracts = abstractData.data;

                    ALR = AbstractListViewHelper.ProcessAbstracts2(parentAbstracts, AbstractViewRole.CoderSupervisor);
                    abstractData.draw = param.draw;
                    abstractData.data = ALR;
                    break;

                case "ODPStaff":
                    param.action = string.Empty;
                    switch (param.filter)
                    {
                        case "": // first call default is review list.
                            param.filter = "review";
                            abstractData = GetParentAbstractsODPStaffMemberReview(param);
                            parentAbstracts = abstractData.data;

                            ALR = AbstractListViewHelper.ProcessAbstracts2(parentAbstracts, AbstractViewRole.ODPStaff);
                            abstractData.data = ALR;
                            abstractData.draw = param.draw;
                            break;

                        case "review":
                        case "reviewuncoded":
                            abstractData = GetParentAbstractsODPStaffMemberReview(param);
                            parentAbstracts = abstractData.data;

                            ALR = AbstractListViewHelper.ProcessAbstracts2(parentAbstracts, AbstractViewRole.ODPStaff);
                            abstractData.data = ALR;
                            abstractData.draw = param.draw;
                            break;

                        default: // return default
                            abstractData = GetParentAbstractsODPStaffMember(param);
                            parentAbstracts = abstractData.data;

                            ALR = AbstractListViewHelper.ProcessAbstracts2(parentAbstracts, AbstractViewRole.ODPStaff);
                            abstractData.data = ALR;
                            abstractData.draw = param.draw;
                            break;
                    }
                    break;
                case "Admin":
                    param.action = string.Empty;
                    abstractData = GetParentAbstractsAdmin(param);
                    parentAbstracts = abstractData.data;

                    ALR = AbstractListViewHelper.ProcessAbstracts2(parentAbstracts, AbstractViewRole.Admin);
                    abstractData.draw = param.draw;
                    abstractData.data = ALR;
                    break;
                default:
                    break;
            }

            return abstractData;
        }

        public static AbstractData GetParentAbstractsAdmin(AbstractParams param)
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
                            IsParent = true,
                            CodingType = a.CodingType
                        };

            switch (param.filter)
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

            return ProcessAbstracts(query.ToList(), param);
        }

        public static AbstractData GetParentAbstractsODPSupervisor(AbstractParams param)
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
                            IsParent = true,
                            CodingType = a.CodingType,
                            FY = a.FY,
                            Activity = a.Activity,
                            Type = a.Type
                        };

            switch (param.filter)
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

            return ProcessAbstracts(query.ToList(), param);
        }

        public static AbstractData GetParentAbstractsODPSupervisorReview(AbstractParams param)
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
                            IsParent = true,
                            CodingType = a.CodingType
                        };

            switch (param.filter)
            {
                case "review":
                    query = query.Where(q => q.AbstractStatusID >= (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N);
                    break;
                case "reviewuncoded":
                    query = query.Where(q => q.AbstractStatusID == (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N);
                    break;
            }

            return ProcessAbstracts(query.ToList(), param);
        }

        public static AbstractData GetParentAbstractsODPSupervisorReportExclude(AbstractParams param)
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
                            IsParent = true,
                            CodingType = a.CodingType
                        };

            switch (param.filter)
            {
                case "reportexclude":
                    query = query.Where(q => q.AbstractStatusID >= (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N);
                    break;

            }

            return ProcessAbstracts(query.ToList(), param);
        }

        public static AbstractData GetParentAbstractsODPStaffMember(AbstractParams param)
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
                            IsParent = true,
                            CodingType = a.CodingType
                        };

            switch (param.filter)
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

            return ProcessAbstracts(query.ToList(), param);
        }

        public static AbstractData GetParentAbstractsODPStaffMemberReview(AbstractParams param)
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
                            IsParent = true,
                            CodingType = a.CodingType
                        };

            switch (param.filter)
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

            return ProcessAbstracts(query.ToList(), param);
        }

        public static AbstractData GetParentAbstractsCoderSupervisor(AbstractParams param)
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
                            IsParent = true,
                            CodingType = a.CodingType
                        };

            switch (param.filter)
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

            return ProcessAbstracts(query.ToList(), param);
        }

        public static AbstractData ProcessAbstracts(List<AbstractListRow> Abstracts, AbstractParams param)
        {
            List<int> actionAbstracts = GetActionAbstracts(param.action);
            if (actionAbstracts.Count > 0)
            {
                Abstracts = Abstracts.Where(a => !actionAbstracts.Contains(a.AbstractID)).ToList();
            }

            switch (param.codeType.ToLower())
            {
                case "aonly":
                    Abstracts = Abstracts.Where(a => a.CodingType != "Basic").ToList();
                    break;

                case "basic":
                    Abstracts = Abstracts.Where(a => a.CodingType == "Basic").ToList();
                    break;

                case "all":
                default:

                    break;
            }

            int total = Abstracts.Count;
            if (!string.IsNullOrEmpty(param.search))
            {
                Abstracts = (from a in Abstracts
                             where
                                 a.AbstractID.ToString().Contains(param.search) ||
                                 a.ApplicationID.ToString().ToLower().Contains(param.search.ToLower()) ||
                                 a.PIProjectLeader.ToLower().Contains(param.search.ToLower()) ||
                                 a.ProjectTitle.ToLower().Contains(param.search.ToLower()) ||
                                 a.Activity.ToLower().Contains(param.search.ToLower()) ||
                                 a.Type.ToString().ToLower().Contains(param.search.ToLower()) ||
                                 a.FY.ToString().ToLower().Contains(param.search.ToLower())
                             select a).ToList();
            }

            Abstracts = AbstractListViewHelper.SortAbstracts(Abstracts, param.sortColumn, param.sortDirection);

            int totalFilter = Abstracts.Count();

            return new AbstractData()
            {
                draw = 1,
                recordsTotal = total,
                recordsFiltered = totalFilter,
                data = param.length == -1 ? Abstracts.Skip(param.start).ToList() : Abstracts.Skip(param.start).Take(param.length).ToList()
            };
        }

        public static List<int> GetActionAbstracts(string action)
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
    }
}
