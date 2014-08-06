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
    public partial class ManageAccounts : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    if ((Session["AM_PageIndex"] != null) && (!string.IsNullOrEmpty(Session["AM_PageIndex"].ToString())))
                    {
                        gvw_users.PageIndex = (int)Session["AM_PageIndex"];
                    }

                    BindData();

                }
                catch (Exception ex)
                {
                    HandlePageError("System error occurred.  Please contact system administrator");
                    Utils.LogError(ex);
                }
            }
        }

        protected void BindData()
        {
            string l_sortBy;
            string l_sortDirection;

            if (Session["AM_SortExpression"] == null)
            {
                l_sortBy = null;
                l_sortDirection = null;

            }
            else
            {
                l_sortBy = Session["AM_SortExpression"].ToString();
                l_sortDirection = Session["AM_SortDirection"].ToString();

            }
            BindData(l_sortBy, l_sortDirection);
        }

        protected void BindData(string sortBy, string sortDirection)
        {
            using (AccountDataLinqDataContext db = new AccountDataLinqDataContext(AccountDAL.connString))
            {
                gvw_users.DataSource = db.select_users(sortBy, sortDirection);
                gvw_users.DataBind();
            }
        }

        protected string GetSortDirection(string column)
        {
            string l_sortDirection = "ASC";

            if (Session["AM_SortExpression"] != null)
            {
                string l_prevSortExpression = Session["AM_SortExpression"].ToString();

                // check if same column
                if (l_prevSortExpression == column)
                {
                    string l_prevSortDirection = Session["AM_SortDirection"].ToString();
                    if ((l_prevSortDirection != null) && (l_prevSortDirection.ToUpper() == "ASC"))
                    {
                        l_sortDirection = "DESC";
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
            string l_roleDesc = "";
            foreach (string role in Roles.GetRolesForUser(userName))
            {
                switch (role)
                {
                    case "CoderSupervisor":
                        l_roleDesc = "Coder Supervisor";
                        break;
                    case "ODPStaffMember":
                        l_roleDesc = "ODP Staff";
                        break;
                    case "ODPStaffSupervisor":
                        l_roleDesc = "ODP Supervisor";
                        break;
                    default:
                        l_roleDesc = role;
                        break;

                }
                l_roleList += l_roleDesc + "|";
            }

            return l_roleList.TrimEnd('|').Replace("|", "<br />").ToString();

        }

        protected void gvw_users_OnSorting(object sender, GridViewSortEventArgs e)
        {
            string l_sortExpression = e.SortExpression;
            string l_sortDirection;

            if (GetSortDirection(l_sortExpression) == "ASC")
            {
                l_sortDirection = "ASC";
            }
            else
            {
                l_sortDirection = "DESC";
            }

            BindData(l_sortExpression, l_sortDirection);
        }

        protected void gvw_users_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Session["AM_PageIndex"] = e.NewPageIndex;
            gvw_users.PageIndex = e.NewPageIndex;
            BindData();
        }

        protected void gvw_users_OnRowEditing(object sender, GridViewEditEventArgs e)
        {
            Session["AM_Action"] = "EDIT";
            Session["AM_UserName"] = gvw_users.Rows[e.NewEditIndex].Cells[0].Text;
            Response.Redirect("EditAccount.aspx");
        }

        protected void lnkbtn_editUser_OnClick(Object sender, EventArgs e)
        {
            Session["AM_Action"] = "EDIT";
            Session["AM_UserName"] = ((LinkButton)sender).CommandArgument.ToString();
            Response.Redirect("EditAccount.aspx");
        }

        protected void lnkbtn_createAccount_OnClick(object sender, EventArgs e)
        {
            Session["AM_Action"] = "ADD";
            Response.Redirect("EditAccount.aspx");
        }

        protected void HandlePageError(string message)
        {
            lbl_error_message.Visible = true;
            lbl_error_message.Text = message;
        }
    }
}