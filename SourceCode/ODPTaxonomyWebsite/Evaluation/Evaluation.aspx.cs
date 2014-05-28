using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ODPTaxonomyUtility_TT;
using ODPTaxonomyCommon;

namespace ODPTaxonomyWebsite.Evaluation
{
    public partial class Evaluation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    lbl_messageUsers.Visible = false;

                    if (Session["ViewAbstractToEvaluation"] != null)
                    {
                        ViewAbstractToEvaluation values = (ViewAbstractToEvaluation)Session["ViewAbstractToEvaluation"];
                        lbl_messageUsers.Visible = true;
                        lbl_messageUsers.Text = "User ID: " + values.UserId.ToString() + "; EvaluationId = " + values.EvaluationId.ToString()
                            + "; SubmissionTypeId = " + values.SubmissionTypeId.ToString() + "; Mode: " + values.ViewMode.ToString();
                        Session.Remove("ViewAbstractToEvaluation");
                    }
                    else
                    {
                        lbl_messageUsers.Visible = true;
                        lbl_messageUsers.Text = "No Session";
                    }
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex);
                throw new Exception("An error has occured while loading page data.");
            }
        }
    }
}