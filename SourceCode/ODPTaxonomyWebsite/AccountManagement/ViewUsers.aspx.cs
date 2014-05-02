using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Linq.Expressions;
using ODPTaxonomyAccountDAL;
using ODPTaxonomyUtility_TT;

namespace ODPTaxonomyWebsite.AccountManagement
{
    public partial class ViewUsers : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    BindData(null, null);

                }
                catch (Exception ex)
                {
                    Utils.LogError(ex);
                }
            }
        }

        

        protected void BindData(string sortBy, string sortDirection)
        {
            using (AccountDataLinqDataContext db = new AccountDataLinqDataContext(AccountDAL.connString))
            {
                gvw_users.DataSource = db.select_users(sortBy, sortDirection);
                gvw_users.DataBind();
            }
        }

        protected SortDirection GetSortDirection(string column)
        {
            SortDirection l_sortDirection = SortDirection.Ascending;

            if (Session["AM_SortExpression"] != null)
            {
                string l_prevSortExpression = Session["AM_SortExpression"].ToString();

                // check if same column
                if (l_prevSortExpression == column)
                {
                    string l_prevSortDirection = Session["AM_SortDirection"].ToString();
                    if ((l_prevSortDirection != null) && (l_prevSortDirection.ToUpper() == "ASCENDING")){
                        l_sortDirection = SortDirection.Descending;
                    }
                }
            }

            Session["AM_SortDirection"] = l_sortDirection.ToString();
            Session["AM_SortExpression"] = column;

            return l_sortDirection;
        }
        
        protected string GetUserRoles(string userName)
        {
            string l_roleList = "";
            foreach (string role in Roles.GetRolesForUser(userName))
            {
                l_roleList += role + "|";
            }

            return l_roleList.TrimEnd('|').Replace("|", "<br />").ToString();
            
        }


        protected void gvw_users_OnSorting(object sender, GridViewSortEventArgs e)
        {
            string l_sortExpression = e.SortExpression;
            string l_sortDirection;

            if (GetSortDirection(l_sortExpression) == SortDirection.Ascending)
            {
                l_sortDirection = "ASC";
            }
            else
            {
                l_sortDirection = "DESC";
            }

            BindData(l_sortExpression, l_sortDirection);
        }

        

        //protected void gvw_users_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        //{

        //}

        protected void HandlePageError(string message)
        {
            lbl_error_message.Visible = true;
            lbl_error_message.Text = message;
        }
    }
}