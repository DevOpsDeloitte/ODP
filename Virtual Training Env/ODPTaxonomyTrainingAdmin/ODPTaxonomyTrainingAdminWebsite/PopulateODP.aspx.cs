using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using ODPTaxonomyUtility_TT;
using ODPTaxonomyTrainingAdminDAL;

namespace ODPTaxonomyTrainingAdminWebsite
{
    public partial class PopulateODP : System.Web.UI.Page
    {
        private string connString = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string l_message;
                int l_resultCnt;
                int? l_returnValue = null ;
                if (!IsPostBack)
                {
                    // if no abstracts, redirect to home
                    if (Session["AbstractIDList"] == null)
                    {
                        Response.Redirect("~/Default.aspx");
                    }
                    else
                    {
                        if (ConfigurationManager.ConnectionStrings["ODPTaxonomy"] != null)
                        {
                            connString = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ToString();
                        }

                        string l_targetInstance = Session["TargetInstance"].ToString();
                        string l_abstractIDList = Session["AbstractIDList"].ToString();
                        List<Tr_Populate_ODPAnswerResult> result;

                        using (TrainingAdminDALDataContext db = new TrainingAdminDALDataContext(connString))
                        {
                            result = db.Tr_Populate_ODPAnswer(l_targetInstance, l_abstractIDList, ref l_returnValue).ToList();
                        }

                        if ((l_returnValue.HasValue == true) && (l_returnValue == 1)){
                            // success
                            l_resultCnt = result.Count();

                            if (l_resultCnt == 0)
                            {
                                l_message = "Error populating ODP data for abstracts: " + l_abstractIDList + " in Target Instance: " + l_targetInstance + ".";
                                ShowMessage(l_message, true);
                            }
                            else
                            {
                                // show grid
                                l_message = "Selections populated to Instance " + l_targetInstance + ".";
                                ShowMessage(l_message, false);
                                gvw_list.DataSource = result;
                                gvw_list.DataBind();
                                gvw_list.Visible = true;
                            }
                        }
                        else {
                            // error
                            l_message = "Error populating ODP data for abstracts: " + l_abstractIDList + " in Target Instance: " + l_targetInstance + ".";
                            ShowMessage(l_message, true);
                        }

                        
                    }
                }
                    
            }
            catch (Exception ex)
            {
                Utils.LogError(ex);
                ShowMessage("An error has occured on ODP Population.  Please check the log.", true);
            }

        }

        private void ShowMessage(string message, bool isError)
        {
            if (isError)
            {
                lbl_Error.Text = message;
                lbl_Error.Visible = true;
            }
            else
            {
                lbl_message.Text = message;
                lbl_message.Visible = true;
            }
        }

    }
}