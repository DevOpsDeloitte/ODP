using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ODPTaxonomyDAL_JY
{
    public class AbstractReviewParams : AbstractParams
    {
        public bool all { get; set; }
        public List<int> includeList { get; set; }
        public List<int> excludeList { get; set; }
        public Guid userGuid { get; set; }

        public AbstractReviewParams(HttpContext context) : base(context)
        {
            all = context.Request["all"] != null && context.Request["all"] == "true";

            Guid guid;
            if (context.Request["guid"] != null && Guid.TryParse(context.Request["guid"], out guid))
            {
                userGuid = guid;
            }

            if (context.Request["includeList"] != null)
            {
                includeList = context.Request["includeList"]
                    .Split(new char[] { ',' })
                    .Select(i => Convert.ToInt32(i))
                    .ToList();
            }
            else if (context.Request["abstracts"] != null)
            {
                includeList = context.Request["abstracts"]
                    .Split(new char[] { ',' })
                    .Select(i => Convert.ToInt32(i))
                    .ToList();
            }

            if (context.Request["excludeList"] != null)
            {
                excludeList = context.Request["excludeList"]
                    .Split(new char[] { ',' })
                    .Select(i => Convert.ToInt32(i))
                    .ToList();
            }
        }

        public List<int> Abstracts
        {
            get
            {
                return null;
            }
        }
    }
}
