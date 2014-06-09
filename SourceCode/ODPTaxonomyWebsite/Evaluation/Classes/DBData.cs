using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Configuration;
using ODPTaxonomyDAL_ST;

namespace ODPTaxonomyWebsite.Evaluation.Classes
{
    public class DBData
    {
  
        public static DataDataContext GetDataContext()
        {
            string connString = null;
            connString = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ToString();
            var db = new DataDataContext(connString);
            return db;

        }

        public static bool isValidCoderEvaluation(Guid userId, int evaluationId, int submissionTypeId)
        {
            var db = DBData.GetDataContext();
            bool res = db.Submissions.Where(x => x.UserId == userId &&
                                    (x.SubmissionTypeId == submissionTypeId) &&
                                    x.EvaluationId == evaluationId)
                          .Any();

            if (!res && submissionTypeId == 1) return true;
            else return false;
          
        }

        public static Submission getSubmissionRecord(Guid userId, int evaluationId, int submissionTypeId)
        {      
            var db = DBData.GetDataContext();
            var rec = db.Submissions.Where(x => x.UserId == userId &&
                                    (x.SubmissionTypeId == submissionTypeId) &&
                                    x.EvaluationId == evaluationId)
                          .FirstOrDefault();

            if (rec != null)
            {
                return rec;
            }
            else
            {
                return null;
            }

        }

        public static int getSubmissionID(Guid userId, int evaluationId, int submissionTypeId)
        {
            int retval;
            var db = DBData.GetDataContext();
            var rec = db.Submissions.Where(x => x.UserId == userId &&
                                    (x.SubmissionTypeId == submissionTypeId) &&
                                    x.EvaluationId == evaluationId)
                          .FirstOrDefault();

            if (rec != null)
            {
                //HttpContext.Current.Response.Write("here");
                retval = rec.SubmissionID;
            }
            else
            {
                retval = 0;
            }

            return retval;

        }


        public static bool isValidConsensus(Guid userId, int evaluationId, int submissionTypeId)
        {
            var db = DBData.GetDataContext();
            bool res = db.Submissions.Where(x => x.UserId == userId &&
                                    (x.SubmissionTypeId == submissionTypeId) &&
                                    x.EvaluationId == evaluationId)
                          .Any();

            if (!res && submissionTypeId == 2) return true;
            else return false;
        }


    }
}