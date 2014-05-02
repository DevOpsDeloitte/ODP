using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ODPTaxonomyDAL_JY
{
    public class AbstractListView_CoderSupervisorModel
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
    }
}
