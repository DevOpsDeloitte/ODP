using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using ODPTaxonomyDAL_TT;
using ODPTaxonomyUtility_TT;

namespace ODPTaxonomyTrainingAdminWebsite
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        private string role_admin = null;
        private string connString = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ////Hide Menu on Login  page
                //string currentPage = Request.Url.ToString().ToLower();
                //if (currentPage.IndexOf("login.aspx") > -1)
                //{
                //    showLeftPush.Visible = false;
                //}
                //else
                //{
                //    showLeftPush.Visible = true;
                //}

                role_admin = Common.RoleNames["admin"];

                connString = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ToString();

                if (!Page.IsPostBack)
                {
                    bool isLoggedIn = HttpContext.Current.User.Identity.IsAuthenticated;
                    if (isLoggedIn)
                    {
                        LoadMenuData();
                    }
                }

            }
            catch (Exception ex)
            {
                Utils.LogError(ex);
                throw new Exception("An error has occured while loading page data.");
            }

        }

        protected void HeadLoginStatus_OnLoggingOut(object sender, LoginCancelEventArgs e)
        {

            Session["AM_UserName"] = null;
        }

        private void LoadMenuData()
        {
            MembershipUser userCurrent = Membership.GetUser();
            string userCurrentName = userCurrent.UserName;
            Guid userId = Common.GetCurrentUserId(connString, userCurrentName);



            if (userCurrent != null)
            {
                if (Roles.IsUserInRole(userCurrent.UserName, role_admin))
                {
                    //Admin
                    //pnl_admin.Visible = true;
                }
            }

        }



    }
}
