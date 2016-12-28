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

namespace ODPTaxonomyWebsite
{
    public partial class Category : System.Web.UI.Page
    {
       SqlConnection constr = new SqlConnection(ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString);
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

                /*
                using (SqlConnection con = new SqlConnection(constr))
                {
                    
                   using (SqlCommand cmd = new SqlCommand("SELECT CategoryID, concat(FY,' ',Category) name FROM Category where statusID ='1'"))
                   {
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = con;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            sda.Fill(ds);
                            ddlCategory.DataSource = ds.Tables[0];
                            ddlCategory.DataTextField = "Name";
                            ddlCategory.DataValueField = "CategoryID";
                            ddlCategory.DataBind();
                        }}*/
                List<Select_CategoryResult> cateList = null;
                using (ReportDataLinqDataContext db = new ReportDataLinqDataContext(ReportDAL.connString))
               
                    {
                        cateList = db.Select_Category().ToList<Select_CategoryResult>();

                        ddlCategory.DataSource = cateList;
                        ddlCategory.DataTextField = "Category"; 
                        ddlCategory.DataValueField = "CategoryID";

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
            if (ddlCategory.SelectedIndex.ToString() == "0"){
                resetPage();
            }else
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

        protected void btn_submit_Click(object sender, EventArgs e)
        {
             string message = "Category Change Confirmation\\n"+
                              "Please confirm you would like to change categories to\\n"+
                              "currently Coding:" + ddlCategory.SelectedItem.ToString();
             this.ClientScript.RegisterStartupScript(typeof(Page), "Popup", "ConfirmCategory('" + message.Replace("'", "\\'") + "');", true);
            //lbl_message.Text = "Update is Complete";
           // lbl_message.Visible = true;
        }



        #region Methods
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
                    lbl_message.Text = "Update is Complete";
                    lbl_message.Visible = true;
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
