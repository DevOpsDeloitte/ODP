using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using ODPTaxonomyDAL_ST;
using ODPTaxonomyWebsite.Evaluation.Classes;
using System.Text;
using ODPTaxonomyCommon;


namespace ODPTaxonomyWebsite.Evaluation.Controls
{
    public class TeamUser // Team user class
    {
        public Guid UserId {get;set;}
        public int TeamId { get; set; }
        public string TeamType { get; set; }
        public string UserName { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public int UserSubmissionID { get; set; }
    }

    public class ComparisonTeamUser
    {
        public Guid? UserId { get; set; }
        public int? TeamId { get; set; }
        public string TeamType { get; set; }
        public int ComparisonSubmissionID { get; set; }

    }



    public partial class EvaluationControl : System.Web.UI.UserControl
    {
        public string studyFocusQuestions = string.Empty;
        public string entitiesStudiedQuestions = string.Empty;
        public string studySettingsQuestions = string.Empty;
        public string populationFocusQuestions = string.Empty;
        public string studyDesignPurposeQuestions = string.Empty;
        public string preventionCategoryQuestions = string.Empty;
        public int AbstractID = Int16.MinValue;
        public int SubmissionID = Int16.MinValue;
        public int EvaluationID = Int16.MinValue;
        public int SubmissionTypeId = Int16.MinValue;
        public int SessionSubmissionTypeId = Int16.MinValue;
        public string applicationID = string.Empty;
        public string firstName = string.Empty;
        public string lastName = string.Empty;
        public string userName = string.Empty;
        public string projectTitle = string.Empty;
        public Guid UserId = new Guid();
        public string DisplayMode = string.Empty;
        public string FormMode = string.Empty;
        public string Comments = string.Empty;
        public bool showConsensusButton = false;
        public bool showComparisonButton = false;
        public string unableCoders = string.Empty;
        public bool consensusAlreadyStarted = false;

        public string isChecked = string.Empty;
    
        private DataDataContext db = null;
        private Dictionary<int, TeamUser> TeamUsers = new Dictionary<int, TeamUser>();
        private Dictionary<string, ComparisonTeamUser> ComparisonTeamUsers = new Dictionary<string, ComparisonTeamUser>();

        


        protected void Page_Load(object sender, EventArgs e)
        {
            db = DBData.GetDataContext();
            //System.Diagnostics.Trace.WriteLine("Eval Page Load Start...");
            loadSession();
            setAndrenderPageVars();
            unableCoders = getUnabletoCodeValues();
            renderStudyFocusQuestions();
            renderEntitiesStudiedQuestions();
            renderStudySettingsQuestions();
            renderPopulationFocusQuestions();
            renderStudyDesignPurposeQuestions();
            renderPreventionCategoryQuestions();
            db.Dispose();
            //System.Diagnostics.Trace.WriteLine("Eval Page Load End...");
        }

        protected Dictionary<int, TeamUser> getTeamUsers()
        {
            Dictionary<int, TeamUser> TeamUsers = new Dictionary<int, TeamUser>();
           
            var TeamID = db.Evaluations.Where(e => e.EvaluationId == EvaluationID && e.AbstractID == AbstractID).Select( e => e.TeamID).FirstOrDefault();
            //Response.Write(" Team ID :: " + TeamID.ToString());
            var teamlist = db.TeamUsers
                                .Join(db.Teams,
                                     tu => tu.TeamID, t => t.TeamID,
                                     (tu, t) => new
                                     {
                                         UserId = tu.UserId,
                                         TeamId = t.TeamID,
                                         TeamType = t.TeamType
                                     })
                                 .Where(t => t.TeamId == TeamID)
                                 .Join(db.aspnet_Users,
                                    tusers => tusers.UserId, u => u.UserId,
                                    (tusers, u) => new TeamUser
                                            {
                                                UserId = tusers.UserId,
                                                TeamId = tusers.TeamId,
                                                TeamType = tusers.TeamType.TeamType1,
                                                UserName = u.UserName,
                                                UserFirstName = u.UserFirstName,
                                                UserLastName = u.UserLastName,
                                                UserSubmissionID = 0

                                            });
            int counter=1;
            foreach( var t in teamlist){
               
                //Response.Write("<br> Team ID : " + t.TeamId.ToString() + " User ID : " + t.UserId.ToString() + " User Name : " + t.UserName.ToString());
                TeamUsers.Add(counter, t);
                counter++;
            }

            return TeamUsers;

        }

        protected void loadSession()
        {
            // load session variable here..

            if (Session["ViewAbstractToEvaluation"] != null)
            {
                ViewAbstractToEvaluation sv = (ViewAbstractToEvaluation)Session["ViewAbstractToEvaluation"];
                UserId = sv.UserId;
                SubmissionTypeId = sv.SubmissionTypeId;
                SessionSubmissionTypeId = sv.SubmissionTypeId;
                EvaluationID = sv.EvaluationId;
                //Response.Write(" SESSION PASSED VIEW MODE : " + sv.ViewMode);
                DisplayMode = sv.ViewMode.ToString() == "view" ? "View" : "Insert";
            }
            else
            {
                //lbl_messageUsers.Visible = true;
                //lbl_messageUsers.Text = "No Session";
                Response.Redirect("/");
                return;

            }



        }
        protected void loadAbstractInfo()
        {
            var evalrec = db.Evaluations.Where(e => e.EvaluationId == EvaluationID).Select(e => e).FirstOrDefault();
            if (evalrec != null)
            {
                AbstractID = Convert.ToInt32(evalrec.AbstractID);
                //Response.Write(" Abstract ID : " + AbstractID);
            }
            else
            {
                // We need to exit -- trouble.

            }

            var absrec = db.Abstracts.Where(a => a.AbstractID == AbstractID).Select(a => a).FirstOrDefault();
            if (absrec != null)
            {
                applicationID = absrec.ApplicationID.ToString();
                projectTitle = absrec.ProjectTitle.ToString();

            }
            var userrec = db.aspnet_Users.Where(u => u.UserId == UserId).Select(u => u).FirstOrDefault();
            if (userrec != null)
            {
                userName = userrec.UserName;
                firstName = userrec.UserFirstName;
                lastName = userrec.UserLastName;
            }

        }

        

        protected void performSecurityChecks()
        {
            var rec = DBData.getSubmissionRecord(UserId, EvaluationID, SubmissionTypeId);
            if (rec != null)
            {
                SubmissionID = rec.SubmissionID;
                Comments = rec.Comments;
                if (rec.UnableToCode)
                {
                    isChecked = "checked";
                }
            }
            else
            {
                SubmissionID = 0;
            }
            //SubmissionID = DBData.getSubmissionID(UserId, EvaluationID, SubmissionTypeId);
            //Response.Write("<br> Submission ID : " + SubmissionID);
            if (SubmissionID > 0)
            {
                //Is definitely view mode
                DisplayMode = "View";
                //Response.Write(" Is Coding :: " + DBData.isValidCoderEvaluation(UserId, EvaluationID, SubmissionTypeId).ToString() + " <br> ");

            }
            else
            {
                DisplayMode = "Insert";
            }

            switch (SubmissionTypeId)
            {
                case 1:
                    FormMode = "Coder Evaluation";
                    break;
                case 2:
                    FormMode = "Coder Consensus";
                    break;
                case 3:
                    FormMode = "ODP Staff Member Evaluation";
                    break;
                case 4:
                    FormMode = "ODP Staff Member Consensus";
                    break;
                case 5:
                    FormMode = "ODP Staff Member Comparison";
                    break;

            }

        }

        protected void startConsensus()
        {
            //Request.QueryString['EQCN'] ?? "default text";
            string startConsensus = string.IsNullOrEmpty(Request.QueryString["startConsenus"]) ? "" : Request.QueryString["startConsenus"];
            bool startC = startConsensus == "true" ? true : false;
            var orig = SubmissionTypeId;
            if (startC)
            {
                if (SubmissionTypeId == 1)
                {
                    SubmissionTypeId = 2;
                }

                if (SubmissionTypeId == 3)
                {
                    SubmissionTypeId = 4;
                }

                var eval = db.Evaluations.Where(e => e.EvaluationId == EvaluationID).FirstOrDefault();
                if (eval != null && DisplayMode != "View" && eval.ConsensusStartedBy == null)
                {
                    eval.ConsensusStartedBy = UserId;
                }
                ///
                if (eval != null && eval.ConsensusStartedBy != null)
                {
                    if (eval.ConsensusStartedBy != UserId)
                    {
                        // Give consensus already started message.
                        this.consensusAlreadyStarted = true;
                        SubmissionTypeId = orig;
                    }

                }
                db.SubmitChanges();

            }

        }

        protected void startComparison()
        {
            //Request.QueryString['EQCN'] ?? "default text";
            string startComparison = string.IsNullOrEmpty(Request.QueryString["startComparison"]) ? "" : Request.QueryString["startComparison"];
            bool startC = startComparison == "true" ? true : false;
            if (startC)
            {
                
                    SubmissionTypeId = 5;

            }

        }

        protected void showComparison()
        {

            // check if show comparison button can be shown
            var abschhistconsensuses = db.AbstractStatusChangeHistories.Where(a => a.AbstractID == AbstractID && (a.AbstractStatusID == 4 || a.AbstractStatusID == 9)).ToList();
            var abschhistcomparisons = db.AbstractStatusChangeHistories.Where(a => a.AbstractID == AbstractID && a.AbstractStatusID == 10).Any();
            string startComparison = string.IsNullOrEmpty(Request.QueryString["startComparison"]) ? "" : Request.QueryString["startComparison"];
            bool startC = startComparison == "true" ? true : false;
            var eval = db.Evaluations.Where(e => e.EvaluationId == EvaluationID).FirstOrDefault();
            if (abschhistconsensuses.Count == 2 && !abschhistcomparisons && !startC && eval.ConsensusStartedBy == UserId && (SubmissionTypeId == 4 || SubmissionTypeId == 3))
            {
                this.showComparisonButton = true;
            }
            else
            {
                this.showComparisonButton = false;
            }
            
        }

        protected void showConsensus()
        {
            // logic to show consensus button 
            if (SubmissionTypeId == 1)
            {
                var subList = db.Submissions.Where(s => s.EvaluationId == EvaluationID && s.SubmissionTypeId == 1).Select(s => s).ToList();
                if (subList.Count == 3) this.showConsensusButton = true;
               

            }

            if (SubmissionTypeId == 3)
            {
                var subList = db.Submissions.Where(s => s.EvaluationId == EvaluationID && s.SubmissionTypeId == 3).Select(s => s).ToList();
                if (subList.Count == 3) this.showConsensusButton = true;

            }

            // Check if Consensus has been already started..

            var eval = db.Evaluations.Where(e => e.EvaluationId == EvaluationID).FirstOrDefault();
            if (eval != null && showConsensusButton == true)
            {
                if (eval.ConsensusStartedBy != null)
                {
                    // check if the user id is the same & consensus was not complete :: button to be shown in this case.
                    var consensusRecord = db.Submissions.Where(s => s.EvaluationId == EvaluationID && (s.SubmissionTypeId == 2 || s.SubmissionTypeId == 4) && s.UserId == UserId).Any();
                    if (UserId == eval.ConsensusStartedBy && !consensusRecord)
                    {
                        this.showConsensusButton = true;
                    }
                    else
                    {
                        this.showConsensusButton = false;
                    }

                }
                else
                {

                    this.showConsensusButton = (eval.ConsensusStartedBy == null) ? true : false;
                }
            }

        }

        protected void setAndrenderPageVars()
        {

            
            
            
            //FormMode = "Consensus";
            //FormMode = "Comparison";
            //UserId = new Guid("3D44C55B-CAB7-4E1F-82BD-62B50B97619E");


            // For testing view mode / coder evaluation
            //DisplayMode = "View"; FormMode = "Coder Evaluation"; AbstractID = 1; SubmissionID = 1; SubmissionTypeId = 1; EvaluationID = 1;
            // For testing insert mode / coder evaluation
            //DisplayMode = "Insert"; FormMode = "Coder Evaluation"; AbstractID = 1; SubmissionID = 1; SubmissionTypeId = 1; EvaluationID = 1;

            
            
            // For testing view mode / coder consensus
            //DisplayMode = "View"; FormMode = "Coder Consensus"; AbstractID = 1; SubmissionID = 1; SubmissionTypeId = 2; EvaluationID = 1;
            // For testing insert mode / coder consensus
            //DisplayMode = "Insert"; FormMode = "Coder Consensus"; AbstractID = 1; SubmissionID = 1; SubmissionTypeId = 2; EvaluationID = 1;

            // We need to find out for Coder Consensus
            // 1. All the team members, with their User IDs and Numbers 1, 2 or 3.
            // 2. All their submissions



            //UserId = new Guid("9C334C38-E22D-46D4-B37A-06E2F5405ED7");
       
            // For testing view mode / ODP Staff Member Evaluation
            //DisplayMode = "View"; FormMode = "ODP Staff Member Evaluation"; AbstractID = 3; SubmissionID = 21; SubmissionTypeId = 3; EvaluationID = 11;
            // For testing insert mode / ODP Staff Member Evaluation
            //DisplayMode = "Insert"; FormMode = "ODP Staff Member Evaluation"; AbstractID = 3; SubmissionID = 21; SubmissionTypeId = 3; EvaluationID = 11;

            // For testing view mode / ODP Staff Member Consensus
            //DisplayMode = "View"; FormMode = "ODP Staff Member Consensus"; AbstractID = 3; SubmissionID = 21; SubmissionTypeId = 4; EvaluationID = 11;
            // For testing insert mode / ODP Staff Member Evaluation
            //DisplayMode = "Insert"; FormMode = "ODP Staff Member Consensus"; AbstractID = 3; SubmissionID = 21; SubmissionTypeId = 4; EvaluationID = 11;

            // For testing insert mode / ODP Staff Member Comparison
            //DisplayMode = "Insert"; FormMode = "ODP Staff Member Comparison"; AbstractID = 3; SubmissionID = 21; SubmissionTypeId = 5; EvaluationID = 11;
            //loadSession();
            startConsensus();
            startComparison();
            performSecurityChecks();
            loadAbstractInfo();
            showConsensus();
            showComparison();



            if (FormMode.IndexOf("Consensus") != -1)
            {
                TeamUsers = getTeamUsers();
                int submissiontypesEval = 1; // default coder evaluation
                submissiontypesEval = FormMode.IndexOf("ODP Staff Member") != -1 ? 3 : 1;  // ODP Staff = 3

                foreach( var tu in TeamUsers ){
                    tu.Value.UserSubmissionID = db.Submissions.Where(s => s.EvaluationId == EvaluationID && s.SubmissionTypeId == submissiontypesEval && s.UserId == tu.Value.UserId).Select(s => s.SubmissionID).FirstOrDefault();
                }
             
            }

            if (FormMode.IndexOf("Comparison") != -1)
            {
                var comparisonTeams = db.Evaluations
                                         .Where(e => e.AbstractID == AbstractID /*&& e.IsComplete*/ && e.ConsensusStartedBy.HasValue)
                                         .Select(e => new { e.TeamID, e.ConsensusStartedBy, e.EvaluationId }).ToList();
                //Dictionary<Guid?, int?> cteamusers = new Dictionary<Guid?, int?>();
                //Response.Write(" Comparison Team Count : " + comparisonTeams.Count.ToString()+ "<br>");
                if (comparisonTeams.Count == 2)
                {   // we are all good
                    // need to get team type.
                    // get the submission ID of the user that did consensus ( submission type 2 & 4 ) for both teams.
                    foreach (var cteam in comparisonTeams)
                    {
                        //cteamusers.Add(cteam.ConsensusStartedBy, cteam.TeamID);
                        var TeamType = db.Teams
                                            .Join(db.TeamTypes,
                                                      t => t.TeamTypeID , tt => tt.TeamTypeID,
                                                      (t,tt) => 
                                                          new { TeamID = t.TeamID, TeamType = tt.TeamType1, TeamStatusID = t.StatusID }
                                                 )
                                                 .Where(tx => tx.TeamID == cteam.TeamID ).FirstOrDefault();

                                                //Please add TeamStatusID ==1
                        if(TeamType.TeamType == "Coder"){
                            var rec = db.Submissions.Where(sb => /*sb.UnableToCode == false &&*/ sb.UserId == cteam.ConsensusStartedBy && sb.SubmissionTypeId == 2 && sb.EvaluationId == cteam.EvaluationId ).Select(sb => sb).FirstOrDefault();
                            if (rec != null)
                            {
                                ComparisonTeamUsers.Add(TeamType.TeamType, new ComparisonTeamUser { UserId = cteam.ConsensusStartedBy, TeamId = cteam.TeamID, TeamType = TeamType.TeamType, ComparisonSubmissionID = rec.SubmissionID });
                            }
                        }
                        if (TeamType.TeamType == "ODP Staff")
                        {
                            var rec = db.Submissions.Where(sb => /*sb.UnableToCode == false &&*/ sb.UserId == cteam.ConsensusStartedBy && sb.SubmissionTypeId == 4 && sb.EvaluationId == cteam.EvaluationId).Select(sb => sb).FirstOrDefault();
                            if (rec != null)
                            {
                                ComparisonTeamUsers.Add(TeamType.TeamType, new ComparisonTeamUser { UserId = cteam.ConsensusStartedBy, TeamId = cteam.TeamID, TeamType = TeamType.TeamType, ComparisonSubmissionID = rec.SubmissionID });
                            }
                        }

                        
                        //Response.Write(" Comparison Team   -- Consensus Started by : " + cteam.ConsensusStartedBy + "   Team Type : " + TeamType.TeamType + "<br>");


                    }
                    

                }

                else
                {
                    //Comparison can't be done. No two consensuses exist.

                    //Response.Write(" NO COMPARISON -- Comparison Team Count : " + comparisonTeams.Count.ToString() + "<br>");
                }

                foreach (var ct in ComparisonTeamUsers)
                {
                    //Response.Write(" Comparison Team  :::: " + ct.Value.ComparisonSubmissionID + "   Team Type : " + ct.Value.TeamType + "<br>");
                }
                

            }



            // NO Submission ID should exist for UserId / EvaluationID / SubmissionTypeId ( This will put the form in View Mode automatically. )


            

        }


        protected string[] getComparerValues(string Type, int ID)
        {
            string[] retval = null;
            //int submissiontypeid = 1; // for retreiving coder submissions.


            if (Type == "A_StudyFocus")
            {
                retval = new string[] { "", "", "" };
            }
            else
            {
                retval = new string[] { "" };
            }
            if (FormMode.IndexOf("Comparison") != -1)
            {
                int CoderTeamSubmissionID = 0;          
                int ODPTeamSubmissionID = 0;
                int? CoderTeamID = 0;          
                int? ODPTeamID = 0;
                try
                {
                    CoderTeamSubmissionID = ComparisonTeamUsers["Coder"].ComparisonSubmissionID;
                    CoderTeamID =  ComparisonTeamUsers["Coder"].TeamId;

                }
                catch (Exception ex) { }
                try
                {

                    ODPTeamSubmissionID = ComparisonTeamUsers["ODP Staff"].ComparisonSubmissionID;
                    ODPTeamID = ComparisonTeamUsers["ODP Staff"].TeamId;
                }
                catch (Exception ex) { }

                //Response.Write(" ODP TEAM Submission ID : "+ODPTeamSubmissionID);

                if (Type == "A_StudyFocus")
                {

                    var rec1 = db.A_StudyFocusAnswers.Where(x => x.SubmissionID == CoderTeamSubmissionID && x.StudyFocusID == ID).Select(x => x).FirstOrDefault();
                    var rec2 = db.A_StudyFocusAnswers.Where(x => x.SubmissionID == ODPTeamSubmissionID && x.StudyFocusID == ID).Select(x => x).FirstOrDefault();
                    if (rec1 != null)
                    {
                        retval[0] = rec1.StudyFocus_A1 ? "c-"+CoderTeamID.ToString() : "";
                        retval[1] = rec1.StudyFocus_A2 ? "c-" + CoderTeamID.ToString() : "";
                        retval[2] = rec1.StudyFocus_A3 ? "c-" + CoderTeamID.ToString() : "";
                    }

                    if (rec2 != null)
                    {

                        retval[0] = rec2.StudyFocus_A1 ? retval[0] + "," + "o-" + ODPTeamID.ToString() : "";
                        retval[1] = rec2.StudyFocus_A2 ? retval[1] + "," + "o-" + ODPTeamID.ToString() : "";
                        retval[2] = rec2.StudyFocus_A3 ? retval[2] + "," + "o-" + ODPTeamID.ToString() : "";
                    }


                    retval[0] = replaceComma(retval[0]);
                    retval[1] = replaceComma(retval[1]);
                    retval[2] = replaceComma(retval[2]);



                }

                if (Type == "B_EntitiesStudied")
                {

                    var rec1 = db.B_EntitiesStudiedAnswers.Where(x => x.SubmissionID == CoderTeamSubmissionID && x.EntitiesStudiedID == ID).Select(x => x).FirstOrDefault();
                    var rec2 = db.B_EntitiesStudiedAnswers.Where(x => x.SubmissionID == ODPTeamSubmissionID && x.EntitiesStudiedID == ID).Select(x => x).FirstOrDefault();
                    if (rec1 != null)
                    {
                        retval[0] = "c-" + CoderTeamID.ToString();

                    }

                    if (rec2 != null)
                    {

                        retval[0] = "," + "o-" + ODPTeamID.ToString();

                    }


                    retval[0] = replaceComma(retval[0]);

                 }

                if (Type == "C_StudySettings")
                {

                    var rec1 = db.C_StudySettingAnswers.Where(x => x.SubmissionID == CoderTeamSubmissionID && x.StudySettingID == ID).Select(x => x).FirstOrDefault();
                    var rec2 = db.C_StudySettingAnswers.Where(x => x.SubmissionID == ODPTeamSubmissionID && x.StudySettingID == ID).Select(x => x).FirstOrDefault();
                    if (rec1 != null)
                    {
                        retval[0] = "c-" + CoderTeamID.ToString();

                    }

                    if (rec2 != null)
                    {

                        retval[0] = "," + "o-" + ODPTeamID.ToString();

                    }


                    retval[0] = replaceComma(retval[0]);

                }

                if (Type == "D_PopulationFocus")
                {

                    var rec1 = db.D_PopulationFocusAnswers.Where(x => x.SubmissionID == CoderTeamSubmissionID && x.PopulationFocusID == ID).Select(x => x).FirstOrDefault();
                    var rec2 = db.D_PopulationFocusAnswers.Where(x => x.SubmissionID == ODPTeamSubmissionID && x.PopulationFocusID == ID).Select(x => x).FirstOrDefault();
                    if (rec1 != null)
                    {
                        retval[0] = "c-" + CoderTeamID.ToString();

                    }

                    if (rec2 != null)
                    {

                        retval[0] = "," + "o-" + ODPTeamID.ToString();

                    }


                    retval[0] = replaceComma(retval[0]);

                }

                if (Type == "E_StudyDesignPurpose")
                {

                    var rec1 = db.E_StudyDesignPurposeAnswers.Where(x => x.SubmissionID == CoderTeamSubmissionID && x.StudyDesignPurposeID == ID).Select(x => x).FirstOrDefault();
                    var rec2 = db.E_StudyDesignPurposeAnswers.Where(x => x.SubmissionID == ODPTeamSubmissionID && x.StudyDesignPurposeID == ID).Select(x => x).FirstOrDefault();
                    if (rec1 != null)
                    {
                        retval[0] = "c-" + CoderTeamID.ToString();

                    }

                    if (rec2 != null)
                    {

                        retval[0] = "," + "o-" + ODPTeamID.ToString();

                    }


                    retval[0] = replaceComma(retval[0]);

                }


                if (Type == "F_PreventionCategory")
                {

                    var rec1 = db.F_PreventionCategoryAnswers.Where(x => x.SubmissionID == CoderTeamSubmissionID && x.PreventionCategoryID == ID).Select(x => x).FirstOrDefault();
                    var rec2 = db.F_PreventionCategoryAnswers.Where(x => x.SubmissionID == ODPTeamSubmissionID && x.PreventionCategoryID == ID).Select(x => x).FirstOrDefault();
                    if (rec1 != null)
                    {
                        retval[0] = "c-" + CoderTeamID.ToString();

                    }

                    if (rec2 != null)
                    {

                        retval[0] = "," + "o-" + ODPTeamID.ToString();

                    }


                    retval[0] = replaceComma(retval[0]);

                }


            }


            return retval;

        }


        protected string getUnabletoCodeValues()
        {
            StringBuilder usersUnable = new StringBuilder();

            if (FormMode.IndexOf("Consensus") != -1)
            {
                var coder1 = db.Submissions.Where(x => x.SubmissionID == TeamUsers[1].UserSubmissionID && x.UnableToCode == true).Any();
                if (coder1)
                {
                    usersUnable.Append(TeamUsers[1].UserName);
                   
                }
                var coder2 = db.Submissions.Where(x => x.SubmissionID == TeamUsers[2].UserSubmissionID && x.UnableToCode == true).Any();
                if (coder2)
                {
                    usersUnable.Append(", ");
                    usersUnable.Append(TeamUsers[2].UserName);
                }
                var coder3 = db.Submissions.Where(x => x.SubmissionID == TeamUsers[3].UserSubmissionID && x.UnableToCode == true).Any();
                if (coder3)
                {
                    usersUnable.Append(", ");
                    usersUnable.Append(TeamUsers[3].UserName);
                }

            }

            if (FormMode.IndexOf("Comparison") != -1)
            {
                int CoderTeamSubmissionID = 0;
                int ODPTeamSubmissionID = 0;
                int? CoderTeamID = 0;
                int? ODPTeamID = 0;
                try
                {
                    CoderTeamSubmissionID = ComparisonTeamUsers["Coder"].ComparisonSubmissionID;
                    CoderTeamID = ComparisonTeamUsers["Coder"].TeamId;

                }
                catch (Exception ex) {
                    Response.Write(ex.Message + " Unable to get Coder Team ID ");
                }
                try
                {

                    ODPTeamSubmissionID = ComparisonTeamUsers["ODP Staff"].ComparisonSubmissionID;
                    ODPTeamID = ComparisonTeamUsers["ODP Staff"].TeamId;
                }
                catch (Exception ex) {
                    Response.Write(ex.Message + " Unable to get ODP Team ID ");
                }
                var team1 = db.Submissions.Where(x => x.SubmissionID == CoderTeamSubmissionID && x.UnableToCode == true).Any();
                if (team1)
                {
                    //usersUnable.Append("Coder Consensus : "+CoderTeamID);
                    usersUnable.Append("Coder Consensus");

                }
                var team2 = db.Submissions.Where(x => x.SubmissionID == ODPTeamSubmissionID && x.UnableToCode == true).Any();
                if (team2)
                {
                    usersUnable.Append(", ");
                    //usersUnable.Append(ODPTeamID);
                    usersUnable.Append("ODP Consensus");
                }
              

            }
            if (usersUnable.ToString().IndexOf(", ") == 0) return usersUnable.ToString().Substring(2);
            else return usersUnable.ToString();

        }

        protected string[] getCoderValues(string Type, int ID)
        {
            string[] retval = null;
            //int submissiontypeid = 1; // for retreiving coder submissions.


            if (Type == "A_StudyFocus")
            {
                retval = new string[] { "", "", "" };
            }
            else
            {
                retval = new string[] { "" };
            }

            if (FormMode.IndexOf("Consensus") != -1)
            {
                var uid1 = (Guid)TeamUsers[1].UserId; var uname1 = (string)TeamUsers[1].UserName;

                //Response.Write(uid1 + "     " + uname1 + "    " + TeamUsers[1].UserSubmissionID + "   <br>");

                if (Type == "A_StudyFocus")
                {
                    var rec1 = db.A_StudyFocusAnswers.Where(x => x.SubmissionID == TeamUsers[1].UserSubmissionID && x.StudyFocusID == ID).Select(x => x).FirstOrDefault();
                    var rec2 = db.A_StudyFocusAnswers.Where(x => x.SubmissionID == TeamUsers[2].UserSubmissionID && x.StudyFocusID == ID).Select(x => x).FirstOrDefault();
                    var rec3 = db.A_StudyFocusAnswers.Where(x => x.SubmissionID == TeamUsers[3].UserSubmissionID && x.StudyFocusID == ID).Select(x => x).FirstOrDefault();

                    if (rec1 != null)
                    {
                        retval[0] = rec1.StudyFocus_A1 ? TeamUsers[1].UserName : "";
                        retval[1] = rec1.StudyFocus_A2 ? TeamUsers[1].UserName : "";
                        retval[2] = rec1.StudyFocus_A3 ? TeamUsers[1].UserName : "";
                    }

                    if (rec2 != null)
                    {
                        retval[0] = rec2.StudyFocus_A1 ? retval[0] + ","+TeamUsers[2].UserName : retval[0] + "";
                        retval[1] = rec2.StudyFocus_A2 ? retval[1] + "," + TeamUsers[2].UserName : retval[1] + "";
                        retval[2] = rec2.StudyFocus_A3 ? retval[2] + "," + TeamUsers[2].UserName : retval[2] + "";
                    }

                    if (rec3 != null)
                    {
                        retval[0] = rec3.StudyFocus_A1 ? retval[0] + "," + TeamUsers[3].UserName : retval[0] + "";
                        retval[1] = rec3.StudyFocus_A2 ? retval[1] + "," + TeamUsers[3].UserName : retval[1] + "";
                        retval[2] = rec3.StudyFocus_A3 ? retval[2] + "," + TeamUsers[3].UserName : retval[2] + "";
                    }

                    retval[0] = replaceComma(retval[0]);
                    retval[1] = replaceComma(retval[1]);
                    retval[2] = replaceComma(retval[2]);
                }

                if (Type == "B_EntitiesStudied")
                {
                    var rec1 = db.B_EntitiesStudiedAnswers.Where(x => x.SubmissionID == TeamUsers[1].UserSubmissionID && x.EntitiesStudiedID == ID).Select(x => x).FirstOrDefault();
                    var rec2 = db.B_EntitiesStudiedAnswers.Where(x => x.SubmissionID == TeamUsers[2].UserSubmissionID && x.EntitiesStudiedID == ID).Select(x => x).FirstOrDefault();
                    var rec3 = db.B_EntitiesStudiedAnswers.Where(x => x.SubmissionID == TeamUsers[3].UserSubmissionID && x.EntitiesStudiedID == ID).Select(x => x).FirstOrDefault();
                    if (rec1 != null)
                    {
                        retval[0] = TeamUsers[1].UserName;

                    }
                    if (rec2 != null)
                    {
                        retval[0] =  retval[0] + ","+ TeamUsers[2].UserName;

                    }
                    if (rec3 != null)
                    {
                        retval[0] = retval[0] + "," + TeamUsers[3].UserName;

                    }

                    retval[0] = replaceComma(retval[0]);

                }

                if (Type == "C_StudySettings")
                {
                    var rec1 = db.C_StudySettingAnswers.Where(x => x.SubmissionID == TeamUsers[1].UserSubmissionID && x.StudySettingID == ID).Select(x => x).FirstOrDefault();
                    var rec2 = db.C_StudySettingAnswers.Where(x => x.SubmissionID == TeamUsers[2].UserSubmissionID && x.StudySettingID == ID).Select(x => x).FirstOrDefault();
                    var rec3 = db.C_StudySettingAnswers.Where(x => x.SubmissionID == TeamUsers[3].UserSubmissionID && x.StudySettingID == ID).Select(x => x).FirstOrDefault();
                    if (rec1 != null)
                    {
                        retval[0] = TeamUsers[1].UserName;

                    }
                    if (rec2 != null)
                    {
                        retval[0] = retval[0] + "," + TeamUsers[2].UserName;

                    }
                    if (rec3 != null)
                    {
                        retval[0] = retval[0] + "," + TeamUsers[3].UserName;

                    }

                    retval[0] = replaceComma(retval[0]);

                }
                if (Type == "D_PopulationFocus")
                {
                    var rec1 = db.D_PopulationFocusAnswers.Where(x => x.SubmissionID == TeamUsers[1].UserSubmissionID && x.PopulationFocusID == ID).Select(x => x).FirstOrDefault();
                    var rec2 = db.D_PopulationFocusAnswers.Where(x => x.SubmissionID == TeamUsers[2].UserSubmissionID && x.PopulationFocusID == ID).Select(x => x).FirstOrDefault();
                    var rec3 = db.D_PopulationFocusAnswers.Where(x => x.SubmissionID == TeamUsers[3].UserSubmissionID && x.PopulationFocusID == ID).Select(x => x).FirstOrDefault();
                    if (rec1 != null)
                    {
                        retval[0] = TeamUsers[1].UserName;

                    }
                    if (rec2 != null)
                    {
                        retval[0] = retval[0] + "," + TeamUsers[2].UserName;

                    }
                    if (rec3 != null)
                    {
                        retval[0] = retval[0] + "," + TeamUsers[3].UserName;

                    }

                    retval[0] = replaceComma(retval[0]);

                }

                if (Type == "E_StudyDesignPurpose")
                {
                    var rec1 = db.E_StudyDesignPurposeAnswers.Where(x => x.SubmissionID == TeamUsers[1].UserSubmissionID && x.StudyDesignPurposeID == ID).Select(x => x).FirstOrDefault();
                    var rec2 = db.E_StudyDesignPurposeAnswers.Where(x => x.SubmissionID == TeamUsers[2].UserSubmissionID && x.StudyDesignPurposeID == ID).Select(x => x).FirstOrDefault();
                    var rec3 = db.E_StudyDesignPurposeAnswers.Where(x => x.SubmissionID == TeamUsers[3].UserSubmissionID && x.StudyDesignPurposeID == ID).Select(x => x).FirstOrDefault();
                    if (rec1 != null)
                    {
                        retval[0] = TeamUsers[1].UserName;

                    }
                    if (rec2 != null)
                    {
                        retval[0] = retval[0] + "," + TeamUsers[2].UserName;

                    }
                    if (rec3 != null)
                    {
                        retval[0] = retval[0] + "," + TeamUsers[3].UserName;

                    }

                    retval[0] = replaceComma(retval[0]);

                }

                if (Type == "F_PreventionCategory")
                {
                    var rec1 = db.F_PreventionCategoryAnswers.Where(x => x.SubmissionID == TeamUsers[1].UserSubmissionID && x.PreventionCategoryID == ID).Select(x => x).FirstOrDefault();
                    var rec2 = db.F_PreventionCategoryAnswers.Where(x => x.SubmissionID == TeamUsers[2].UserSubmissionID && x.PreventionCategoryID  == ID).Select(x => x).FirstOrDefault();
                    var rec3 = db.F_PreventionCategoryAnswers.Where(x => x.SubmissionID == TeamUsers[3].UserSubmissionID && x.PreventionCategoryID  == ID).Select(x => x).FirstOrDefault();
                    if (rec1 != null)
                    {
                        retval[0] = TeamUsers[1].UserName;

                    }
                    if (rec2 != null)
                    {
                        retval[0] = retval[0] + "," + TeamUsers[2].UserName;

                    }
                    if (rec3 != null)
                    {
                        retval[0] = retval[0] + "," + TeamUsers[3].UserName;

                    }

                    retval[0] = replaceComma(retval[0]);

                }  

            }
           
            return retval;
        }

        protected string replaceComma(string instr)
        {
            if (instr.IndexOf(',') == 0) return instr.Substring(1);
            else return instr;
        }

        protected string[] getViewValues(string Type, int ID)
        {
            string[] retval = null;

                if(Type == "A_StudyFocus"){
                    retval = new string[] { "no", "no", "no" };
                }
                else{
                    retval = new string[] {"no"};
                }

       
            //FormMode, DisplayMode

            if (DisplayMode == "View")
            {
                if (FormMode.IndexOf("Evaluation") != -1 || FormMode.IndexOf("Consensus") != -1 || FormMode.IndexOf("Comparison") != -1) // Currently viewing a Evaluation by UserID.
                {
                    if (Type == "A_StudyFocus")
                    {
                        var rec = db.A_StudyFocusAnswers.Where(x => x.SubmissionID == SubmissionID && x.StudyFocusID == ID).Select(x => x).FirstOrDefault();
                        if (rec != null)
                        {
                            retval[0] = rec.StudyFocus_A1 ? "yes" : "no";
                            retval[1] = rec.StudyFocus_A2 ? "yes" : "no";
                            retval[2] = rec.StudyFocus_A3 ? "yes" : "no";
                        }
                    }

                    if (Type == "B_EntitiesStudied")
                    {
                        var rec = db.B_EntitiesStudiedAnswers.Where(x => x.SubmissionID == SubmissionID && x.EntitiesStudiedID == ID).Select(x => x).FirstOrDefault();
                        if (rec != null)
                        {
                            retval[0] = "yes";

                        }
                    }


                    if (Type == "C_StudySettings")
                    {
                        var rec = db.C_StudySettingAnswers.Where(x => x.SubmissionID == SubmissionID && x.StudySettingID == ID).Select(x => x).FirstOrDefault();
                        if (rec != null)
                        {
                            retval[0] = "yes";

                        }
                    }


                    if (Type == "D_PopulationFocus")
                    {
                        var rec = db.D_PopulationFocusAnswers.Where(x => x.SubmissionID == SubmissionID && x.PopulationFocusID == ID).Select(x => x).FirstOrDefault();
                        if (rec != null)
                        {
                            retval[0] = "yes";

                        }
                    }

                    if (Type == "E_StudyDesignPurpose")
                    {
                        var rec = db.E_StudyDesignPurposeAnswers.Where(x => x.SubmissionID == SubmissionID && x.StudyDesignPurposeID == ID).Select(x => x).FirstOrDefault();
                        if (rec != null)
                        {
                            retval[0] = "yes";

                        }
                    }

                    if (Type == "F_PreventionCategory")
                    {
                        var rec = db.F_PreventionCategoryAnswers.Where(x => x.SubmissionID == SubmissionID && x.PreventionCategoryID == ID).Select(x => x).FirstOrDefault();
                        if (rec != null)
                        {
                            retval[0] = "yes";

                        }
                    }
     
     
             
                }

            }

            return retval;

        }

        protected void renderStudyFocusQuestions()
        {
            
            var questions = db.A_StudyFocus.Where(sf => sf.Status.Status1 == "Active" ).OrderBy( sf => sf.Sort).Select(sf => sf).ToList();
            StringBuilder finalStr = new StringBuilder();
            var count = 1;
            foreach(var question in questions){
                var getViewVals = getViewValues("A_StudyFocus", question.StudyFocusID);
                var getCoderVals = getCoderValues("A_StudyFocus", question.StudyFocusID);
                var getComparerVals = getComparerValues("A_StudyFocus", question.StudyFocusID);
                StringBuilder row = new StringBuilder();
                row.AppendLine("<tr>");
                if (question.A1_IsEnabled || question.A2_IsEnabled || question.A3_IsEnabled)
                {
                    row.AppendLine("<td scope=\"row\">" + question.StudyFocusID.ToString() + ". " + question.StudyFocus + "<div class=\"icon open\" ng-click=\"showDescription('studyfocus-" + question.StudyFocusID.ToString() + "')\"></div>"+"</td>");
                }
                else
                {
                    row.AppendLine("<td scope=\"row\">" + question.StudyFocusID.ToString() + ". " + question.StudyFocus + "</td>");
                }
                
                row.AppendLine("<td class=\"box-three\"><div outcome-box=\"mdata.studyfocus[1][" + question.StudyFocusID + "]\" is-checked='"+getViewVals[0]+"' show-coders='"+getCoderVals[0]+"' show-comparers='"+getComparerVals[0]+"' is-enabled='" + (question.A1_IsEnabled == true ? "yes" : "no") + "' name=\"studyfocus-" + question.StudyFocusID + "-1\" data-cat-id=\"studyfocus\" data-q-id =\"" + question.StudyFocusID + "-1\"></div></td>");
                row.AppendLine("<td class=\"box-three\"><div outcome-box=\"mdata.studyfocus[2][" + question.StudyFocusID + "]\" is-checked='" + getViewVals[1] + "'  show-coders='" + getCoderVals[1] + "' show-comparers='" + getComparerVals[1] + "' is-enabled='" + (question.A2_IsEnabled == true ? "yes" : "no") + "' name=\"studyfocus-" + question.StudyFocusID + "-2\" data-cat-id=\"studyfocus\" data-q-id =\"" + question.StudyFocusID + "-2\"></div></td>");
                row.AppendLine("<td class=\"box-three\"><div outcome-box=\"mdata.studyfocus[3][" + question.StudyFocusID + "]\" is-checked='" + getViewVals[2] + "'  show-coders='" + getCoderVals[2] + "' show-comparers='" + getComparerVals[2] + "' is-enabled='" + (question.A3_IsEnabled == true ? "yes" : "no") + "' name=\"studyfocus-" + question.StudyFocusID + "-3\" data-cat-id=\"studyfocus\" data-q-id =\"" + question.StudyFocusID + "-3\"></div></td>");
              
                 row.AppendLine("</tr>");
                finalStr.Append(row);
                //if (count > 0) break;
                count++;
            }

            studyFocusQuestions = finalStr.ToString();

        }

        protected void renderEntitiesStudiedQuestions()
        {
            //var db = DBData.GetDataContext();
            var questions = db.B_EntitiesStudieds.Where(sf => sf.Status.Status1 == "Active").OrderBy(sf => sf.Sort).Select(sf => sf).ToList();
            StringBuilder finalStr = new StringBuilder();
            foreach (var question in questions)
            {
                var getViewVals = getViewValues("B_EntitiesStudied", question.EntitiesStudiedID);
                var getCoderVals = getCoderValues("B_EntitiesStudied", question.EntitiesStudiedID);
                var getComparerVals = getComparerValues("B_EntitiesStudied", question.EntitiesStudiedID);
                StringBuilder row = new StringBuilder();
                row.AppendLine("<tr>");
                row.AppendLine("<td scope=\"row\">" + question.EntitiesStudiedID.ToString() + ". " + question.EntitiesStudied + "<div class=\"icon open\" ng-click=\"showDescription('entitiesstudied-" + question.EntitiesStudiedID.ToString() + "')\"><div>" + "</td>");
                row.AppendLine("<td class=\"box-three big\"><div outcome-box=\"mdata.entitiesstudied[" + question.EntitiesStudiedID + "]\" is-checked='" + getViewVals[0] + "'  show-coders='" + getCoderVals[0] + "' show-comparers='" + getComparerVals[0] + "' is-enabled='yes' " + " name=\"entitiesstudied-" + question.EntitiesStudiedID + "\" data-cat-id=\"entitiesstudied\" data-q-id =\"" + question.EntitiesStudiedID + "\"></div></td>");
                
                row.AppendLine("</tr>");
                finalStr.Append(row);
            }


            entitiesStudiedQuestions = finalStr.ToString();

        }

        protected void renderStudySettingsQuestions()
        {
            //var db = DBData.GetDataContext();
            var questions = db.C_StudySettings.Where(sf => sf.Status.Status1 == "Active").OrderBy(sf => sf.Sort).Select(sf => sf).ToList();
            StringBuilder finalStr = new StringBuilder();
            foreach (var question in questions)
            {
                var getViewVals = getViewValues("C_StudySettings", question.StudySettingID);
                var getCoderVals = getCoderValues("C_StudySettings", question.StudySettingID);
                var getComparerVals = getComparerValues("C_StudySettings", question.StudySettingID);
                StringBuilder row = new StringBuilder();
                row.AppendLine("<tr>");
                row.AppendLine("<td scope=\"row\">"  + question.StudySettingID.ToString() + ". " + question.StudySetting + "<div class=\"icon open\" ng-click=\"showDescription('studysetting-" + question.StudySettingID.ToString() + "')\"></div>"+"</td>");
                row.AppendLine("<td class=\"box-three big\"><div outcome-box=\"mdata.studysetting[" + question.StudySettingID + "]\"  is-checked='" + getViewVals[0] + "' show-coders='" + getCoderVals[0] + "' show-comparers='" + getComparerVals[0] + "' is-enabled='yes' " + " name=\"studysetting-" + question.StudySettingID + "\" data-cat-id=\"studysetting\" data-q-id =\"" + question.StudySettingID + "\"></div></td>");
                
                row.AppendLine("</tr>");
                finalStr.Append(row);
            }


            studySettingsQuestions = finalStr.ToString();
        }

        protected void renderPopulationFocusQuestions()
        {
            //var db = new DBDataContext();
            var questions = db.D_PopulationFocus.Where(sf => sf.Status.Status1 == "Active").OrderBy(sf => sf.Sort).Select(sf => sf).ToList();
            StringBuilder finalStr = new StringBuilder();
            foreach (var question in questions)
            {
                var getViewVals = getViewValues("D_PopulationFocus", question.PopulationFocusID);
                var getCoderVals = getCoderValues("D_PopulationFocus", question.PopulationFocusID);
                var getComparerVals = getComparerValues("D_PopulationFocus", question.PopulationFocusID);
                StringBuilder row = new StringBuilder();
                row.AppendLine("<tr>");
                row.AppendLine("<td scope=\"row\">" + question.PopulationFocusID.ToString() + ". " + question.PopulationFocus +"<div class=\"icon open\" ng-click=\"showDescription('populationfocus-" + question.PopulationFocusID.ToString() + "')\"></div>"+ "</td>");
                row.AppendLine("<td class=\"box-three big\"><div outcome-box=\"mdata.populationfocus[" + question.PopulationFocusID + "]\"  is-checked='" + getViewVals[0] + "' show-coders='" + getCoderVals[0] + "' show-comparers='" + getComparerVals[0] + "' is-enabled='yes' " + " name=\"populationfocus-" + question.PopulationFocusID + "\" data-cat-id=\"populationfocus\" data-q-id =\"" + question.PopulationFocusID + "\"></div></td>");
                
                row.AppendLine("</tr>");
                finalStr.Append(row);
            }


            populationFocusQuestions = finalStr.ToString();

        }
        protected void renderStudyDesignPurposeQuestions()
        {
            //var db = new DBDataContext();
            var questions = db.E_StudyDesignPurposes.Where(sf => sf.Status.Status1 == "Active").OrderBy(sf => sf.Sort).Select(sf => sf).ToList();
            StringBuilder finalStr = new StringBuilder();
            foreach (var question in questions)
            {
                var getViewVals = getViewValues("E_StudyDesignPurpose", question.StudyDesignPurposeID);
                var getCoderVals = getCoderValues("E_StudyDesignPurpose", question.StudyDesignPurposeID);
                var getComparerVals = getComparerValues("E_StudyDesignPurpose", question.StudyDesignPurposeID);
                StringBuilder row = new StringBuilder();
                row.AppendLine("<tr>");
                row.AppendLine("<td scope=\"row\">" + question.StudyDesignPurposeID.ToString() + ". " + question.StudyDesignPurpose + "<div class=\"icon open\" ng-click=\"showDescription('studydesignpurpose-" + question.StudyDesignPurposeID.ToString() + "')\"></div>"+"</td>");
                row.AppendLine("<td class=\"box-three big\"><div outcome-box=\"mdata.studydesignpurpose[" + question.StudyDesignPurposeID + "]\"  is-checked='" + getViewVals[0] + "' show-coders='" + getCoderVals[0] + "' show-comparers='" + getComparerVals[0] + "' is-enabled='yes' " + " name=\"studydesignpurpose-" + question.StudyDesignPurposeID + "\" data-cat-id=\"studydesignpurpose\" data-q-id =\"" + question.StudyDesignPurposeID + "\"></div></td>");
                
                row.AppendLine("</tr>");
                finalStr.Append(row);
            }


           studyDesignPurposeQuestions = finalStr.ToString();
        }

        protected void renderPreventionCategoryQuestions()
        {
            //var db = new DBDataContext();
            var questions = db.F_PreventionCategories.Where(sf => sf.Status.Status1 == "Active").OrderBy(sf => sf.Sort).Select(sf => sf).ToList();
            StringBuilder finalStr = new StringBuilder();
            foreach (var question in questions)
            {
                var getViewVals = getViewValues("F_PreventionCategory", question.PreventionCategoryID);
                var getCoderVals = getCoderValues("F_PreventionCategory", question.PreventionCategoryID);
                var getComparerVals = getComparerValues("F_PreventionCategory", question.PreventionCategoryID);
                StringBuilder row = new StringBuilder();
                row.AppendLine("<tr>");
                row.AppendLine("<td scope=\"row\">"+ question.PreventionCategoryID.ToString() + ". " + question.PreventionCategory +"<div class=\"icon open\" ng-click=\"showDescription('preventioncategory-" + question.PreventionCategoryID.ToString() + "')\"></div>" + "</td>");
                row.AppendLine("<td class=\"box-three big\"><div outcome-box=\"mdata.preventioncategory[" + question.PreventionCategoryID + "]\"   is-checked='" + getViewVals[0] + "' show-coders='" + getCoderVals[0] + "' show-comparers='" + getComparerVals[0] + "' is-enabled='yes' " + " name=\"preventioncategory-" + question.PreventionCategoryID + "\" data-cat-id=\"preventioncategory\" data-q-id =\"" + question.PreventionCategoryID + "\"></div></td>");
                
                row.AppendLine("</tr>");
                finalStr.Append(row);
            }


            preventionCategoryQuestions = finalStr.ToString();
        }
    
    }
}