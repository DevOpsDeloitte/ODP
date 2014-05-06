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
    
    public partial class ManageTeams : System.Web.UI.Page
    {
        private string role_coder = null;
        private string role_coderSup = null;
        private string role_odp = null;
        private string role_odpSup = null;
        private string connString = null;
        private int membersTotal = 3;
        private Dictionary<int, List<tbl_aspnet_User>> dic_teamUsers = new Dictionary<int, List<tbl_aspnet_User>>();

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
                    btn_saveteam.Visible = false;
                    gc_noTeam.Visible = false;
                    gc_noUsers.Visible = false;

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

        public string GetUserInitials(object firstName, object lastName)
        {
            string sFirstName = null;
            string sLastName = null;
            string initials = "";
            StringBuilder sb = new StringBuilder();

            if (firstName != null)
            {
                if (lastName != null)
                {
                    sFirstName = firstName.ToString();
                    sLastName = lastName.ToString();

                    if (sFirstName.Length > 0 && sLastName.Length > 0)
                    {
                        sb.Append(sFirstName.Substring(0, 1).ToUpper());
                        sb.Append(sLastName.Substring(0, 1).ToUpper());
                        
                        initials = sb.ToString();
                    }
                }
            }

            return initials;
        }

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
                    pnl_content.Visible = false;
                }

                if (doLoadData)
                {
                    btn_saveteam.Visible = true;
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
                    var matches = (from u in db.tbl_aspnet_Users
                                  join m in db.tbl_aspnet_Memberships on u.UserId equals m.UserId
                                  join ur in db.tbl_aspnet_UsersInRoles on u.UserId equals ur.UserId
                                  join r in db.tbl_aspnet_Roles on ur.RoleId equals r.RoleId
                                  where roles.Contains(r.RoleName) && !list_teamUsers.Contains(u.UserId) && m.IsApproved
                                  select u).Distinct().OrderBy(e => e.UserFirstName).ThenBy(e => e.UserLastName);
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
                    //lbl_messageUsers.Text = "No users are currently evailable for you to select for a new team.";
                    //lbl_messageUsers.Visible = true;
                    btn_saveteam.Visible = false;
                    gc_noUsers.Visible = true;
                }
            }

        }

        protected void LoadTeams(int teamTypeID)
        {
            List<tbl_Team> list_teams = new List<tbl_Team>();
            
            int currentTeamID = 0;
            tbl_aspnet_User currentUser = null;
            List<tbl_aspnet_User> currentUserList = null;

            //Check current user's role
            if (!String.IsNullOrEmpty(connString))
            {
                using (DataDataContext db = new DataDataContext(connString))
                {
                    var matches = from t in db.tbl_Teams
                                  where (t.StatusID == (int)ODPTaxonomyDAL_TT.Status.Active) && (t.TeamTypeID == teamTypeID)
                                  select t;
                    list_teams = matches.ToList<tbl_Team>();

                    var matches_2 = from t in db.tbl_Teams
                                    join tu in db.tbl_TeamUsers on t.TeamID equals tu.TeamID
                                    join u in db.tbl_aspnet_Users on tu.UserId equals u.UserId
                                    where (t.StatusID == (int)ODPTaxonomyDAL_TT.Status.Active) && (t.TeamTypeID == teamTypeID)
                                    orderby t.TeamID, u.UserFirstName, u.UserLastName
                                    select new { t.TeamID, u.UserId, u.UserName, u.UserLastName, u.UserFirstName};

                    foreach (var i in matches_2)
                    {
                        currentTeamID = i.TeamID;
                        currentUser = new tbl_aspnet_User();
                        currentUser.UserId = i.UserId;
                        currentUser.UserName = i.UserName;
                        currentUser.UserLastName = i.UserLastName;
                        currentUser.UserFirstName = i.UserFirstName;

                        if (!dic_teamUsers.ContainsKey(currentTeamID))
                        {
                            currentUserList = new List<tbl_aspnet_User>();
                            currentUserList.Add(currentUser);

                            dic_teamUsers.Add(currentTeamID, currentUserList);
                        }
                        else
                        {
                            currentUserList = dic_teamUsers[currentTeamID];
                            if (!currentUserList.Contains(currentUser))
                            {
                                currentUserList.Add(currentUser);
                            }
                        }
                    }

                }

                if (list_teams.Count > 0)
                {
                    rpt_teams.DataSource = list_teams;
                    rpt_teams.DataBind();
                }
                else
                {
                    gc_noTeam.Visible = true;
                }
                
            }
        }

        #endregion

        #region EventHandlers

        protected void DeleteTeam_Click(object sender, EventArgs e)
        {
            try
            {
                bool isLoggedIn = HttpContext.Current.User.Identity.IsAuthenticated;
                if (isLoggedIn)
                {
                    //Get the reference of the clicked button.
                    Button button = (sender as Button);

                    //Get the command argument
                    string commandArgument = button.CommandArgument;
                    int teamId = -1;
                    tbl_Team team = null;
                    bool isActivelyWorking = true;

                    if (Int32.TryParse(commandArgument, out teamId))
                    {
                        MembershipUser userCurrent = Membership.GetUser();
                        string userCurrentName = userCurrent.UserName;
                        System.Guid userCurrentId = Common.GetCurrentUserId(connString, userCurrentName);

                        if (!String.IsNullOrEmpty(connString))
                        {
                            //Check if  Team to be deleted is currently working on any Abstract
                            using (DataDataContext db = new DataDataContext(connString))
                            {
                                var matches = from ev in db.tbl_Evaluations
                                              where ev.TeamID == teamId && ev.IsComplete == false && ev.IsStopped == false
                                              select ev;
                                isActivelyWorking = matches.Any();
                            }

                            if (isActivelyWorking)
                            {
                                lbl_messageUsers.Visible = true;
                                lbl_messageUsers.Text = "The team could NOT be deleted as it is currently working on abstract evaluation.";

                            }
                            else
                            {
                                //OK to Delete the team
                                using (DataDataContext db = new DataDataContext(connString))
                                {
                                    var matches = from t in db.tbl_Teams
                                                  where t.TeamID == teamId
                                                  select t;
                                    team = matches.First();

                                    if (team != null)
                                    {
                                        team.StatusID = (int)ODPTaxonomyDAL_TT.Status.Deleted;
                                        team.UpdatedBy = userCurrentId;
                                        team.UpdatedDateTime = DateTime.Now;
                                        db.SubmitChanges();
                                    }
                                }

                                //Reload Page Data
                                Response.Redirect("ManageTeams.aspx", false);
                            }


                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex);
                throw new Exception("An error has occured while loading data.");
            }  
            
        }

        protected void rp_teams_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            HiddenField hf_teamID = e.Item.FindControl("hf_teamID") as HiddenField;
            Repeater rpt_teamMembers = e.Item.FindControl("rpt_teamMembers") as Repeater;
            int teamID = 0;
            List<tbl_aspnet_User> currentUserList = null;

            if (hf_teamID != null)
            {
                if (Int32.TryParse(hf_teamID.Value.ToString(), out teamID))
                {
                    currentUserList = dic_teamUsers[teamID];
                    if (rpt_teamMembers != null)
                    {
                        rpt_teamMembers.DataSource = currentUserList;
                        rpt_teamMembers.DataBind();
                    }
                }
            }

        }

        protected void btn_saveteam_Click(object sender, EventArgs e)
        {
            try
            {
                bool isLoggedIn = HttpContext.Current.User.Identity.IsAuthenticated;
                if (isLoggedIn)
                {
                    MembershipUser userCurrent = Membership.GetUser();
                    string userCurrentName = userCurrent.UserName;
                    System.Guid userCurrentId = Common.GetCurrentUserId(connString, userCurrentName);

                    //Validate User's Input   
                    HiddenField hf_userID = null;
                    HiddenField hf_userInitials = null;
                    int teamtypeId;
                    bool teamtypeIsOK = Int32.TryParse(hf_teamTypeId.Value.ToString(), out teamtypeId);
                    CheckBox chBox = null;
                    int membersCheckedCount = 0;
                    System.Guid userId = Guid.Empty;
                    StringBuilder sb = new StringBuilder();
                    string initials = "";
                    List<tbl_aspnet_User> listUsers = new List<tbl_aspnet_User>();
                    tbl_aspnet_User currentUser = null;
                    List<System.Guid> list_teamUsersID = new List<System.Guid>();

                    //get list of users already in teams
                    if (!String.IsNullOrEmpty(connString) && teamtypeIsOK)
                    {
                        //Select users available for this team type
                        using (DataDataContext db = new DataDataContext(connString))
                        {
                            var matches = from t in db.tbl_Teams
                                          join tu in db.tbl_TeamUsers on t.TeamID equals tu.TeamID
                                          where (t.StatusID == (int)ODPTaxonomyDAL_TT.Status.Active) && (t.TeamTypeID == teamtypeId)
                                          select tu.UserId;
                            list_teamUsersID = matches.ToList<System.Guid>();
                        }

                        //create a list of selected users
                        foreach (RepeaterItem ri in rpt_users.Items)
                        {
                            hf_userID = ri.FindControl("hf_userID") as HiddenField;
                            hf_userInitials = ri.FindControl("hf_userInitials") as HiddenField;
                            chBox = ri.FindControl("checkbox") as CheckBox;

                            if (chBox != null)
                            {
                                if (hf_userID != null && hf_userInitials != null)
                                {

                                    if (chBox.Checked)
                                    {
                                        userId = Guid.Parse(hf_userID.Value);
                                        initials = hf_userInitials.Value;

                                        //verify that user is NOT in a team already
                                        if (!list_teamUsersID.Contains(userId))
                                        {
                                            currentUser = new tbl_aspnet_User();
                                            currentUser.UserId = userId;
                                            currentUser.UserName = initials;
                                            listUsers.Add(currentUser);
                                            membersCheckedCount++;
                                        }
                                        else
                                        {
                                            lbl_messageUsers.Text = "The user " + initials + " is in a team already.";
                                            lbl_messageUsers.Visible = true;
                                            return;
                                        }
                                    }
                                }
                            }

                        }



                    }


                    if (membersCheckedCount == membersTotal)
                    {
                        //Save new Team
                        using (DataDataContext db = new DataDataContext(connString))
                        {
                            tbl_Team team = new tbl_Team();
                            foreach (tbl_aspnet_User u in listUsers)
                            {
                                tbl_TeamUser teamUser = new tbl_TeamUser();
                                teamUser.UserId = u.UserId;
                                team.tbl_TeamUsers.Add(teamUser);
                                sb.Append(u.UserName + "_");
                            }
                            //sb.Length--;
                            initials = sb.ToString();

                            team.StatusID = (int)ODPTaxonomyDAL_TT.Status.Active;
                            team.TeamTypeID = teamtypeId;
                            team.Createdby = userCurrentId;
                            team.CreatedDateTime = DateTime.Now;
                            team.TeamCode = Common.GetTeamCode(initials);

                            db.tbl_Teams.InsertOnSubmit(team);
                            db.SubmitChanges();
                        }

                        //Reload Page Data
                        //LoadPageData();
                        Response.Redirect("ManageTeams.aspx", true);
                    }
                    else
                    {
                        lbl_messageUsers.Text = "You have to pick 3 users to create a new team.";
                        lbl_messageUsers.Visible = true;
                    }

                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex);
                throw new Exception("An error has occured while loading data.");
            }  

        }

        #endregion

    }
}