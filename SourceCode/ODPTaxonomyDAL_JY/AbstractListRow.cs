using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        private string _ProjectTitle;
        public string ProjectTitle
        {
            get
            {
                string title = _ProjectTitle;
                title += !string.IsNullOrEmpty(AbstractStatusCode) ? " (" + AbstractStatusCode + ")" : "";
                return title;
            }
            set
            {
                _ProjectTitle = value;
            }
        }

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

        private bool KappaDone;
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
            this.KappaDone = false;
            this.IsParent = false;
        }

        public void FillKappaValues()
        {
            if (!this.KappaDone)
            {
                AbstractListViewData data = new AbstractListViewData();

                string specifier = "#.##";
                KappaData kappa = data.GetKappaData(this.AbstractID,this.KappaType);

                if (kappa != null)
                {
                    this.A1 = ((decimal)(kappa.A1)).ToString(specifier);
                    this.A2 = ((decimal)(kappa.A2)).ToString(specifier);
                    this.A3 = ((decimal)(kappa.A3)).ToString(specifier);
                    this.B = ((decimal)(kappa.B)).ToString(specifier);
                    this.C = ((decimal)(kappa.C)).ToString(specifier);
                    this.D = ((decimal)(kappa.D)).ToString(specifier);
                    this.E = ((decimal)(kappa.E)).ToString(specifier);
                    this.F = ((decimal)(kappa.F)).ToString(specifier);
                    this.G = this.UnableToCode ? "Y" : "";
                }

                this.KappaDone = true;
            }
        }
    }
}
