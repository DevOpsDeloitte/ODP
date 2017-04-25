using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using ODPTaxonomyDAL_ST;
using ODPTaxonomyWebsite.Evaluation.Classes;

namespace ODPTaxonomyWebsite.Evaluation
{
    public partial class PrintAbstract : System.Web.UI.Page
    {
        public int abstractId = 0;
        public string userID = "";
        public string projectTitle;
        public string administeringIC;
        public string applicationID;
        public string PIProjectLeader;
        public string FY;
        public string ProjectNumber;
        public string desc;
        public string healthpart;
        public string codingType;


        private DataDataContext db = null;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!String.IsNullOrEmpty(Request.QueryString["id"]))
            {
                abstractId = int.Parse(Request.QueryString["id"]);
            }
            if (abstractId > 0 && HttpContext.Current.User.Identity.IsAuthenticated)
            {
                db = DBData.GetDataContext();
               
                MembershipUser user = Membership.GetUser();
                userID = user.UserName;

                var absrec = db.Abstracts.Where(a => a.AbstractID == abstractId).FirstOrDefault();
                var absrec_text = db.Abstract_Texts.Where(a => a.AbstractID == abstractId).FirstOrDefault();
                if (absrec != null && absrec_text !=null)
                {
                    projectTitle = absrec.ProjectTitle;
                    administeringIC = absrec.AdministeringIC;
                    applicationID = absrec.ChrApplicationID.ToString();
                    PIProjectLeader = absrec.PIProjectLeader;
                    FY = absrec.FY;
                    ProjectNumber = absrec.ProjectNumber;
                    desc = absrec_text.AbstractDescPart;
                    healthpart = absrec_text.AbstractPublicHeathPart;
                    codingType = absrec.CodingType == null ? "Regular" : "Basic";

                }

                
            }

        }
    }
}