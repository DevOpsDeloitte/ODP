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
using ODPTaxonomyDAL_TT;
using ODPTaxonomyUtility_TT;

namespace ODPTaxonomyWebsite.Evaluation
{
    
    public partial class ManageTeams : System.Web.UI.Page
    {
        private string role_coder = null;
        private string role_coderSup = null;
        private string role_odp = null;
        private string role_odpSup = null;
        private string connString = null;
        private int membersTotal = 3;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //ODPTaxonomyDAL_TT.Common c = new ODPTaxonomyDAL_TT.Common();
                role_coder = Common.RoleNames["coder"];
                role_coderSup = Common.RoleNames["coderSup"];
                role_odp = Common.RoleNames["odp"];
                role_odpSup = Common.RoleNames["odpSup"];

                connString = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ToString();
                if (!Page.IsPostBack)
                {
                    lbl_Error.Text = "";
                    lbl_Error.Visible = false;
                    lbl_messageUsers.Text = "";
                    lbl_messageUsers.Visible = false;


                    //Check current user's role
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
                throw new Exception("An error has occured while loading data.");
            }       
            
        }

        #region Methods

        private void LoadPageData()
        {
            MembershipUser userCurrent = Membership.GetUser();
            List<string> list_roles = new List<string>();
            bool doLoadData = false;
            int teamTypeID = 0;

            if (userCurrent != null)
            {
                if (Roles.IsUserInRole(userCurrent.UserName, role_coderSup))
                {
                    doLoadData = true;
                    list_roles.Add(role_coder);
                    list_roles.Add(role_coderSup);
                    teamTypeID = (int)ODPTaxonomyDAL_TT.TeamType.Coder;
                }
                else if (Roles.IsUserInRole(userCurrent.UserName, role_odpSup))
                {
                    doLoadData = true;
                    list_roles.Add(role_odp);
                    list_roles.Add(role_odpSup);
                    teamTypeID = (int)ODPTaxonomyDAL_TT.TeamType.ODPStaff;
                }
                else
                {
                    lbl_Error.Text = "You are not allowed to manage Teams.";
                    lbl_Error.Visible = true;
                }

                if (doLoadData)
                {

                    LoadUsers(list_roles, teamTypeID);
                    LoadTeams(teamTypeID);
                }
            }
        }

        protected void LoadUsers(List<string> roles, int teamTypeID)
        {
            List<tbl_aspnet_User> list_users = new List<tbl_aspnet_User>();
            List<System.Guid> list_teamUsers = new List<System.Guid>();

            if (!String.IsNullOrEmpty(connString))
            {
                //Select users available for this team type
                using (DataDataContext db = new DataDataContext(connString))
                {
                    var matches = from t in db.tbl_Teams
                                  join tu in db.tbl_TeamUsers on t.TeamID equals tu.TeamID
                                  where (t.StatusID == (int)ODPTaxonomyDAL_TT.Status.Active) && (t.TeamTypeID == teamTypeID)
                                  select tu.UserId;
                    list_teamUsers = matches.ToList<System.Guid>();
                }
                //Select users who are NOT currently in the team of this type
                using (DataDataContext db = new DataDataContext(connString))
                {
                    var matches = from u in db.tbl_aspnet_Users
                                  join ur in db.tbl_aspnet_UsersInRoles on u.UserId equals ur.UserId
                                  join r in db.tbl_aspnet_Roles on ur.RoleId equals r.RoleId
                                  where roles.Contains(r.RoleName) && !list_teamUsers.Contains(u.UserId)
                                  select u;
                    list_users = matches.ToList<tbl_aspnet_User>();
                }

                if (list_users.Count > 0)
                {
                    rpt_users.DataSource = list_users;
                    rpt_users.DataBind();
                    hf_teamTypeId.Value = teamTypeID.ToString();
                }
                else
                {
                    lbl_messageUsers.Text = "No users are currently evailable for you to select.";
                    lbl_messageUsers.Visible = true;
                }
            }

        }

        protected void LoadTeams(int teamTypeID)
        {
            List<tbl_Team> list_teams = new List<tbl_Team>();
            //Check current user's role
            if (!String.IsNullOrEmpty(connString))
            {
                using (DataDataContext db = new DataDataContext(connString))
                {
                    var matches = from t in db.tbl_Teams
                                  join tu in db.tbl_TeamUsers on t.TeamID equals tu.TeamID
                                  where (t.StatusID == (int)ODPTaxonomyDAL_TT.Status.Active) && (t.TeamTypeID == teamTypeID)
                                  select t;
                    list_teams = matches.ToList<tbl_Team>();
                }

                if (list_teams.Count > 0)
                {
                    rpt_teams.DataSource = list_teams;
                    rpt_teams.DataBind();
                }
            }
        }

        #endregion

        #region EventHandlers

        protected void rp_topics_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
        }

        protected void btn_saveteam_Click(object sender, EventArgs e)
        {
            bool isLoggedIn = HttpContext.Current.User.Identity.IsAuthenticated;
            if (isLoggedIn)
            {
                MembershipUser userCurrent = Membership.GetUser();
                string userCurrentName = userCurrent.UserName;
                System.Guid userCurrentId = Common.GetCurrentUserId(connString, userCurrentName);

                //Validate User's Input   
                HiddenField hf_userID = null;
                int teamtypeId;
                bool teamtypeIsOK = Int32.TryParse(hf_teamTypeId.Value.ToString(), out teamtypeId);
                CheckBox chBox = null;
                int membersCheckedCount = 0;
                System.Guid userId = Guid.Empty;
                List<Guid> list_userIds = new List<Guid>();

                foreach (RepeaterItem ri in rpt_users.Items)
                {
                    hf_userID = ri.FindControl("hf_userID") as HiddenField;
                    chBox = ri.FindControl("checkbox") as CheckBox;

                    if (chBox != null)
                    {
                        if (hf_userID != null)
                        {
                            
                            if (chBox.Checked)
                            {
                                userId = Guid.Parse(hf_userID.Value);
                                list_userIds.Add(userId);
                                membersCheckedCount++;
                            }
                        }
                    }

                }

                if (membersCheckedCount == membersTotal)
                {
                    //Save new Team
                    if (teamtypeIsOK)
                    {
                        if (!String.IsNullOrEmpty(connString))
                        {
                            using (DataDataContext db = new DataDataContext(connString))
                            {
                                foreach (Guid uId in list_userIds)
                                {
                                    tbl_Team team = new tbl_Team();
                                    tbl_TeamUser teamUser = new tbl_TeamUser();
                                    teamUser.UserId = uId;
                                    team.tbl_TeamUsers.Add(teamUser);
                                    team.StatusID = (int)ODPTaxonomyDAL_TT.Status.Active;
                                    team.TeamTypeID = teamtypeId;
                                    team.Createdby = userCurrentId;
                                    team.CreatedDateTime = DateTime.Now;


                                    db.tbl_Teams.InsertOnSubmit(team);
                                    db.SubmitChanges();
                                }
                            }
                        }
                    }

                    //Reload Page Data
                    LoadPageData();
                }
                else
                {
                    lbl_messageUsers.Text = "You have to pick 3 users to create a new team.";
                    lbl_messageUsers.Visible = true;
                }

            }

        }

        #endregion

    }
}