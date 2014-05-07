using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace ODPTaxonomyWebsite.Evaluation.AbstractListViews
{
    public class AbstractGridView : GridView
    {
        public AbstractGridView()
            : base()
        {
            this.AutoGenerateColumns = false;
            this.AllowPaging = true;
            this.PageSize = 10;

            this.RowStyle.CssClass = "data-row";

            this.PageIndexChanging += new GridViewPageEventHandler(this.PageIndexChangingHandler);

            this.EmptyDataText = "No Abstracts";
        }

        protected void PageIndexChangingHandler(object sender, GridViewPageEventArgs e)
        {
            this.PageIndex = e.NewPageIndex;
            this.DataBind();
        }
    }
}