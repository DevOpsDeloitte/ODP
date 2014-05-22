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
    public partial class AdminView : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // bind gridview sort event
            AbstractViewGridView.Sorting += new GridViewSortEventHandler(this.AbstractSortHandler);
            AbstractViewGridView.RowCreated += new GridViewRowEventHandler(this.AbstractListRowCreatedHandler);
            try
            {
                var parentAbstracts = GetTableData();

                AbstractViewGridView.DataSource = parentAbstracts;
                AbstractViewGridView.DataBind();
            }
            catch (Exception exp)
            {
                Utils.LogError(exp);
            }
        }

        protected List<AbstractListRow> GetTableData(string sort = "Date", SortDirection direction = SortDirection.Ascending)
        {
            string connString = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString;
            DataJYDataContext db = new DataJYDataContext(connString);

            var data = from a in db.Abstracts
                       /* get status */
                       join h in db.AbstractStatusChangeHistories on a.AbstractID equals h.AbstractID
                       join s in db.AbstractStatus on h.AbstractStatusID equals s.AbstractStatusID
                       join scn in db.AbstractScans on h.EvaluationId equals scn.EvaluationId into evscn
                       from scn in evscn.DefaultIfEmpty()
                       where (
                          h.CreatedDate == db.AbstractStatusChangeHistories
                           .Where(h2 => h2.AbstractID == a.AbstractID)
                           .Select(h2 => h2.CreatedDate).Max()
                           )
                       select new AbstractListRow
                       {
                           AbstractID = a.AbstractID,
                           ProjectTitle = a.ProjectTitle,
                           ApplicationID = a.ApplicationID,
                           AbstractStatusID = s.AbstractStatusID,
                           AbstractStatusCode = s.AbstractStatusCode,
                           StatusDate = h.CreatedDate,
                           EvaluationID = h.EvaluationId,
                           AbstractScan = scn.FileName,
                           IsParent = true
                       };

            List<AbstractListRow> abstracts = data.ToList();
            foreach (AbstractListRow abs in abstracts)
            {
                abs.FillKappaValues();
            }

            if (AbstractViewGridView.Attributes["CurrentSortExp"] != null)
            {
                sort = AbstractViewGridView.Attributes["CurrentSortExp"];
                direction = AbstractViewGridView.Attributes["CurrentSortDir"] == "ASC" ? SortDirection.Ascending : SortDirection.Descending;
            }

            switch (sort)
            {
                case "ApplicationID":
                    if (direction == SortDirection.Ascending)
                    {
                        return abstracts.OrderBy(d => d.ApplicationID).ToList();
                    }
                    else
                    {
                        return abstracts.OrderByDescending(d => d.ApplicationID).ToList();
                    }
                case "Title":
                    if (direction == SortDirection.Ascending)
                    {
                        return abstracts.OrderBy(d => d.ProjectTitle).ToList();
                    }
                    else
                    {
                        return abstracts.OrderByDescending(d => d.ProjectTitle).ToList();
                    }
                case "Date":
                default:
                    if (direction == SortDirection.Ascending)
                    {
                        return abstracts.OrderBy(d => d.StatusDate).ToList();
                    }
                    else
                    {
                        return abstracts.OrderByDescending(d => d.StatusDate).ToList();
                    }

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

            AbstractViewGridView.DataSource = abstracts;
            AbstractViewGridView.DataBind();
        }

        protected void AbstractListRowCreatedHandler(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                foreach (TableCell tc in e.Row.Cells)
                {
                    if (tc.HasControls())
                    {
                        if (AbstractViewGridView.Attributes["CurrentSortExp"] != null)
                        {
                            LinkButton headerLink = (LinkButton)tc.Controls[0];

                            if (AbstractViewGridView.Attributes["CurrentSortExp"] == headerLink.CommandArgument)
                            {
                                tc.CssClass += " sorted ";
                                tc.CssClass += AbstractViewGridView.Attributes["CurrentSortDir"] == "ASC" ? "ascending " : "descending ";
                            }

                            tc.CssClass = tc.CssClass.Trim();
                        }
                        else
                        {
                        }
                    }
                }
            }
        }
    }
}