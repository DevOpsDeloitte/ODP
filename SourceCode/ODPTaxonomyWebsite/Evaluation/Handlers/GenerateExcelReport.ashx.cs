using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ODPTaxonomyUtility_TT;
using ODPTaxonomyDAL_TT;

namespace ODPTaxonomyWebsite.Evaluation.Handlers
{
    /// <summary>
    /// Summary description for GenerateExcelReport
    /// </summary>
    public class GenerateExcelReport : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                //Test
                List<AbstractGroup> listOfAbstractGroups = new List<AbstractGroup>();
                listOfAbstractGroups.Add(new AbstractGroup(1, 1));
                listOfAbstractGroups.Add(new AbstractGroup(2, 2));
                listOfAbstractGroups.Add(new AbstractGroup(3, 3));

                CreateExcelFile.CreateExcelDocument(listOfAbstractGroups, "AbstractGroups.xlsx", context.Response);

            }
            catch (Exception ex)
            {
                Utils.LogError(ex);
            }
            //context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
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