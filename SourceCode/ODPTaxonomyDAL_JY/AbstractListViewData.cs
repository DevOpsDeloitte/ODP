using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ODPTaxonomyDAL_JY
{
    public class AbstractListViewData
    {
        private DataJYDataContext db;

        public AbstractListViewData(string ConnStr)
        {
            db = new DataJYDataContext(ConnStr);
        }

        public List<AbstractListRow> GetCoderEvaluations_1A(int ParentAbstractID)
        {
            var coderEvaluations = (from a in db.Abstracts
                                    /* get status */
                                    join h in db.AbstractStatusChangeHistories on a.AbstractID equals h.AbstractID
                                    join s in db.AbstractStatus on h.AbstractStatusID equals s.AbstractStatusID
                                    join ev in db.Evaluations on h.EvaluationId equals ev.EvaluationId
                                    join sb in db.Submissions on h.EvaluationId equals sb.EvaluationId
                                    join u in db.aspnet_Users on sb.UserId equals u.UserId
                                    where (
                                        // Parent abstract
                                        a.AbstractID == ParentAbstractID &&
                                        // 1A Coded by coder
                                        h.AbstractStatusID == (int)AbstractStatusEnum.CODED_BY_CODER_1A &&
                                        // Make sure this evaluation is coder's evaluation, not ODP's
                                        ev.EvaluationTypeId == (int)EvaluationTypeEnum.CODER_EVALUATION &&
                                        // Make sure this submission is coder evaluation
                                        sb.SubmissionTypeId == (int)SubmissionTypeEnum.CODER_EVALUATION
                                    )
                                    orderby sb.SubmissionDateTime descending
                                    select new AbstractListRow
                                    {
                                        AbstractID = a.AbstractID,
                                        ProjectTitle = u.UserFirstName + " " + u.UserLastName,
                                        ApplicationID = null,
                                        AbstractStatusID = s.AbstractStatusID,
                                        AbstractStatusCode = s.AbstractStatusCode,
                                        StatusDate = sb.SubmissionDateTime,
                                        SubmissionID = sb.SubmissionID,
                                        EvaluationID = h.EvaluationId,
                                        Comment = sb.comments
                                    }).Take(3).ToList();

            return coderEvaluations;
        }

        public List<AbstractListRow> GetODPStaffConsensus_2B(int ParentAbstractID)
        {
            var odpStaffConsensus = (from a in db.Abstracts
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
                                         sb.SubmissionTypeId == (int)SubmissionTypeEnum.CODER_CONSENSUS
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
                                         Comment = sb.comments
                                     }).ToList();

            return odpStaffConsensus;
        }
        
        public List<AbstractListRow> GetODPStaffAndCoderConsensus_2C(int ParentAbstractID)
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
                                              sb.SubmissionTypeId == (int)SubmissionTypeEnum.CODER_CONSENSUS
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
                                              Comment = sb.comments
                                          }).ToList();

            return odpStaffCoderConsensus;
        }

        public List<AbstractListRow> GetODPConsensusWithNotes_2N(int ParentAbstractID)
        {
            var odpConsensus = (from a in db.Abstracts
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
                                              ev.EvaluationTypeId == (int)EvaluationTypeEnum.ODP_EVALUATION &&
                                              // Make sure this submission is coder evaluation
                                              sb.SubmissionTypeId == (int)SubmissionTypeEnum.CODER_CONSENSUS
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
                                              Comment = sb.comments
                                          }).ToList();

            return odpConsensus;
        }
    }
}
