using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ODPTaxonomyDAL_JY
{
    public class AbstractListRow
    {
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

        public int? SubmissionID { get; set; }
        public string Comment { get; set; }

        public int? EvaluationID { get; set; }

        public int AbstractStatusID { get; set; }
        public string AbstractStatusCode { get; set; }
        public DateTime? StatusDate { get; set; }

        public string AbstractScan { get; set; }

        public bool UnableToCode { get; set; }

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

        }

        public void FillKappaValues()
        {
            AbstractListViewData data = new AbstractListViewData();

            string specifier = "#.##";
            KappaData kappa = null;

            if (this.AbstractStatusID == (int)AbstractStatusEnum.CONSENSUS_COMPLETE_1B)
            {
                kappa = data.GetKappaData(this.AbstractID, (int)KappaTypeEnum.CODER_COMPARISON_K1);
            }
            else if (this.AbstractStatusID == (int)AbstractStatusEnum.CODED_BY_CODER_1A)
            {
            }

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
                this.G = this.UnableToCode ? "Y" : "N";
            }
        }
    }
}
