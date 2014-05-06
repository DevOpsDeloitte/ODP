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
    public partial class CoderSupervisorView_Coded : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string connString = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString;
                DataJYDataContext db = new DataJYDataContext(connString);

                List<AbstractListRow> abstracts = new List<AbstractListRow>();

                /**
                 * Grabs abstract and other related data
                 */
                var parentAbstracts = (from a in db.Abstracts
                                       /* get status */
                                       join h in db.AbstractStatusChangeHistories on a.AbstractID equals h.AbstractID
                                       join s in db.AbstractStatus on h.AbstractStatusID equals s.AbstractStatusID
                                       join ev in db.Evaluations on h.EvaluationId equals ev.EvaluationId
                                       join sb in db.Submissions on h.EvaluationId equals sb.EvaluationId
                                       join scn in db.AbstractScans on h.EvaluationId equals scn.EvaluationId into evscn
                                       from scn in evscn.DefaultIfEmpty()
                                       where (
                                           // 1B or 1N Consensus complete status with/without notes uploaded
                                          (h.AbstractStatusID == (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N ||
                                          h.AbstractStatusID == (int)AbstractStatusEnum.CONSENSUS_COMPLETE_1B) &&
                                           // Make sure the history is the latest one
                                          h.CreatedDate == db.AbstractStatusChangeHistories
                                           .Where(h2 => h2.AbstractID == a.AbstractID && (
                                               h2.AbstractStatusID == (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N ||
                                               h2.AbstractStatusID == (int)AbstractStatusEnum.CONSENSUS_COMPLETE_1B
                                               ))
                                           .Select(h2 => h2.CreatedDate).Max() &&
                                           // Make sure this evaluation is coder's evaluation, not ODP's
                                          ev.EvaluationTypeId == (int)EvaluationTypeEnum.CODER_EVALUATION &&
                                           // Make sure this submission is coder consensus, NOT individual coder evaluation
                                          sb.SubmissionTypeId == (int)SubmissionTypeEnum.CODER_CONSENSUS
                                           )
                                       orderby h.CreatedDate descending
                                       orderby a.ProjectTitle ascending
                                       select new AbstractListRow
                                       {
                                           AbstractID = a.AbstractID,
                                           ProjectTitle = a.ProjectTitle,
                                           ApplicationID = a.ApplicationID,
                                           AbstractStatusID = s.AbstractStatusID,
                                           AbstractStatusCode = s.AbstractStatusCode,
                                           StatusDate = h.CreatedDate,
                                           SubmissionID = sb.SubmissionID,
                                           EvaluationID = h.EvaluationId,
                                           Comment = sb.comments,
                                           AbstractScan = scn.FileName
                                       }).ToList();

                for (int i = 0; i < parentAbstracts.Count; i++)
                {
                    abstracts.Add(parentAbstracts[i]);
                    var j = i;

                    if ((parentAbstracts[i].AbstractStatusID == (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N ||
                        parentAbstracts[i].AbstractStatusID == (int)AbstractStatusEnum.CONSENSUS_COMPLETE_1B) &&
                        parentAbstracts[i].EvaluationID != null)
                    {
                        // Inserts Coder Evaluation rows, latest 3 only
                        var coderEvaluations = (from a in db.Abstracts
                                                /* get status */
                                                join h in db.AbstractStatusChangeHistories on a.AbstractID equals h.AbstractID
                                                join s in db.AbstractStatus on h.AbstractStatusID equals s.AbstractStatusID
                                                join ev in db.Evaluations on h.EvaluationId equals ev.EvaluationId
                                                join sb in db.Submissions on h.EvaluationId equals sb.EvaluationId
                                                join u in db.aspnet_Users on sb.UserId equals u.UserId
                                                where (
                                                    // Parent abstract
                                                    a.AbstractID == parentAbstracts[i].AbstractID &&
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

                        abstracts.AddRange(coderEvaluations);

                        // ODP Staff consensus row
                        var odpStaffConsensus = (from a in db.Abstracts
                                                 /* get status */
                                                 join h in db.AbstractStatusChangeHistories on a.AbstractID equals h.AbstractID
                                                 join s in db.AbstractStatus on h.AbstractStatusID equals s.AbstractStatusID
                                                 join ev in db.Evaluations on h.EvaluationId equals ev.EvaluationId
                                                 join sb in db.Submissions on h.EvaluationId equals sb.EvaluationId
                                                 join u in db.aspnet_Users on sb.UserId equals u.UserId
                                                 where (
                                                     // Parent abstract
                                                     a.AbstractID == parentAbstracts[i].AbstractID &&
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

                        abstracts.AddRange(odpStaffConsensus);

                        // ODP Staff vs Coder consensus row
                        var odpStaffCoderConsensus = (from a in db.Abstracts
                                                      /* get status */
                                                      join h in db.AbstractStatusChangeHistories on a.AbstractID equals h.AbstractID
                                                      join s in db.AbstractStatus on h.AbstractStatusID equals s.AbstractStatusID
                                                      join ev in db.Evaluations on h.EvaluationId equals ev.EvaluationId
                                                      join sb in db.Submissions on h.EvaluationId equals sb.EvaluationId
                                                      join u in db.aspnet_Users on sb.UserId equals u.UserId
                                                      where (
                                                          // Parent abstract
                                                          a.AbstractID == parentAbstracts[i].AbstractID &&
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
                                                          ProjectTitle = "ODP Staff",
                                                          ApplicationID = null,
                                                          AbstractStatusID = s.AbstractStatusID,
                                                          AbstractStatusCode = s.AbstractStatusCode,
                                                          StatusDate = sb.SubmissionDateTime,
                                                          SubmissionID = sb.SubmissionID,
                                                          EvaluationID = h.EvaluationId,
                                                          Comment = sb.comments
                                                      }).ToList();

                        abstracts.AddRange(odpStaffCoderConsensus);
                    }
                }

                AbstractViewGridView.DataSource = abstracts;
                AbstractViewGridView.DataBind();
            }
            catch (Exception exp)
            {
                Utils.LogError(exp);
            }
        }

        protected void AbstractListRowBindingHandle(object sender, GridViewRowEventArgs e)
        {
            AbstractListRow item = e.Row.DataItem as AbstractListRow;
            HyperLink AbstractScanLink = e.Row.FindControl("AbstractScanLink") as HyperLink;

            if (item != null && AbstractScanLink != null)
            {
                if (!string.IsNullOrEmpty(item.AbstractScan))
                {
                    AbstractScanLink.Text = "Scanned File";
                    AbstractScanLink.ToolTip = item.AbstractScan;
                    AbstractScanLink.NavigateUrl = "#";
                    AbstractScanLink.Visible = true;
                }
                else
                {
                    AbstractScanLink.Visible = false;
                }
            }
        }
    }
}