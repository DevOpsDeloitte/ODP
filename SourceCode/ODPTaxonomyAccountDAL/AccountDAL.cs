using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace ODPTaxonomyAccountDAL
{
    public class AccountDAL
    {
        public static string connString = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString;

        public static AccountDataLinqDataContext dataContext = new AccountDataLinqDataContext(connString);
    }
}
