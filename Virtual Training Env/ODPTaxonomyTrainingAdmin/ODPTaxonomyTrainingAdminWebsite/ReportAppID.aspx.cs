using ODPTaxonomyUtility_TT;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ODPTaxonomyTrainingAdminWebsite
{
    public partial class ReportAppID : System.Web.UI.Page
    {

        SqlConnection prodConn = new SqlConnection(ConfigurationManager.ConnectionStrings["ODPTaxonomyProd"].ConnectionString);
        SqlConnection trainConn = new SqlConnection(ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString);
         
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindRepGrid();
            }
        }

        private void BindRepGrid()
        {
            string rdoValRepEnv = (string)(Session["ssRdoValRepEnv"]);
            string consensus = (string)(Session["ssConsensus"]);
            string appID = (string)Session["ssApplicationID"];
            string frmDate = (string)(Session["ssFrmDate"]);
            string toDate = (string)(Session["ssToDate"]);


            //ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + rdoValRepEnv + consensus + appID + frmDate + toDate + "');", true);
            SqlCommand Com = new SqlCommand();
            DataSet ds = new DataSet();
        try
        {
            if (rdoValRepEnv == "1") //Production
            {
                Com.Connection = prodConn;
                Com.CommandText = "Report_Answerkey_ConnectionString";
                Com.CommandType = CommandType.StoredProcedure;
                Com.Parameters.Add("@RCCC", SqlDbType.VarChar, 10).Value = consensus; //"CC";//
            }
            else       //Training
            {
                Com.Connection = trainConn;
                Com.CommandText = "Report_Tr_Answerkey_ConnectionString";
                Com.CommandType = CommandType.StoredProcedure;
            }
           // SqlCommand Com = new SqlCommand("Report_Answerkey_ConnectionString", prodConn);

            if (appID != null)
                Com.Parameters.Add("@ApplicationID_p", SqlDbType.Int).Value = appID;
            else
            Com.Parameters.Add("@ApplicationID_p", SqlDbType.Int).Value = DBNull.Value;

            if (frmDate != null)
                Com.Parameters.Add("@DateStart", SqlDbType.VarChar, 50).Value = frmDate;
            else
                Com.Parameters.Add("@DateStart", SqlDbType.VarChar, 50).Value = DBNull.Value;

            if (frmDate != null)
                Com.Parameters.Add("@DateEnd", SqlDbType.VarChar, 50).Value = toDate;
            else
                Com.Parameters.Add("@DateEnd", SqlDbType.VarChar, 50).Value = DBNull.Value; 
            
            SqlDataAdapter adp = new SqlDataAdapter(Com);

            adp.Fill(ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvw_list.DataSource = ds;
                gvw_list.DataBind();
            }
            else
            {
                gvw_list.DataSource = null;
                gvw_list.DataBind();
            }
        }
        catch(Exception ex)
        {
            Response.Write("Error Occured: " + ex.ToString());
        }
        finally
        {
            ds.Clear();
            ds.Dispose();
        }    
    
        }
    }
}