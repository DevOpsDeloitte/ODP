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

namespace ODPTaxonomyWebsite.Evaluation
{
    public partial class ViewAbstract : System.Web.UI.Page
    {
        #region Fields

        private string role_coder = null;
        private string role_coderSup = null;
        private string role_odp = null;
        private string role_odpSup = null;
        private string role_admin = null;
        private string connString = null;

        #endregion

        #region EventHandlers
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                role_coder = Common.RoleNames["coder"];
                role_coderSup = Common.RoleNames["coderSup"];
                role_odp = Common.RoleNames["odp"];
                role_odpSup = Common.RoleNames["odpSup"];
                role_admin = Common.RoleNames["admin"];

                connString = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ToString();

                if (!Page.IsPostBack)
                {
                    bool isLoggedIn = HttpContext.Current.User.Identity.IsAuthenticated;
                    if (isLoggedIn)
                    {
                        //*********************
                        // Check Session["CurrentRole"]
                        if (Session["CurrentRole"] != null)
                        {
                            lbl_messageUsers.Text = Session["CurrentRole"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex);
                throw new Exception("An error has occured while loading page data.");
            }
        }

        #endregion
    }
}