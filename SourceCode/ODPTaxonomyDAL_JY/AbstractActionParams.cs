using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ODPTaxonomyDAL_JY
{
    public class AbstractActionParams : AbstractParams
    {
        public bool all { get; set; }
        public bool basic { get; set; }
        public List<int> includeList { get; set; }
        public List<int> excludeList { get; set; }
        public Guid userGuid { get; set; }
        public string type { get; set; }

        public AbstractActionParams(HttpContext context) : base(context)
        {
            includeList = new List<int>();
            excludeList = new List<int>();

            type = context.Request["type"] ?? "";
            all = context.Request["all"] != null && context.Request["all"] == "true";
            basic = context.Request["basic"] != null && context.Request["basic"] == "true";

            Guid guid;
            if (context.Request["guid"] != null && Guid.TryParse(context.Request["guid"], out guid))
            {
                userGuid = guid;
            }

            if (!string.IsNullOrEmpty(context.Request["includeList"]))
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

            if (!string.IsNullOrEmpty(context.Request["excludeList"]))
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
                // get all
                start = 0;
                length = 99999;

                List<int> IDs = new List<int>();

                if (all)
                {
                    var abstractData = AbstractHelper.GetAbstracts(this);

                    if (basic) {
                        IDs = abstractData.data.Where(a=>a.ApplicationID.Contains("_B")).Select(a => a.AbstractID).ToList(); ;
                    }
                    else {
                        IDs = abstractData.data.Select(a => a.AbstractID).ToList(); ;
                    }
                    //excludeList.Add(4094);        
                    if (excludeList.Count > 0)
                    {
                        IDs.RemoveAll(id => excludeList.Contains(id));
                    }
                    
                }
                else if (!all && includeList.Count > 0)
                {
                    IDs = includeList;
                }


                return IDs;
            }
        }
    }
}