using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ODPTaxonomyDAL_JY;
using ODPTaxonomyUtility_TT;
using System.Configuration;

namespace ODPTaxonomyWebsite.Evaluation.AbstractListViews
{
    public partial class CoderSupervisorView : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string connString = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString;
                DataJYDataContext db = new DataJYDataContext(connString);

                /**
                 * Grabs abstract and other related data
                 */
                var abstracts = (from a in db.Abstracts
                                 /* get status */
                                 join h in db.AbstractStatusChangeHistories on a.AbstractID equals h.AbstractID
                                 join s in db.AbstractStatus on h.AbstractStatusID equals s.AbstractStatusID
                                 join ev in db.Evaluations on h.EvaluationId equals ev.EvaluationId
                                 join sb in db.Submissions on h.EvaluationId equals sb.EvaluationId
                                 where (
                                     // 1N Consensus complete status with notes uploaded
                                    h.AbstractStatusID == (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N &&
                                     // Make sure the history is the latest one
                                    h.CreatedDate == db.AbstractStatusChangeHistories
                                     .Where(h2 => h2.AbstractID == a.AbstractID)
                                     .Select(h2 => h2.CreatedDate).Max() &&
                                     // Make sure this evaluation is coder's evaluation, not ODP's
                                    ev.EvaluationTypeId == (int)EvaluationTypeEnum.CODER_EVALUATION &&
                                     // Make sure this submission is coder consensus, NOT individual coder evaluation
                                    sb.SubmissionTypeId == (int)SubmissionTypeEnum.CODER_CONSENSUS
                                     )
                                 orderby h.CreatedDate descending
                                 orderby a.ProjectTitle ascending
                                 select new AbstractListView_CoderSupervisorModel
                                 {
                                     AbstractID = a.AbstractID,
                                     ProjectTitle = a.ProjectTitle,
                                     ApplicationID = a.ApplicationID,
                                     AbstractStatusID = s.AbstractStatusID,
                                     AbstractStatusCode = s.AbstractStatusCode,
                                     StatusDate = h.CreatedDate,
                                     SubmissionID = sb.SubmissionID,
                                     EvaluationID = h.EvaluationId,
                                     Comment = sb.comments
                                 }).ToList();

                for (int i = 0; i < abstracts.Count; i++)
                {
                    if (abstracts[i].AbstractStatusID == (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N &&
                        abstracts[i].EvaluationID != null)
                    {
                        // Inserts Coder Evaluation rows
                        var coderEvaluations = (from a in db.Abstracts
                                                /* get status */
                                                join h in db.AbstractStatusChangeHistories on a.AbstractID equals h.AbstractID
                                                join s in db.AbstractStatus on h.AbstractStatusID equals s.AbstractStatusID
                                                join ev in db.Evaluations on h.EvaluationId equals ev.EvaluationId
                                                join sb in db.Submissions on h.EvaluationId equals sb.EvaluationId
                                                join u in db.aspnet_Users on sb.UserId equals u.UserId
                                                where (
                                                    // Parent abstract
                                                    a.AbstractID == abstracts[i].AbstractID &&
                                                    // 1A Coded by coder
                                                    h.AbstractStatusID == (int)AbstractStatusEnum.CODED_BY_CODER &&
                                                    // Make sure this evaluation is coder's evaluation, not ODP's
                                                    ev.EvaluationTypeId == (int)EvaluationTypeEnum.CODER_EVALUATION &&
                                                    // Make sure this submission is coder consensus, NOT individual coder evaluation
                                                    sb.SubmissionTypeId == (int)SubmissionTypeEnum.CODER_EVALUATION
                                                )
                                                orderby sb.SubmissionDateTime descending
                                                select new AbstractListView_CoderSupervisorModel
                                                {
                                                    AbstractID = a.AbstractID,
                                                    ProjectTitle = u.UserFirstName + " " + u.UserLastName,
                                                    ApplicationID = a.ApplicationID,
                                                    AbstractStatusID = s.AbstractStatusID,
                                                    AbstractStatusCode = s.AbstractStatusCode,
                                                    StatusDate = sb.SubmissionDateTime,
                                                    SubmissionID = sb.SubmissionID,
                                                    EvaluationID = h.EvaluationId,
                                                    Comment = sb.comments
                                                }).ToList();

                        if (coderEvaluations != null)
                        {
                            abstracts.InsertRange(i+1, coderEvaluations);
                        }

                        // Insert Coder Consesus row
                        var coderConsensus = (from a in db.Abstracts
                                              /* get status */
                                              join h in db.AbstractStatusChangeHistories on a.AbstractID equals h.AbstractID
                                              join s in db.AbstractStatus on h.AbstractStatusID equals s.AbstractStatusID
                                              join ev in db.Evaluations on h.EvaluationId equals ev.EvaluationId
                                              join sb in db.Submissions on h.EvaluationId equals sb.EvaluationId
                                              join u in db.aspnet_Users on sb.UserId equals u.UserId
                                              where (
                                                  // Parent abstract
                                                  a.AbstractID == abstracts[i].AbstractID &&
                                                  // 1B Consensus complete status
                                                  h.AbstractStatusID == (int)AbstractStatusEnum.CONSENSUS_COMPLETE_1B &&
                                                  // Make sure this evaluation is coder's evaluation, not ODP's
                                                  ev.EvaluationTypeId == (int)EvaluationTypeEnum.CODER_EVALUATION &&
                                                  // Make sure this submission is coder consensus, NOT individual coder evaluation
                                                  sb.SubmissionTypeId == (int)SubmissionTypeEnum.CODER_EVALUATION
                                              )
                                              orderby sb.SubmissionDateTime descending
                                              select new AbstractListView_CoderSupervisorModel
                                              {
                                                  AbstractID = a.AbstractID,
                                                  ProjectTitle = u.UserFirstName + " " + u.UserLastName,
                                                  ApplicationID = a.ApplicationID,
                                                  AbstractStatusID = s.AbstractStatusID,
                                                  AbstractStatusCode = s.AbstractStatusCode,
                                                  StatusDate = sb.SubmissionDateTime,
                                                  SubmissionID = sb.SubmissionID,
                                                  EvaluationID = h.EvaluationId,
                                                  Comment = sb.comments
                                              }).ToList();
                    }
                }

                AbstractView.DataSource = abstracts;
                AbstractView.DataBind();
            }
            catch (Exception exp)
            {
                Utils.LogError(exp);
            }
        }
    }
}