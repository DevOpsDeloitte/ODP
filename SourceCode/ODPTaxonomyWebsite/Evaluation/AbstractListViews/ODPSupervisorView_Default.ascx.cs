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
    public partial class ODPSupervisorView_Default : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // bind gridview sort event
            AbstractViewGridView.Sorting += new GridViewSortEventHandler(this.AbstractSortHandler);

            try
            {
                var parentAbstracts = GetTableData();

                AbstractViewGridView.DataSource = ProcessTableData(parentAbstracts);
                AbstractViewGridView.DataBind();
            }
            catch (Exception exp)
            {
                Utils.LogError(exp);
            }
        }

        protected List<AbstractListRow> GetTableData(string sort = "Date", SortDirection direction = SortDirection.Ascending)
        {
            DataJYDataContext db = new DataJYDataContext();

            var data = from a in db.Abstracts
                       /* get status */
                       join h in db.AbstractStatusChangeHistories on a.AbstractID equals h.AbstractID
                       join s in db.AbstractStatus on h.AbstractStatusID equals s.AbstractStatusID
                       join ev in db.Evaluations on h.EvaluationId equals ev.EvaluationId
                       join sb in db.Submissions on h.EvaluationId equals sb.EvaluationId
                       join scn in db.AbstractScans on h.EvaluationId equals scn.EvaluationId into evscn
                       from scn in evscn.DefaultIfEmpty()
                       where (
                          h.AbstractStatusID >= (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N &&
                          h.CreatedDate == db.AbstractStatusChangeHistories
                           .Where(h2 => h2.AbstractID == a.AbstractID)
                           .Select(h2 => h2.CreatedDate).Max() &&
                          ev.EvaluationTypeId == (int)EvaluationTypeEnum.ODP_EVALUATION &&
                          sb.SubmissionTypeId == (int)SubmissionTypeEnum.ODP_STAFF_CONSENSUS
                           )
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
                           AbstractScan = scn.FileName,
                           UnableToCode = sb.UnableToCode,
                           IsParent = true
                       };

            switch (sort)
            {
                case "ApplicationID":
                    if (direction == SortDirection.Ascending)
                    {
                        return data.OrderBy(d => d.ApplicationID).ToList();
                    }
                    else
                    {
                        return data.OrderByDescending(d => d.ApplicationID).ToList();
                    }
                case "Title":
                    if (direction == SortDirection.Ascending)
                    {
                        return data.OrderBy(d => d.ProjectTitle).ToList();
                    }
                    else
                    {
                        return data.OrderByDescending(d => d.ProjectTitle).ToList();
                    }
                case "Date":
                default:
                    if (direction == SortDirection.Ascending)
                    {
                        return data.OrderBy(d => d.StatusDate).ToList();
                    }
                    else
                    {
                        return data.OrderByDescending(d => d.StatusDate).ToList();
                    }
            }
        }

        protected List<AbstractListRow> ProcessTableData(List<AbstractListRow> ParentAbstracts)
        {
            AbstractListViewData data = new AbstractListViewData();

            List<AbstractListRow> abstracts = new List<AbstractListRow>();

            for (int i = 0; i < ParentAbstracts.Count; i++)
            {
                abstracts.Add(ParentAbstracts[i]);

                if (ParentAbstracts[i].IsParent)
                {
                    // Inserts Coder Evaluation rows, latest 3 only
                    var coderEvaluations = data.GetCoderEvaluations_1A(ParentAbstracts[i].AbstractID);
                    abstracts.AddRange(coderEvaluations);

                    // Insert ODP Evaluation rows, latest 3 only
                    var odpEvaluations = data.GetODPStaffEvaluations_2A(ParentAbstracts[i].AbstractID);
                    abstracts.AddRange(odpEvaluations);

                    // ODP Staff consensus row
                    var odpStaffConsensus = data.GetODPStaffConsensus_2B(ParentAbstracts[i].AbstractID);
                    abstracts.AddRange(odpStaffConsensus);

                    // ODP Staff vs Coder consensus row
                    var odpStaffCoderConsensus = data.GetODPStaffAndCoderConsensus_2C(ParentAbstracts[i].AbstractID);
                    abstracts.AddRange(odpStaffCoderConsensus);
                }
            }

            foreach (AbstractListRow abs in abstracts)
            {
                abs.FillKappaValues();
            }

            return abstracts;
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

        protected void AbstractSortHandler(object sender, GridViewSortEventArgs e)
        {
            string SortExpression = e.SortExpression;
            SortDirection SortDirection = e.SortDirection;

            if (AbstractViewGridView.Attributes["CurrentSortExp"] != null)
            {
                if (AbstractViewGridView.Attributes["CurrentSortExp"] == SortExpression)
                {
                    SortDirection = AbstractViewGridView.Attributes["CurrentSortDir"] == "ASC" ? SortDirection.Descending : SortDirection.Ascending;
                    AbstractViewGridView.Attributes["CurrentSortDir"] = SortDirection == SortDirection.Ascending ? "ASC" : "DESC";
                }
                else
                {
                    SortDirection = SortDirection.Ascending;
                    AbstractViewGridView.Attributes["CurrentSortExp"] = e.SortExpression;
                }
            }
            else
            {
                AbstractViewGridView.Attributes["CurrentSortExp"] = e.SortExpression;
                AbstractViewGridView.Attributes["CurrentSortDir"] = e.SortDirection == SortDirection.Ascending ? "ASC" : "DESC";
            }

            var abstracts = GetTableData(SortExpression, SortDirection);

            AbstractViewGridView.DataSource = ProcessTableData(abstracts);
            AbstractViewGridView.DataBind();
        }
    }
}