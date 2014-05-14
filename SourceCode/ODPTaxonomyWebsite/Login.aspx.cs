using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ODPTaxonomyWebsite.Account
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void LoginButton_Click(object sender, EventArgs e)
        {
            LoginUser.FailureText = "";
            string continueUrl = "~/";
            if (Request.QueryString["ReturnUrl"] != null)
            {
                continueUrl = Request.QueryString["ReturnUrl"].ToString();
            }
            if (Membership.ValidateUser(LoginUser.UserName, LoginUser.Password))
            {

                FormsAuthentication.SetAuthCookie(LoginUser.UserName, false);
                Response.Redirect(continueUrl);
            }
            else
            {
                LoginUser.FailureText = "Either user name or password is wrong.";
            }
        }
    }
}
