using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Transactions;
using ODPTaxonomyUtility_TT;
using ODPTaxonomyCommon;

namespace ODPTaxonomyDAL_TT
{
    public enum Status
    {
        Active = 1,
        InActive = 2,
        Deleted = 3

    }

    public class AbstractEvaluation
    {
        public int EvaluationId;
        public tbl_Abstract Abstract;
        public string Message;
        public bool IsAbstractEvailable;
        public bool IsAbstractTaken;
    }

    public enum SubmissionTypeId
    {
        CoderEvaluation = 1,
        CoderConsensus = 2,
        ODPStaffMembersEvaluation = 3,
        ODPStaffMemberConsensus = 4,
        ODPStaffMemberComparison = 5
    }

    public enum AbstractStatusID
    {
        none = 0,
        _0 = 1,
        _1 = 2,
        _1A = 3,
        _1B = 4,
        _1U = 5,
        _1N = 6,
        _2 = 7,
        _2A = 8,
        _2B = 9,
        _2C = 10,
        _2U = 11,
        _2N = 12,
        _3 = 13,
        _4 = 14,
        _5 = 15
    }

    public enum TeamType
    {
        Coder = 1,
        ODPStaff = 2
    }

    public enum EvaluationType
    {
        CoderEvaluation = 1,
        ODPEvaluation = 2
    }

    public static class Common
    {
        
        static Common()
        {
            RoleNames = new Dictionary<string, string>();
            RoleNames.Add("admin", "Admin");
            RoleNames.Add("coder", "Coder");
            RoleNames.Add("coderSup", "CoderSupervisor");
            RoleNames.Add("odp", "ODPStaffMember");
            RoleNames.Add("odpSup", "ODPStaffSupervisor");
        }

        public static Dictionary<string, string> RoleNames
        {
            set;
            get;
        }

        public static bool ProcessIsStopped(string connString, int evaluationId)
        {
            bool isStopped = false;
            using (DataDataContext db = new DataDataContext(connString))
            {
                var matches = from e in db.tbl_Evaluations
                              where e.EvaluationId == evaluationId && e.IsStopped == true
                              select e.EvaluationId;

                isStopped = matches.Any();
            }
            return isStopped;
        }

        public static string GetDisplayRoleName(string connString, string roleName)
        {
            string displayRoleName = null;
            using (DataDataContext db = new DataDataContext(connString))
            {
                var matches = from r in db.tbl_aspnet_Roles
                              where r.RoleName == roleName
                              select r.DisplayRoleName;
                displayRoleName = matches.FirstOrDefault();
            }

            return displayRoleName;
        }


        public static bool OdpMemberIsAllowedToCode(string connString, int abstractId, int teamTypeID, int evaluationTypeId, Guid userId)
        {
            bool isAllowed = false;
            int i_teamId = -1;
            int? teamId = null;
            

            using (DataDataContext db = new DataDataContext(connString))
            {
                var matches = from e in db.tbl_Evaluations
                              where e.AbstractID == abstractId && e.EvaluationTypeId == evaluationTypeId && e.IsComplete == false && e.IsStopped == false
                              select e.TeamID;
                teamId = matches.FirstOrDefault();

                if (teamId != null)
                {
                    i_teamId = (int)teamId;

                    isAllowed = (from t in db.tbl_Teams
                                     join tu in db.tbl_TeamUsers on t.TeamID equals tu.TeamID
                                     join u in db.tbl_aspnet_Users on tu.UserId equals u.UserId
                                     where u.UserId == userId && t.TeamID == i_teamId
                                     select t.TeamID).Any();
                    
                }
                else
                {
                    isAllowed = true;
                }

            }

            
            return isAllowed;
        }

        public static bool UserIsInTeam(string connString, int teamTypeID, Guid userId)
        {
            bool isInTeam = false;
            using (DataDataContext db = new DataDataContext(connString))
            {
                var matches = from t in db.tbl_Teams
                              join tu in db.tbl_TeamUsers on t.TeamID equals tu.TeamID
                              where (t.StatusID == (int)ODPTaxonomyDAL_TT.Status.Active) && (t.TeamTypeID == teamTypeID) && (tu.UserId == userId)
                              select t.TeamID;
                isInTeam = matches.Any();
            }
            return isInTeam;
        }

        public static int? GetTeamIdForUser(string connString, int teamTypeID, Guid userId)
        {
            int? teamId = null;
            using (DataDataContext db = new DataDataContext(connString))
            {
                var matches = from t in db.tbl_Teams
                              join tu in db.tbl_TeamUsers on t.TeamID equals tu.TeamID
                              where (t.StatusID == (int)ODPTaxonomyDAL_TT.Status.Active) && (t.TeamTypeID == teamTypeID) && (tu.UserId == userId)
                              select t.TeamID;
                foreach (var i in matches)
                {
                    teamId = i;
                }
                
            }
            return teamId;
        }

        public static string GetAbstractScan(string connString, int evaluationId)
        {
            string fileName = null;
            using (DataDataContext db = new DataDataContext(connString))
            {
                var matches = from s in db.tbl_AbstractScans
                              where s.EvaluationId == evaluationId
                              select s.FileName;
                fileName = matches.FirstOrDefault();
            }

            return fileName;
        }

        public static void UploadNotes(string connString, int evaluationId, Guid userId)
        {

            tbl_AbstractScan scan = null;            

            using (DataDataContext db = new DataDataContext(connString))
            {
                var matches = from s in db.tbl_AbstractScans
                              where s.EvaluationId == evaluationId
                              select s;
                scan = matches.FirstOrDefault();
                scan.UploadedBy = userId;
                scan.UploadedDateTime = DateTime.Now;

                db.SubmitChanges();
            }
        }

        public static void UploadNotes(string connString, int evaluationId, int abstractId, Guid userId, int abstractStatusId, string fileName)
        {
            tbl_AbstractStatusChangeHistory history = new tbl_AbstractStatusChangeHistory();

            history.AbstractID = abstractId;
            history.AbstractStatusID = abstractStatusId;
            history.CreatedDate = DateTime.Now;
            history.CreatedBy = userId;
            history.EvaluationId = evaluationId;

            tbl_AbstractScan scan = new tbl_AbstractScan();
            scan.EvaluationId = evaluationId;
            scan.FileName = fileName;
            scan.UploadedBy = userId;
            scan.UploadedDateTime = DateTime.Now;

            using (DataDataContext db = new DataDataContext(connString))
            {
                db.tbl_AbstractStatusChangeHistories.InsertOnSubmit(history);
                db.tbl_AbstractScans.InsertOnSubmit(scan);
                db.SubmitChanges();
            }
        }

        public static List<SubmissionLinkData> GetSubmissions(string connString, int evaluationId)
        {
            Guid userId = Guid.Empty;
            int submissionTypeId = -1;
            List<SubmissionLinkData> data = new List<SubmissionLinkData>();

            using (DataDataContext db = new DataDataContext(connString))
            {
                var matches = from s in db.tbL_Submissions
                              join u in db.tbl_aspnet_Users on s.UserId equals u.UserId
                              where s.EvaluationId == evaluationId
                              select new {s.EvaluationId, s.UserId, s.SubmissionTypeId, u.UserName};
                foreach(var i in matches)
                {
                    if (i.UserId != null)
                    {
                        userId = (Guid)i.UserId;
                        if (i.SubmissionTypeId != null)
                        {
                            submissionTypeId = (int)i.SubmissionTypeId;
                            data.Add(new SubmissionLinkData((int)i.EvaluationId, userId, submissionTypeId, i.UserName));
                        }
                    }
                    
                }
            }
            return data;
        }

        public static string OverrideAbstract(string connString, int evaluationId, int abstractId, Guid userId, int abstractStatusId)
        {
            tbl_Evaluation evaluation = null;
            List<tbL_Submission> submissions = null;
            tbl_AbstractStatusChangeHistory history = new tbl_AbstractStatusChangeHistory();
            int? evaluationIdForHistory = -1;
            string mess = null;

            if (abstractStatusId == (int)AbstractStatusID._0)
            {
                evaluationIdForHistory = null;
            }

            if (abstractStatusId == (int)AbstractStatusID._1N)
            {
                using (DataDataContext db = new DataDataContext(connString))
                {
                    var matches = from e in db.tbl_AbstractStatusChangeHistories
                                  where e.AbstractID == abstractId && e.AbstractStatusID == abstractStatusId
                                  select e.EvaluationId;
                    evaluationIdForHistory = matches.FirstOrDefault();
                }
            }

            history.AbstractID = abstractId;
            history.AbstractStatusID = abstractStatusId;
            history.CreatedDate = DateTime.Now;
            history.CreatedBy = userId;
            history.EvaluationId = evaluationIdForHistory;

            using (DataDataContext db = new DataDataContext(connString))
            {
                try
                {
                    evaluation = (from e in db.tbl_Evaluations
                                  where e.EvaluationId == evaluationId
                                  select e).FirstOrDefault();
                    submissions = (from s in db.tbL_Submissions
                                   where s.EvaluationId == evaluationId
                                   select s).ToList<tbL_Submission>();

                    db.tbl_AbstractStatusChangeHistories.InsertOnSubmit(history);

                    if (evaluation != null)
                    {
                        evaluation.IsStopped = true;
                        evaluation.StoppedBy = userId;
                        evaluation.StoppedDateTime = DateTime.Now;

                        foreach (var s in submissions)
                        {
                            s.StatusID = (int)Status.Deleted;
                            s.UpdatedBy = userId;
                            s.UpdatedDate = DateTime.Now;
                        }

                        db.SubmitChanges();
                    }
                    else
                    {
                       mess = "No evaluation exists for evaluationId = " + evaluationId;
                    }
                    
                }
                catch (Exception ex)
                {
                    Utils.LogError(ex);
                    throw new Exception("An error has occured while overriding abstract.");
                }                               
                              
            }
            return mess;
        }

        public static AbstractEvaluation StartAbstractCoding(string connString, int evaluationTypeId, int teamId, Guid userId)
        {
            AbstractEvaluation output = new AbstractEvaluation();
            output.Message = "";
            output.Abstract = null;
            output.EvaluationId = -1;
            output.IsAbstractEvailable = true;
            output.IsAbstractTaken = false;
            output.EvaluationId = -1;

            int? abstractId = -1;
            int? evaluationId = -1;
            bool? isAbstractTaken = false;
            bool? isAbstractEvailable = true;
            tbl_Abstract abstr = null;
            

            using (DataDataContext db = new DataDataContext(connString))
            {
                try
                {
                    db.CommandTimeout = 0;
                    int returnValue = -1;
                    returnValue = db.start_abstract_coding_tt(teamId, userId, ref abstractId, ref evaluationId,
                        ref isAbstractTaken, ref isAbstractEvailable);

                    if (returnValue == 0)
                    {
                        goto Finish;
                    }
                    else
                    {
                        if (abstractId.HasValue)
                        {
                            int id = (int) abstractId;
                            if (id >= 0)
                            {
                                var matches = from a in db.tbl_Abstracts
                                              where a.AbstractID == id
                                              select a;
                                abstr = matches.ToList<tbl_Abstract>().First();
                            }
                            
                        }
                        
                    }

                }
                catch (Exception ex)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("An error has occured on starting abstract coding.");
                    sb.AppendLine("TeamId: " + teamId + "; UserId: " + userId);
                    sb.AppendLine(ex.Message);
                    
                    throw new Exception(sb.ToString());
                }
            }

            //update output object
            output.Abstract = abstr;
            output.EvaluationId = evaluationId.HasValue ? (int)evaluationId : -1;
            output.IsAbstractTaken = isAbstractTaken.HasValue ? (bool)isAbstractTaken : false;
            output.IsAbstractEvailable = isAbstractEvailable.HasValue ? (bool)isAbstractEvailable : true;

            return output;

        Finish:
            throw new Exception("An error has occured on SQL server on starting abstract coding. TeamId: " + teamId + "; UserId: " + userId);
            
        }

        public static AbstractEvaluation StartEvaluationProcess(string connString, int evaluationTypeId, int abstractId, int teamId, Guid userId)
        {
            AbstractEvaluation output = new AbstractEvaluation();
            output.Message = "";
            output.Abstract = null;
            output.EvaluationId = -1;
            output.IsAbstractEvailable = true;
            output.IsAbstractTaken = false;
            output.EvaluationId = -1;

            int? evaluationId = -1;
            bool? isAbstractTaken = false;

            using (DataDataContext db = new DataDataContext(connString))
            {
                try
                {
                    int returnValue = -1;
                    returnValue = db.start_evaluation_odp_tt(teamId, userId, abstractId, ref evaluationId,
                        ref isAbstractTaken);

                    if (returnValue == 0)
                    {
                        goto Finish;
                    }
                }
                catch (Exception ex)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("An error has occured on starting evaluation process for odp team.");
                    sb.AppendLine("TeamId: " + teamId + "; UserId: " + userId + "; AbstractId: " + abstractId);
                    sb.AppendLine(ex.Message);

                    throw new Exception(sb.ToString());
                }
            }

            //update output object
            output.EvaluationId = evaluationId.HasValue ? (int)evaluationId : -1;
            output.IsAbstractTaken = isAbstractTaken.HasValue ? (bool)isAbstractTaken : false;

            return output;

        Finish:
            throw new Exception("An error has occured on SQL server on starting evaluation process for odp team. TeamId: " + teamId + "; UserId: " + userId + "; AbstractId: " + abstractId);
            
        }

        

        public static AbstractStatusID GetAbstractStatus(string connString, int abstractId)
        {
            AbstractStatusID statusId = AbstractStatusID.none;
            select_abstract_status_ttResult item = null;
            int id = -1;
            
            using (DataDataContext db = new DataDataContext(connString))
            {
                var matches = db.select_abstract_status_tt(abstractId);
                item = matches.FirstOrDefault();

                if (item != null)
                {
                    id = item.AbstractStatusID;
                    foreach (AbstractStatusID i in Enum.GetValues(typeof(AbstractStatusID)))
                    {
                        if ((int)i == id)
                        {
                            statusId = i;
                            break;
                        }
                    }
                }
            }

            return statusId;
        }

        public static tbl_Abstract GetAbstractByAbstractId(string connString, int abstractId)
        {
            tbl_Abstract abstr = null;

            using (DataDataContext db = new DataDataContext(connString))
            {
                var matches = from a in db.tbl_Abstracts
                              where a.AbstractID == abstractId
                              select a;
                abstr = matches.ToList<tbl_Abstract>().FirstOrDefault();
            }

            return abstr;
        }

        public static tbl_Abstract GetAbstract_CoderEvaluation(string connString, out string message)
        {
            message = "OK";
            int abstractId = -1;
            tbl_Abstract abstr = null;
            List<int> abstracts = new List<int>();

            using (DataDataContext db = new DataDataContext(connString))
            {
                var matches = db.select_abstracts_coding_tt((int)AbstractStatusID._0);
                try
                {
                    abstractId = (int)matches.FirstOrDefault().AbstractID;
                    var matches2 = from a in db.tbl_Abstracts
                                   where a.AbstractID == abstractId
                                   select a;
                    abstr = matches2.ToList<tbl_Abstract>().First();
                }
                catch (Exception ex)
                {
                    message = "No abstracts are available.";
                    Utils.LogError(ex);
                    return abstr;
                }

            }

            return abstr;
        }

        
        public static int? GetEvaluationIdForAbstract(string connString, int abstractId, EvaluationType type)
        {
            int? id = null;
            using (DataDataContext db = new DataDataContext(connString))
            {
                var matches = from e in db.tbl_Evaluations
                              where e.EvaluationTypeId == (int)type && e.AbstractID == abstractId
                              && e.IsStopped == false
                              select e.EvaluationId;
                foreach (var i in matches)
                {
                    id = i;
                }
            }

            return id;
        }


        public static ViewAbstractData GetEvaluationData(string connString, int teamId, int evaluationTypeId)
        {
            ViewAbstractData data = null;            

            using (DataDataContext db = new DataDataContext(connString))
            {
                var matches = from e in db.tbl_Evaluations
                              join a in db.tbl_Abstracts on e.AbstractID equals a.AbstractID
                              where e.TeamID == teamId && e.EvaluationTypeId == evaluationTypeId && e.IsStopped == false 
                                && e.IsComplete == false 
                              select new { e.EvaluationId, a };
                foreach (var i in matches)
                {
                    data = new ViewAbstractData();
                    data.Abstract = i.a;
                    data.EvaluationId = i.EvaluationId;                    
                }

                
            }

            return data;
        }

        public static Guid GetCurrentUserId(string connString, string userCurrentName)
        {
            Guid userCurrentId = Guid.Empty;
            using (DataDataContext db = new DataDataContext(connString))
            {
                var matches = (from u in db.tbl_aspnet_Users
                               where u.UserName == userCurrentName
                               select u).FirstOrDefault();
                userCurrentId = matches.UserId;
            }

            return userCurrentId;
        }        

        public static string GetTeamCode(string initials)
        {
            string teamCode = "";
            string format = "yyyyMMdd_HHmmss";
            string sDateTime = DateTime.Now.ToString(format);
            StringBuilder sb = new StringBuilder();

            sb.Append(initials);
            sb.Append(sDateTime);
            teamCode = sb.ToString();

            return teamCode;
        }

        public static void ExportAbstract(string connString, int abstractId, Guid userId)
        {
            tbl_AbstractStatusChangeHistory history = new tbl_AbstractStatusChangeHistory();

            history.AbstractID = abstractId;
            history.AbstractStatusID = (int)AbstractStatusID._4;
            history.CreatedDate = DateTime.Now;
            history.CreatedBy = userId;
            history.EvaluationId = null;

            tbl_Abstract abstractItem = null;

            using (DataDataContext db = new DataDataContext(connString))
            {
                try
                {
                    //verify user
                    var matches = from m in db.tbl_aspnet_Memberships
                                  where m.UserId == userId && m.IsApproved == true && m.IsLockedOut == false
                                  select m.UserId;
                    if (matches.Any())
                    {
                        //verify abstract
                        var matchesAbstract = from a in db.tbl_Abstracts
                                              where a.AbstractID == abstractId
                                              select a;
                        foreach (var i in matchesAbstract)
                        {
                            abstractItem = i;
                            int AbsStatusID = db.tbl_AbstractStatusChangeHistories.Where(h2 => h2.AbstractID == abstractId).OrderByDescending(h2 => h2.AbstractStatusChangeHistoryID).Select(h2 => h2.AbstractStatusID).FirstOrDefault();
                            if (AbsStatusID != 13 && AbsStatusID != 14) return;
                        }

                        if (abstractItem != null)
                        {
                            using (TransactionScope tr = new TransactionScope())
                            {
                                db.tbl_AbstractStatusChangeHistories.InsertOnSubmit(history);
                                db.SubmitChanges();

                                abstractItem.LastExportDate = DateTime.Now;
                                db.SubmitChanges();

                                tr.Complete();
                            }
                        }
                        else
                        {
                            throw new Exception("Abstract is NOT valid");
                        }
                        
                    }
                    else
                    {
                        throw new Exception("User is NOT valid");
                    }
                }
                catch (Exception ex)
                {
                    Utils.LogError(ex);
                    throw ex;
                }
            }
        }

        public static void CloseAbstract(string connString, int abstractId, Guid userId)
        {
            tbl_AbstractStatusChangeHistory history = new tbl_AbstractStatusChangeHistory();

            history.AbstractID = abstractId;
            history.AbstractStatusID = (int)AbstractStatusID._3;
            history.CreatedDate = DateTime.Now;
            history.CreatedBy = userId;
            history.EvaluationId = null;

            using (DataDataContext db = new DataDataContext(connString))
            {
                try
                {
                    //verify user
                    var matches = from m in db.tbl_aspnet_Memberships
                                  where m.UserId == userId && m.IsApproved == true && m.IsLockedOut == false
                                  select m.UserId;
                    if (matches.Any())
                    {
                        //verify abstract
                        var matchesAbstract = from a in db.tbl_Abstracts
                                              where a.AbstractID == abstractId
                                              select a;
                        if (matchesAbstract.Any())
                        {
                            db.tbl_AbstractStatusChangeHistories.InsertOnSubmit(history);
                            db.SubmitChanges();
                        }
                        else
                        {
                            throw new Exception("Abstract is NOT valid");
                        }
                    }
                    else
                    {
                        throw new Exception("User is NOT valid");
                    }
                }
                catch (Exception ex)
                {
                    Utils.LogError(ex);
                    throw ex;
                }
            }
        }

        public static void OpenClosedAbstract(string connString, int abstractId, Guid userId)
        {
            tbl_AbstractStatusChangeHistory history = new tbl_AbstractStatusChangeHistory();

            using (DataDataContext db = new DataDataContext(connString))
            {
                try
                {
                    //verify user
                    var matches1 = from m in db.tbl_aspnet_Memberships
                                  where m.UserId == userId && m.IsApproved == true && m.IsLockedOut == false
                                  select m.UserId;
                    if (matches1.Any())
                    {
                        //verify abstract
                        var matchesAbstract = from a in db.tbl_Abstracts
                                              where a.AbstractID == abstractId
                                              select a;
                        if (matchesAbstract.Any())
                        {
                            var matches = db.select_abstracts_no_reopen_tt();
                            bool isInList = matches.Any(x => x.AbstractID == abstractId);

                            if (!isInList)
                            {
                                //could be re-opened
                                history.AbstractID = abstractId;
                                history.AbstractStatusID = (int)AbstractStatusID._1N;
                                history.CreatedDate = DateTime.Now;
                                history.CreatedBy = userId;
                                history.EvaluationId = null;

                                db.tbl_AbstractStatusChangeHistories.InsertOnSubmit(history);
                                db.SubmitChanges();
                            }
                        }
                        else
                        {
                            throw new Exception("Abstract is NOT valid");
                        }
                    }
                    else
                    {
                        throw new Exception("User is NOT valid");
                    }
                }
                catch (Exception ex)
                {
                    Utils.LogError(ex);
                    throw ex;
                }
            }
        }

        public static List<int> GetAbstractsNotToReopen(string connString, Guid userId)
        {
            List<int> abstractIds = null;
            using (DataDataContext db = new DataDataContext(connString))
            {
                try
                {
                    //verify user
                    var matches1 = from m in db.tbl_aspnet_Memberships
                                  where m.UserId == userId && m.IsApproved == true && m.IsLockedOut == false
                                  select m.UserId;
                    if (matches1.Any())
                    {
                        var matches = db.select_abstracts_no_reopen_tt();
                        if (matches != null)
                        {
                            abstractIds = matches.Select(x => x.AbstractID).ToList();
                        }
                    }
                    else
                    {
                        throw new Exception("User is NOT valid");
                    }
                }
                catch (Exception ex)
                {
                    Utils.LogError(ex);
                    throw ex;
                }
            }

            return abstractIds;
        }
        

        public static IEnumerable<T> ListMerge<T>(params List<T>[] objects)
        {
            foreach (var obj in objects)
            {
                var enumerable = obj as System.Collections.IEnumerable;
                if (enumerable != null)
                    foreach (var item in enumerable)
                        yield return (T)item;
                else
                    yield return default(T);
            }
        }



        public static List<rpt_OPAResult> GetReportData_OpaData(string connString, string abstracts)
        {
            List<rpt_OPAResult> matches = null;
            
            using (DataDataContext db = new DataDataContext(connString))
            {
                db.CommandTimeout = 0;
                try
                {
                    matches = db.rpt_OPA(abstracts).ToList<rpt_OPAResult>();
                                       
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return matches;
        }

        public static List<rpt_KappaDataResult> GetReportData_KappaData(string connString, string abstracts)
        {
            List<rpt_KappaDataResult> matches = null;
            
            using (DataDataContext db = new DataDataContext(connString))
            {
                db.CommandTimeout = 0;
                try
                {
                    matches = db.rpt_KappaData(abstracts).ToList<rpt_KappaDataResult>();
                    
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return matches;
        }

        public static List<rpt_Cdr_ODPNotesPDFResult> GetReportData_Cdr_ODPNotesPDF(string connString, string abstracts, string domain)
        {
            List<rpt_Cdr_ODPNotesPDFResult> matches = null;
            
            using (DataDataContext db = new DataDataContext(connString))
            {
                db.CommandTimeout = 0;
                try
                {
                    matches = db.rpt_Cdr_ODPNotesPDF(abstracts, domain).ToList<rpt_Cdr_ODPNotesPDFResult>();
                   
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return matches;
        }

        public static tbl_Abstract_Text GetAbstractText(string connString, int abstractID)
        {
            tbl_Abstract_Text matches = null;

            using (DataDataContext db = new DataDataContext(connString))
            {
                db.CommandTimeout = 0;
                try
                {
                    matches = db.tbl_Abstract_Texts.Where(x => x.AbstractID == abstractID).FirstOrDefault();

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return matches;

        }

        public static List<rpt_AbstractStatusTrailResult> GetReportData_AbstractStatusTrail(string connString, string abstracts)
        {
            List<rpt_AbstractStatusTrailResult> matches = null;
             
            using (DataDataContext db = new DataDataContext(connString))
            {
                db.CommandTimeout = 0;
                try
                {
                    matches = db.rpt_AbstractStatusTrail(abstracts).ToList<rpt_AbstractStatusTrailResult>();
                   
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return matches;
        }

        public static List<rpt_Cdr_ODP_IndividualCodingResult> GetReportData_Cdr_ODP_IndividualCoding(string connString, string abstracts)
        {
            List<rpt_Cdr_ODP_IndividualCodingResult> matches = null;
            
            using (DataDataContext db = new DataDataContext(connString))
            {
                db.CommandTimeout = 0;
                try
                {
                    matches = db.rpt_Cdr_ODP_IndividualCoding(abstracts).ToList<rpt_Cdr_ODP_IndividualCodingResult>();
                    
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return matches;
        }

        public static List<rpt_Team_User_UCResult> GetReportData_Team_User_UCResult(string connString, string abstracts)
        {
            List<rpt_Team_User_UCResult> matches = null;
           
            using (DataDataContext db = new DataDataContext(connString))
            {
                try
                {
                    matches = db.rpt_Team_User_UC(abstracts).ToList<rpt_Team_User_UCResult>();                    
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return matches;
        }

        public static List<rpt_AbstractExportedResult> GetReportData_AbstractExported(string connString, string abstracts)
        {
            List<rpt_AbstractExportedResult> matches = null;

            using (DataDataContext db = new DataDataContext(connString))
            {
                try
                {
                    matches = db.rpt_AbstractExported(abstracts).ToList<rpt_AbstractExportedResult>();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return matches;
        }

        public static string GetAbstarctIdForTeamMember(string connString, string userCurrentName)
        {
            string abstractId = null;
            string abstractTitle = null;

            using (DataDataContext db = new DataDataContext(connString))
            {
                var matches = db.select_abstractId_team_member_tt(userCurrentName);
                foreach (var m in matches)
                {
                    abstractId = m.AbstractID.ToString();
                    abstractTitle = m.ProjectTitle;
                    break;
                }

            }

            return abstractId + "," + abstractTitle;
        }


    }
}
