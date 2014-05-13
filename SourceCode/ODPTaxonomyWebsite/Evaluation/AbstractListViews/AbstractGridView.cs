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
            this.AllowSorting = true;

            this.RowStyle.CssClass = "data-row";

            this.PageIndexChanging += new GridViewPageEventHandler(this.PageIndexChangingHandler);

            this.EmptyDataText = "No Abstracts";

            // determine pager size from cookie
            this.PageSize = 25;
            if (HttpContext.Current.Request.Cookies["Pager"] != null)
            {
                int TempPagerSize;

                if (int.TryParse(HttpContext.Current.Request.Cookies["Pager"]["Size"].ToString(), out TempPagerSize))
                {
                    switch (TempPagerSize)
                    {
                        case 25:
                        case 50:
                        case 100:
                            this.PageSize = TempPagerSize;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        protected void PageIndexChangingHandler(object sender, GridViewPageEventArgs e)
        {
            this.PageIndex = e.NewPageIndex;
            this.DataBind();
        }
    }
}