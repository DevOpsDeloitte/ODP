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
using System.Net;
using System.ComponentModel;
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
        private string messStatusIsChanged = "You are not allowed to override as the abstract's status was changed.";
        private string messOverrideSuccess = "Abstract override successful, please ask effected coders to log out and log back in.";
        private string messUploadNotesSuccess = "Upload Notes successful, please ask effected coders to log out and log back in.";
        private string messMaxSizeExceeded = "The file size exceeded 8M maximum allowed.";
        private string messNoFile = "Please select the file to upload.";
        private string messWrongFileType = "Only PDF files are allowed to upload.";
        private int maxLen = 8388608;

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
                            hf_currentRole.Value = currentRole;
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

        protected void btn_code_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("Evaluation.aspx", false);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex);
                throw new Exception("An error has occured on code button click.");
            }
        }
        protected void btn_notes_Click(object sender, EventArgs e)
        {
            try
            {
                string fileName = null;
                int abstractId = -1;
                int evaluationId = -1;
                int abstractStatusId = -1;
                currentRole = hf_currentRole.Value;
                AbstractStatusID currentStatus = AbstractStatusID.none;
                Guid userId = Guid.Empty;
                bool uploadSuccess = false;
                if (Guid.TryParse(hf_userId.Value, out userId))
                {
                    //Check abstract's status again - it could be changed
                    if (Int32.TryParse(hf_abstractId.Value, out abstractId))
                    {
                        currentStatus = Common.GetAbstractStatus(connString, abstractId);
                        if (currentRole == role_coderSup)
                        {
                            if (currentStatus == AbstractStatusID._1B || currentStatus == AbstractStatusID._1N)
                            {
                                if (Int32.TryParse(hf_evaluationId_coder.Value, out evaluationId))
                                {
                                    fileName = Common.GetAbstractScan(connString, evaluationId);

                                    if (!String.IsNullOrEmpty(fileName))
                                    {
                                        //Overwrite file on hard drive
                                        UploadNotes(fileName, abstractId, evaluationId, EvaluationType.CoderEvaluation, out uploadSuccess);
                                        if (uploadSuccess)
                                        {
                                            //Update date and user in database table
                                            Common.UploadNotes(connString, evaluationId, userId);

                                            lbl_messageUsers.Visible = true;
                                            lbl_messageUsers.Text = messUploadNotesSuccess;
                                        }
                                    }
                                    else
                                    {
                                        //Save file
                                        fileName = UploadNotes(null, abstractId, evaluationId, EvaluationType.CoderEvaluation, out uploadSuccess);
                                        if (uploadSuccess)
                                        {
                                            //Save data to database
                                            abstractStatusId = (int)AbstractStatusID._1N;
                                            Common.UploadNotes(connString, evaluationId, abstractId, userId, abstractStatusId, fileName);
                                            //regenerate links for coderSup - a new link should appear

                                            lbl_messageUsers.Visible = true;
                                            lbl_messageUsers.Text = messUploadNotesSuccess;
                                        }
                                    }                                    
                                    
                                    
                                    
                                }
                                else
                                {
                                    throw new Exception("Evaluation ID either was not saved between page postbacks Or could not be parssed.");
                                }
                            }
                            else
                            {
                                lbl_messageUsers.Visible = true;
                                lbl_messageUsers.Text = messStatusIsChanged;
                            }
                        }

                        if (currentRole == role_odpSup)
                        {
                            if (currentStatus == AbstractStatusID._2C || currentStatus == AbstractStatusID._2N)
                            {
                                if (Int32.TryParse(hf_evaluationId_odp.Value, out evaluationId))
                                {
                                    fileName = Common.GetAbstractScan(connString, evaluationId);

                                    if (!String.IsNullOrEmpty(fileName))
                                    {
                                        //Overwrite file on hard drive
                                        UploadNotes(fileName, abstractId, evaluationId, EvaluationType.ODPEvaluation, out uploadSuccess);
                                        if (uploadSuccess)
                                        {
                                            //Update date and user in database table
                                            Common.UploadNotes(connString, evaluationId, userId);

                                            lbl_messageUsers.Visible = true;
                                            lbl_messageUsers.Text = messUploadNotesSuccess;
                                        }
                                    }
                                    else
                                    {
                                        //Save file
                                        fileName = UploadNotes(null, abstractId, evaluationId, EvaluationType.ODPEvaluation, out uploadSuccess);
                                        if (uploadSuccess)
                                        {
                                            //Save data to database
                                            abstractStatusId = (int)AbstractStatusID._2N;
                                            Common.UploadNotes(connString, evaluationId, abstractId, userId, abstractStatusId, fileName);
                                            //regenerate links for odpSup - a new link should appear

                                            lbl_messageUsers.Visible = true;
                                            lbl_messageUsers.Text = messUploadNotesSuccess;
                                        }
                                    }                
                                    
                                }
                                else
                                {
                                    throw new Exception("Evaluation ID either was not saved between page postbacks Or could not be parssed.");
                                }
                            }
                            else
                            {
                                lbl_messageUsers.Visible = true;
                                lbl_messageUsers.Text = messStatusIsChanged;
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("Abstract's ID either was not saved between page postbacks Or could not be parssed.");
                    }
                }
                else
                {
                    throw new Exception("User ID either was not saved between page postbacks Or could not be parssed.");
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex);
                throw new Exception("An error has occured on upload notes button click.");
            }
        }


        protected void btn_override_Click(object sender, EventArgs e)
        {
            try
            {
                int abstractId = -1;
                int evaluationId = -1;
                int abstractStatusId = -1;
                currentRole = hf_currentRole.Value;
                AbstractStatusID currentStatus = AbstractStatusID.none;
                Guid userId = Guid.Empty;
                if (Guid.TryParse(hf_userId.Value, out userId))
                {
                    //Check abstract's status again - it could be changed
                    if (Int32.TryParse(hf_abstractId.Value, out abstractId))
                    {
                        currentStatus = Common.GetAbstractStatus(connString, abstractId);
                        if (currentRole == role_coderSup)
                        {
                            if (currentStatus == AbstractStatusID._1 || currentStatus == AbstractStatusID._1A)
                            {
                                if (Int32.TryParse(hf_evaluationId_coder.Value, out evaluationId))
                                {
                                    abstractStatusId = (int)AbstractStatusID._0;
                                    Common.OverrideAbstract(connString, evaluationId, abstractId, userId, abstractStatusId);

                                    lbl_messageUsers.Visible = true;
                                    lbl_messageUsers.Text = messOverrideSuccess;
                                    pnl_printBtns.Visible = false;
                                    pnl_overrideBtns.Visible = false;
                                    pnl_extraData.Visible = false;
                                    pnl_abstract.Visible = false;
                                }
                                else
                                {
                                    throw new Exception("Evaluation ID either was not saved between page postbacks Or could not be parssed.");
                                }
                            }
                            else
                            {
                                lbl_messageUsers.Visible = true;
                                lbl_messageUsers.Text = messStatusIsChanged;
                            }
                        }

                        if (currentRole == role_odpSup)
                        {
                            if (currentStatus == AbstractStatusID._2 || currentStatus == AbstractStatusID._2A)
                            {
                                if (Int32.TryParse(hf_evaluationId_coder.Value, out evaluationId))
                                {
                                    abstractStatusId = (int)AbstractStatusID._1N;
                                    Common.OverrideAbstract(connString, evaluationId, abstractId, userId, abstractStatusId);

                                    lbl_messageUsers.Visible = true;
                                    lbl_messageUsers.Text = messOverrideSuccess;
                                    pnl_printBtns.Visible = false;
                                    pnl_overrideBtns.Visible = false;
                                    pnl_extraData.Visible = false;
                                    pnl_abstract.Visible = false;
                                }
                                else
                                {
                                    throw new Exception("Evaluation ID either was not saved between page postbacks Or could not be parssed.");
                                }
                            }
                            else
                            {
                                lbl_messageUsers.Visible = true;
                                lbl_messageUsers.Text = messStatusIsChanged;
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("Abstract's ID either was not saved between page postbacks Or could not be parssed.");
                    }
                }
                else
                {
                    throw new Exception("User ID either was not saved between page postbacks Or could not be parssed.");
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex);
                throw new Exception("An error has occured on override button click.");
            }
        }



        


        protected void link_Notes_Click(object sender, EventArgs e)
        {
            try
            {
                string fileName = (sender as LinkButton).CommandArgument;
                string fName = Request.PhysicalApplicationPath + "notes\\" + fileName;
                Response.Clear();
                Response.BufferOutput = false;
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "filename=" + fileName);
                //Response.Flush();   
                Response.TransmitFile(fName);
                Response.End();

            }
            catch (Exception ex)
            {
                Utils.LogError(ex);
                throw new Exception("An error has occured on download notes link click.");
            }
        }


        #endregion

        #region Methods

        private void GenerateLinks(EvaluationType type, int evaluationId, int abstractId)
        {
            if (type == EvaluationType.CoderEvaluation)
            {
                string fileName = Common.GetAbstractScan(connString, evaluationId);
                link_coderNotes.CommandArgument = fileName;
            }

            if (type == EvaluationType.ODPEvaluation)
            {

            }
        }


        private string UploadNotes(string fileName, int abstractId, int evaluationId, EvaluationType type, out bool uploadSuccess)
        {
            string fileName_new = "";
            StringBuilder sb = new StringBuilder();
            string sType = type.ToString();
            uploadSuccess = false;
            string path = "";
            string fileExtention = "";
            string name = "";
            int dotPos = -1;
            int size = 0;
            string fileType = "";
            
            //Save file 
            if (fu_notes.PostedFile != null)
            {
                HttpPostedFile myFile = fu_notes.PostedFile;
                fileType = myFile.ContentType.ToLower();
                size = myFile.ContentLength;

                if (size > maxLen)
                {
                    lbl_messageUsers.Visible = true;
                    lbl_messageUsers.Text = messMaxSizeExceeded;
                }
                else
                {
                    if (size > 0)
                    {
                        if (fileType == "application/pdf")
                        {
                            name = myFile.FileName;
                            dotPos = name.LastIndexOf(".");
                            if (dotPos > -1)
                            {
                                fileExtention = name.Substring(dotPos + 1);

                                if (!String.IsNullOrEmpty(fileName))
                                {
                                    fileName_new = fileName;
                                }
                                else
                                {
                                    sb.Append("notesFor_");
                                    sb.Append(abstractId);
                                    sb.Append("_");
                                    sb.Append(sType);
                                    sb.Append("_");
                                    sb.Append(evaluationId);
                                    sb.Append(".");
                                    sb.Append(fileExtention);
                                    fileName_new = sb.ToString();
                                }

                                path = Request.PhysicalApplicationPath + "notes\\" + fileName_new;
                                myFile.SaveAs(path);
                                uploadSuccess = true;
                                lbl_messageUsers.Visible = false;
                            }
                        }
                        else
                        {
                            lbl_messageUsers.Visible = true;
                            lbl_messageUsers.Text = messWrongFileType;
                        }
                        
                    }
                    else
                    {
                        lbl_messageUsers.Visible = true;
                        lbl_messageUsers.Text = messNoFile;
                    }
                    
                }
            }
            else
            {
                lbl_messageUsers.Visible = true;
                lbl_messageUsers.Text = messNoFile;
            }

            return fileName_new;
        }

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

            pnl_printBtns.Visible = true;
            pnl_abstract.Visible = true;
            pnl_extraData.Visible = true;
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

                    btn_code.Visible = true;
                }
                
            }
            else
            {
                lbl_messageUsers.Visible = true;
                lbl_messageUsers.Text = messUserNotInTeam;
            }         

            
        }

        private int? GetAbstractIDToView()
        {
            int id = -1;
            int? abstractId = null;
            if (Request.QueryString["AbstractID"] != null)
            {
                if (Int32.TryParse(Request.QueryString["AbstractID"].ToString(), out id))
                {
                    abstractId = id;
                }
            }

            return abstractId;

        }

        

        private void LoadData(string currentRole)
        {
            if (!String.IsNullOrEmpty(currentRole))
            {
                MembershipUser userCurrent = Membership.GetUser();
                int? abstractId = null;
                int? evaluationId_coder = null;
                int? evaluationId_odp = null;
                int i_abstractId = -1;
                int i_evaluationId_coder = -1;
                int i_evaluationId_odp = -1;
                bool isViewMode = false;
                tbl_Abstract abstr = null;
                AbstractStatusID currentStatus = AbstractStatusID.none;
                
                if (userCurrent != null)
                {
                    userCurrentName = userCurrent.UserName;
                }
                Guid userId = Common.GetCurrentUserId(connString, userCurrentName);
                hf_userId.Value = userId.ToString();

                //Coder
                if (currentRole == role_coder)
                {
                    GetAbstract_CoderEvaluation(userId);
                }

                //Coder Sup
                if (currentRole == role_coderSup)
                {
                    //Check User's intentions: /Evaluation/ViewAbstract.aspx?AbstractID=1
                    abstractId = GetAbstractIDToView();
                    if (abstractId != null)
                    {
                        i_abstractId = (int)abstractId;
                        isViewMode = true;
                    }

                    if (isViewMode)
                    {
                        //user wants to review
                        abstr = Common.GetAbstractByAbstractId(connString, i_abstractId);
                        if (abstr != null)
                        {
                            //Display abstract on screen 
                            LoadAbstract(abstr);
                            hf_abstractId.Value = i_abstractId.ToString();
                            //Get Evaluation ID for the current Abstract
                            evaluationId_coder = Common.GetEvaluationIdForAbstract(connString, i_abstractId, EvaluationType.CoderEvaluation);
                            if (evaluationId_coder != null)
                            {
                                i_evaluationId_coder = (int)evaluationId_coder;
                                hf_evaluationId_coder.Value = i_evaluationId_coder.ToString();
                            }
                            //Check abstract's status
                            currentStatus = Common.GetAbstractStatus(connString, i_abstractId);
                            if (currentStatus == AbstractStatusID._1 || currentStatus == AbstractStatusID._1A)
                            {
                                //Override action is available
                                pnl_overrideBtns.Visible = true;                                                             
                            }

                            if (currentStatus == AbstractStatusID._1B || currentStatus == AbstractStatusID._1N)
                            {
                                //Upload Notes action is available
                                pnl_uploadNotes.Visible = true;

                                GenerateLinks(EvaluationType.CoderEvaluation, i_evaluationId_coder, i_abstractId);
                                evaluationId_odp = Common.GetEvaluationIdForAbstract(connString, i_abstractId, EvaluationType.ODPEvaluation);
                                if (evaluationId_odp != null)
                                {
                                    i_evaluationId_odp = (int)evaluationId_odp;
                                    GenerateLinks(EvaluationType.ODPEvaluation, i_evaluationId_odp, i_abstractId);
                                }
                                
                                
                            }
                        }
                    }
                    else
                    {
                        //user wants to do coding
                        GetAbstract_CoderEvaluation(userId);
                        btn_code.Visible = true;
                    }
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

        

        

        

    }
}