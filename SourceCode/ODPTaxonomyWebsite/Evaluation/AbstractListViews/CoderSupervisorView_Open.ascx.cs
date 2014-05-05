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
    public partial class CoderSupervisorView_Open : System.Web.UI.UserControl
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
                var abstracts = (from a in db.Abstracts
                                 /* get status */
                                 join h in db.AbstractStatusChangeHistories on a.AbstractID equals h.AbstractID
                                 join s in db.AbstractStatus on h.AbstractStatusID equals s.AbstractStatusID
                                 where (
                                     // 1N Consensus complete status with notes uploaded
                                    h.AbstractStatusID == (int)AbstractStatusEnum.OPEN &&
                                     // Make sure the history is the latest one
                                    h.CreatedDate == db.AbstractStatusChangeHistories
                                     .Where(h2 => h2.AbstractID == a.AbstractID && h2.AbstractStatusID == (int)AbstractStatusEnum.OPEN)
                                     .Select(h2 => h2.CreatedDate).Max()
                                     )
                                 orderby h.CreatedDate descending
                                 orderby a.ProjectTitle ascending
                                 select new AbstractListView_CoderSupervisorModel
                                 {
                                     AbstractID = a.AbstractID,
                                     ProjectTitle = a.ProjectTitle,
                                     ApplicationID = a.ApplicationID,
                                     AbstractStatusID = s.AbstractStatusID,
                                     AbstractStatusCode = s.AbstractStatusCode,
                                     StatusDate = h.CreatedDate
                                 }).ToList();

                AbstractViewGridView.DataSource = abstracts;
                AbstractViewGridView.DataBind();
            }
            catch (Exception exp)
            {
                Utils.LogError(exp);
            }
        }

        protected void AbstractListRowBindingHandle(object sender, GridViewRowEventArgs e)
        {

        }
    }
}