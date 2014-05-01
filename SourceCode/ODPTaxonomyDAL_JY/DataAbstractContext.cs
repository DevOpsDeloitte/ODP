using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ODPTaxonomyDAL_JY
{
    public class DataAbstractContext : DataJYDataContext
    {
        public DataAbstractContext()
            : base("ODP_Taxonomy_DEV")
        {
        }
    }
}
