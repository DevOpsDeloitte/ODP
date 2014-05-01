using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ODPTaxonomyDAL_JY
{
    public class AbstractListView_CoderSupervisorModel
    {
        /**
         * Abstract table
         */
        public int AbstractID { get; set; }
        public int? ApplicationID { get; set; }
        public string ProjectTitle { get; set; }

        /**
         * AbstractStatusChangeHistory table
         */
        public DateTime? StatusDate { get; set; }
    }
}
