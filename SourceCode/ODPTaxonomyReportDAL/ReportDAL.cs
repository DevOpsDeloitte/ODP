using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace ODPTaxonomyReportDAL
{
    public class ReportDAL
    {
        public static string connString = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString;

        public static ReportDataLinqDataContext dataContext = new ReportDataLinqDataContext(connString);
    }
}
