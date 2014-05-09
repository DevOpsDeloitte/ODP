using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using ODPTaxonomyDAL_JY;
using ODPTaxonomyUtility_TT;

namespace ODPTaxonomyWebsite.Evaluation.AbstractListViews
{
    public partial class ODPStaffMemberView_Review : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string connString = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString;
            DataJYDataContext db = new DataJYDataContext(connString);

            List<AbstractListRow> abstracts = new List<AbstractListRow>();

            try
            {
                /**
                 * Grabs abstract and other related data
                 */
                var parentAbstracts = (from a in db.Abstracts
                                       /* get status */
                                       join h in db.AbstractStatusChangeHistories on a.AbstractID equals h.AbstractID
                                       join s in db.AbstractStatus on h.AbstractStatusID equals s.AbstractStatusID
                                       join ev in db.Evaluations on h.EvaluationId equals ev.EvaluationId
                                       join sb in db.Submissions on h.EvaluationId equals sb.EvaluationId
                                       join rv in db.AbstractReviewLists on a.AbstractID equals rv.AbstractID
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
            Panel TitleWrapper = e.Row.FindControl("TitleWrapper") as Panel;
            HyperLink AbstractScanLink = e.Row.FindControl("AbstractScanLink") as HyperLink;

            if (item != null && TitleWrapper != null && AbstractScanLink != null)
            {
                if (!string.IsNullOrEmpty(item.AbstractScan))
                {
                    TitleWrapper.CssClass += " has-file";

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