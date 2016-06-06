using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using System.Configuration;
using ODPTaxonomyDAL_JY;
using ODPTaxonomyUtility_TT;
using System.Web.Security;
using Newtonsoft.Json;

namespace ODPTaxonomyWebsite.Evaluation.Handlers
{
    /// <summary>
    /// Summary description for AbstractDetail
    /// </summary>
    public class AbstractDetail : IHttpHandler
    {
        public string roleRequested = "";
        public string abstractID = "";
        public int sentAbstractID = 0;
        public string filter = "";

        protected void serializeResponse(HttpContext context, List<AbstractListRow> ALR)
        {
            IEnumerable<AbstractListRow> ALRX = ALR.Where(x => x.IsParent == true);
            context.Response.Write("{  \"data\" : " + JsonConvert.SerializeObject(ALRX.ToList()) + "  }");
            return;
        }

        public void ProcessRequest(HttpContext context)
        {
            List<AbstractListRow> parentAbstracts;
            List<AbstractListRow> ALR;
            AbstractViewRole AVR;
            roleRequested = context.Request["role"] ?? "";
            //abstractID = context.Request["abstractid"] ?? "0";
            filter = context.Request["filter"] ?? "";
            sentAbstractID = Int16.Parse(context.Request["abstractid"] ?? "0");
            parentAbstracts = getParentRecord(sentAbstractID);

            switch (roleRequested)
            {
                case "ODPSupervisor":
                    AVR = AbstractViewRole.ODPSupervisor;
                    break;
                default:
                    AVR = AbstractViewRole.CoderSupervisor;
                    break;

            }
            //AVR = AbstractViewRole.ODPSupervisor;
            ALR = AbstractListViewHelper.ProcessAbstractsIndividual(parentAbstracts, AVR);
            serializeResponse(context, ALR);


            //AbstractListViewData data = new AbstractListViewData();
            //serializeResponse(context, parentAbstracts);
        }

        private List<AbstractListRow> getParentRecord(int sentAbstractID)
        {
            string connStr = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString;
            DataJYDataContext db = new DataJYDataContext(connStr);


            var data = from a in db.Abstracts
                       join h in db.AbstractStatusChangeHistories on a.AbstractID equals h.AbstractID
                       join s in db.AbstractStatus on h.AbstractStatusID equals s.AbstractStatusID
                       where (
                          h.AbstractStatusChangeHistoryID == db.AbstractStatusChangeHistories.Where(h2 => h2.AbstractID == a.AbstractID).Select(h2 => h2.AbstractStatusChangeHistoryID).Max()
                          && a.AbstractID == sentAbstractID
                           )
                       select new AbstractListRow
                       {
                           AbstractID = a.AbstractID,
                           ProjectTitle = a.ProjectTitle + " (" + s.AbstractStatusCode + ")",
                           PIProjectLeader = a.PIProjectLeader,
                           ApplicationID = a.ChrApplicationID,
                           AbstractStatusID = s.AbstractStatusID,
                           AbstractStatusCode = s.AbstractStatusCode,
                           StatusDate = h.CreatedDate,
                           LastExportDate = a.LastExportDate,
                           EvaluationID = h.EvaluationId,
                           IsParent = true,
                           CodingType = a.CodingType == null ? "" : "Basic"
                       };



            List<AbstractListRow> abstracts = data.ToList();
            db.Dispose();
            return abstracts;

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}