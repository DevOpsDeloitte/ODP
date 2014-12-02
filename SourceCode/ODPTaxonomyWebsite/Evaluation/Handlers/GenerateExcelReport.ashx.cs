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
                abstractIDs = abstracts.Split(',').ToList();

                List<rpt_OPAResult> opaData = Common.GetReportData_OpaData(connString, abstractIDs);

                //Test
                //List<AbstractGroup> listOfAbstractGroups = new List<AbstractGroup>();
                //listOfAbstractGroups.Add(new AbstractGroup(1, 1));
                //listOfAbstractGroups.Add(new AbstractGroup(2, 2));
                //listOfAbstractGroups.Add(new AbstractGroup(3, 3));

                //CreateExcelFile.CreateExcelDocument(listOfAbstractGroups, "AbstractGroups.xlsx", context.Response);

                CreateExcelFile.CreateExcelDocument(opaData, "AbstractGroups.xlsx", context.Response);

                
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