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
using ODPTaxonomyCommon;

namespace ODPTaxonomyWebsite.Evaluation
{
    public partial class ViewAbstract : System.Web.UI.Page
    {
        #region Fields
        private string userCurrentName = "";
        private string role_coder = null;
        private string role_coderSup = null;
        private string role_odp = null;
        private string role_odpSup = null;
        private string role_admin = null;
        private string connString = null;
        private string currentRole = null;
        private string messUserNotInTeam = "You have not been added to a team at this time. Once you are on a team, refresh this page and you can begin.";
        private string messNoCurrentRole = "Your current role is not identified. You will be redirected to the homepage in 10 seconds.";

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
                    lbl_messageUsers.Visible = false;

                    bool isLoggedIn = HttpContext.Current.User.Identity.IsAuthenticated;
                    if (isLoggedIn)
                    {
                        //*********************
                        // Check Session["CurrentRole"]
                        if (Session["CurrentRole"] != null)
                        {
                            currentRole = Session["CurrentRole"].ToString();
                            LoadData(currentRole);
                        }
                        else
                        {
                            lbl_messageUsers.Visible = true;
                            lbl_messageUsers.Text = messNoCurrentRole;
                            Response.AddHeader("REFRESH", "10;URL=/Default.aspx");  
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

        #region Methods

        private void LoadAbstract(tbl_Abstract abstr)
        {
            AbstractDescPart.InnerText = abstr.AbstractDescPart;
            AbstractPublicHeathPart.InnerText = abstr.AbstractPublicHeathPart;
            ProjectTitle.InnerText = abstr.ProjectTitle;
            AdministeringIC.InnerText = abstr.AdministeringIC;
            ApplicationID.InnerText = abstr.ApplicationID.ToString();
            PIProjectLeader.InnerText = abstr.PIProjectLeader;
            FY.InnerText = abstr.FY;
            ProjectNumber.InnerHtml = abstr.ProjectNumber;
            DateTime time = DateTime.Now;             
            string format = "MM/d/yyyy";
            userId.InnerText = userCurrentName;
            date.InnerText = time.ToString(format);  
        }

        private void GetAbstract_CoderEvaluation(Guid userId)
        {
            string message = "";
            int abstractId = -1;
            int evaluationId = -1;
            int teamTypeID = (int)ODPTaxonomyDAL_TT.TeamType.Coder;
            tbl_Abstract abstr = null;

            //Check if user is in a Team
            int? teamId = Common.GetTeamIdForUser(connString, teamTypeID, userId);
            if (teamId != null)
            {
                //Check if Evaluation has started already
                int i_teamId = (int)teamId;
                int evaluationTypeId = (int)ODPTaxonomyDAL_TT.EvaluationType.CoderEvaluation;
                
                ViewAbstractData evaluationData = Common.GetEvaluationData(connString, i_teamId, evaluationTypeId);
                if (evaluationData != null)
                {
                    evaluationId = evaluationData.EvaluationId;
                    abstractId = evaluationData.Abstract.AbstractID;
                    abstr = evaluationData.Abstract;
                }
                else //Evaluation has NOT started yet
                {                    
                    //Generate AbstractID available for coding
                    abstr = Common.GetAbstract_CoderEvaluation(connString, out message);
                    if (abstr != null)
                    {
                        abstractId = abstr.AbstractID;
                        //Start Evaluation process
                        evaluationId = Common.StartEvaluationProcess(connString, evaluationTypeId, abstractId, i_teamId, userId);                        
                    }
                    else
                    {
                        lbl_messageUsers.Visible = true;
                        lbl_messageUsers.Text = message;
                    }
                }

                if (abstr != null)
                {
                    //Display abstract on screen 
                    LoadAbstract(abstr);
                    //Store UserId and EvaluationId in Session. These values are passed to Submission page later.
                    ViewAbstractToEvaluation values = new ViewAbstractToEvaluation();
                    values.ViewMode = Mode.code;
                    values.EvaluationId = evaluationId;
                    values.SubmissionTypeId = (int)SubmissionTypeId.CoderEvaluation;
                    values.UserId = userId;
                    Session["ViewAbstractToEvaluation"] = values;

                    pnl_printBtns.Visible = true;
                    pnl_abstract.Visible = true;
                    pnl_extraData.Visible = true;
                }
                
            }
            else
            {
                lbl_messageUsers.Visible = true;
                lbl_messageUsers.Text = messUserNotInTeam;
            }         

            
        }

        private void LoadData(string currentRole)
        {
            if (!String.IsNullOrEmpty(currentRole))
            {
                MembershipUser userCurrent = Membership.GetUser();
                
                if (userCurrent != null)
                {
                    userCurrentName = userCurrent.UserName;
                }
                Guid userId = Common.GetCurrentUserId(connString, userCurrentName);

                //Coder
                if (currentRole == role_coder)
                {
                    GetAbstract_CoderEvaluation(userId);
                }

                //Coder Sup
                if (currentRole == role_coderSup)
                {
                    //Check User's intentions /Evaluation/ViewAbstract.aspx?AbstractID=1
                }

                //ODP Staff
                if (currentRole == role_odp)
                {

                }

                //ODP Sup
                if (currentRole == role_odpSup)
                {

                }

                //Admin
                if (currentRole == role_admin)
                {

                }

            }
        }

        #endregion

        protected void btn_code_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("Evaluation.aspx", false);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex);
                throw new Exception("An error has occured while loading page data.");
            }
        }

    }
}