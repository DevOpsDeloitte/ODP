﻿using System;
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
    public partial class ODPSupervisorView_Review_Uncoded : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Visible)
                return;

            AbstractViewGridView.Sorting += new GridViewSortEventHandler(this.AbstractSortHandler);
            AbstractViewGridView.RowCreated += new GridViewRowEventHandler(AbstractListViewHelper.AbstractListRowCreatedHandler);
            AbstractViewGridView.RowDataBound += new GridViewRowEventHandler(AbstractListViewHelper.AbstractListRowBindingHandler);

            try
            {
                if (!IsPostBack)
                {
                    var parentAbstracts = GetParentAbstracts();

                    AbstractViewGridView.DataSource = AbstractListViewHelper.ProcessAbstracts(parentAbstracts, AbstractViewRole.ODPSupervisor);
                    AbstractViewGridView.DataBind();
                }
            }
            catch (Exception exp)
            {
                Utils.LogError(exp);
            }
        }

        protected List<AbstractListRow> GetParentAbstracts(string sort = "", SortDirection direction = SortDirection.Ascending)
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

            if (AbstractViewGridView.Attributes["CurrentSortExp"] != null)
            {
                sort = AbstractViewGridView.Attributes["CurrentSortExp"];
                direction = AbstractViewGridView.Attributes["CurrentSortDir"] == "ASC" ? SortDirection.Ascending : SortDirection.Descending;
            }

            return AbstractListViewHelper.SortAbstracts(abstracts, sort, direction);
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

            var abstracts = GetParentAbstracts(SortExpression, SortDirection);

            AbstractViewGridView.DataSource = AbstractListViewHelper.ProcessAbstracts(abstracts, AbstractViewRole.ODPSupervisor);
            AbstractViewGridView.DataBind();
        }

        protected void RemoveFromReviewHandler(object sender, EventArgs e)
        {
            AbstractListViewData data = new AbstractListViewData();

            foreach (GridViewRow row in AbstractViewGridView.Rows)
            {
                CheckBox Review = row.FindControl("Review") as CheckBox;
                HiddenField AbstractIDField = row.FindControl("AbstractID") as HiddenField;
                int AbstractID = 0;

                if (Review != null && Review.Checked &&
                    AbstractIDField != null && int.TryParse(AbstractIDField.Value, out AbstractID))
                {
                    data.RemoveAbstractFromReview(AbstractID);
                }
            }

            var parentAbstracts = GetParentAbstracts();

            AbstractViewGridView.DataSource = AbstractListViewHelper.ProcessAbstracts(parentAbstracts, AbstractViewRole.ODPSupervisor);
            AbstractViewGridView.DataBind();
        }
    }
}