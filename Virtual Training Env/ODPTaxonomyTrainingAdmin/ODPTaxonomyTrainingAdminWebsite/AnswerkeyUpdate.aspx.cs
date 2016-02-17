using ODPTaxonomyUtility_TT;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace ODPTaxonomyTrainingAdminWebsite
{
    public partial class AnswerkeyUpdate : System.Web.UI.Page
    {
        SqlConnection prodConn = new SqlConnection(ConfigurationManager.ConnectionStrings["ODPTaxonomyProd"].ConnectionString);
        SqlConnection trainConn = new SqlConnection(ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString);
        private int temp;
        private int getAppID;
        private string getFY;
        private string getPN;
        private string getPI;
        private string getPT;

        protected void Page_Load(object sender, EventArgs e)
        {
            txt_app_id1.Enabled = false;
            txt_frm_date.Enabled = false;
            txt_to_date.Enabled = false;
        }

        protected void btn_update_Click(object sender, EventArgs e)
        {
            resetRerunKappa();
            resetValueReport();

                   //Check if data exist
            
                      string checkExist = "select count(ApplicationID) from abstract where ApplicationID = '" + Convert.ToInt32(txt_app_id.Text) + "'";
                      string abstarctData = "select ApplicationID,FY,ProjectNumber,PIProjectLeader,ProjectTitle from abstract where ApplicationID = '" + Convert.ToInt32(txt_app_id.Text) + "'";

                      if (rdoValUpEnv.SelectedItem.Value.ToString() == "1")  ///Production
                      {
                          prodConn.Open();
                          SqlCommand com = new SqlCommand(checkExist, prodConn); //check if data exist in prodution
                          temp = Convert.ToInt32(com.ExecuteScalar().ToString());
                          SqlCommand abs = new SqlCommand(abstarctData, prodConn);//get value from abstract
                          SqlDataReader dr = abs.ExecuteReader();
                          while (dr.Read())
                           {
                             getAppID = dr.GetInt32(0);
                             getFY = dr.GetString(1);
                             getPN = dr.GetString(2);
                             getPI = dr.GetString(3);
                             getPT = dr.GetString(4);
                            }
                          prodConn.Close();
                      }
                      else if (rdoValUpEnv.SelectedItem.Value.ToString() == "2") //Training
                      {
                          trainConn.Open();
                          SqlCommand com = new SqlCommand(checkExist, trainConn);//check if data exist in training
                          temp = Convert.ToInt32(com.ExecuteScalar().ToString());
                          SqlCommand abs = new SqlCommand(abstarctData, trainConn);//get value from abstract
                          SqlDataReader dr = abs.ExecuteReader();
                          while (dr.Read())
                           {
                             getAppID = dr.GetInt32(0);
                             getFY = dr.GetString(1);
                             getPN = dr.GetString(2);
                             getPI = dr.GetString(3);
                             getPT = dr.GetString(4);
                            }
                          trainConn.Close();
                      }

                      if (temp >= 1)
                      {
                         // DialogResult dialogResult = MessageBox.Show("Data Exist! Are you sure do you want to update", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                           DialogResult dialogResult = MessageBox.Show
                               ("Data Exist as following!" + "\n\n" + 
                                "1. Application ID =" + getAppID + "\n" +
                                "2. Fiscal Year =" + getFY + "\n" +
                                "3. Project Number =" + getPN + "\n" +
                                "4. PI Project Leader =" + getPI + "\n" +
                                "5. Project Title =" + getPT + "\n\n" +
                                "Are you sure do you want to update?", "Message",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                          if (dialogResult == DialogResult.Yes)
                          {
                              if (rdoValUpEnv.SelectedItem.Value.ToString() == "1")  ///Production
                                prodUpdateRC();
                              else if (rdoValUpEnv.SelectedItem.Value.ToString() == "2") //Training
                                trainingUpdateRC();
                          }
                          else if (dialogResult == DialogResult.No)
                          {
                              Response.Redirect("~/AnswerkeyUpdate.aspx", true);
                          }
                      }
                      else
                      {
                          MessageBox.Show("Data Not Exists!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                          Response.Redirect("~/AnswerkeyUpdate.aspx", true);
                      }
        }
        
        
        protected void btn_chk_val_Click(object sender, EventArgs e)
        {
            resetValueUpdates();
            resetRerunKappa();
            Session.Clear();
             if (rdoValRepEnv.SelectedItem.Value.ToString() == "1")  //Production
             {
                 Session["ssRdoValRepEnv"] = rdoValRepEnv.SelectedItem.Value.ToString().Trim();
                 Session["ssConsensus"] = ddl_consensus1.SelectedValue.ToString().Trim();
             }
             else if (rdoValRepEnv.SelectedItem.Value.ToString() == "2") //Training
             {
                 Session["ssRdoValRepEnv"] = rdoValRepEnv.SelectedItem.Value.ToString().Trim();
                 Session["ssConsensus"] = null;
             }

                 if (ddlReport.SelectedItem.Value == "1") //ApplicationID
                 {
                     if (txt_app_id1.Text != string.Empty)
                     {
                         try
                         {
                             Session["ssFrmDate"] = null;
                             Session["ssToDate"] = null;
                             Session["ssApplicationID"] = txt_app_id1.Text;
                             Response.Redirect("~/ReportAppID.aspx", true);
                         }
                         catch (Exception ex)
                         {
                             // Utils.LogError(ex);
                             // throw new Exception("An error has occured on ReportAppID data.");
                             Response.Write("Error:" + ex.ToString());
                         }
                     }
                     else
                     {
                         MessageBox.Show("Please fill Application ID!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                         txt_app_id1.Enabled = true;
                         txt_app_id1.Focus();
                     }
                 }
                 else if (ddlReport.SelectedItem.Value == "2") //Data Range
                 {
                     if (txt_frm_date.Text != string.Empty & txt_to_date.Text != string.Empty)
                     {
                        
                       DateTime dtFrmDate = DateTime.Parse(txt_frm_date.Text);
                       DateTime dtToDate = DateTime.Parse(txt_to_date.Text);
                       if (dtFrmDate <= dtToDate)
                       {
                           try
                           {
                               Session["ssApplicationID"] = null;
                               DateTime dt1 = DateTime.ParseExact(txt_frm_date.Text, "mm/dd/yyyy", CultureInfo.InvariantCulture);
                               DateTime dt2 = DateTime.ParseExact(txt_to_date.Text, "mm/dd/yyyy", CultureInfo.InvariantCulture);
                               string frmDateSession = dt1.ToString("mm/dd/yyyy");
                               string toDateSession = dt2.ToString("mm/dd/yyyy");
                               Session["ssFrmDate"] = frmDateSession;
                               Session["ssToDate"] = toDateSession;
                               Response.Redirect("~/ReportAppID.aspx", true);

                           }
                           catch (Exception ex)
                           {
                               // Utils.LogError(ex);
                               // throw new Exception("An error has occured on ReportAppID data.");
                               Response.Write("Error:" + ex.ToString());
                           }
                       }
                       else
                       {
                           MessageBox.Show("To date must be after From date!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                           txt_frm_date.Enabled = true;
                           txt_to_date.Enabled = true;
                           txt_frm_date.Focus();
                       }
                     }
                     else
                     {
                       if (txt_frm_date.Text == string.Empty)
                         {
                             MessageBox.Show("Please fill From Date!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                             txt_frm_date.Enabled = true;
                             txt_to_date.Enabled = true;
                             txt_frm_date.Focus();
                         }
                         else if (txt_to_date.Text == string.Empty)
                         {
                             MessageBox.Show("Please fill To Date!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                             txt_to_date.Enabled = true;
                             txt_frm_date.Enabled = true;
                             txt_to_date.Focus();
                         }
                     }
                 }
        }

        protected void btn_rerun_kappa_Click(object sender, EventArgs e)
        {
            resetValueUpdates();
            resetValueReport();
            if (txt_app_id2.Text != String.Empty)
            {
                prodConn.Open();
                //Check if exist
                string checkExist = "select isnull(count(*),0) from report_answerkey_update_log where ApplicationID = '" + Convert.ToInt32(txt_app_id2.Text) + "' ";
                SqlCommand com = new SqlCommand(checkExist, prodConn);

                int temp = Convert.ToInt32(com.ExecuteScalar().ToString());

                prodConn.Close();
                if (temp >= 1)
                {
                    DialogResult dialogResult = MessageBox.Show("Data Exist! Are you sure do you want to update", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                    if (dialogResult == DialogResult.Yes)
                    {
                        rerunKapp();
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        Response.Redirect("~/AnswerkeyUpdate.aspx", true);
                    }
                }
                else
                {
                    MessageBox.Show("Data Not Exists!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    Response.Redirect("~/AnswerkeyUpdate.aspx", true);
                }
            }
            else
            {
                MessageBox.Show("Please fill Application ID!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                txt_app_id2.Focus();
            }

        }
        
        protected void rdoValUpEnv_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdoValUpEnv.SelectedItem.Value.ToString() == "1")
            {
                lbl_consensus.Visible = true;
                ddl_consensus.Visible = true;
                ConsensusRequired.Visible = true;
            }
            else if (rdoValUpEnv.SelectedItem.Value.ToString() == "2")
            {
                lbl_consensus.Visible = false;
                ddl_consensus.Visible = false;
                ConsensusRequired.Visible = false;
                ddl_consensus.SelectedIndex = 0;
            }

        }

        protected void rdoValRepEnv_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdoValRepEnv.SelectedItem.Value.ToString() == "1")
            {
                lbl_consensus1.Visible = true;
                ddl_consensus1.Visible = true;
                ConsensusRequired1.Visible = true;
            }
            else if (rdoValRepEnv.SelectedItem.Value.ToString() == "2")
            {
                lbl_consensus1.Visible = false;
                ddl_consensus1.Visible = false;
                ConsensusRequired1.Visible = false;
                ddl_consensus1.SelectedIndex = 0;
            }
        }

        protected void ddlReport_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = ddlReport.SelectedItem.Value;
            // ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + selected + "');", true);
            if (selected == "1")
            {
                txt_app_id1.Enabled = true;
                txt_app_id1.Focus();
                txt_frm_date.Text = string.Empty;
                txt_to_date.Text = string.Empty;
            }
            else if (selected == "2")
            {
                txt_frm_date.Enabled = true;
                txt_frm_date.Focus();
                txt_to_date.Enabled = true;
                txt_app_id1.Text = string.Empty;
            }
            else
            {
                txt_app_id1.Text = string.Empty;
                txt_frm_date.Text = string.Empty;
                txt_to_date.Text = string.Empty;
            }
        }

        #region Methods
           
        protected void prodUpdateRC()
        {
               prodConn.Open();
               SqlCommand Com = new SqlCommand("Update_RCCC_ABCDEF_ByAppID", prodConn);
               Com.CommandType = CommandType.StoredProcedure;
                Com.Parameters.Add("@ApplicationID", SqlDbType.Int).Value = Convert.ToInt32(txt_app_id.Text);
                Com.Parameters.Add("@StrRCCC", SqlDbType.VarChar, 10).Value = ddl_consensus.SelectedValue.ToString().Trim(); 
                Com.Parameters.Add("@StrSectionName", SqlDbType.VarChar, 100).Value = ddl_frm_section.SelectedValue.ToString().Trim();
                Com.Parameters.Add("@strSectionID ", SqlDbType.VarChar, 200).Value = txt_upd_values.Text.ToString().Trim();
               Com.Parameters.Add("@UpdateFileName", SqlDbType.VarChar, 500).Value = txt_file_name.Text.ToString().Trim();

             try
                {
                    if (Com.Connection.State == ConnectionState.Closed)
                    {
                        Com.Connection.Open();
                    }
                    Com.ExecuteNonQuery();
             
                    MessageBox.Show("Update Successful in Production!!", "AnswerkeyUpdate", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                }
                catch (Exception ex)
                {
                    Response.Write("Error:" + ex.ToString());
                }
                finally
                {
                    Com.Connection.Close();
                    Response.Redirect("~/AnswerkeyUpdate.aspx", true);
                }
           
        }

        protected void trainingUpdateRC()
        {
            
            trainConn.Open();
            SqlCommand Com = new SqlCommand("Update_RC_TR_ByAppID", trainConn);
            Com.CommandType = CommandType.StoredProcedure;
            Com.Parameters.Add("@ApplicationID", SqlDbType.Int).Value = Convert.ToInt32(txt_app_id.Text);
            Com.Parameters.Add("@StrColName", SqlDbType.VarChar, 100).Value = ddl_frm_section.SelectedValue.ToString().Trim();
            Com.Parameters.Add("@strSectionID ", SqlDbType.VarChar, 100).Value = txt_upd_values.Text.ToString().Trim();
            Com.Parameters.Add("@UpdateFileName", SqlDbType.VarChar, 500).Value = txt_file_name.Text.ToString().Trim();


            try
            {
                if (Com.Connection.State == ConnectionState.Closed)
                {
                    Com.Connection.Open();
                }
                Com.ExecuteNonQuery();
                MessageBox.Show("Update Successful in Training!!", "AnswerkeyUpdate",MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

            }
            catch (Exception ex)
            {
                Response.Write("Error:" + ex.ToString());
            }
            finally
            {
                Com.Connection.Close();
                Response.Redirect("~/AnswerkeyUpdate.aspx", true);
            }
        }

        protected void rerunKapp() //production only
        {
                prodConn.Open();
                SqlCommand Com = new SqlCommand("[Update_RCCC_ReRunKappa_ByAppID]", prodConn);
                Com.CommandType = CommandType.StoredProcedure;
                Com.Parameters.Add("@RCCC", SqlDbType.VarChar, 10).Value = ddl_consensus2.SelectedValue.ToString().Trim(); 
                Com.Parameters.Add("@ApplicationID", SqlDbType.Int).Value = Convert.ToInt32(txt_app_id2.Text);    


                try
                {
                    if (Com.Connection.State == ConnectionState.Closed)
                    {
                        Com.Connection.Open();
                    }

                    Com.ExecuteNonQuery();

                    MessageBox.Show("Update Successful!!", "AnswerkeyUpdate", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                }
                catch (Exception ex)
                {
                    Response.Write("Error:" + ex.ToString());
                    Console.WriteLine(ex.ToString());
                }
                finally
                {
                    Com.Connection.Close();
                    Response.Redirect("~/AnswerkeyUpdate.aspx", true);
                } 
           
            }

        protected void resetValueUpdates() 
        {
            rdoValUpEnv.SelectedValue = "1";
            txt_file_name.Text = "";
            txt_app_id.Text = "";
            ddl_consensus.SelectedIndex = 0;
            ddl_frm_section.SelectedIndex = 0;
            txt_upd_values.Text = "";

        }
        protected void resetValueReport()
        {
            rdoValRepEnv.SelectedValue = "1";
            ddlReport.SelectedIndex = 0;
            ddl_consensus1.SelectedIndex = 0;
            txt_app_id1.Text = "";
            txt_frm_date.Text = "";
            txt_to_date.Text = "";
        }
        protected void resetRerunKappa()
        {
            txt_app_id2.Text = "";
            ddl_consensus2.SelectedIndex = 0;
        }
        #endregion


    }
}

