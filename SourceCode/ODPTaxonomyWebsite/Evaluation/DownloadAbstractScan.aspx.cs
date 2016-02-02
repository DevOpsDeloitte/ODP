using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace ODPTaxonomyWebsite.Evaluation
{
    public partial class DownloadAbstractScan : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["scan"] != null)
            {
                string fileName = Request.QueryString["scan"].ToString();

                string fName = Request.PhysicalApplicationPath + "notes\\" + fileName;
                if (File.Exists(fName))
                {
                    Response.Clear();
                    Response.BufferOutput = false;
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "filename=" + fileName);
                    
                    Response.TransmitFile(fName);
                }
                else
                {
                    Response.ContentType = "text/plain";
                    Response.Write("File name is either wrong or not specified.");
                    //goto End;
                }
            }
            else
            {
                Response.ContentType = "text/plain";
                Response.Write("File name is either wrong or not specified.");
                //goto End;
            }

            //End:
            //Response.ContentType = "text/plain";
            //Response.Write("File name is either wrong or not specified.");
        }
    }
}