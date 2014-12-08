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
using System.Text;

namespace ODPTaxonomyWebsite.Evaluation.Handlers
{
    public static class EnumerableEx
    {
        public static IEnumerable<string> SplitBy(this string str, int chunkLength)
        {
            if (String.IsNullOrEmpty(str)) throw new ArgumentException();
            if (chunkLength < 1) throw new ArgumentException();

            for (int i = 0; i < str.Length; i += chunkLength)
            {
                if (chunkLength + i > str.Length)
                    chunkLength = str.Length - i;

                yield return str.Substring(i, chunkLength);
            }
        }
    }
    /// <summary>
    /// Summary description for GenerateExcelReport
    /// </summary>
    public class GenerateExcelReport : IHttpHandler
    {
        #region Fields
        private int paramMaxLegth = 50;
        private string filenameBase = "PACT-Abstract-Export-";
        private string filename = "PACT-Abstract-Export-YYYY-MM-DD.xlsx";
        private string connString = null;
        private string abstracts = "";
        private List<string> abstractIDs;
        private List<string> listAbstracts;
        private int abstractId = -1;
        private string excelFileName = "";
        private string filePath = "";
                    
        #endregion
                

        public void ProcessRequest(HttpContext context)
        {
            int paramLength = 0;
            listAbstracts = new List<string>();
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
                    for (var i = 0; i < 1; i++)
                    {
                        sb1.Append(",116,120,149,162,192,197,202,215,220,292,301,328,381,391,397,406,466,479,480,496,526,555,586,599,603,612,621,635,676,695,700,707");
                      
                    }
                    abstracts += sb1.ToString();
#endif
                    //check abstractIDs
                    paramLength = abstracts.Length;
                    abstractIDs = abstracts.Split(',').ToList();


                    if (paramLength >= paramMaxLegth)
                    {
                        StringBuilder sb = new StringBuilder();
                        
                        foreach (string abs in abstractIDs)
                        {
                            if (!Int32.TryParse(abs, out abstractId))
                            {
                                context.Response.Write("abstractID '" + abs + "' is incorrect");
                                break;
                            }
                            else
                            {
                                listAbstracts = abstracts.SplitBy(paramMaxLegth).ToList(); 
                            }
                        }   
                    }
                    else
                    {
                        foreach (string abs in abstractIDs)
                        {
                            if (!Int32.TryParse(abs, out abstractId))
                            {
                                context.Response.Write("abstractID '" + abs + "' is incorrect");
                                break;
                            }
                        }
                        listAbstracts.Add(abstracts);
                    }
                                    
                                                          

                    filename = filenameBase + DateTime.Now.ToString(format) + ".xlsx";
                    excelFileName = context.Request.PhysicalApplicationPath + "Reports\\" + filename;
                    filePath = "/Reports/" + filename;

                    List<rpt_OPAResult> opaData = new List<rpt_OPAResult>();
                    List<rpt_KappaDataResult> kappaData = new List<rpt_KappaDataResult>();
                    List<rpt_Cdr_ODPNotesPDFResult> cdr_ODPNotesPDF = new List<rpt_Cdr_ODPNotesPDFResult>();
                    List<rpt_AbstractStatusTrailResult> abstractStatusTrail = new List<rpt_AbstractStatusTrailResult>();
                    List<rpt_Cdr_ODP_IndividualCodingResult> cdr_ODP_IndividualCoding = new List<rpt_Cdr_ODP_IndividualCodingResult>();
                    List<rpt_Team_User_UCResult> team_User_UCResult = new List<rpt_Team_User_UCResult>();

                    foreach (string s in listAbstracts)
                    {
                        opaData.Union(Common.GetReportData_OpaData(connString, s)).ToList();
                        kappaData.Union(Common.GetReportData_KappaData(connString, s)).ToList();
                        cdr_ODPNotesPDF.Union(Common.GetReportData_Cdr_ODPNotesPDF(connString, s)).ToList();
                        abstractStatusTrail.Union(Common.GetReportData_AbstractStatusTrail(connString, s));
                        cdr_ODP_IndividualCoding.Union(Common.GetReportData_Cdr_ODP_IndividualCoding(connString, s));
                        team_User_UCResult.Union(Common.GetReportData_Team_User_UCResult(connString, s));
                    }

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