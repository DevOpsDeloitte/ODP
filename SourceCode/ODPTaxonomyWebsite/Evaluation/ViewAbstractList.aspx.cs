using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using ODPTaxonomyDAL_TT;

namespace ODPTaxonomyWebsite.Evaluation
{
    public partial class ViewAbstractList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List<string> roles = Roles.GetRolesForUser().ToList();
            roles.Remove("Coder");

            if (!IsPostBack)
            {
                LoadViewDropDownData(roles);
            }
        }

        /**
         * Loads data for select a view dropdown
         */
        private void LoadViewDropDownData(List<string> roles)
        {
            ViewDDL.Items.Clear();
            ViewDDL.Items.Add(new ListItem("Select a view", ""));
            foreach (string role in roles)
            {
                ViewDDL.Items.Add(new ListItem(role + " view", role.ToLower()));
            }
        }

        protected void ViewDDLIndexChangedHandle(object sender, EventArgs e)
        {
        }
    }
}