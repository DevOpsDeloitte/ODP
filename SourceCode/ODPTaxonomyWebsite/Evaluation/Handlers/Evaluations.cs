using System;
using System.Web;
using System.Web.Security;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using ODPTaxonomyDAL_ST;
using System.Text;
using ODPTaxonomyWebsite.Evaluation.Classes;


namespace ODPTaxonomyWebsite.Evaluation.Handlers
{
    public class Evaluations : IHttpHandler
    {
        public Int32 submissionID = 0;
        public int evaluationID = 0;
        public int abstractID = 0;
        public short? submissiontypeID = 0;
        public bool unabletocode = false;
        public Guid userID;
        public string formmode = string.Empty;
        public string comments = string.Empty;
        public string superusername = string.Empty;
        public string superpassword = string.Empty;
        public Guid superuserID;
        public List<nvClass> formVals = null;
        public List<nvClass> NVs = null;
        DataDataContext db;

        /// <summary>
        /// You will need to configure this handler in the Web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpHandler Members

        public bool IsReusable
        {
            // Return false in case your Managed Handler cannot be reused for another request.
            // Usually this would be false in case you have some state information preserved per request.
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            //System.Diagnostics.Trace.WriteLine("Post process request...");
            //write your handler implementation here.
            db = DBData.GetDataContext();
            if (!initReadForm(context))
            {
                context.Response.Write(JsonConvert.SerializeObject(new { success = false, supervisorauthfailed = true }));
                return;

            }
            processAndOrganize(context);
            addAbstractChangeHistory();
            context.Response.ContentType = "application/json";
            if (submissionID > 0)
            {
                // logic to show consensus button 
                var showButton = false; 
                var showComparison = false;
                if (submissiontypeID == 1)
                {
                    var subList = db.Submissions.Where(s => s.EvaluationId == evaluationID && s.SubmissionTypeId == 1).Select(s => s).ToList();
                    if (subList.Count == 3) showButton = true;

                }

                if (submissiontypeID == 3)
                {
                    var subList = db.Submissions.Where(s => s.EvaluationId == evaluationID && s.SubmissionTypeId == 3).Select(s => s).ToList();
                    if (subList.Count == 3) showButton = true;

                }

                // Check if Consensus has been already started..
                if ((submissiontypeID == 1 || submissiontypeID == 3) && showButton)
                {
                    var eval = db.Evaluations.Where(e => e.EvaluationId == evaluationID).FirstOrDefault();
                    if (eval != null)
                    {

                        showButton = (eval.ConsensusStartedBy == null) ? true : false;
                    }
                }

                // show comparison button 
                if (submissiontypeID == 4)
                {
                    showComparison = true;

                }

                //
                context.Response.Write(JsonConvert.SerializeObject(new { success = true, submissionID = submissionID, showConsensusButton = showButton, showComparisonButton = showComparison }));
            }
            else
            {
                if (submissionID == -2)
                {
                    context.Response.Write(JsonConvert.SerializeObject(new { success = false, multipleconsensusexists = true }));
                }
                else
                {
                    context.Response.Write(JsonConvert.SerializeObject(new { success = false }));
                }
            }
        }

        #endregion


        #region Process Forms

        private void addAbstractChangeHistory()
        {
            switch (submissiontypeID)
            {

   
                case 1:
                    //FormMode = "Coder Evaluation";
                    // Insert Status 1A record or ID = 3 
                    // Insert only on the 3rd submission of this evaluation.
                     var subListC = db.Submissions.Where(s => s.EvaluationId == evaluationID && s.SubmissionTypeId == 1).Select(s => s).ToList();
                     if (subListC.Count == 3)
                     {
                
                         insertAbstractChangeHistory(getAbstractStatusID("1A"));
                     }
                   
                    break;
                case 2:
                    //FormMode = "Coder Consensus";
                    // Insert Status 1B record or ID = 4
                    insertAbstractChangeHistory(getAbstractStatusID("1B"));
                    //update is complete to 1 on the evaluation record.
                    updateEvaluationRecordToComplete();
                    //System.Diagnostics.Trace.WriteLine("abstract change history 1B : " + evaluationID + " abstract ID : " + abstractID);

                    // Adding Stored Procedure call for Kappa Data Calculations.
                    try
                    {
                        db.KappaBaseData_Insert_ByAbs_EvlID(abstractID, evaluationID, 4);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.WriteLine("Kappa Procedure 1B/4 Error : " + evaluationID + " abstract ID : " + abstractID + " exception message : "+ ex.Message);
                    }

                    break;
                case 3:
                    //FormMode = "ODP Staff Member Evaluation";
                    // Insert Status 2A record or ID = 8 
                    // Insert only on the 3rd submission of this evaluation.
                    var subListO = db.Submissions.Where(s => s.EvaluationId == evaluationID && s.SubmissionTypeId == 3).Select(s => s).ToList();
                    if (subListO.Count == 3)
                    {
                        insertAbstractChangeHistory(getAbstractStatusID("2A"));
                    }

                    
                    break;
                case 4:
                    //FormMode = "ODP Staff Member Consensus";
                    // Insert Status 2B record or ID = 9
                    insertAbstractChangeHistory(getAbstractStatusID("2B"));

                    // Adding Stored Procedure call for Kappa Data Calculations.
                    try
                    {
                        db.KappaBaseData_Insert_ByAbs_EvlID(abstractID, evaluationID, 9);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.WriteLine("Kappa Procedure 2B/9 Error : " + evaluationID + " abstract ID : " + abstractID + " exception message : " + ex.Message);
                    }
                    

                    break;
                case 5:
                    //FormMode = "ODP Staff Member Comparison";
                    // Insert Status 2C record or ID = 10
                    insertAbstractChangeHistory(getAbstractStatusID("2C"));
                    //update is complete to 1 on the evaluation record.
                    updateEvaluationRecordToComplete();
                    // Adding Stored Procedure call for Kappa Data Calculations.
                    try
                    {
                        db.ODPComparison_ByAbs_EvlID(abstractID, evaluationID, 10);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.WriteLine("Kappa Procedure 2C/10 Error : " + evaluationID + " abstract ID : " + abstractID + " exception message : " + ex.Message);
                    }
                    

                    break;

           



            }


        }

        private void updateEvaluationRecordToComplete()
        {
            var evalrec = db.Evaluations.Where(e=>e.EvaluationId == evaluationID).FirstOrDefault();
            if(evalrec != null){
                evalrec.IsComplete = true;
                db.SubmitChanges();
            }


        }

        private int getAbstractStatusID(string code)
        {
            var rec = db.AbstractStatus.Where(s => s.AbstractStatusCode == code).Select( s => s).FirstOrDefault();
            if (rec != null)
            {
                return Convert.ToInt32(rec.AbstractStatusID);
            }
            else
            {
                return 0;
            }
        }

        private int createSubmissionRecord()
        {
            // do a check for consensus record existing as a fail safe, just to prevent multiple consensus being written.
            if (submissiontypeID == 2 || submissiontypeID == 4)
            {
                var consensusexists = db.Submissions.Where(s => s.SubmissionTypeId == submissiontypeID && s.EvaluationId == evaluationID).Any();
                if (consensusexists)
                {
                    return -2;
                }
            }
            Submission sb = new Submission();
            sb.SubmissionTypeId = submissiontypeID;
            sb.SubmissionDateTime = DateTime.Now;
            sb.StatusID = 1;
            sb.EvaluationId = evaluationID;
            sb.UserId = userID;
            sb.UnableToCode = unabletocode;
            if (unabletocode)
            {
                sb.ApproveSupervisorUserID = superuserID;
            }
            sb.Comments = comments;

            db.Submissions.InsertOnSubmit(sb);
            try
            {
                db.SubmitChanges();
                submissionID = sb.SubmissionID;
            }
            catch
            {
                return 0;
            }
            

            return sb.SubmissionID;

        }

        private void insertAbstractChangeHistory(int abstractstatusID)
        {
            AbstractStatusChangeHistory asch = new AbstractStatusChangeHistory();
            asch.AbstractID = abstractID;
            asch.AbstractStatusID = abstractstatusID;
            asch.CreatedBy = userID;
            asch.CreatedDate = DateTime.Now;
            asch.EvaluationId = evaluationID;
            db.AbstractStatusChangeHistories.InsertOnSubmit(asch);
            db.SubmitChanges();
        }

        private bool initReadForm(HttpContext context)
        {
            Stream s = context.Request.InputStream;
            StreamReader sr = new StreamReader(s);
            //System.Diagnostics.Trace.Write(sr.ReadToEnd().ToString());
            var jsonString = sr.ReadToEnd().ToString();
            NVs = JsonConvert.DeserializeObject<List<nvClass>>(jsonString);        
            formVals = NVs;
            abstractID = Int32.Parse(getFormVal("abstractid"));    
            evaluationID = Int32.Parse(getFormVal("evaluationid"));
            submissiontypeID = Int16.Parse(getFormVal("submissiontypeid"));
            comments = getFormVal("comments");
            unabletocode = getFormValBool("unabletocode");
            // If unable to code the supervisor does need to be authenticated.
            if (unabletocode)
            {
                superusername = getFormVal("superusername");
                superpassword = getFormVal("superpassword");
                System.Diagnostics.Trace.WriteLine(" super username : " + superusername);
                if (superusername != "" && superpassword != "")
                {
                    if (Membership.ValidateUser(superusername, superpassword))
                    {
                        System.Diagnostics.Trace.WriteLine(" validate super username : " + superusername + "     " + superpassword);
                        MembershipUser user = Membership.GetUser(superusername);
                        superuserID = (Guid)user.ProviderUserKey;
                        List<string> UserRoles = Roles.GetRolesForUser(superusername).ToList();
                        var isSupervisor = false;
                        foreach (var role in UserRoles)
                        {
                            if (role.ToLower().Contains("supervisor")) isSupervisor = true;

                        }
                        if (!isSupervisor) return false;
                        
                    }
                    else
                    {
                        return false;
                    }
                

                }
                else
                {

                    return false;
                }

            }


            formmode = getFormVal("mode");
            userID = new Guid(getFormVal("userid"));
            //submissionID = Int32.Parse(getFormVal("submissionid"));
            submissionID = createSubmissionRecord();
            //System.Diagnostics.Trace.WriteLine("In Submission --- Submission ID : " + submissionID.ToString());
            return true;

        }

        private bool processAndOrganize(HttpContext context)
        {
            Boolean retVal = false;
            //Stream s = context.Request.InputStream;
            //StreamReader sr = new StreamReader(s);
            //var jsonString = sr.ReadToEnd().ToString();
            //List<nvClass> NVs = JsonConvert.DeserializeObject<List<nvClass>>(jsonString);
            //System.Diagnostics.Trace.WriteLine("In Submission --- Submission ID : " + submissionID.ToString());
            //formVals = NVs;
            //submissionID = Int32.Parse(getFormVal("submissionid"));

            if (submissionID > 0)
            {
                //var db = new DBDataContext();
                //var db = DBData.GetDataContext();
                System.Diagnostics.Trace.WriteLine("Submission ID : " + submissionID.ToString());
                List<studyFocus> studyfocusInsertList = createStudyFocusInsertList(NVs);

                DeleteAnswers(db);

                InsertStudyFocus(db, studyfocusInsertList);

                List<nvClass> entitiesstudiedNVs = NVs.FindAll(nv => nv.name.Contains("entitiesstudied"));
                foreach (var es in entitiesstudiedNVs)
                {
                    var ids = es.name.Split('-');
                    B_EntitiesStudiedAnswer b_es = new B_EntitiesStudiedAnswer();
                    b_es.SubmissionID = submissionID;
                    b_es.EntitiesStudiedID = Int32.Parse(ids[1]);
                    db.B_EntitiesStudiedAnswers.InsertOnSubmit(b_es);

                }
                db.SubmitChanges();

                List<nvClass> studysettingNVs = NVs.FindAll(nv => nv.name.Contains("studysetting"));
                foreach (var ss in studysettingNVs)
                {
                    var ids = ss.name.Split('-');
                    C_StudySettingAnswer c_ss = new C_StudySettingAnswer();
                    c_ss.SubmissionID = submissionID;
                    c_ss.StudySettingID = Int32.Parse(ids[1]);
                    db.C_StudySettingAnswers.InsertOnSubmit(c_ss);

                }
                db.SubmitChanges();

                List<nvClass> polulationfocusNVs = NVs.FindAll(nv => nv.name.Contains("populationfocus"));
                foreach (var pf in polulationfocusNVs)
                {
                    var ids = pf.name.Split('-');
                    D_PopulationFocusAnswer d_pf = new D_PopulationFocusAnswer();
                    d_pf.SubmissionID = submissionID;
                    d_pf.PopulationFocusID = Int32.Parse(ids[1]);
                    db.D_PopulationFocusAnswers.InsertOnSubmit(d_pf);

                }
                db.SubmitChanges();

                List<nvClass> studydesignpurposeNVs = NVs.FindAll(nv => nv.name.Contains("studydesignpurpose"));
                foreach (var sdp in studydesignpurposeNVs)
                {
                    var ids = sdp.name.Split('-');
                    E_StudyDesignPurposeAnswer e_sdp = new E_StudyDesignPurposeAnswer();
                    e_sdp.SubmissionID = submissionID;
                    e_sdp.StudyDesignPurposeID = Int32.Parse(ids[1]);
                    db.E_StudyDesignPurposeAnswers.InsertOnSubmit(e_sdp);

                }
                db.SubmitChanges();


                List<nvClass> preventioncategoryNVs = NVs.FindAll(nv => nv.name.Contains("preventioncategory"));
                foreach (var pc in preventioncategoryNVs)
                {
                    //System.Diagnostics.Trace.WriteLine(" detected prevention category ");
                    var ids = pc.name.Split('-');
                    F_PreventionCategoryAnswer f_pc = new F_PreventionCategoryAnswer();
                    f_pc.SubmissionID = submissionID;
                    f_pc.PreventionCategoryID = Int32.Parse(ids[1]);
                    db.F_PreventionCategoryAnswers.InsertOnSubmit(f_pc);

                }
                db.SubmitChanges();

                retVal = true;

            }
            else
            {
                retVal = false;
            }

            return retVal;


        }

        private void InsertStudyFocus(DataDataContext db, List<studyFocus> studyfocusInsertList)
        {
            // insert study focus 
            foreach (var sfi in studyfocusInsertList)
            {
                A_StudyFocusAnswer a_sfa = new A_StudyFocusAnswer();
                a_sfa.SubmissionID = submissionID;
                a_sfa.StudyFocusID = sfi.StudyFocusId;
                a_sfa.StudyFocus_A1 = sfi.StudyFocus1;
                a_sfa.StudyFocus_A2 = sfi.StudyFocus2;
                a_sfa.StudyFocus_A3 = sfi.StudyFocus3;

                db.A_StudyFocusAnswers.InsertOnSubmit(a_sfa);
            }

            db.SubmitChanges();
        }

        private void DeleteAnswers(DataDataContext db)
        {
            // code only for testing ::
            var delanswers = db.A_StudyFocusAnswers.Where(x => x.SubmissionID == submissionID).Select(x => x);
            foreach (var dela in delanswers)
            {
                db.A_StudyFocusAnswers.DeleteOnSubmit(dela);
            }
            db.SubmitChanges();
            var esanswers = db.B_EntitiesStudiedAnswers.Where(x => x.SubmissionID == submissionID).Select(x => x);
            foreach (var es in esanswers)
            {
                db.B_EntitiesStudiedAnswers.DeleteOnSubmit(es);
            }
            db.SubmitChanges();
            var ssanswers = db.C_StudySettingAnswers.Where(x => x.SubmissionID == submissionID).Select(x => x);
            foreach (var ss in ssanswers)
            {
                db.C_StudySettingAnswers.DeleteOnSubmit(ss);
            }
            db.SubmitChanges();
            var pfanswers = db.D_PopulationFocusAnswers.Where(x => x.SubmissionID == submissionID).Select(x => x);
            foreach (var pf in pfanswers)
            {
                db.D_PopulationFocusAnswers.DeleteOnSubmit(pf);
            }
            db.SubmitChanges();

            var sdpanswers = db.E_StudyDesignPurposeAnswers.Where(x => x.SubmissionID == submissionID).Select(x => x);
            foreach (var sdp in sdpanswers)
            {
                db.E_StudyDesignPurposeAnswers.DeleteOnSubmit(sdp);
            }
            db.SubmitChanges();

            var pcanswers = db.F_PreventionCategoryAnswers.Where(x => x.SubmissionID == submissionID).Select(x => x);
            foreach (var pc in pcanswers)
            {
                db.F_PreventionCategoryAnswers.DeleteOnSubmit(pc);
            }
            db.SubmitChanges();

            // end of code only for testing ::
        }

        private string getFormVal(string name)
        {
            try
            {
                return !String.IsNullOrEmpty(formVals.Find(x => x.name == name).value) ? formVals.Find(x => x.name == name).value : "";
            }
            catch
            {
                return "";
            }
        }

        private bool getFormValBool(string name)
        {
            string valofcode = string.Empty;
            try
            {
                valofcode = !String.IsNullOrEmpty(formVals.Find(x => x.name == name).value) ? formVals.Find(x => x.name == name).value : "";
            }
            catch
            {
                return false;
            }
            return valofcode == "on" ? true : false;
       
        }

        private List<studyFocus> createStudyFocusInsertList(List<nvClass> NVs)
        {
            List<nvClass> studyfocusNVs = NVs.FindAll(nv => nv.name.Contains("studyfocus"));
            List<studyFocus> studyfocusInsertList = new List<studyFocus>();
            foreach (var sf in studyfocusNVs)
            {
                var ids = sf.name.Split('-');
                var sfexists = studyfocusInsertList.Find(x => x.StudyFocusId == Int32.Parse(ids[1]));
                if (sfexists != null)
                {   // update
                    switch (Int32.Parse(ids[2]))
                    {
                        case 1:
                            sfexists.StudyFocus1 = true;
                            break;
                        case 2:
                            sfexists.StudyFocus2 = true;
                            break;
                        case 3:
                            sfexists.StudyFocus3 = true;
                            break;
                    }

                }
                else
                {   // add
                    studyFocus sfnew = new studyFocus();
                    sfnew.StudyFocus1 = sfnew.StudyFocus2 = sfnew.StudyFocus3 = false;
                    sfnew.StudyFocusId = Int32.Parse(ids[1]);
                    switch (Int32.Parse(ids[2]))
                    {
                        case 1:
                            sfnew.StudyFocus1 = true;
                            break;
                        case 2:
                            sfnew.StudyFocus2 = true;
                            break;
                        case 3:
                            sfnew.StudyFocus3 = true;
                            break;
                    }

                    studyfocusInsertList.Add(sfnew);

                }

                

            }

            return studyfocusInsertList;
        }

        


        #endregion
    }



    #region Helper Classes

    public class nvClass
    {
        public string name { get; set; }
        public string value { get; set; }
    }

    public class studyFocus
    {
        public int StudyFocusId { get; set; }
        public Boolean StudyFocus1 { get; set; }
        public Boolean StudyFocus2 { get; set; }
        public Boolean StudyFocus3 { get; set; }
    }

    class EntitiesStudied
    {
        public int EntitiesStudiedId { get; set; }
    }

    
    #endregion


}
