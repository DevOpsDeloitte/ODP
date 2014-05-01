using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ODPTaxonomyDAL_JY;

namespace ODPTaxonomyWebsite.Evaluation.AbstractViews
{
    public partial class CoderSupervisorView : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DataAbstractContext db = new DataAbstractContext();

            var abstracts = from a in db.Abstracts
                            select a;
        }
    }
}