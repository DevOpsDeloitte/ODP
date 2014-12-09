﻿#define ADD_STRESS_TEST
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ODPTaxonomyUtility_TT;
using ODPTaxonomyDAL_TT;
using System.Configuration;
using Newtonsoft.Json;
using System.Data;
using System.Text;

namespace ODPTaxonomyWebsite.Evaluation.Handlers
{
   
    public class GenerateExcelReport : IHttpHandler
    {
        #region Fields
        private string filenameBase = "PACT-Abstract-Export-";
        private string filename = "PACT-Abstract-Export-YYYY-MM-DD.xlsx";
        private string connString = null;
        private string abstracts = "";
        private List<string> abstractIDs;
        private int abstractId = -1;
        private string excelFileName = "";
        private string filePath = "";
                    
        #endregion
                

        public void ProcessRequest(HttpContext context)
        {
            DataSet ds = new DataSet();
            //Do Not return any text from this method
            //Otherwise an error message appears on opening saved Excel file
            try
            {
                
                string format = "yyyy-MM-dd-h-mm-ss";
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
                    StringBuilder sb1 = new StringBuilder();
                    for (var i = 0; i < 300; i++)
                    {
                        sb1.Append(",116,120,149,162,192,197,202,215,220,292,301,328,381,391,397,406,466,479,480,496,526,555,586,599,603,612,621,635,676,695,700,707");
                      
                    }
                    abstracts += sb1.ToString();
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
                    excelFileName = context.Request.PhysicalApplicationPath + "Reports\\" + filename;
                    filePath = "/Reports/" + filename;

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
                    opaData.Clear();
                    CreateExcelFile.CreateExcelDocument<rpt_KappaDataResult>(kappaData, context.Response, "Kappa Data", ds);
                    kappaData.Clear();
                    CreateExcelFile.CreateExcelDocument<rpt_Cdr_ODPNotesPDFResult>(cdr_ODPNotesPDF, context.Response, "Cdr_ODPNotesPDF", ds);
                    cdr_ODPNotesPDF.Clear();
                    CreateExcelFile.CreateExcelDocument<rpt_AbstractStatusTrailResult>(abstractStatusTrail, context.Response, "AbstractStatusTrail", ds);
                    abstractStatusTrail.Clear();
                    CreateExcelFile.CreateExcelDocument<rpt_Cdr_ODP_IndividualCodingResult>(cdr_ODP_IndividualCoding, context.Response, "Cdr&ODP IndividualCoding", ds);
                    cdr_ODP_IndividualCoding.Clear();
                    CreateExcelFile.CreateExcelDocument<rpt_Team_User_UCResult>(team_User_UCResult, context.Response, "Team_User_UC", ds);
                    team_User_UCResult.Clear();
                    
                    //Write to memory stream
                    //code below throws out of memory exception for more than 1000 abstract ids
                    //CreateExcelFile.CreateExcelDocumentAsStream(ds, filename, context.Response);

                    //Write to file                    
                    CreateExcelFile.CreateExcelDocument(ds, excelFileName);
                    context.Response.Write(JsonConvert.SerializeObject(new { success = true, filePath = filePath }));
                }


            }
            catch (Exception ex)
            {
                Utils.LogError(ex);
                context.Response.Write(JsonConvert.SerializeObject(new { success = false, message = ex.Message }));
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