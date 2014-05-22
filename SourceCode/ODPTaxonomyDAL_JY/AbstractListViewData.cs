using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace ODPTaxonomyDAL_JY
{
    public class AbstractListViewData
    {
        private DataJYDataContext db;

        public AbstractListViewData()
        {
            string connString = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString;
            db = new DataJYDataContext(connString);
        }

        public AbstractListViewData(string ConnStr)
        {
            db = new DataJYDataContext(ConnStr);
        }

        public List<AbstractListRow> GetCoderEvaluations_1A(int ParentAbstractID)
        {
            var query = from a in db.Abstracts
                        /* get status */
                        join h in db.AbstractStatusChangeHistories on a.AbstractID equals h.AbstractID
                        join s in db.AbstractStatus on h.AbstractStatusID equals s.AbstractStatusID
                        join ev in db.Evaluations on h.EvaluationId equals ev.EvaluationId
                        join sb in db.Submissions on h.EvaluationId equals sb.EvaluationId
                        join u in db.aspnet_Users on sb.UserId equals u.UserId
                        join ki in db.KappaUserIdentifies on
                            new { ev.TeamID, u.UserId } equals new { ki.TeamID, UserId = ki.UserId.Value }
                        where (
                            // Parent abstract
                            a.AbstractID == ParentAbstractID &&
                            // 1A Coded by coder
                            h.AbstractStatusID == (int)AbstractStatusEnum.CODED_BY_CODER_1A &&
                            // Make sure this evaluation is coder's evaluation, not ODP's
                            ev.EvaluationTypeId == (int)EvaluationTypeEnum.CODER_EVALUATION &&
                            // Make sure this submission is coder evaluation
                            sb.SubmissionTypeId == (int)SubmissionTypeEnum.CODER_EVALUATION &&
                            h.AbstractStatusChangeHistoryID == (
                                (from h2 in db.AbstractStatusChangeHistories
                                 where h2.AbstractID == a.AbstractID &&
                                 h2.AbstractStatusID == (int)AbstractStatusEnum.CODED_BY_CODER_1A
                                 select h2.AbstractStatusChangeHistoryID).Max()
                            )
                        )
                        orderby sb.SubmissionDateTime descending
                        select new AbstractListRow
                        {
                            AbstractID = a.AbstractID,
                            ProjectTitle = "Coder: " + u.UserName,
                            ApplicationID = null,
                            AbstractStatusID = s.AbstractStatusID,
                            AbstractStatusCode = s.AbstractStatusCode,
                            StatusDate = sb.SubmissionDateTime,
                            SubmissionID = sb.SubmissionID,
                            EvaluationID = h.EvaluationId,
                            Comment = sb.comments,
                            TeamID = ev.TeamID,
                            UserID = sb.UserId,
                            KappaCoderAlias = ki.UserAlias
                        };

            List<AbstractListRow> abstracts = query.Take(3).OrderBy(i => i.KappaCoderAlias).ToList();
            for (int i = (int)KappaTypeEnum.CODER_A_VS_CONSENSUS_K2, j = 0;
                i <= (int)KappaTypeEnum.CODER_C_VS_CONSENSUS_K4 && j < abstracts.Count;
                i++, j++)
            {
                abstracts[j].KappaType = (KappaTypeEnum)i;
            }

            return abstracts;
        }

        public List<AbstractListRow> GetODPStaffEvaluations_2A(int ParentAbstractID)
        {
            var query = from a in db.Abstracts
                        /* get status */
                        join h in db.AbstractStatusChangeHistories on a.AbstractID equals h.AbstractID
                        join s in db.AbstractStatus on h.AbstractStatusID equals s.AbstractStatusID
                        join ev in db.Evaluations on h.EvaluationId equals ev.EvaluationId
                        join sb in db.Submissions on h.EvaluationId equals sb.EvaluationId
                        join u in db.aspnet_Users on sb.UserId equals u.UserId
                        join ki in db.KappaUserIdentifies on
                            new { ev.TeamID, u.UserId } equals new { ki.TeamID, UserId = ki.UserId.Value }
                        where (
                            // Parent abstract
                            a.AbstractID == ParentAbstractID &&
                            // 2A Coded by odp staff
                            h.AbstractStatusID == (int)AbstractStatusEnum.CODED_BY_ODP_STAFF_2A &&
                            // Make sure this evaluation is odp's evaluation
                            ev.EvaluationTypeId == (int)EvaluationTypeEnum.ODP_EVALUATION &&
                            // Make sure this submission is coder evaluation
                            sb.SubmissionTypeId == (int)SubmissionTypeEnum.ODP_STAFF_EVALUATION &&
                            h.AbstractStatusChangeHistoryID == (
                                (from h2 in db.AbstractStatusChangeHistories
                                 where h2.AbstractID == a.AbstractID &&
                                 h2.AbstractStatusID == (int)AbstractStatusEnum.CODED_BY_ODP_STAFF_2A
                                 select h2.AbstractStatusChangeHistoryID).Max()
                            )
                        )
                        orderby sb.SubmissionDateTime descending
                        select new AbstractListRow
                        {
                            AbstractID = a.AbstractID,
                            ProjectTitle = "ODP Coder: " + u.UserName,
                            ApplicationID = null,
                            AbstractStatusID = s.AbstractStatusID,
                            AbstractStatusCode = s.AbstractStatusCode,
                            StatusDate = sb.SubmissionDateTime,
                            SubmissionID = sb.SubmissionID,
                            EvaluationID = h.EvaluationId,
                            Comment = sb.comments,
                            TeamID = ev.TeamID,
                            UserID = sb.UserId,
                            KappaCoderAlias = ki.UserAlias
                        };

            List<AbstractListRow> abstracts = query.Take(3).OrderBy(i => i.KappaCoderAlias).ToList();
            for (int i = (int)KappaTypeEnum.ODP_STAFF_A_VS_CONSENSUS_K6, j = 0;
                i <= (int)KappaTypeEnum.ODP_STAFF_C_VS_CONSENSUS_K8 && j < abstracts.Count;
                i++, j++)
            {
                abstracts[j].KappaType = (KappaTypeEnum)i;
            }
            return abstracts;
        }

        public List<AbstractListRow> GetODPStaffConsensus_2B(int ParentAbstractID, KappaTypeEnum KappaType)
        {
            var data = from a in db.Abstracts
                       /* get status */
                       join h in db.AbstractStatusChangeHistories on a.AbstractID equals h.AbstractID
                       join s in db.AbstractStatus on h.AbstractStatusID equals s.AbstractStatusID
                       join ev in db.Evaluations on h.EvaluationId equals ev.EvaluationId
                       join sb in db.Submissions on h.EvaluationId equals sb.EvaluationId
                       join u in db.aspnet_Users on sb.UserId equals u.UserId
                       where (
                           // Parent abstract
                           a.AbstractID == ParentAbstractID &&
                           // ODP Staff consensus
                           h.AbstractStatusID == (int)AbstractStatusEnum.ODP_STAFF_CONSENSUS_2B &&
                           // Make sure the history is the latest one
                           h.CreatedDate == db.AbstractStatusChangeHistories
                           .Where(h2 => h2.AbstractID == a.AbstractID &&
                               h2.AbstractStatusID == (int)AbstractStatusEnum.ODP_STAFF_CONSENSUS_2B)
                           .Select(h2 => h2.CreatedDate).Max() &&
                           // Make sure this evaluation is coder's evaluation, not ODP's
                           ev.EvaluationTypeId == (int)EvaluationTypeEnum.ODP_EVALUATION &&
                           // Make sure this submission is coder evaluation
                           sb.SubmissionTypeId == (int)SubmissionTypeEnum.ODP_STAFF_CONSENSUS
                       )
                       orderby sb.SubmissionDateTime descending
                       select new AbstractListRow
                       {
                           AbstractID = a.AbstractID,
                           ProjectTitle = "ODP Staff",
                           ApplicationID = null,
                           AbstractStatusID = s.AbstractStatusID,
                           AbstractStatusCode = s.AbstractStatusCode,
                           StatusDate = sb.SubmissionDateTime,
                           SubmissionID = sb.SubmissionID,
                           EvaluationID = h.EvaluationId,
                           Comment = sb.comments,
                           KappaType = KappaType
                       };

            return data.ToList();
        }

        public List<AbstractListRow> GetODPStaffAndCoderConsensus_2C(int ParentAbstractID, KappaTypeEnum KappaType)
        {
            var odpStaffCoderConsensus = (from a in db.Abstracts
                                          /* get status */
                                          join h in db.AbstractStatusChangeHistories on a.AbstractID equals h.AbstractID
                                          join s in db.AbstractStatus on h.AbstractStatusID equals s.AbstractStatusID
                                          join ev in db.Evaluations on h.EvaluationId equals ev.EvaluationId
                                          join sb in db.Submissions on h.EvaluationId equals sb.EvaluationId
                                          join u in db.aspnet_Users on sb.UserId equals u.UserId
                                          where (
                                              // Parent abstract
                                              a.AbstractID == ParentAbstractID &&
                                              // ODP Staff consensus
                                              h.AbstractStatusID == (int)AbstractStatusEnum.ODP_STAFF_AND_CODER_CONSENSUS_2C &&
                                              // Make sure the history is the latest one
                                              h.CreatedDate == db.AbstractStatusChangeHistories
                                              .Where(h2 => h2.AbstractID == a.AbstractID &&
                                                  h2.AbstractStatusID == (int)AbstractStatusEnum.ODP_STAFF_AND_CODER_CONSENSUS_2C)
                                              .Select(h2 => h2.CreatedDate).Max() &&
                                              // Make sure this evaluation is coder's evaluation, not ODP's
                                              ev.EvaluationTypeId == (int)EvaluationTypeEnum.ODP_EVALUATION &&
                                              // Make sure this submission is coder evaluation
                                              sb.SubmissionTypeId == (int)SubmissionTypeEnum.ODP_STAFF_CONSENSUS
                                          )
                                          orderby sb.SubmissionDateTime descending
                                          select new AbstractListRow
                                          {
                                              AbstractID = a.AbstractID,
                                              ProjectTitle = "ODP vs. Coder",
                                              ApplicationID = null,
                                              AbstractStatusID = s.AbstractStatusID,
                                              AbstractStatusCode = s.AbstractStatusCode,
                                              StatusDate = sb.SubmissionDateTime,
                                              SubmissionID = sb.SubmissionID,
                                              EvaluationID = h.EvaluationId,
                                              Comment = sb.comments,
                                              KappaType = KappaType
                                          }).ToList();

            return odpStaffCoderConsensus;
        }

        public List<AbstractListRow> GetODPConsensusWithNotes_2N(int ParentAbstractID, KappaTypeEnum KappaType)
        {
            var query = from a in db.Abstracts
                        /* get status */
                        join h in db.AbstractStatusChangeHistories on a.AbstractID equals h.AbstractID
                        join s in db.AbstractStatus on h.AbstractStatusID equals s.AbstractStatusID
                        join ev in db.Evaluations on h.EvaluationId equals ev.EvaluationId
                        join sb in db.Submissions on h.EvaluationId equals sb.EvaluationId
                        join u in db.aspnet_Users on sb.UserId equals u.UserId
                        where (
                            // Parent abstract
                            a.AbstractID == ParentAbstractID &&
                            // ODP consensus
                            h.AbstractStatusID == (int)AbstractStatusEnum.ODP_CONSENSUS_WITH_NOTES_2N &&
                            // Make sure the history is the latest one
                            h.CreatedDate == db.AbstractStatusChangeHistories
                            .Where(h2 => h2.AbstractID == a.AbstractID &&
                                h2.AbstractStatusID == (int)AbstractStatusEnum.ODP_CONSENSUS_WITH_NOTES_2N)
                            .Select(h2 => h2.CreatedDate).Max() &&
                            // Make sure this evaluation is coder's evaluation, not ODP's
                            ev.EvaluationTypeId == (int)EvaluationTypeEnum.CODER_EVALUATION &&
                            // Make sure this submission is coder evaluation
                            sb.SubmissionTypeId == (int)SubmissionTypeEnum.ODP_STAFF_CONSENSUS
                        )
                        orderby sb.SubmissionDateTime descending
                        select new AbstractListRow
                        {
                            AbstractID = a.AbstractID,
                            ProjectTitle = "ODP Staff",
                            ApplicationID = null,
                            AbstractStatusID = s.AbstractStatusID,
                            AbstractStatusCode = s.AbstractStatusCode,
                            StatusDate = sb.SubmissionDateTime,
                            SubmissionID = sb.SubmissionID,
                            EvaluationID = h.EvaluationId,
                            Comment = sb.comments,
                            KappaType = KappaType
                        };

            return query.ToList();
        }

        public KappaData GetKappaData(int AbstractID, KappaTypeEnum KappaType)
        {
            return (from k in db.KappaDatas
                    where k.AbstractID == AbstractID && k.KappaTypeID == (int)KappaType
                    select k).FirstOrDefault();
        }

        public string GetKappaCoderAlias(Guid UserID, int TeamID)
        {
            var query = from ku in db.KappaUserIdentifies
                        where ku.UserId == UserID && ku.TeamID == TeamID
                        select ku.UserAlias;

            return query.FirstOrDefault();
        }
    }
}
