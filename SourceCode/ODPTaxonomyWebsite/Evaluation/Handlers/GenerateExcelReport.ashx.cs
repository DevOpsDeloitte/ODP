using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ODPTaxonomyUtility_TT;
using ODPTaxonomyDAL_TT;
using System.Configuration;
using Newtonsoft.Json;

namespace ODPTaxonomyWebsite.Evaluation.Handlers
{
    /// <summary>
    /// Summary description for GenerateExcelReport
    /// </summary>
    public class GenerateExcelReport : IHttpHandler
    {
        #region Fields

        private string connString = null;
        private string abstracts = "";
        private List<string> abstractIDs;      

        #endregion

        public void ProcessRequest(HttpContext context)
        {
            //Do Not return any text from this method
            //Otherwise an error message appears on opening saved Excel file
            try
            {
                connString = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ToString();
                abstracts = context.Request["abstracts"] ?? "";
                if (String.IsNullOrEmpty(abstracts))
                {
                    context.Response.Write("Important parameter is missing");
                }
                else
                {
                    abstractIDs = abstracts.Split(',').ToList();
                    //List<rpt_OPAResult> opaData = Common.GetReportData_OpaData(connString, abstractIDs);
                    //List<rpt_KappaDataResult> kappaData = Common.GetReportData_KappaData(connString, abstractIDs);
                    //List<rpt_Cdr_ODPNotesPDFResult> cdr_ODPNotesPDF = Common.GetReportData_Cdr_ODPNotesPDF(connString, abstractIDs);
                    //List<rpt_AbstractStatusTrailResult> abstractStatusTrail = Common.GetReportData_AbstractStatusTrail(connString, abstractIDs);
                    //List<rpt_Cdr_ODP_IndividualCodingResult> cdr_ODP_IndividualCoding = Common.GetReportData_Cdr_ODP_IndividualCoding(connString, abstractIDs);
                    List<rpt_Team_User_UCResult> team_User_UCResult = Common.GetReportData_Team_User_UCResult(connString, abstractIDs);

                    //Test
                    //List<AbstractGroup> listOfAbstractGroups = new List<AbstractGroup>();
                    //listOfAbstractGroups.Add(new AbstractGroup(1, 1));
                    //listOfAbstractGroups.Add(new AbstractGroup(2, 2));
                    //listOfAbstractGroups.Add(new AbstractGroup(3, 3));

                    //CreateExcelFile.CreateExcelDocument(listOfAbstractGroups, "AbstractGroups.xlsx", context.Response);

                    CreateExcelFile.CreateExcelDocument(team_User_UCResult, "AbstractGroups.xlsx", context.Response);
                }               

                
            }
            catch (Exception ex)
            {
                Utils.LogError(ex);                
            }
            
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