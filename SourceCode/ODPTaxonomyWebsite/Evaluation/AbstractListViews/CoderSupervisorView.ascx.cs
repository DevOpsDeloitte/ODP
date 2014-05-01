using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ODPTaxonomyDAL_JY;
using ODPTaxonomyUtility_TT;
using System.Configuration;

namespace ODPTaxonomyWebsite.Evaluation.AbstractListViews
{
    public partial class CoderSupervisorView : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string connString = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString;
                DataJYDataContext db = new DataJYDataContext(connString);

                /**
                 * Grabs abstract and other related data
                 */
                var abstracts = from a in db.Abstracts
                                /* get status */
                                join h in db.AbstractStatusChangeHistories on a.AbstractID equals h.AbstractID
                                join s in db.AbstractStatus on h.AbstractStatusID equals s.AbstractStatusID
                                where (
                                    /* 1N Consensus complete status*/
                                h.AbstractStatusID == (int)AbstractStatusType.CONSENSUS_COMPLETE_1N &&
                                    /* Make sure the history is the latest one */
                                h.CreatedDate == db.AbstractStatusChangeHistories
                                    .Where(h2 => h2.AbstractID == a.AbstractID)
                                    .Select(h2 => h2.CreatedDate).Max()
                                    )
                                orderby h.CreatedDate descending
                                orderby a.ProjectTitle ascending
                                select new AbstractListView_CoderSupervisorModel
                                {
                                    AbstractID = a.AbstractID,
                                    ProjectTitle = a.ProjectTitle,
                                    ApplicationID = a.ApplicationID,
                                    AbstractStatusCode = s.AbstractStatusCode,
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