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
    public partial class ODPSupervisorView_Open : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // bind gridview sort event
            AbstractViewGridView.Sorting += new GridViewSortEventHandler(this.AbstractSortHandler);

            try
            {
                var parentAbstracts = GetParentAbstracts();

                AbstractViewGridView.DataSource = ProcessAbstracts(parentAbstracts);
                AbstractViewGridView.DataBind();
            }
            catch (Exception exp)
            {
                Utils.LogError(exp);
            }
        }

        protected List<AbstractListRow> GetParentAbstracts(string sort = "Date", SortDirection direction = SortDirection.Ascending)
        {
            string connStr = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString;
            DataJYDataContext db = new DataJYDataContext(connStr);

            var data = from a in db.Abstracts
                       join h in db.AbstractStatusChangeHistories on a.AbstractID equals h.AbstractID
                       join s in db.AbstractStatus on h.AbstractStatusID equals s.AbstractStatusID
                       where (
                          (h.AbstractStatusID == (int)AbstractStatusEnum.RETRIEVED_FOR_ODP_CODING_2 ||
                          h.AbstractStatusID == (int)AbstractStatusEnum.CODED_BY_ODP_STAFF_2A) &&
                          h.AbstractStatusChangeHistoryID == db.AbstractStatusChangeHistories
                           .Where(h2 => h2.AbstractID == a.AbstractID)
                           .Select(h2 => h2.AbstractStatusChangeHistoryID).Max()
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
                           KappaType = KappaTypeEnum.K1,
                           IsParent = true
                       };

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
        
        protected List<AbstractListRow> ProcessAbstracts(List<AbstractListRow> ParentAbstracts)
        {
            List<AbstractListRow> Abstracts = new List<AbstractListRow>();
            AbstractListViewData data = new AbstractListViewData();

            for (int i = 0; i < ParentAbstracts.Count; i++)
            {
                ParentAbstracts[i].GetComment();
                ParentAbstracts[i].GetAbstractScan();

                // gets all kappa data for abstract
                var KappaData = data.GetAbstractKappaData(ParentAbstracts[i].AbstractID);

                if (KappaData.Count() > 0)
                {
                    // fill in k1 value
                    Abstracts.Add(data.FillInKappaValue(ParentAbstracts[i], KappaData, KappaTypeEnum.K1));

                    // get coder evaluation row
                    // and fill in k2 - k4 value
                    var CoderEvaluation = data.GetCoderEvaluations(ParentAbstracts[i].AbstractID);
                    if (CoderEvaluation != null && CoderEvaluation.TeamID != null)
                    {
                        var CoderKapperIdentities = data.GetKappaIdentities(CoderEvaluation.TeamID.Value);
                        if (CoderKapperIdentities.Count() > 0)
                        {
                            foreach (var iden in CoderKapperIdentities)
                            {
                                foreach (var kappa in KappaData)
                                {
                                    if (iden.UserAlias == "CdrA" && kappa.KappaTypeID == (int)KappaTypeEnum.K2)
                                    {
                                        Abstracts.Add(data.ConstructNewAbstractListRow(kappa, "Coder: " + iden.UserName));
                                    }
                                    else if (iden.UserAlias == "CdrB" && kappa.KappaTypeID == (int)KappaTypeEnum.K3)
                                    {
                                        Abstracts.Add(data.ConstructNewAbstractListRow(kappa, "Coder: " + iden.UserName));
                                    }
                                    else if (iden.UserAlias == "CdrC" && kappa.KappaTypeID == (int)KappaTypeEnum.K4)
                                    {
                                        Abstracts.Add(data.ConstructNewAbstractListRow(kappa, "Coder: " + iden.UserName));
                                    }
                                }
                            }
                        }
                    }

                    // fill in k5 value
                    foreach (var kappa in KappaData)
                    {
                        if (kappa.KappaTypeID == (int)KappaTypeEnum.K5)
                        {
                            Abstracts.Add(data.ConstructNewAbstractListRow(kappa, "ODP Staff"));
                        }
                    }

                    // get odp staff evaluation row
                    // and fill in k6 - k8 value
                    var ODPEvaluation = data.GetODPEvaluations(ParentAbstracts[i].AbstractID);
                    if (ODPEvaluation != null && ODPEvaluation.TeamID != null)
                    {
                        var ODPCoderKapperIdentities = data.GetKappaIdentities(ODPEvaluation.TeamID.Value);
                        if (ODPCoderKapperIdentities.Count() > 0)
                        {
                            foreach (var iden in ODPCoderKapperIdentities)
                            {
                                foreach (var kappa in KappaData)
                                {
                                    if (iden.UserAlias == "ODPA" && kappa.KappaTypeID == (int)KappaTypeEnum.K6)
                                    {
                                        Abstracts.Add(data.ConstructNewAbstractListRow(kappa, "ODP Coder: " + iden.UserName));
                                    }
                                    else if (iden.UserAlias == "ODPB" && kappa.KappaTypeID == (int)KappaTypeEnum.K7)
                                    {
                                        Abstracts.Add(data.ConstructNewAbstractListRow(kappa, "ODP Coder: " + iden.UserName));
                                    }
                                    else if (iden.UserAlias == "ODPC" && kappa.KappaTypeID == (int)KappaTypeEnum.K8)
                                    {
                                        Abstracts.Add(data.ConstructNewAbstractListRow(kappa, "ODP Coder: " + iden.UserName));
                                    }
                                }
                            }
                        }
                    }

                    // fill in k9 value
                    foreach (var kappa in KappaData)
                    {
                        if (kappa.KappaTypeID == (int)KappaTypeEnum.K9)
                        {
                            Abstracts.Add(data.ConstructNewAbstractListRow(kappa, "ODP vs. Coder"));
                        }
                    }
                }
                else
                {
                    Abstracts.Add(ParentAbstracts[i]);
                }
            }

            return Abstracts;
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

            var abstracts = GetParentAbstracts(SortExpression, SortDirection);

            AbstractViewGridView.DataSource = ProcessAbstracts(abstracts);
            AbstractViewGridView.DataBind();
        }
    }
}