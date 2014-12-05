#define ADD_STRESS_TEST
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ODPTaxonomyUtility_TT;
using ODPTaxonomyDAL_TT;
using System.Configuration;
using Newtonsoft.Json;
using System.Data;

namespace ODPTaxonomyWebsite.Evaluation.Handlers
{
    /// <summary>
    /// Summary description for GenerateExcelReport
    /// </summary>
    public class GenerateExcelReport : IHttpHandler
    {
        #region Fields
        private string filenameBase = "PACT-Abstract-Export-";
        private string filename = "PACT-Abstract-Export-YYYY-MM-DD.xlsx";
        private string connString = null;
        private string abstracts = "";
        private List<string> abstractIDs;
        private int abstractId = -1;
        
                    
        #endregion

        public void ProcessRequest(HttpContext context)
        {
            DataSet ds = new DataSet();
            //Do Not return any text from this method
            //Otherwise an error message appears on opening saved Excel file
            try
            {
                
                string format = "yyyy-MM-dd-h:mm:ss";
                connString = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ToString();
                abstracts = context.Request["abstracts"] ?? "";
                if (String.IsNullOrEmpty(abstracts))
                {
                    context.Response.Write("Important parameter is missing");
                }
                else
                {
#if ADD_STRESS_TEST
                    //Stress TEST
                    for (var i = 0; i <10; i++)
                    {
                        abstracts += ",116,120,149,162,192,197,202,215,220,292,301,328,381,391,397,406,466,479,480,496,526,555,586,599,603,612,621,635,676,695,700,707";

                    }
#endif
                    //check abstractIDs
                    abstractIDs = abstracts.Split(',').ToList();
                    foreach (string abs in abstractIDs)
                    {
                        if (!Int32.TryParse(abs, out abstractId))
                        {
                            context.Response.Write("abstractID '" + abs + "' is incorrect");
                            break;
                        }
                    }


                    filename = filenameBase + DateTime.Now.ToString(format) + ".xlsx";

                    List<rpt_OPAResult> opaData = Common.GetReportData_OpaData(connString, abstracts);
                    List<rpt_KappaDataResult> kappaData = Common.GetReportData_KappaData(connString, abstracts);
                    List<rpt_Cdr_ODPNotesPDFResult> cdr_ODPNotesPDF = Common.GetReportData_Cdr_ODPNotesPDF(connString, abstracts);
                    List<rpt_AbstractStatusTrailResult> abstractStatusTrail = Common.GetReportData_AbstractStatusTrail(connString, abstracts);
                    List<rpt_Cdr_ODP_IndividualCodingResult> cdr_ODP_IndividualCoding = Common.GetReportData_Cdr_ODP_IndividualCoding(connString, abstracts);
                    List<rpt_Team_User_UCResult> team_User_UCResult = Common.GetReportData_Team_User_UCResult(connString, abstracts);

                    //Test
                    //List<AbstractGroup> listOfAbstractGroups = new List<AbstractGroup>();
                    //listOfAbstractGroups.Add(new AbstractGroup(1, 1));
                    //listOfAbstractGroups.Add(new AbstractGroup(2, 2));
                    //listOfAbstractGroups.Add(new AbstractGroup(3, 3));

                    //CreateExcelFile.CreateExcelDocument(listOfAbstractGroups, "AbstractGroups.xlsx", context.Response);

                    CreateExcelFile.CreateExcelDocument<rpt_OPAResult>(opaData, context.Response, "OPA Data", ds);
                    CreateExcelFile.CreateExcelDocument<rpt_KappaDataResult>(kappaData, context.Response, "Kappa Data", ds);
                    CreateExcelFile.CreateExcelDocument<rpt_Cdr_ODPNotesPDFResult>(cdr_ODPNotesPDF, context.Response, "Cdr_ODPNotesPDF", ds);
                    CreateExcelFile.CreateExcelDocument<rpt_AbstractStatusTrailResult>(abstractStatusTrail, context.Response, "AbstractStatusTrail", ds);
                    CreateExcelFile.CreateExcelDocument<rpt_Cdr_ODP_IndividualCodingResult>(cdr_ODP_IndividualCoding, context.Response, "Cdr&ODP IndividualCoding", ds);
                    CreateExcelFile.CreateExcelDocument<rpt_Team_User_UCResult>(team_User_UCResult, context.Response, "Team_User_UC", ds);
                    
                    CreateExcelFile.CreateExcelDocumentAsStream(ds, filename, context.Response);
                }


            }
            catch (Exception ex)
            {
                Utils.LogError(ex);
            }
            finally
            {
                ds.Dispose();
                ds = null;
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