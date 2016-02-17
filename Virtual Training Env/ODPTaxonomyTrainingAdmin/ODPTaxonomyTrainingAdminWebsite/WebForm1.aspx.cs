using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace ODPTaxonomyTrainingAdminWebsite
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
          
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Data Not Exists!", "Message");
          // Response.Redirect("~/AnswerkeyUpdate.aspx", true);
           // string queryString = "AbstratData.aspx";
         //   string newWin = "window.open('" + queryString + "');";
         //   ClientScript.RegisterStartupScript(this.GetType(), "pop", newWin, true);
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Data Not Exists!", "Message");
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            int num = 0;
            DialogResult dialogResult = MessageBox.Show
               ("Application ID =" + num.ToString() + "\n" + 
                "Fiscal Year =" + num.ToString() + "\n"+
                "Project Number =" + num.ToString() + "\n"+
                "PI Project Leader =" + num.ToString() + "\n"+
                "Project Title =" + num.ToString() + "\n"+
                "Are you sure do you want to update?", "Data Exist!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
          //  MessageBox.Show(num.ToString());
        }



    }
}