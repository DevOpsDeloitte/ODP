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
        private string KappaSpecifier = "#.##";

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

        public AbstractListRow ConstructNewAbstractListRow(KappaData Kappa, string Title)
        {
            return new AbstractListRow
            {
                AbstractID = 0,
                ProjectTitle = Title,
                A1 = ((decimal)(Kappa.A1)).ToString(KappaSpecifier),
                A2 = ((decimal)(Kappa.A2)).ToString(KappaSpecifier),
                A3 = ((decimal)(Kappa.A3)).ToString(KappaSpecifier),
                B = ((decimal)(Kappa.B)).ToString(KappaSpecifier),
                C = ((decimal)(Kappa.C)).ToString(KappaSpecifier),
                D = ((decimal)(Kappa.D)).ToString(KappaSpecifier),
                E = ((decimal)(Kappa.E)).ToString(KappaSpecifier),
                F = ((decimal)(Kappa.F)).ToString(KappaSpecifier)
            };
        }

        public AbstractListRow FillInKappaValue(AbstractListRow Abstract, IEnumerable<KappaData> KappaData, KappaTypeEnum KappaType)
        {
            foreach (var Kappa in KappaData)
            {
                if (Kappa.KappaTypeID == (int)KappaType)
                {
                    Abstract.A1 = ((decimal)(Kappa.A1)).ToString(KappaSpecifier);
                    Abstract.A2 = ((decimal)(Kappa.A2)).ToString(KappaSpecifier);
                    Abstract.A3 = ((decimal)(Kappa.A3)).ToString(KappaSpecifier);
                    Abstract.B = ((decimal)(Kappa.B)).ToString(KappaSpecifier);
                    Abstract.C = ((decimal)(Kappa.C)).ToString(KappaSpecifier);
                    Abstract.D = ((decimal)(Kappa.D)).ToString(KappaSpecifier);
                    Abstract.E = ((decimal)(Kappa.E)).ToString(KappaSpecifier);
                    Abstract.F = ((decimal)(Kappa.F)).ToString(KappaSpecifier);

                    break;
                }
            }

            return Abstract;
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
