using System;
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
using ODPTaxonomyTrainingAdminDAL;


namespace ODPTaxonomyTrainingAdminWebsite
{
    public partial class _Default : System.Web.UI.Page
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
                if (ConfigurationManager.ConnectionStrings["ODPTaxonomy"] != null)
                {
                    connString = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ToString();
                }

                if (!Page.IsPostBack)
                {
                    bool isLoggedIn = HttpContext.Current.User.Identity.IsAuthenticated;
                    if (isLoggedIn)
                    {
                        LoadPageData();
                    }
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex);
                throw new Exception("An error has occured while loading page data.");
            }
        }


        protected void btn_push_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddl_instances.SelectedValue == "-1")
                {
                    lbl_Error.Text = "Please select an instance.";
                    lbl_Error.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex);
                throw new Exception("An error has occured on button push click.");
            }
        }

        protected void btn_pull_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddl_instances.SelectedValue == "-1")
                {
                    lbl_Error.Text = "Please select an instance.";
                    lbl_Error.Visible = true;
                }

            }
            catch (Exception ex)
            {
                Utils.LogError(ex);
                throw new Exception("An error has occured on button push click.");
            }
        }

        #endregion

        #region Methods

        private void LoadPageData()
        {
            MembershipUser userCurrent = Membership.GetUser();
            string userCurrentName = "";
            if (userCurrent != null)
            {
                userCurrentName = userCurrent.UserName;
            }
            Guid userId = Common.GetCurrentUserId(connString, userCurrentName);

            // load the instance dropdown
            using (TrainingAdminDALDataContext db = new TrainingAdminDALDataContext(connString))
            {
                ddl_instances.DataSource = db.Tr_SelectInstances();
                ddl_instances.DataValueField = "InstanceID";
                ddl_instances.DataTextField = "InstanceName";
                ddl_instances.DataBind();
                ddl_instances.Items.Insert(0, new ListItem("Select Instance", "-1"));
                ddl_instances.SelectedValue = "-1";
            }
        }


        #endregion


    }
}
