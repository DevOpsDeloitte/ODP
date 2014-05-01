using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using ODPTaxonomyUtility_TT;


namespace ODPTaxonomyWebsite.AccountManagement
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_changePassword_OnClick(object sender, EventArgs e)
        {

            try
            {
                MembershipUser l_user = Membership.GetUser(User.Identity.Name);

                if (l_user.ChangePassword(l_user.ResetPassword(), txt_new_password.Text))
                {
                    l_user.Comment = "";
                    Membership.UpdateUser(l_user);

                    pnl_change_password.Visible = false;
                    pnl_confirmation.Visible = true;
                }
                else
                {
                    HandlePageError("Password change failed. Please re-enter your values and try again.");
                    
                }
            }
            catch (Exception ex)
            {
                HandlePageError("A system error has occurred.  Please contact the administrator.");
                Utils.LogError(ex);
            }
        }

        protected void HandlePageError(string message)
        {
            lbl_error_message.Visible = true;
            lbl_error_message.Text = message;
        }

    }
}