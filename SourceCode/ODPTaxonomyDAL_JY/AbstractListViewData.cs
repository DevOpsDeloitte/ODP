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

        public IEnumerable<Submission> getAllSubmissionRecords()
        {
            var query = from k in db2.Submissions
                        select k;
            return query.ToList<Submission>();
        }

        public IEnumerable<Evaluation> getAllEvaluationRecords()
        {
            var query = from k in db2.Evaluations
                        select k;
            return query.ToList<Evaluation>();
        }

        public IEnumerable<E_StudyDesignPurposeAnswer> getAllE_StudyDesignPurposeRecords()
        {
            var query = from k in db2.E_StudyDesignPurposeAnswers
                        select k;
            return query.ToList<E_StudyDesignPurposeAnswer>();
        }

        public IEnumerable<F_PreventionCategoryAnswer> getAllF_PreventionCategoryRecords()
        {
            var query = from k in db2.F_PreventionCategoryAnswers
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
    }
}
