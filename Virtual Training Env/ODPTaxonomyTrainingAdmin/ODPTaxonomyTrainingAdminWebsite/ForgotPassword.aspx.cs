using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Net.Mail;
using System.Text;
using ODPTaxonomyUtility_TT;

namespace ODPTaxonomyTrainingAdminWebsite
{
    public partial class ForgotPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_return_Click(object sender, EventArgs e)
        {
            Response.Redirect("login.aspx");
        }

        protected void btn_forgotpwd_cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("login.aspx");
        }

        protected void btn_forgotpwd_submit_Click(object sender, EventArgs e)
        {
            try
            {
                MembershipUser user = Membership.GetUser(txt_forgotpwd_username.Text.Trim());

                if (user == null)
                {
                    HandlePageError("Invalid username. Please try again.");
                }
                else
                {
                    // check if user is locked out, if so
                    if (user.IsLockedOut)
                    {
                        user.UnlockUser();
                    }

                    // update comment to "reset"
                    user.Comment = "reset";
                    Membership.UpdateUser(user);

                    string l_useremail = user.Email;
                    string l_newpwd = user.ResetPassword();
                    // email
                    string emailFromAddress = System.Configuration.ConfigurationManager.AppSettings["emailFromAddress"];
                    string emailToAddress = l_useremail;
                    string subject = "ODP Taxonomy - Forgot Password";
                    string body = "Your password has been reset to: " + l_newpwd + " <p>Please login to the ODP Taxonomy website and change your password.</p>";
                    SmtpClient smptServerClient = new SmtpClient();
                    MailMessage messageToBeSent = new MailMessage();

                    messageToBeSent.From = new MailAddress(emailFromAddress);
                    messageToBeSent.To.Add(l_useremail);

                    messageToBeSent.Subject = subject;
                    messageToBeSent.Body = body;
                    messageToBeSent.IsBodyHtml = true;
                    smptServerClient.Send(messageToBeSent);

                    pnl_forgot_password.Visible = false;
                    pnl_confirmation.Visible = true;
                    btn_return.Visible = true;


                }
            }
            catch (Exception ex)
            {
                HandlePageError("Error sending password.");
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