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
        ODPStaffMemberConsensus = 1,
        ODPStaffMemberComparison = 1
    }

    public enum AbstractStatusID
    {
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

        //public static tbl_Abstract GetAbstractByAbstractId(string connString, int abstractId)
        //{
        //    tbl_Abstract abstr = null;

        //    using (DataDataContext db = new DataDataContext(connString))
        //    {
        //        var matches = from a in db.tbl_Abstracts
        //                      where a.AbstractID == abstractId
        //                      select a;
        //        abstr = matches.ToList<tbl_Abstract>().First();
        //    }

        //    return abstr;
        //}

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
                        var matches = from at in db.tbl_AbstractTopics
                                      join sh in db.tbl_AbstractStatusChangeHistories on at.AbstractID equals sh.AbstractID
                                      where sh.AbstractStatusID == (int)AbstractStatusID._0 && at.StudyFocusID == i
                                      select at.AbstractID;
                        foreach (var item in matches)
                        {
                            abstracts.Add(item);
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


        public static ViewAbstractData GetEvaluationData(string connString, int teamId, int evaluationTypeId)
        {
            ViewAbstractData data = null;
            List<int> abstractStatusIds = new List<int>();

            if (evaluationTypeId == (int)EvaluationType.CoderEvaluation)
            {
                abstractStatusIds.Add((int)AbstractStatusID._1);
                abstractStatusIds.Add((int)AbstractStatusID._1A);
            }

            if (evaluationTypeId == (int)EvaluationType.ODPEvaluation)
            {
                abstractStatusIds.Add((int)AbstractStatusID._2);
                abstractStatusIds.Add((int)AbstractStatusID._2A);
            }

            using (DataDataContext db = new DataDataContext(connString))
            {
                var matches = from e in db.tbl_Evaluations
                              join a in db.tbl_Abstracts on e.AbstractID equals a.AbstractID
                              join sh in db.tbl_AbstractStatusChangeHistories on a.AbstractID equals sh.AbstractID
                              where abstractStatusIds.Contains(sh.AbstractStatusID) && e.TeamID == teamId && e.EvaluationTypeId == evaluationTypeId
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
