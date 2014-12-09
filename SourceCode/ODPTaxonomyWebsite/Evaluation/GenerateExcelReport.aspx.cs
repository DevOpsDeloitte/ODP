using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ODPTaxonomyUtility_TT;
using ODPTaxonomyDAL_TT;

namespace ODPTaxonomyWebsite.Evaluation
{
    public partial class GenerateExcelReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //Test
                List<AbstractGroup> listOfAbstractGroups = new List<AbstractGroup>();
                listOfAbstractGroups.Add(new AbstractGroup(1, 1));
                listOfAbstractGroups.Add(new AbstractGroup(2, 2));
                listOfAbstractGroups.Add(new AbstractGroup(3, 3));

                CreateExcelFile.CreateExcelDocument(listOfAbstractGroups, "AbstractGroups.xlsx", Response);

            }
            catch (Exception ex)
            {
                Utils.LogError(ex);
            }

        }

        protected void btn_export_Click(object sender, EventArgs e)
        {
            try
            {
                //Test
                List<AbstractGroup> listOfAbstractGroups = new List<AbstractGroup>();
                listOfAbstractGroups.Add(new AbstractGroup(1, 1));
                listOfAbstractGroups.Add(new AbstractGroup(2, 2));
                listOfAbstractGroups.Add(new AbstractGroup(3, 3));

                CreateExcelFile.CreateExcelDocument(listOfAbstractGroups, "AbstractGroups.xlsx", Response);

            }
            catch (Exception ex)
            {
                Utils.LogError(ex);

            }
        }
    }
}