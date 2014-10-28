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
    /// Summary description for Abstracts
    /// </summary>
    public class Abstracts : IHttpHandler
    {
        public string roleRequested = "";
        public string filter1 = "";
        public short? submissiontypeID = 0;


        protected void serializeResponse(HttpContext context, List<AbstractListRow> ALR){
              IEnumerable<AbstractListRow> ALRX = ALR.Where(x => x.IsParent == true);                                      
              context.Response.Write("{  \"data\" : " +JsonConvert.SerializeObject(ALRX.ToList())+"  }");
               return;
        }
        
        public void ProcessRequest(HttpContext context)
        {
            List<AbstractListRow> parentAbstracts;
            List<AbstractListRow> ALR;
            roleRequested = context.Request["role"] ?? "";
            filter1 = context.Request["filter1"] ?? "";

            switch (roleRequested)
            {

                case "ODPSupervisor":

                    switch (filter1)
                    {
                        case "open":

                            break;

                        case "coded":

                            break;

                        default: // return default
                            parentAbstracts = GetParentAbstractsODPSupervisorDefault();
                            System.Diagnostics.Stopwatch objStopWatch = new System.Diagnostics.Stopwatch();
                            objStopWatch.Start();
                            AbstractListViewData data = new AbstractListViewData();
                            foreach (var abs in parentAbstracts)
                            {
                                if (data.IsAbstractInReview(abs.AbstractID))
                                {
                                    abs.InReview  = true;
                                }
                                else
                                {
                                    abs.InReview = false;
                                }
                            }
                            ALR = AbstractListViewHelper.ProcessAbstracts2(parentAbstracts, AbstractViewRole.ODPSupervisor);
                            objStopWatch.Stop();
                            serializeResponse(context, ALR);
                            System.Diagnostics.Trace.WriteLine("The time taken to execute ProcessAbstracts is : " +
                            objStopWatch.ElapsedMilliseconds.ToString() + " MillionSeconds<br>");
                            objStopWatch.Reset();
                            break;

                    }
                   
                    break;
                case "CoderSupervisor":

                    switch (filter1)
                    {
                        case "open" :

                            break;

                        case "coded" :

                            break;

                        default: // return open
                              parentAbstracts = this.GetParentAbstractsCoderSupervisorOpen();
                              ALR = AbstractListViewHelper.ProcessAbstracts2(parentAbstracts, AbstractViewRole.CoderSupervisor);
                              serializeResponse(context, ALR);
                            break;

                    }

                  
                    break;

                case "ODPStaff":
                    parentAbstracts = this.GetParentAbstractsODPStaffMemberDefault();
                    ALR = AbstractListViewHelper.ProcessAbstracts2(parentAbstracts, AbstractViewRole.ODPStaff);
                    serializeResponse(context, ALR);
                    break;
                case "Admin" :

                    break;

                case null :

                    break;

                default:

                    break;



            }

            

           


          
            //context.Response.Write(JsonConvert.SerializeObject(new { success = false, supervisorauthfailed = true }));
            return;
        }

        protected List<AbstractListRow> GetParentAbstractsCoderSupervisorOpen(string sort = "", SortDirection direction = SortDirection.Ascending)
        {
            string connString = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString;
            DataJYDataContext db = new DataJYDataContext(connString);

            var data = from a in db.Abstracts
                       join h in db.AbstractStatusChangeHistories on a.AbstractID equals h.AbstractID
                       join s in db.AbstractStatus on h.AbstractStatusID equals s.AbstractStatusID
                       where (
                          (h.AbstractStatusID == (int)AbstractStatusEnum.RETRIEVED_FOR_CODING_1 ||
                          h.AbstractStatusID == (int)AbstractStatusEnum.CODED_BY_CODER_1A) &&
                          h.AbstractStatusChangeHistoryID == db.AbstractStatusChangeHistories
                           .Where(h2 => h2.AbstractID == a.AbstractID)
                           .Select(h2 => h2.AbstractStatusChangeHistoryID).Max()
                           )
                       select new AbstractListRow
                       {
                           AbstractID = a.AbstractID,
                           ProjectTitle = a.ProjectTitle + " (" + s.AbstractStatusCode + ")",
                           ApplicationID = a.ApplicationID,
                           AbstractStatusID = s.AbstractStatusID,
                           AbstractStatusCode = s.AbstractStatusCode,
                           StatusDate = h.CreatedDate,
                           KappaType = KappaTypeEnum.K1,
                           IsParent = true
                       };

            List<AbstractListRow> abstracts = data.ToList();

            //if (AbstractViewGridView.Attributes["CurrentSortExp"] != null)
            //{
            //    sort = AbstractViewGridView.Attributes["CurrentSortExp"];
            //    direction = AbstractViewGridView.Attributes["CurrentSortDir"] == "ASC" ? SortDirection.Ascending : SortDirection.Descending;
            //}

            return AbstractListViewHelper.SortAbstracts(abstracts, sort, direction);
        }

        protected List<AbstractListRow> GetParentAbstractsODPSupervisorDefault(string sort = "", SortDirection direction = SortDirection.Ascending)
        {
            string connStr = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString;
            DataJYDataContext db = new DataJYDataContext(connStr);

            System.Diagnostics.Stopwatch objStopWatch = new System.Diagnostics.Stopwatch();
            objStopWatch.Start();

            var data = from a in db.Abstracts
                       join h in db.AbstractStatusChangeHistories on a.AbstractID equals h.AbstractID
                       join s in db.AbstractStatus on h.AbstractStatusID equals s.AbstractStatusID
                       where (
                          h.AbstractStatusID >= (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N &&
                          h.AbstractStatusChangeHistoryID == db.AbstractStatusChangeHistories
                           .Where(h2 => h2.AbstractID == a.AbstractID)
                           .Select(h2 => h2.AbstractStatusChangeHistoryID).Max()
                           )
                       select new AbstractListRow
                       {
                           AbstractID = a.AbstractID,
                           ProjectTitle = a.ProjectTitle + " (" + s.AbstractStatusCode + ")",
                           ApplicationID = a.ApplicationID,
                           AbstractStatusID = s.AbstractStatusID,
                           AbstractStatusCode = s.AbstractStatusCode,
                           StatusDate = h.CreatedDate,
                           EvaluationID = h.EvaluationId,
                           IsParent = true
                       };

            var genSQL = db.GetCommand(data).CommandText;
            //System.Diagnostics.Trace.WriteLine(genSQL);

           

            List<AbstractListRow> abstracts = data.ToList();

            objStopWatch.Stop();
            System.Diagnostics.Trace.Write("The time taken to execute query without compilation is : " +
            objStopWatch.ElapsedMilliseconds.ToString() + " MillionSeconds<br>");
            objStopWatch.Reset();
           
            //db.Log = Console.Out;

            //if (AbstractViewGridView.Attributes["CurrentSortExp"] != null)
            //{
            //    sort = AbstractViewGridView.Attributes["CurrentSortExp"];
            //    direction = AbstractViewGridView.Attributes["CurrentSortDir"] == "ASC" ? SortDirection.Ascending : SortDirection.Descending;
            //}

            return AbstractListViewHelper.SortAbstracts(abstracts, sort, direction);
        }

        protected List<AbstractListRow> GetParentAbstractsODPSupervisorReview(string sort = "", SortDirection direction = SortDirection.Ascending)
        {
            string connString = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString;
            DataJYDataContext db = new DataJYDataContext(connString);

            var data = from a in db.Abstracts
                       join h in db.AbstractStatusChangeHistories on a.AbstractID equals h.AbstractID
                       join s in db.AbstractStatus on h.AbstractStatusID equals s.AbstractStatusID
                       join rv in db.AbstractReviewLists on a.AbstractID equals rv.AbstractID
                       where (
                          h.AbstractStatusID >= (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N &&
                          h.AbstractStatusChangeHistoryID == db.AbstractStatusChangeHistories
                           .Where(h2 => h2.AbstractID == a.AbstractID)
                           .Select(h2 => h2.AbstractStatusChangeHistoryID).Max()
                           )
                       select new AbstractListRow
                       {
                           AbstractID = a.AbstractID,
                           ProjectTitle = a.ProjectTitle + " (" + s.AbstractStatusCode + ")",
                           ApplicationID = a.ApplicationID,
                           AbstractStatusID = s.AbstractStatusID,
                           AbstractStatusCode = s.AbstractStatusCode,
                           StatusDate = h.CreatedDate,
                           EvaluationID = h.EvaluationId,
                           KappaType = KappaTypeEnum.K1,
                           IsParent = true
                       };

            List<AbstractListRow> abstracts = data.ToList();

            //if (AbstractViewGridView.Attributes["CurrentSortExp"] != null)
            //{
            //    sort = AbstractViewGridView.Attributes["CurrentSortExp"];
            //    direction = AbstractViewGridView.Attributes["CurrentSortDir"] == "ASC" ? SortDirection.Ascending : SortDirection.Descending;
            //}

            return AbstractListViewHelper.SortAbstracts(abstracts, sort, direction);
        }

        protected List<AbstractListRow> GetParentAbstractsODPStaffMemberDefault(string sort = "", SortDirection direction = SortDirection.Ascending)
        {
            string connString = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString;
            DataJYDataContext db = new DataJYDataContext(connString);

            var data = from a in db.Abstracts
                       join h in db.AbstractStatusChangeHistories on a.AbstractID equals h.AbstractID
                       join s in db.AbstractStatus on h.AbstractStatusID equals s.AbstractStatusID
                       where (
                          h.AbstractStatusID >= (int)AbstractStatusEnum.CONSENSUS_COMPLETE_WITH_NOTES_1N &&
                          h.AbstractStatusChangeHistoryID == db.AbstractStatusChangeHistories
                           .Where(h2 => h2.AbstractID == a.AbstractID)
                           .Select(h2 => h2.AbstractStatusChangeHistoryID).Max()
                           )
                       select new AbstractListRow
                       {
                           AbstractID = a.AbstractID,
                           ProjectTitle = a.ProjectTitle + " (" + s.AbstractStatusCode + ")",
                           ApplicationID = a.ApplicationID,
                           AbstractStatusID = s.AbstractStatusID,
                           AbstractStatusCode = s.AbstractStatusCode,
                           StatusDate = h.CreatedDate,
                           EvaluationID = h.EvaluationId,
                           KappaType = KappaTypeEnum.K1,
                           IsParent = true
                       };

            List<AbstractListRow> abstracts = data.ToList();

            //if (AbstractViewGridView.Attributes["CurrentSortExp"] != null)
            //{
            //    sort = AbstractViewGridView.Attributes["CurrentSortExp"];
            //    direction = AbstractViewGridView.Attributes["CurrentSortDir"] == "ASC" ? SortDirection.Ascending : SortDirection.Descending;
            //}

            return AbstractListViewHelper.SortAbstracts(abstracts, sort, direction);
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