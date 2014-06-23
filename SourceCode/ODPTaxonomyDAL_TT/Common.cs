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


        public static int StartEvaluationProcess(string connString, int evaluationTypeId, int abstractId, int teamId, Guid userId)
        {
            int evaluationId = -1;
            int abstractStatusId = -1;

            if (evaluationTypeId == (int)EvaluationType.CoderEvaluation)
            {
                abstractStatusId = (int)AbstractStatusID._1;
            }

            if (evaluationTypeId == (int)EvaluationType.ODPEvaluation)
            {
                abstractStatusId = (int)AbstractStatusID._2;
            }

            tbl_Evaluation evaluation = new tbl_Evaluation();
            evaluation.ConsensusStartedBy = null;
            evaluation.AbstractID = abstractId;
            evaluation.TeamID = teamId;
            evaluation.DateTimeEnded = null;
            evaluation.DateTimeStarted = DateTime.Now;
            evaluation.EvaluationTypeId = (short)evaluationTypeId;
            evaluation.IsComplete = false;
            evaluation.StoppedBy = null;
            evaluation.StoppedDateTime = null;

            tbl_AbstractStatusChangeHistory history = new tbl_AbstractStatusChangeHistory();

            history.AbstractID = abstractId;
            history.AbstractStatusID = abstractStatusId;
            history.CreatedDate = DateTime.Now;
            history.CreatedBy = userId;
            
            using (DataDataContext db = new DataDataContext(connString))
            {
                try
                {
                    using (TransactionScope tr = new TransactionScope())
                    {
                        db.tbl_Evaluations.InsertOnSubmit(evaluation);
                        db.SubmitChanges();
                        evaluationId = evaluation.EvaluationId;

                        history.EvaluationId = evaluationId;
                        db.tbl_AbstractStatusChangeHistories.InsertOnSubmit(history);
                        db.SubmitChanges();

                        tr.Complete();
                    }
                }
                catch(Exception ex)
                {
                    Utils.LogError(ex);
                    throw new Exception("An error has occured while saving data.");
                }
                
                
                              
            }

            return evaluationId;
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
            int index = 0;
            int topicsCount = 0;
            int abstractId = -1;
            tbl_Abstract abstr = null;
            List<int> topics = new List<int>();
            List<int> abstracts =  new List<int>();

            //Get available Topics
            using (DataDataContext db = new DataDataContext(connString))
            {
                var matches = from a in db.tbl_A_StudyFocus
                              where a.ShowAsAbstractTopic == true && a.StatusID == (int)Status.Active
                              orderby a.AbstractStudyFocusSort
                              select a.StudyFocusID;

                foreach(int i in matches)
                {
                    topics.Add(i);
                }
            }

            topicsCount = topics.Count;

            if (topicsCount > 0)
            {
                //Get available Abstracts
                using (DataDataContext db = new DataDataContext(connString))
                {
                    foreach (int i in topics)
                    {
                        
                        var matches = db.select_abstracts_coding_tt((int)AbstractStatusID._0, i);

                        foreach (var item in matches)
                        {
                            abstracts.Add(item.AbstractID);
                        }

                        index++;

                        if (abstracts.Count > 0)
                        {
                            break;
                        }
                        else
                        {
                            if (index == topicsCount)
                            {
                                message = "No abstracts are available.";
                            }
                        }
                    }

                }

                
                if (abstracts.Count > 0)
                {
                    Random rnd = new Random();
                    //Get Abstract for coding
                    abstractId = abstracts.OrderBy(x => rnd.Next()).First();

                    using (DataDataContext db = new DataDataContext(connString))
                    {
                        var matches = from a in db.tbl_Abstracts
                                      where a.AbstractID == abstractId
                                      select a;
                        abstr = matches.ToList<tbl_Abstract>().First();
                    }
                }
            }
            else
            {
                message = "No topics are available.";
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
                id = matches.FirstOrDefault();
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
    }
}
