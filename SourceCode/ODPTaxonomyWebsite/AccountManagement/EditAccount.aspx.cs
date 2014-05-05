using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using ODPTaxonomyAccountDAL;
using ODPTaxonomyUtility_TT;

namespace ODPTaxonomyWebsite.AccountManagement
{
    public partial class EditAccount : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    LoadRoles();

                    if (Session["AM_Action"].ToString() == "EDIT")
                    {
                        LoadUserData();
                    }
                    else
                    {
                        
                    }
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex);
            }
        }

        protected void HandlePageError(string message)
        {
            lbl_error_message.Visible = true;
            lbl_error_message.Text = message;
        }

        protected void LoadUserData()
        {
            string l_username = Session["AM_UserName"].ToString();
            select_userByUserNameResult rec;
            using (AccountDataLinqDataContext db = new AccountDataLinqDataContext(AccountDAL.connString))
            {
                rec = (select_userByUserNameResult)db.select_userByUserName(l_username).SingleOrDefault();
            }

            txt_username.Text = rec.UserName;
            txt_firstName.Text = rec.UserFirstName;
            txt_lastName.Text = rec.UserLastName;
            txt_Email.Text = rec.Email;
            rdl_activeYN.SelectedValue = rec.IsApproved.ToString().ToUpper() == "TRUE" ? "1" : "0";

            // show roles for user
            string[] l_rolesForUser = Roles.GetRolesForUser(l_username);
            foreach (string role in l_rolesForUser)
            {
                ListItem li = cbl_roles.Items.FindByValue(role);
                if (li != null)
                {
                    li.Selected = true;
                }
            }
        }

        protected void LoadRoles()
        {
            using (AccountDataLinqDataContext db = new AccountDataLinqDataContext(AccountDAL.connString))
            {
                cbl_roles.DataSource = db.select_roles();
                cbl_roles.DataBind();                   
            }
        }

        protected void btn_cancel_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("ManageAccounts.aspx");
        }

        protected void btn_save_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid && isValidForm())
                {
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex);
            }
        }

        protected bool isValidForm()
        {
            bool l_isValid = true;

            //check password; if enter one, need to enter the other
            string l_newpassword = txt_new_password.Text;
            string l_confirmpassword = txt_confirm_password.Text;

            if ((!String.IsNullOrEmpty(l_newpassword)) && (String.IsNullOrEmpty(l_confirmpassword)))
            {
                CustomValidator cv = new CustomValidator();
                cv.IsValid = false;
                cv.ErrorMessage = "Confirm Password is required.";
                this.Page.Validators.Add(cv);
                l_isValid = false;
            }

            if ((String.IsNullOrEmpty(l_newpassword)) && (!String.IsNullOrEmpty(l_confirmpassword)))
            {
                CustomValidator cv = new CustomValidator();
                cv.IsValid = false;
                cv.ErrorMessage = "Password is required.";
                this.Page.Validators.Add(cv);
                l_isValid = false;
            }

            // check roles

            return l_isValid;
        }
    }
}