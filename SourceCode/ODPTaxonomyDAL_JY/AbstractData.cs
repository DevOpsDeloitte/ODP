using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ODPTaxonomyDAL_JY
{
    public class AbstractData
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public List<AbstractListRow> data { get; set; }
        public string error { get; set; }
    }
}
