using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using ODPTaxonomyDAL_ST;
using ODPTaxonomyWebsite.Evaluation.Classes;
using System.Text;

namespace ODPTaxonomyWebsite.Evaluation
{
    public partial class Glossary : System.Web.UI.Page
    {
        public string studyFocus = string.Empty;
        public string entitiesStudied = string.Empty;
        public string studySettings = string.Empty;
        public string populationFocus = string.Empty;
        public string studyDesignPurpose = string.Empty;
        public string preventionCategory = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            studyFocus = RenderCategory("A_StudyFocus");
            entitiesStudied = RenderCategory("B_EntitiesStudied");
            studySettings = RenderCategory("C_StudySetting");
            populationFocus = RenderCategory("D_PopulationFocus");
            studyDesignPurpose = RenderCategory("E_StudyDesignPurpose");
            preventionCategory = RenderCategory("F_PreventionCategory");

        }

        private string RenderCategory(string LookupName)
        {

            var db = DBData.GetDataContext();
            var topics = db.Protocols.Where(p => p.LookUpTable == LookupName).OrderBy(p => p.LookUpID).Select(p => p).ToList();
            StringBuilder finalStr = new StringBuilder();
            string[] catname = LookupName.ToLower().Split('_');
            var count = 1;
            foreach (var topic in topics)
            {

                StringBuilder row = new StringBuilder();
                row.AppendLine("<div class=\"topicitem\">");
                row.AppendLine("<div class=\"subtitle\"><a name=\""+ catname[1] + "-" + topic.LookUpID.ToString() + "\"> " + topic.SubTitle + "</a></div>");
                row.AppendLine("<div class=\"content\">" + topic.Protocol1 + "</div>");
                row.AppendLine("</div>");
                finalStr.Append(row);
                //if (count > 0) break;
                count++;
            }

            return finalStr.ToString();
        }



        //private void RenderStudyFocus()
        //{

        //    var db = new DBDataContext();
        //    var topics = db.Protocols.Where(p => p.LookUpTable == "A_StudyFocus").OrderBy(p => p.LookUpID).Select(p => p).ToList();
        //    StringBuilder finalStr = new StringBuilder();
        //    var count = 1;
        //    foreach (var topic in topics)
        //    {

        //        StringBuilder row = new StringBuilder();
        //        row.AppendLine("<div class=\"row\">");
        //        row.AppendLine("<div class=\"subtitle\"><a name=\"studyfocus-" + topic.LookUpID.ToString()+"\"> " +topic.SubTitle+"<div>");
        //        row.AppendLine("<div class=\"content\">" + topic.Protocol1 + "<div>");
        //        row.AppendLine("</div>");
        //        finalStr.Append(row);
        //        //if (count > 0) break;
        //        count++;
        //    }

        //    studyFocus = finalStr.ToString();
        //}

        //private void RenderEntitiesStudied()
        //{

        //    var db = new DBDataContext();
        //    var topics = db.Protocols.Where(p => p.LookUpTable == "B_EntitiesStudied").OrderBy(p => p.LookUpID).Select(p => p).ToList();
        //    StringBuilder finalStr = new StringBuilder();
        //    var count = 1;
        //    foreach (var topic in topics)
        //    {

        //        StringBuilder row = new StringBuilder();
        //        row.AppendLine("<div class=\"row\">");
        //        row.AppendLine("<div class=\"subtitle\"><a name=\"entitiesstudied-" + topic.LookUpID.ToString() + "\"> " + topic.SubTitle + "<div>");
        //        row.AppendLine("<div class=\"content\">" + topic.Protocol1 + "<div>");
        //        row.AppendLine("</div>");
        //        finalStr.Append(row);
        //        //if (count > 0) break;
        //        count++;
        //    }

        //    entitiesStudied = finalStr.ToString();
        //}

        //private void RenderStudySettings()
        //{

        //    var db = new DBDataContext();
        //    var topics = db.Protocols.Where(p => p.LookUpTable == "C_StudySettings").OrderBy(p => p.LookUpID).Select(p => p).ToList();
        //    StringBuilder finalStr = new StringBuilder();
        //    var count = 1;
        //    foreach (var topic in topics)
        //    {

        //        StringBuilder row = new StringBuilder();
        //        row.AppendLine("<div class=\"row\">");
        //        row.AppendLine("<div class=\"subtitle\"><a name=\"studysettings-" + topic.LookUpID.ToString() + "\"> " + topic.SubTitle + "<div>");
        //        row.AppendLine("<div class=\"content\">" + topic.Protocol1 + "<div>");
        //        row.AppendLine("</div>");
        //        finalStr.Append(row);
        //        //if (count > 0) break;
        //        count++;
        //    }

        //    studySettings = finalStr.ToString();
        //}


    }
}