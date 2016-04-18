using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace ODPTaxonomyDAL_JY
{
    public class AbstractListViewData
    {
        private DataJYDataContext db;
        private DataJYDataContext db2;
       
        public AbstractListViewData()
        {
            string connString = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString;
            db2 = new DataJYDataContext(connString) { CommandTimeout = 0, ObjectTrackingEnabled = false };
            db = new DataJYDataContext(connString);
        }

        public AbstractListViewData(string ConnStr)
        {
            db = new DataJYDataContext(ConnStr);
        }

        public Evaluation GetCoderEvaluations(int AbstractID)
        {
            var query = (from e in db.Evaluations
                         where e.AbstractID == AbstractID &&
                               e.EvaluationTypeId == (int)EvaluationTypeEnum.CODER_EVALUATION &&
                               e.IsComplete == true
                         select e).FirstOrDefault();

            return query;
        }

        public Evaluation GetODPEvaluations(int AbstractID)
        {
            var query = (from e in db.Evaluations
                         where e.AbstractID == AbstractID &&
                               e.EvaluationTypeId == (int)EvaluationTypeEnum.ODP_EVALUATION &&
                               e.IsComplete == true
                         select e).FirstOrDefault();

            return query;
        }

        public IEnumerable<KappaUserIdentify> GetKappaIdentities(int TeamID)
        {
            var query = from ku in db.KappaUserIdentifies
                        where ku.TeamID == TeamID
                        orderby ku.UserAlias ascending
                        select ku;

            return query;
        }

        public IEnumerable<KappaData> GetAbstractKappaData(int AbstractID)
        {
            var query = from k in db.KappaDatas
                        where k.AbstractID == AbstractID
                        orderby k.KappaTypeID ascending
                        select k;

            return query;
        }

        public IEnumerable<KappaData> getAllKappaRecords()
        {
            var query = from k in db2.KappaDatas
                        select k;
            return query.ToList<KappaData>();
        }

        public IEnumerable<KappaData> getAllKappaRecordsK1K2Only()
        {
            var query = from k in db2.KappaDatas
                        where k.KappaTypeID == 1 || k.KappaTypeID == 2
                        select k;
            return query.ToList<KappaData>();
        }

        public IEnumerable<KappaData> getAllKappaRecords(int AbstractID)
        {
            var query = from k in db2.KappaDatas
                        where k.AbstractID == AbstractID
                        select k;
            return query.ToList<KappaData>();
        }

        public IEnumerable<Submission> getAllSubmissionRecords()
        {
            var query = from k in db2.Submissions
                        select k;
            return query.ToList<Submission>();
        }

        public IEnumerable<int> getSubmissionIds(List<int> EvaluationIds)
        {
            var query = from k in db2.Submissions
                        where EvaluationIds.Contains(k.EvaluationId ?? 0)
                        select k.SubmissionID;
            return query.ToList<int>();

        }

        public IEnumerable<Submission> getAllSubmissionRecords(List<int> EvaluationIds)
        {
            var query = from k in db2.Submissions
                        where EvaluationIds.Contains( k.EvaluationId ?? 0)
                        select k;
            return query.ToList<Submission>();
        }

        public IEnumerable<Evaluation> getAllEvaluationRecords()
        {
            var query = from k in db2.Evaluations
                        select k;
            return query.ToList<Evaluation>();
        }

        public IEnumerable<Evaluation> getAllEvaluationRecords(int AbstractID)
        {
            var query = from k in db2.Evaluations
                        where k.AbstractID == AbstractID
                        select k;
            return query.ToList<Evaluation>();
        }

        public IEnumerable<E_StudyDesignPurposeAnswer> getAllE_StudyDesignPurposeRecords()
        {
            var query = from k in db2.E_StudyDesignPurposeAnswers
                        select k;
            return query.ToList<E_StudyDesignPurposeAnswer>();
        }

        public IEnumerable<E_StudyDesignPurposeAnswer> getAllE_StudyDesignPurposeRecordsID7()
        {
            var query = from k in db2.E_StudyDesignPurposeAnswers
                        where k.StudyDesignPurposeID == 7
                        select k;
            return query.ToList<E_StudyDesignPurposeAnswer>();
        }
        public IEnumerable<E_StudyDesignPurposeAnswer> getAllE_StudyDesignPurposeRecords(IEnumerable<int> SubmissionIds)
        {
            var query = from k in db2.E_StudyDesignPurposeAnswers
                        where SubmissionIds.Contains(k.SubmissionID)
                        select k;
            return query.ToList<E_StudyDesignPurposeAnswer>();
        }

        public IEnumerable<F_PreventionCategoryAnswer> getAllF_PreventionCategoryRecords()
        {
            var query = from k in db2.F_PreventionCategoryAnswers
                        select k;
            return query.ToList<F_PreventionCategoryAnswer>();
        }
        public IEnumerable<F_PreventionCategoryAnswer> getAllF_PreventionCategoryRecordsID6()
        {
            var query = from k in db2.F_PreventionCategoryAnswers
                        where k.PreventionCategoryID == 6
                        select k;
            return query.ToList<F_PreventionCategoryAnswer>();
        }

        public IEnumerable<F_PreventionCategoryAnswer> getAllF_PreventionCategoryRecords(IEnumerable<int> SubmissionIds)
        {
            var query = from k in db2.F_PreventionCategoryAnswers
                        where SubmissionIds.Contains(k.SubmissionID)
                        select k;
            return query.ToList<F_PreventionCategoryAnswer>();
        }


        public IEnumerable<KappaData> GetAbstractKappaData2(int AbstractID, IEnumerable<KappaData> cacheKappaData)
        {
            var query = from k in cacheKappaData
                        where k.AbstractID == AbstractID
                        orderby k.KappaTypeID ascending
                        select k;

            return query.ToList<KappaData>();
        }

        

        public bool IsAbstractInReview(int AbstractID)
        {
            return db.AbstractReviewLists.Where(i => i.AbstractID == AbstractID).Count() > 0;
        }

        public void AddAbstractToReview(int AbstractID, Guid UserID)
        {
            if (AbstractID > 0 && UserID != null)
            {
                AbstractReviewList review = new AbstractReviewList();
                review.AbstractID = AbstractID;
                review.CreatedBy = UserID;
                review.CreatedDate = DateTime.Now;

                db.AbstractReviewLists.InsertOnSubmit(review);
                db.SubmitChanges();
            }
        }

        public void RemoveAbstractFromReview(int AbstractID)
        {
            if (AbstractID > 0)
            {
                var Review = db.AbstractReviewLists.Where(i => i.AbstractID == AbstractID).FirstOrDefault();

                if (Review != null)
                {
                    db.AbstractReviewLists.DeleteOnSubmit(Review);
                    db.SubmitChanges();
                }
            }
        }

        public bool IsAbstractInReportExclude(int AbstractID)
        {
            return db.Report_AbstractExcludedLists.Where(i => i.AbstractID == AbstractID).Count() > 0;
        }

        public void AddAbstractToReportExclude(int AbstractID, Guid UserID)
        {
            if (AbstractID > 0 && UserID != null)
            {
                Report_AbstractExcludedList review = new Report_AbstractExcludedList();
                review.AbstractID = AbstractID;
                review.CreatedBy = UserID;
                review.CreatedDate = DateTime.Now;

                db.Report_AbstractExcludedLists.InsertOnSubmit(review);
                db.SubmitChanges();
            }
        }

        public void RemoveAbstractFromReportExclude(int AbstractID)
        {
            if (AbstractID > 0)
            {
                var Review = db.Report_AbstractExcludedLists.Where(i => i.AbstractID == AbstractID).FirstOrDefault();

                if (Review != null)
                {
                    db.Report_AbstractExcludedLists.DeleteOnSubmit(Review);
                    db.SubmitChanges();
                }
            }
        }
    }
}
