using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using ODPTaxonomyUtility_TT;

namespace ODPTaxonomyTrainingAdminWebsite.Account
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void LoginUser_OnLoggingIn(object sender, LoginCancelEventArgs e)
        {
            string l_username = LoginUser.UserName.ToString().Trim();
            int roleCnt = Roles.GetRolesForUser(l_username).Count();

            if((Roles.IsUserInRole(l_username, "Coder")) && (roleCnt == 1)){
                e.Cancel=true;
                ltlCoderAuthenticate.Text = "<p class='errorMessage'>You do not have the appropriate permission to access this website.</p>";
            }
        }

        protected void LoginUser_OnAuthenticate(object sender, AuthenticateEventArgs e)
        {
            try
            {
                string l_username = LoginUser.UserName.ToString().Trim();


                if (Membership.ValidateUser(l_username, LoginUser.Password))
                {

                    MembershipUser user = Membership.GetUser(l_username);

                    // check if user is locked out, if so unlock
                    if (user.IsLockedOut)
                    {
                        user.UnlockUser();
                    }

                    bool isPasswordReset = false;

                    if (!(user.Comment == null))
                        isPasswordReset = user.Comment.ToString() == "reset" ? true : false;


                    if (isPasswordReset)
                    {
                        FormsAuthentication.SetAuthCookie(l_username, false);
                        Response.Redirect("~/AccountManagement/ChangePassword.aspx");
                    }
                    else
                    {
                        FormsAuthentication.RedirectFromLoginPage(l_username, false);
                    }
                }
                else
                {
                    LoginUser.FailureText = "<p class='errorMessage'>Either user name or password is incorrect.  Please try again.</p>";
                }

            }
            catch (Exception ex)
            {
                Utils.LogError(ex);
            }
        }


        protected void LoginButton_Click(object sender, EventArgs e)
        {
            

        }


    }
}
