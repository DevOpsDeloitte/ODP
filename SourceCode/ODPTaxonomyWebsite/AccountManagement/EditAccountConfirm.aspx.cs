using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ODPTaxonomyUtility_TT;
using ODPTaxonomyAccountDAL;

namespace ODPTaxonomyWebsite.AccountManagement
{
    public partial class EditAccountConfim : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                try
                {
                    if (Session["AM_Action"] == null)
                    {
                        //redirect to manage account page
                        Response.Redirect("ManageAccounts.aspx");
                    }
                    else
                    {
                        string l_action = Session["AM_Action"].ToString().ToUpper();
                        if (l_action == "EDIT")
                        {
                            lnkbtn_create_account.Visible = false;
                            ltl_message.Text = "Account Successfully Saved.";
                        }
                        string l_username = Session["AM_UserName"].ToString();

                        select_userByUserNameResult rec;

                        using (AccountDataLinqDataContext db = new AccountDataLinqDataContext(AccountDAL.connString))
                        {
                            rec = (select_userByUserNameResult)db.select_userByUserName(l_username).SingleOrDefault();
                        }

                        lbl_userName.Text = l_username;
                        lbl_name.Text = rec.UserFirstName + " " + rec.UserLastName;
                        lbl_email.Text = rec.Email;

                    }
                }
                catch (Exception ex)
                {
                    HandlePageError("Error saving account.");
                    Utils.LogError(ex.ToString());
                }
                
            }

        }

        protected void HandlePageError(string message)
        {
            lbl_error_message.Visible = true;
            lbl_error_message.Text = message;
        }

        protected void lnkbtn_createAccount_OnClick(Object sender, EventArgs e)
        {
            Session["AM_Action"] = "ADD";
            Response.Redirect("EditAccount.aspx");
        }

        protected void lnkbtn_manageAccount_OnClick(Object sender, EventArgs e)
        {
            Response.Redirect("ManageAccounts.aspx");
        }
    }
}