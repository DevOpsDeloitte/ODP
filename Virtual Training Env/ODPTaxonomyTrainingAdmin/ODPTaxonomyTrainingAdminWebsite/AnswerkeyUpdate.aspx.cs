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
            trDate.Visible = false;
            trAppID.Visible = false;
        }

        protected void btn_update_Click(object sender, EventArgs e)
        {
            resetRerunKappa();
            resetValueReport();
            
            string checkExist = "select count(ApplicationID) from abstract where ApplicationID = '" + Convert.ToInt32(txt_app_id.Text.Trim()) + "'";
            string abstarctData = "select ApplicationID,FY,ProjectNumber,PIProjectLeader,ProjectTitle from abstract where ApplicationID = '" + Convert.ToInt32(txt_app_id.Text.Trim()) + "'";

            if (rdoValUpEnv.SelectedItem.Value.ToString() == "1")  ///Production
            {
                prodConn.Open();
                 /**************check if data exist in prodution*********/
                 SqlCommand com = new SqlCommand(checkExist, prodConn); 
                 temp = Convert.ToInt32(com.ExecuteScalar().ToString());
                 SqlCommand abs = new SqlCommand(abstarctData, prodConn);//get value from abstract to show on message box
                 SqlDataReader dr = abs.ExecuteReader();
                 while (dr.Read())
                 {
                    getAppID = dr.GetInt32(0);
                    getFY = dr.GetString(1).Trim();
                    getPN = dr.GetString(2).Trim();
                    getPI = dr.GetString(3).Trim();
                    getPT = dr.GetString(4).Trim();
                 }
                 prodConn.Close();
             }
             else if (rdoValUpEnv.SelectedItem.Value.ToString() == "2") //Training
             {
                  trainConn.Open();
                  SqlCommand com = new SqlCommand(checkExist, trainConn);//check if data exists in training
                  temp = Convert.ToInt32(com.ExecuteScalar().ToString());
                  SqlCommand abs = new SqlCommand(abstarctData, trainConn);//get value from abstract
                  SqlDataReader dr = abs.ExecuteReader();
                  while (dr.Read())
                  {
                     getAppID = dr.GetInt32(0);
                     getFY = dr.GetString(1).Trim();
                     getPN = dr.GetString(2).Trim();
                     getPI = dr.GetString(3).Trim();
                     getPT = dr.GetString(4).Trim();
                  }
                     trainConn.Close();
             }

             if (temp >= 1)  //data exists
             {
                  string message ="Data Exists as following!\\n" +
                                "1. Application ID =" + getAppID + "\\n" +
                                "2. Fiscal Year =" + getFY + "\\n" +
                                "3. Project Number =" + getPN + "\\n" +
                                "4. PI Project Leader =" + getPI + "\\n" +
                                "5. Project Title =" + getPT + "\\n" +
                                "Are you sure you want to update?";

                  this.ClientScript.RegisterStartupScript(typeof(Page), "Popup", "ConfirmUpdateValue('" + message.Replace("'", "\\'") + "');", true);
              }
              else  //data not exists
              {
                   ScriptManager.RegisterStartupScript(this, this.GetType(), "Message",
                   "alert('Data Not Exists!'); window.location='" +
                   Request.ApplicationPath + "AnswerkeyUpdate.aspx';", true);
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
                             Response.Write("Error:" + ex.ToString());
                         }
                     }
                     else
                     {  
                         ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Please fill Application ID!')" , true);
                        //txt_app_id1.Focus();
                         ddlReport_changed();
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
                               Response.Write("Error:" + ex.ToString());
                           }
                       }
                       else
                       {
                           ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('To date must be after From date!')", true);
                           //txt_frm_date.Focus();
                           ddlReport_changed();
                       }
                     }
                     else
                     {
                       if (txt_frm_date.Text == string.Empty)
                         {
                             ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Please fill From Date!')", true);
                             ddlReport_changed();
                             txt_frm_date.Focus();
                         }
                         else if (txt_to_date.Text == string.Empty)
                         {
                             ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Please fill To Date!')", true);
                             ddlReport_changed();
                             txt_to_date.Focus();
                         }
                     }
                 }
        }

        

        protected void btn_rerun_kappa_Click(object sender, EventArgs e)
        {
            resetValueUpdates();
            resetValueReport();

                prodConn.Open();
                //**********************Check if data exists**************************************//
                string checkExist = "select isnull(count(*),0) from report_answerkey_update_log where ApplicationID = '" + Convert.ToInt32(txt_app_id2.Text.Trim()) + "' ";
                SqlCommand com = new SqlCommand(checkExist, prodConn);

                int temp = Convert.ToInt32(com.ExecuteScalar().ToString());

                prodConn.Close();
                if (temp >= 1) //data exists
                {
                    string message = "Data Exists! Do you want to rerun kappa?";

                    this.ClientScript.RegisterStartupScript(typeof(Page), "Popup", "ConfirmRerunKappa('" + message + "');", true);
                }
                else    //data not exists
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Message",
                    "alert('Data Not Exists!'); window.location='" +
                    Request.ApplicationPath + "AnswerkeyUpdate.aspx';", true);
                }
        
        }

        protected void rdoValUpEnv_SelectedIndexChanged(object sender, EventArgs e) //RadioButton changed Pord/Training in Value Update
        {
            if (rdoValUpEnv.SelectedItem.Value.ToString() == "1") //Prod
            {
                tdDdlCons.Visible = true;
              /*  lbl_consensus.Visible = true;
                ddl_consensus.Visible = true;
                ConsensusRequired.Visible = true;*/
            }
            else if (rdoValUpEnv.SelectedItem.Value.ToString() == "2") //Training
            {
                tdDdlCons.Visible = false;
               /* lbl_consensus.Visible = false;
                ddl_consensus.Visible = false;
                ConsensusRequired.Visible = false;*/
                ddl_consensus.SelectedIndex = 0;
            }

        }

        protected void rdoValRepEnv_SelectedIndexChanged(object sender, EventArgs e)//RadioButton changed Pord/Training in Value Report
        {
            if (rdoValRepEnv.SelectedItem.Value.ToString() == "1") //Prod 
            {
                tdDdlCons1.Visible = true;
               /* lbl_consensus1.Visible = true;
                ddl_consensus1.Visible = true;
                ConsensusRequired1.Visible = true;*/
                ddlReport.SelectedIndex = 0;
                ddl_consensus1.SelectedIndex = 0;
                txt_app_id1.Text = string.Empty;
                txt_frm_date.Text = string.Empty;
                txt_to_date.Text = string.Empty;
            }
            else if (rdoValRepEnv.SelectedItem.Value.ToString() == "2") //Training
            {
                tdDdlCons1.Visible = false;
              /*  lbl_consensus1.Visible = false;
                ddl_consensus1.Visible = false;
                ConsensusRequired1.Visible = false;*/
                ddl_consensus1.SelectedIndex = 0;
                ddlReport.SelectedIndex = 0;
                txt_app_id1.Text = string.Empty;
                txt_frm_date.Text = string.Empty;
                txt_to_date.Text = string.Empty;
            }
        }
        protected void ddl_frm_section_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = ddl_frm_section.SelectedItem.Value;
           // ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('"+selected+"')", true);
            if (selected == "E7F6") //E7F6
            {
                tdTxtUpdVal.Visible = false;
               // lbl_upd_values.Visible = false;
              //  txt_upd_values.Visible = false;
                txt_upd_values.Text = "1";
            }
            else
            {
                tdTxtUpdVal.Visible = true;
              //  lbl_upd_values.Visible = true;
               // txt_upd_values.Visible = true;
                txt_upd_values.Text = "";
            }
        }

       protected void ddlReport_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlReport_changed();
        }

        #region MethodBtn

        protected void rerunKappaMethod(object sender, EventArgs e)//hidding method javascript call --production only (rerunKappa)
        {
            prodConn.Open();
            SqlCommand Com = new SqlCommand("[Update_RCCC_ReRunKappa_ByAppID]", prodConn);
            Com.CommandType = CommandType.StoredProcedure;
            Com.Parameters.Add("@RCCC", SqlDbType.VarChar, 10).Value = ddl_consensus2.SelectedValue.ToString().Trim();
            Com.Parameters.Add("@ApplicationID", SqlDbType.Int).Value = Convert.ToInt32(txt_app_id2.Text.Trim());


            try
            {
                if (Com.Connection.State == ConnectionState.Closed)
                {
                    Com.Connection.Open();
                }

                Com.ExecuteNonQuery();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message",
               "alert('Rerun Kappa Successful!'); window.location='" +
               Request.ApplicationPath + "AnswerkeyUpdate.aspx';", true);

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

        protected void updValProdMethod(object sender, EventArgs e)//hidding method javascript call --production (UpdateValue)
        {
            prodConn.Open();
            SqlCommand Com = new SqlCommand("Update_RCCC_ABCDEF_ByAppID", prodConn);
            Com.CommandType = CommandType.StoredProcedure;
            Com.Parameters.Add("@ApplicationID", SqlDbType.Int).Value = Convert.ToInt32(txt_app_id.Text.Trim());
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message",
                    "alert('Update Value Successful in Production!'); window.location='" +
                    Request.ApplicationPath + "AnswerkeyUpdate.aspx';", true);
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

        protected void updValTrainMethod(object sender, EventArgs e)//hidding method javascript call --training (UpdateValue)
        {
            trainConn.Open();
            SqlCommand Com = new SqlCommand("Update_RC_TR_ByAppID", trainConn);
            Com.CommandType = CommandType.StoredProcedure;
            Com.Parameters.Add("@ApplicationID", SqlDbType.Int).Value = Convert.ToInt32(txt_app_id.Text.Trim());
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message",
                  "alert('Update Value Successful in Training!'); window.location='" +
                  Request.ApplicationPath + "AnswerkeyUpdate.aspx';", true);
            }
            catch (Exception ex)
            {
                Response.Write("Error:" + ex.ToString());
            }
            finally
            {
                Com.Connection.Close();
            }
        }

        #endregion

        #region MethodsResetVal
        protected void ddlReport_changed()
        {
            string report = ddlReport.SelectedItem.Value;
           // ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('" + report + "')", true);
            if (report == "1") //ApplicationID
            {
                trDate.Visible = false;
                trAppID.Visible = true;
                txt_app_id1.Focus();
                txt_frm_date.Text = "";
                txt_to_date.Text = "";
            }
            if (report == "2") //Date
            {
                trDate.Visible = true;
                trAppID.Visible = false;
                txt_frm_date.Focus();
                txt_app_id1.Text = "";
            }
            else if (report == "0") //None
            {
                trDate.Visible = false;
                trAppID.Visible = false;
                txt_app_id1.Text = "";
                txt_frm_date.Text = "";
                txt_to_date.Text = "";
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

