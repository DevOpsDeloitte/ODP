using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using ODPTaxonomyUtility_TT;

namespace ODPTaxonomyWebsite.Account
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void LoginButton_Click(object sender, EventArgs e)
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
                    LoginUser.FailureText = "Either user name or password is incorrect.  Please try again.";
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex);
            }

        }


    }
}
