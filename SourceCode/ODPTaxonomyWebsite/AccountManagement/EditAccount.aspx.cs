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
                    // if no action, redirect to manage account
                    if (Session["AM_Action"] == null)
                    {
                        Response.Redirect("~/AccountManagement/ManageAccounts.aspx");
                    }
                    else
                    {
                        string l_action = Session["AM_Action"].ToString().ToUpper();
                        ViewState["Action"] = l_action;

                        if (l_action == "ADD")
                        {
                            // add user; hide userid
                            ltl_page_title.Text = "Create Account";
                            txt_new_password.Attributes.Remove("placeholder");
                            txt_confirm_password.Attributes.Remove("placeholder");

                            pnl_username.Visible = false;
                            reqval_newPassword.Enabled = true;
                            reqval_NewPasswordConfirm.Enabled = true;
                            reqval_confirm_email.Enabled = true;

                            rdl_activeYN.Items.FindByValue("1").Selected = true;

                        }
                        else
                        {
                            // update user, load existing user data
                            LoadUserData();

                            reqval_newPassword.Enabled = false;
                            reqval_NewPasswordConfirm.Enabled = false;                           
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HandlePageError("Error occur creating/editing account.");
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
            ViewState["UserName"] = l_username;
            select_userByUserNameResult rec;

            using (AccountDataLinqDataContext db = new AccountDataLinqDataContext(AccountDAL.connString))
            {
                rec = (select_userByUserNameResult)db.select_userByUserName(l_username).SingleOrDefault();
            }

            txt_username.Text = rec.UserName;
            txt_firstName.Text = rec.UserFirstName;
            txt_lastName.Text = rec.UserLastName;
            txt_email.Text = rec.Email;
            txt_confirm_email.Text = rec.Email;
            ViewState["Email"] = rec.Email;

            string l_activeYN;
            l_activeYN = rec.IsApproved.ToString().ToUpper() == "TRUE" ? "1" : "0";
            ViewState["ActiveYN"] = l_activeYN;
            rdl_activeYN.SelectedValue = l_activeYN;

            // show roles for user
            string[] l_rolesForUser = Roles.GetRolesForUser(l_username);
            foreach (string role in l_rolesForUser)
            {
                switch (role.ToLower())
                {
                    case "admin":
                        cbx_Admin.Checked = true;
                        break;
                    case "odpstaffsupervisor":
                        cbx_ODPStaffSupervisor.Checked = true;
                        break;
                    case "odpstaffmember":
                        cbx_ODPStaffMember.Checked = true;
                        break;
                    case "codersupervisor":
                        cbx_CoderSupervisor.Checked = true;
                        break;
                    case "coder":
                        cbx_Coder.Checked = true;
                        break;
                }
            }
        }

        string GetActiveYN()
        {
            string s = "";
            if (ViewState["ActiveYN"] != null)
            {
                s = ViewState["ActiveYN"].ToString();
            }

            return s;
        }

        string GetUserName()
        {
            string s = "";
            if (ViewState["UserName"] != null)
            {
                s = ViewState["UserName"].ToString();
            }

            return s;

        }

        string GetAction()
        {
            string s = "";
            if (ViewState["Action"] != null)
            {
                s = ViewState["Action"].ToString();
            }

            return s;

        }

        string GetEmail()
        {
            string s = "";
            if (ViewState["Email"] != null)
            {
                s = ViewState["Email"].ToString();
            }

            return s;

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
                    int l_intReturn;
                    string l_username;
                    Guid l_userID;
                    
                    string l_fname = txt_firstName.Text.ToString().Trim();
                    string l_lname = txt_lastName.Text.ToString().Trim();
                    string l_email = txt_email.Text.ToString().Trim();
                    string l_password = txt_new_password.Text;
                    bool l_activeYN = rdl_activeYN.SelectedValue == "1" ? true : false;

                    if (GetAction() == "ADD")
                    {
                        // get roles list
                        string l_roleList = "";

                        if (cbx_Admin.Checked)
                            l_roleList +=  "Admin,";

                        if (cbx_ODPStaffSupervisor.Checked)
                            l_roleList += "ODPStaffSupervisor,";

                        if (cbx_ODPStaffMember.Checked)
                            l_roleList += "ODPStaffMember,";

                        if (cbx_CoderSupervisor.Checked)
                            l_roleList += "CoderSupervisor,";

                        if (cbx_Coder.Checked)
                            l_roleList += "Coder";

                        l_roleList = l_roleList.TrimEnd(',');
                
                        // create new user
                        l_username = ""; 
                        using (AccountDataLinqDataContext db = new AccountDataLinqDataContext(AccountDAL.connString))
                        {
                            var qry = db.create_user(l_fname, l_lname, l_email, l_password, l_activeYN, l_roleList, ref l_username);
                        }

                        if (String.IsNullOrEmpty(l_username))
                        {
                            HandlePageError("Error creating user account.  Please try again.");
                        }
                        else
                        {
                            Session["AM_UserName"] = l_username;
                            Response.Redirect("EditAccountConfirm.aspx");
                        }
                    }
                    else
                    {
                        // update user
                        l_username = GetUserName();
                        l_userID = (Guid)Membership.GetUser(l_username).ProviderUserKey;
                        
                        using (AccountDataLinqDataContext db = new AccountDataLinqDataContext(AccountDAL.connString))
                        {
                            l_intReturn = db.update_userProfileByID(l_userID, l_fname, l_lname);
                        }

                        if (l_intReturn == 0)
                        {
                            HandlePageError("Error updating user account.  Please try again.");
                        }
                        else
                        { 
                            MembershipUser updateUser = Membership.GetUser(l_username);
                            updateUser.Email = l_email;
                            updateUser.IsApproved = l_activeYN;
                            
                            if (updateUser.IsLockedOut)
                                updateUser.UnlockUser();

                            if (!String.IsNullOrEmpty(l_password)){
                                updateUser.ChangePassword(updateUser.ResetPassword(), l_password);
                            }
                            Membership.UpdateUser(updateUser);

                            // remove/add roles as appropriate
                            setRoles(cbx_Admin, updateUser.UserName, "Admin");
                            setRoles(cbx_ODPStaffSupervisor, updateUser.UserName, "ODPStaffSupervisor");
                            setRoles(cbx_ODPStaffMember, updateUser.UserName, "ODPStaffMember");
                            setRoles(cbx_CoderSupervisor, updateUser.UserName, "CoderSupervisor");
                            setRoles(cbx_Coder, updateUser.UserName, "Coder");                          

                            // successfully save, redirect to confirm page
                            Response.Redirect("EditAccountConfirm.aspx");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lbl_error_message.Text = "Error saving this user account.";
                Utils.LogError(ex);
            }
        }

        protected void setRoles(CheckBox cbx, string username, string role)
        {
            if (cbx.Checked)
            {
                if (!Roles.IsUserInRole(username, role))
                {
                    Roles.AddUserToRole(username, role);
                }
            }
            else
            {
                if (Roles.IsUserInRole(username, role))
                {
                    Roles.RemoveUserFromRole(username, role);
                }
            }
        }

        protected bool isValidForm()
        {
            bool l_isValid = true;

            //check if email changed, if so need to confirm email
            string l_oldEmail = GetEmail();
            string l_newEmail = txt_email.Text.Trim().ToString();
            string l_confirmEmail = txt_confirm_email.Text.Trim().ToString();

            // if create, confirm email required
            if (GetAction() == "ADD")
            {
                if ((!String.IsNullOrEmpty(l_newEmail)) && (String.IsNullOrEmpty(l_confirmEmail)))
                {
                    CustomValidator cv = new CustomValidator();
                    cv.IsValid = false;
                    cv.ErrorMessage = "Confirm Email is required.";
                    this.Page.Validators.Add(cv);
                    l_isValid = false;
                }
            }
            else
            {
                //edit, if change changed, confirm email is required
                if ((l_oldEmail != l_newEmail) && (String.IsNullOrEmpty(l_confirmEmail)))
                {
                    CustomValidator cv = new CustomValidator();
                    cv.IsValid = false;
                    cv.ErrorMessage = "Confirm Email is required.";
                    this.Page.Validators.Add(cv);
                    l_isValid = false;
                }
            }


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
            bool isAdmin = false;
            bool isODPSupervisor = false;
            bool isODPStaff = false;
            bool isCoderSupervisor = false;
            bool isCoder = false;

            if (cbx_Admin.Checked)
                isAdmin = true;

            if (cbx_ODPStaffSupervisor.Checked)
                isODPSupervisor = true;

            if (cbx_ODPStaffMember.Checked)
                isODPStaff = true;

            if (cbx_CoderSupervisor.Checked)
                isCoderSupervisor = true;

            if (cbx_Coder.Checked)
                isCoder = true;

            if (!isAdmin && !isODPSupervisor && !isODPStaff && !isCoderSupervisor && !isCoder)
            {
                CustomValidator cv = new CustomValidator();
                cv.IsValid = false;
                cv.ErrorMessage = "Select at least one role.";
                this.Page.Validators.Add(cv);
                l_isValid = false;
            }

            if ((isAdmin) && (isCoder || isCoderSupervisor))
            {
                CustomValidator cv = new CustomValidator();
                cv.IsValid = false;
                cv.ErrorMessage = "Admin role cannot be assign with the Coder or Coder Supervisor roles.";
                this.Page.Validators.Add(cv);
                l_isValid = false;
            }

            if ((isAdmin) && (!isODPSupervisor) && (isODPStaff))
            {
                CustomValidator cv = new CustomValidator();
                cv.IsValid = false;
                cv.ErrorMessage = "Admin role cannot be assign with just the ODP Staff role.";
                this.Page.Validators.Add(cv);
                l_isValid = false;
            }

            if ((isODPSupervisor || isODPStaff) && (isCoder || isCoderSupervisor))
            {
                CustomValidator cv = new CustomValidator();
                cv.IsValid = false;
                cv.ErrorMessage = "ODP Supervisor/ODP Staff cannot be assign with the Coder/Coder Supervsior roles.";
                this.Page.Validators.Add(cv);
                l_isValid = false;
            }

            // check status if change from active to inactive; cannot deactive user on active team
            if ((GetActiveYN() == "1") && (rdl_activeYN.SelectedValue == "0"))
            {
                int l_cnt;
                using (AccountDataLinqDataContext db = new AccountDataLinqDataContext(AccountDAL.connString))
                {
                    l_cnt = (int)db.select_activeTeamByUserName(GetUserName()).Count();
                } 

                if (l_cnt > 0) 
                {
                    CustomValidator cv = new CustomValidator();
                    cv.IsValid = false;
                    cv.ErrorMessage = "This user account is currently a member of a team and cannot be deactivated.";
                    this.Page.Validators.Add(cv);
                    l_isValid = false;
                }
            }
            return l_isValid;
        }
    }
}