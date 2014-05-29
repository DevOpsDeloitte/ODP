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
       
        public AbstractListViewData()
        {
            string connString = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString;
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
