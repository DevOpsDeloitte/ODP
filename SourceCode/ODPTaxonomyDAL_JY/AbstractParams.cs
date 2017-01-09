using System;
using System.Web;
using System.Web.UI.WebControls;

namespace ODPTaxonomyDAL_JY
{
    public class AbstractParams
    {
        public string role { get; set; }
        public string filter { get; set; }
        public string action { get; set; }
        public string codeType { get; set; }
        public int draw { get; set; }
        public int start { get; set; }
        public int length { get; set; }
        public string search { get; set; }
        public int sortColumnIndex { get; set; }
        public string sortColumn { get; set; }
        public SortDirection sortDirection { get; set; }

        public AbstractParams(HttpContext context)
        {
            role = context.Request["role"] ?? "";
            filter = context.Request["filter"] ?? "";
            action = context.Request["action"] ?? "";
            codeType = context.Request["codingType"] ?? "";

            draw = context.Request["draw"] != null ? Convert.ToInt32(context.Request["draw"]) : 0;
            start = context.Request["start"] != null ? Convert.ToInt32(context.Request["start"]) : 0;
            length = context.Request["length"] != null ? Convert.ToInt32(context.Request["length"]) : 10;
            search = context.Request["search[value]"];
            sortColumnIndex = Convert.ToInt32(context.Request["order[0][column]"]);
            sortColumn = context.Request["columns[" + sortColumnIndex + "][data]"] ?? "StatusDate";
            sortDirection = context.Request["order[0][dir]"] == "desc" ? SortDirection.Descending : SortDirection.Ascending;
        }
    }
}
