using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ODPTaxonomyWebsite
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void HeadLoginStatus_OnLoggingOut(object sender, LoginCancelEventArgs e)
        {
            Session["AM_PageIndex"] = null;
            Session["AM_SortExpression"] = null;
            Session["AM_SortDirection"] = null;
            Session["AM_UserName"] = null;
            Session["AM_Action"] = null;
        }
    }
}
