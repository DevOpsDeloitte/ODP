using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using ODPTaxonomyDAL_TT;
using ODPTaxonomyUtility_TT;
using ODPTaxonomyReportDAL;

namespace ODPTaxonomyWebsite.SwitchCategories
{
    public partial class Category : System.Web.UI.Page
    {
        SqlConnection constr = new SqlConnection(ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString);
        public string CurrentlyCoding = String.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (!this.IsPostBack)
                {

                    lbl_category.Text = "";
                    lbl_message.Text = "";
                    lbl_message.Visible = false;
                    btn_submit.Enabled = false;
                    btn_reset.Enabled = false;

                    List<Select_CategoryResult> cateList = null;
                    using (ReportDataLinqDataContext db = new ReportDataLinqDataContext(ReportDAL.connString))
                    {
                        cateList = db.Select_Category().ToList<Select_CategoryResult>();

                        var selectedValue = cateList.Where(cl => cl.Status == "Active").Select(cl => new { id = cl.CategoryID, text = cl.Category }).FirstOrDefault();

                        ddlCategory.DataSource = cateList;
                        ddlCategory.DataTextField = "Category";
                        ddlCategory.DataValueField = "CategoryID";
                        ddlCategory.SelectedValue = selectedValue.id.ToString();
                        CurrentlyCoding = selectedValue.text;
                        lbl_active_category.Text = selectedValue.text;


                        ddlCategory.DataBind();

                    }

                    ddlCategory.Items.Insert(0, new ListItem("--Select Category--", "0"));
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex);
                throw new Exception("An error has occured while loading data.");
            }
        }

        protected void ddlCategory_Change(object sender, EventArgs e)
        {
            if (ddlCategory.SelectedIndex.ToString() == "0")
            {
                resetPage();
            }
            else
            {
                btn_submit.Enabled = true;
                btn_reset.Enabled = true;
                btn_submit.Attributes.Add("class", "button yes");
                btn_reset.Attributes.Add("class", "button no");
                lbl_category.Text = ddlCategory.SelectedItem.ToString();
            }

        }

        protected void btn_reset_Click(object sender, EventArgs e)
        {
            resetPage();
        }


        #region Page Methods
        protected void resetPage()
        {
            ddlCategory.SelectedIndex = 0;
            btn_submit.Enabled = false;
            btn_reset.Enabled = false;
            lbl_message.Text = "";
            lbl_message.Visible = false;
            lbl_category.Text = "";
        }

        protected void switchCategoryMethod(object sender, EventArgs e)//hidding method javascript call (switch Category)
        {
            constr.Open();
            SqlCommand Com = new SqlCommand("[Update_CategoryStatus]", constr);
            Com.CommandType = CommandType.StoredProcedure;
            Com.Parameters.Add("@CategoryID", SqlDbType.VarChar).Value = ddlCategory.SelectedValue.ToString().Trim();
            Com.Parameters.Add("@intReturn", SqlDbType.Int).Direction = ParameterDirection.Output;

            try
            {
                if (Com.Connection.State == ConnectionState.Closed)
                {
                    Com.Connection.Open();
                }

                Com.ExecuteNonQuery();
                int returnVALUE = (int)Com.Parameters["@intReturn"].Value;

                if (returnVALUE == 1)
                {
                    lbl_message.Text = "Successfully updated!";
                    lbl_message.Visible = true;
                    lbl_active_category.Text = ddlCategory.SelectedItem.Text;
                    lbl_category.Text = "";
                }
                else
                {
                    lbl_message.Text = "Error";
                    lbl_message.Visible = true;
                }

            }
            catch (Exception ex)
            {
                Response.Write("Error:" + ex.ToString());
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Com.Connection.Close();
            }

        }
        #endregion
    }
}
