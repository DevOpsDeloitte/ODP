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

namespace ODPTaxonomyWebsite
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
        private string messUserNotInTeam = "You have not been added to a team at this time. Once you are on a team, refresh this page and you can begin.";

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

        
        //Coder
        protected void btn_viewAbstract_coder_Click(object sender, EventArgs e)
        {
            try
            {
                Session["CurrentRole"] = role_coder;
                Response.Redirect("/Evaluation/ViewAbstract.aspx", false);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex);
                throw new Exception("An error has occured on button click.");
            }   
            
        }
        //Coder Supervisor
        protected void btn_manageTeams_coderSup_Click(object sender, EventArgs e)
        {
            try
            {
                Session["CurrentRole"] = role_coderSup;
                Response.Redirect("/Evaluation/ManageTeams.aspx", false);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex);
                throw new Exception("An error has occured on button click.");
            }   
        }

        protected void btn_viewAbstractList_coderSup_Click(object sender, EventArgs e)
        {
            try
            {
                Session["CurrentRole"] = role_coderSup;
                Response.Redirect("/Evaluation/ViewAbstractList.aspx", false);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex);
                throw new Exception("An error has occured on button click.");
            }   
        }

        protected void btn_viewAbstract_coderSup_Click(object sender, EventArgs e)
        {
            try
            {
                Session["CurrentRole"] = role_coderSup;
                Response.Redirect("/Evaluation/ViewAbstract.aspx", false);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex);
                throw new Exception("An error has occured on button click.");
            }   
        }
        //ODP Staff
        protected void btn_viewAbstractList_odp_Click(object sender, EventArgs e)
        {
            try
            {
                Session["CurrentRole"] = role_odp;
                Response.Redirect("/Evaluation/ViewAbstractList.aspx", false);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex);
                throw new Exception("An error has occured on button click.");
            }   
        }
        //ODP Supervisor
        protected void btn_manageTeams_odpSup_Click(object sender, EventArgs e)
        {
            try
            {
                Session["CurrentRole"] = role_odpSup;
                Response.Redirect("/Evaluation/ManageTeams.aspx", false);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex);
                throw new Exception("An error has occured on button click.");
            }   
        }

        protected void btn_viewAbstractList_odpSup_Click(object sender, EventArgs e)
        {
            try
            {
                Session["CurrentRole"] = role_odpSup;
                Response.Redirect("/Evaluation/ViewAbstractList.aspx", false);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex);
                throw new Exception("An error has occured on button click.");
            }   
        }

        
        //Admin
        protected void btn_manageUserAccounts_admin_Click(object sender, EventArgs e)
        {
            try
            {
                Session["CurrentRole"] = role_admin;
                Response.Redirect("/AccountManagement/ManageAccounts.aspx", false);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex);
                throw new Exception("An error has occured on button click.");
            }   
        }

        protected void btn_viewAbstractList_admin_Click(object sender, EventArgs e)
        {
            try
            {
                Session["CurrentRole"] = role_admin;
                Response.Redirect("/Evaluation/ViewAbstractList.aspx", false);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex);
                throw new Exception("An error has occured on button click.");
            }   
        }


        #endregion

        #region Methods        

        private void LoadPageData()
        {
            MembershipUser userCurrent = Membership.GetUser();
            string userCurrentName = userCurrent.UserName;
            Guid userId = Common.GetCurrentUserId(connString, userCurrentName);
            int teamTypeID = 0;
           

            if (userCurrent != null)
            {
                if (Roles.IsUserInRole(userCurrent.UserName, role_coder))
                {
                    //Coder
                    pnl_coder.Visible = true;
                    //Checking Abstract Coding Option Evailuability
                    teamTypeID = (int)ODPTaxonomyDAL_TT.TeamType.Coder;
                    bool userIsInTeam = Common.UserIsInTeam(connString, teamTypeID, userId);
                    if (userIsInTeam)
                    {
                        btn_viewAbstract_coder.Visible = true;
                        lbl_messCoder.Visible = false;
                    }
                    else
                    {
                        btn_viewAbstract_coder.Visible = false;
                        lbl_messCoder.Visible = true;
                        lbl_messCoder.Text = messUserNotInTeam;
                    }
                }
                
                if (Roles.IsUserInRole(userCurrent.UserName, role_coderSup))
                {
                    //Coder Supervisor
                    pnl_coderSup.Visible = true;
                    //Checking Abstarct Coding Option Evailuability
                    teamTypeID = (int)ODPTaxonomyDAL_TT.TeamType.Coder;
                    bool userIsInTeam = Common.UserIsInTeam(connString, teamTypeID, userId);
                    if (userIsInTeam)
                    {
                        btn_viewAbstract_coderSup.Visible = true;
                        lbl_messageUsers.Visible = false;
                    }
                    else
                    {
                        btn_viewAbstract_coderSup.Visible = false;
                        //lbl_messageUsers.Visible = true;
                        //lbl_messageUsers.Text = messUserNotInTeam;
                    }
                }
                if (Roles.IsUserInRole(userCurrent.UserName, role_odp))
                {
                    //ODP Staff
                    pnl_odp.Visible = true;
                }
                if (Roles.IsUserInRole(userCurrent.UserName, role_odpSup))
                {
                    //ODP Supervisor
                    pnl_odpSup.Visible = true;
                }
                if (Roles.IsUserInRole(userCurrent.UserName, role_admin))
                {
                    //Admin
                    pnl_admin.Visible = true;
                }
                
            }
            
        }

        #endregion

        
    }
}
