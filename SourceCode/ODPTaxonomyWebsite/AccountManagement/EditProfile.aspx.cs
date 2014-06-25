﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ODPTaxonomyAccountDAL;
using ODPTaxonomyUtility_TT;
using System.Web.Security;

namespace ODPTaxonomyWebsite.AccountManagement
{
    public partial class EditProfile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    MembershipUser l_user = Membership.GetUser(User.Identity.Name);
                    txt_email.Text = l_user.Email;
                    txt_confirm_email.Text = l_user.Email;

                    Guid l_userID = (Guid)l_user.ProviderUserKey;

                    select_userByIDResult rec;
                    using (AccountDataLinqDataContext db = new AccountDataLinqDataContext(AccountDAL.connString))
                    {
                        rec = (select_userByIDResult)db.select_userByID(l_userID).SingleOrDefault();
                    }

                    txt_fname.Text = rec.UserFirstName;
                    txt_lname.Text = rec.UserLastName;

                }
                catch (Exception ex)
                {
                    HandlePageError("Error editing profile.");
                    Utils.LogError(ex);
                }
            }
        }

        protected void btn_changePassword_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("~/AccountManagement/ChangePassword.aspx");
        }

        protected void btn_saveProfile_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid && isValidForm())
                {
                    MembershipUser l_user = Membership.GetUser(User.Identity.Name);
                    Guid l_userID = (Guid)l_user.ProviderUserKey;

                    int l_intReturn;
                    string l_fname = txt_fname.Text.Trim();
                    string l_lname = txt_lname.Text.Trim();

                    using (AccountDataLinqDataContext db = new AccountDataLinqDataContext(AccountDAL.connString))
                    {
                        l_intReturn = db.update_userProfileByID(l_userID, l_fname, l_lname);
                    }

                    if (l_intReturn == 0)
                    {
                        HandlePageError("Error updating profile.  Please try again.");
                    }
                    else
                    {
                        l_user.Email = txt_email.Text;
                        Membership.UpdateUser(l_user);

                        pnl_confirmation.Visible = true;
                        pnl_edit_profile.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                HandlePageError("Error saving profile.");
                Utils.LogError(ex);
            }
        }

        protected bool isValidForm()
        {
            bool l_isValid = true;

            //check if email changed, if so need to confirm email
            MembershipUser l_user = Membership.GetUser(User.Identity.Name);
            string l_oldEmail = l_user.Email;
            string l_newEmail = txt_email.Text.Trim().ToString();
            string l_confirmEmail = txt_confirm_email.Text.Trim().ToString();

            //if change changed, confirm email is required
            if ((l_oldEmail != l_newEmail) && (String.IsNullOrEmpty(l_confirmEmail)))
            {
                CustomValidator cv = new CustomValidator();
                cv.IsValid = false;
                cv.ErrorMessage = "Confirm Email is required.";
                this.Page.Validators.Add(cv);
                l_isValid = false;
            }

            return l_isValid;
        }

        protected void btn_cancel_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("~/default.aspx");
        }

        protected void HandlePageError(string message)
        {
            lbl_error_message.Visible = true;
            lbl_error_message.Text = message;
        }
    }
}