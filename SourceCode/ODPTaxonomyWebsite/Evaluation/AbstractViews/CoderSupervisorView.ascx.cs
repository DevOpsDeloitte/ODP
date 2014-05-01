using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ODPTaxonomyDAL_JY;
using ODPTaxonomyUtility_TT;
using System.Configuration;

namespace ODPTaxonomyWebsite.Evaluation.AbstractViews
{
    public partial class CoderSupervisorView : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string connString = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString;
                DataJYDataContext db = new DataJYDataContext(connString);

                var abstracts = from a in db.Abstracts
                                join h in db.AbstractStatusChangeHistories on a.AbstractID equals h.AbstractID
                                where (h.AbstractStatusID == (int)AbstractStatus.CONSENSUS_COMPLETE_1N &&
                                h.CreatedDate == db.AbstractStatusChangeHistories
                                .Where(h2 => h2.AbstractID == a.AbstractID)
                                .Select(h2 => h2.CreatedDate).Max())
                                orderby h.CreatedDate descending
                                orderby a.ProjectTitle ascending
                                select new AbstractListView_CoderSupervisorModel
                                {
                                    AbstractID = a.AbstractID,
                                    ProjectTitle = a.ProjectTitle,
                                    ApplicationID = a.ApplicationID,
                                    StatusDate = h.CreatedDate
                                };

                AbstractView.DataSource = abstracts;
                AbstractView.DataBind();
            }
            catch (Exception exp)
            {
                Utils.LogError(exp);
            }
        }
    }
}