using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace ODPTaxonomyDAL_JY
{
    public class AbstractListRow
    {
        /**
         * Determine if a row is a parent row
         */
        public bool IsParent { get; set; }

        public int AbstractID { get; set; }
        public int? ApplicationID { get; set; }

        public string ProjectTitle { get; set; }

        public int? TeamID { get; set; }
        public Guid? UserID { get; set; }

        public int? SubmissionID { get; set; }
        public string Comment { get; set; }

        public int? EvaluationID { get; set; }

        public int AbstractStatusID { get; set; }
        public string AbstractStatusCode { get; set; }
        public DateTime? StatusDate { get; set; }
        public string StatusDateDisplay
        {
            get
            {
                return this.StatusDate != null ? this.StatusDate.Value.ToString("d") : "";
            }
        }

        public string AbstractScan { get; set; }

        public bool UnableToCode { get; set; }

        public string KappaCoderAlias { get; set; }
        public KappaTypeEnum KappaType { get; set; }
        public string A1 { get; set; }
        public string A2 { get; set; }
        public string A3 { get; set; }
        public string B { get; set; }
        public string C { get; set; }
        public string D { get; set; }
        public string E { get; set; }
        public string F { get; set; }
        public string G { get; set; }

        public AbstractListRow()
        {
            this.IsParent = false;
        }

        public SubmissionTypeEnum GetSubmissionType()
        {
            SubmissionTypeEnum SubmissionType = SubmissionTypeEnum.NA;

            switch ((AbstractStatusEnum)this.AbstractStatusID)
            {
                case AbstractStatusEnum.CODED_BY_CODER_1A:
                    SubmissionType = SubmissionTypeEnum.CODER_EVALUATION;
                    break;
                case AbstractStatusEnum.CODED_BY_ODP_STAFF_2A:
                    SubmissionType = SubmissionTypeEnum.ODP_STAFF_EVALUATION;
                    break;
                case AbstractStatusEnum.CONSENSUS_COMPLETE_1B:
                case AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N:
                    SubmissionType = SubmissionTypeEnum.CODER_CONSENSUS;
                    break;
                case AbstractStatusEnum.ODP_CONSENSUS_WITH_NOTES_2N:
                case AbstractStatusEnum.ODP_STAFF_CONSENSUS_2B:
                    SubmissionType = SubmissionTypeEnum.ODP_STAFF_CONSENSUS;
                    break;
                default:
                    break;
            }

            return SubmissionType;
        }

        public void GetComment()
        {
            if (this.EvaluationID != null)
            {
                string connStr = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString;
                DataJYDataContext db = new DataJYDataContext(connStr);

                SubmissionTypeEnum SubmissionType = GetSubmissionType();

                this.Comment = (from s in db.Submissions
                                where s.EvaluationId == this.EvaluationID &&
                                s.SubmissionTypeId == (int)SubmissionType
                                select s.comments).FirstOrDefault();
            }
        }

        public void GetUnableToCode(SubmissionTypeEnum SubmissionType)
        {
            string connStr = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString;
            DataJYDataContext db = new DataJYDataContext(connStr);

            var query = from s in db.Submissions
                        join e in db.Evaluations on s.EvaluationId equals e.EvaluationId
                        where e.AbstractID == this.AbstractID && e.IsComplete == true &&
                        s.SubmissionTypeId == (int)SubmissionType
                        orderby s.SubmissionDateTime descending
                        select s.UnableToCode;

            this.G = query.FirstOrDefault() ? "UC" : "-";
        }

        public void GetUnableToCode(SubmissionTypeEnum SubmissionType, Guid? UserID)
        {
            if (UserID != null)
            {
                string connStr = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString;
                DataJYDataContext db = new DataJYDataContext(connStr);

                var query = from s in db.Submissions
                            join e in db.Evaluations on s.EvaluationId equals e.EvaluationId
                            where e.AbstractID == this.AbstractID && e.IsComplete == true &&
                            s.SubmissionTypeId == (int)SubmissionType && s.UserId == UserID.Value
                            orderby s.SubmissionDateTime descending
                            select s.UnableToCode;

                this.G = query.FirstOrDefault() ? "UC" : "-";
            }
            else
            {
                this.G = "-";
            }
            
        }

        public void GetAbstractScan()
        {
            if (this.EvaluationID != null)
            {
                string connStr = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString;
                DataJYDataContext db = new DataJYDataContext(connStr);

                this.AbstractScan = (from s in db.AbstractScans
                                     where s.EvaluationId == this.EvaluationID
                                     select s.FileName).FirstOrDefault();
            }
        }
    }
}
