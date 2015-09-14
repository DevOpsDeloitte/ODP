using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace ODPTaxonomyDAL_JY
{
    public class SubmissionData
    {
        public int SubmissionID { get; set; }
        public string Comment { get; set; }
        public bool UnableToCode { get; set; }
    }

    public class AbstractListRow
    {
        /**
         * Determine if a row is a parent row
         */
        
        public bool IsParent { get; set; }
        public bool InReview { get; set; }

        public int AbstractID { get; set; }
        public int? ApplicationID { get; set; }

        public string ProjectTitle { get; set; }
        public string PIProjectLeader { get; set; }
        public DateTime? LastExportDate { get; set; }

        public int? TeamID { get; set; }
        public Guid? UserID { get; set; }

        public int? SubmissionID { get; set; }
        public string Comment { get; set; }
        public bool Flag_E7 { get; set; }
        public bool Flag_F6 { get; set; }

        public string Flags
        {
            get
            {
                string flags = "";

                if (!string.IsNullOrEmpty(this.Comment))
                {
                    flags += "Com, ";
                }

                if (this.Flag_E7 && this.Flag_F6)
                {
                    flags += "E7F6, ";
                }

                return flags.Trim(new char[] { ' ', ',' });
            }
        }
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

        public List<AbstractListRow> ChildRows { get; set; }

        public AbstractListRow()
        {
            this.IsParent = false;
        }

        public void GetSubmissionData(SubmissionTypeEnum SubmissionType)
        {
            string connStr = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString;
            DataJYDataContext db = new DataJYDataContext(connStr);

            var query = (from s in db.Submissions
                         join e in db.Evaluations on s.EvaluationId equals e.EvaluationId
                         where e.AbstractID == this.AbstractID && e.IsComplete == true &&
                         s.SubmissionTypeId == (int)SubmissionType
                         orderby s.SubmissionDateTime descending
                         select new SubmissionData
                         {
                             SubmissionID = s.SubmissionID,
                             UnableToCode = s.UnableToCode,
                             Comment = s.comments
                         }).FirstOrDefault();

            if (query != null)
            {
                this.G = query.UnableToCode ? "UC" : "";
                this.Comment = query.Comment;

                this.Flag_E7 = db.E_StudyDesignPurposeAnswers
                    .Where(e => e.SubmissionID == query.SubmissionID && e.StudyDesignPurposeID == 7)
                    .Count() > 0;
                this.Flag_F6 = db.F_PreventionCategoryAnswers
                    .Where(f => f.SubmissionID == query.SubmissionID && f.PreventionCategoryID == 6)
                    .Count() > 0;
            }
        }

        public void GetSubmissionData2(SubmissionTypeEnum SubmissionType, IEnumerable<Submission> cacheSubmissions, IEnumerable<Evaluation> cacheEvaluations, IEnumerable<E_StudyDesignPurposeAnswer> cacheE_StudyDesignPurposeAnswers, IEnumerable<F_PreventionCategoryAnswer> cacheF_PreventionCategoryAnswers)
        {
            //string connStr = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString;
            //DataJYDataContext db = new DataJYDataContext(connStr);

            var query = (from s in cacheSubmissions
                         join e in cacheEvaluations on s.EvaluationId equals e.EvaluationId
                         where e.AbstractID == this.AbstractID && e.IsComplete == true &&
                         s.SubmissionTypeId == (int)SubmissionType
                         orderby s.SubmissionDateTime descending
                         select new SubmissionData
                         {
                             SubmissionID = s.SubmissionID,
                             UnableToCode = s.UnableToCode,
                             Comment = s.comments
                         }).FirstOrDefault();

            if (query != null)
            {
                this.G = query.UnableToCode ? "UC" : "";
                this.Comment = query.Comment;

                this.Flag_E7 = cacheE_StudyDesignPurposeAnswers
                    .Where(e => e.SubmissionID == query.SubmissionID && e.StudyDesignPurposeID == 7)
                    .Count() > 0;
                this.Flag_F6 = cacheF_PreventionCategoryAnswers
                    .Where(f => f.SubmissionID == query.SubmissionID && f.PreventionCategoryID == 6)
                    .Count() > 0;
            }
        }

        public void GetSubmissionData(SubmissionTypeEnum SubmissionType, Guid? UserID)
        {
            if (UserID != null)
            {
                string connStr = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString;
                DataJYDataContext db = new DataJYDataContext(connStr);

                var query = (from s in db.Submissions
                             join e in db.Evaluations on s.EvaluationId equals e.EvaluationId
                             where e.AbstractID == this.AbstractID && e.IsComplete == true &&
                             s.SubmissionTypeId == (int)SubmissionType && s.UserId == UserID.Value
                             orderby s.SubmissionDateTime descending
                             select new SubmissionData
                             {
                                 SubmissionID = s.SubmissionID,
                                 UnableToCode = s.UnableToCode,
                                 Comment = s.comments
                             }).FirstOrDefault();

                if (query != null)
                {
                    this.G = query.UnableToCode ? "UC" : "";
                    this.Comment = query.Comment;

                    this.Flag_E7 = db.E_StudyDesignPurposeAnswers
                    .Where(e => e.SubmissionID == query.SubmissionID && e.StudyDesignPurposeID == 7)
                    .Count() > 0;
                    this.Flag_F6 = db.F_PreventionCategoryAnswers
                        .Where(f => f.SubmissionID == query.SubmissionID && f.PreventionCategoryID == 6)
                        .Count() > 0;
                }
            }
            else
            {
                this.G = "-";
            }

        }

        public void GetSubmissionData2(SubmissionTypeEnum SubmissionType, Guid? UserID, IEnumerable<Submission> cacheSubmissions, IEnumerable<Evaluation> cacheEvaluations, IEnumerable<E_StudyDesignPurposeAnswer> cacheE_StudyDesignPurposeAnswers, IEnumerable<F_PreventionCategoryAnswer> cacheF_PreventionCategoryAnswers)
        {
            if (UserID != null)
            {
                //string connStr = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString;
                //DataJYDataContext db = new DataJYDataContext(connStr);

                var query = (from s in cacheSubmissions
                             join e in cacheEvaluations on s.EvaluationId equals e.EvaluationId
                             where e.AbstractID == this.AbstractID && e.IsComplete == true &&
                             s.SubmissionTypeId == (int)SubmissionType && s.UserId == UserID.Value
                             orderby s.SubmissionDateTime descending
                             select new SubmissionData
                             {
                                 SubmissionID = s.SubmissionID,
                                 UnableToCode = s.UnableToCode,
                                 Comment = s.comments
                             }).FirstOrDefault();

                if (query != null)
                {
                    this.G = query.UnableToCode ? "UC" : "";
                    this.Comment = query.Comment;

                    this.Flag_E7 = cacheE_StudyDesignPurposeAnswers
                    .Where(e => e.SubmissionID == query.SubmissionID && e.StudyDesignPurposeID == 7)
                    .Count() > 0;
                    this.Flag_F6 = cacheF_PreventionCategoryAnswers
                        .Where(f => f.SubmissionID == query.SubmissionID && f.PreventionCategoryID == 6)
                        .Count() > 0;
                }
            }
            else
            {
                this.G = "-";
            }

        }

        public void GetAbstractScan(AbstractViewRole AbstractViewRole)
        {
            if (this.EvaluationID != null)
            {
                //string connStr = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString;
                //DataJYDataContext db = new DataJYDataContext(connStr);

                if (this.AbstractStatusID >= 6 && AbstractViewRole.CoderSupervisor == AbstractViewRole)
                {
                    this.AbstractScan = "file_exists";
                }

                if (this.AbstractStatusID >= 12 && (AbstractViewRole.ODPStaff == AbstractViewRole || AbstractViewRole.ODPSupervisor == AbstractViewRole))
                {
                    this.AbstractScan = "file_exists";
                }
                


                //this.AbstractScan = (from s in db.AbstractScans
                //                     where s.EvaluationId == this.EvaluationID
                //                     select s.FileName).FirstOrDefault();
            }
        }
    }
}
