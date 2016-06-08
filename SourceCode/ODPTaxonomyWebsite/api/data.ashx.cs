using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Data;
using Newtonsoft.Json;
using ODPTaxonomyDAL_ST;
using System.Configuration;
using System.Text;

namespace ODPTaxonomyWebsite.api
{
    /// <summary>
    /// Summary description for data
    /// </summary>
    /// 

    public class ApiResponse
    {
        public Int32 totalRecords { get; set; }
        public Int32 start { get; set; }
        public Int32 length { get; set; }
        public string type { get; set; }
        public List<Report_SelectionDatapullingResult> data { get; set; }
    }
    public class data : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {

            try
            {
                //http://stackoverflow.com/questions/25855698/how-can-i-retrieve-basic-authentication-credentials-from-the-header
                checkCredentials(context);
                getALLData(context);

            }
            catch(Exception ex)
            {
                context.Response.Write(JsonConvert.SerializeObject(new { StatusCode = 401, Exception = ex.Message }));
            }

        }

        public void checkCredentials(HttpContext context)
        {
            var request = context.Request;
            var authHeader = request.Headers["Authorization"];
            //if (authHeader != null)
            //{
            //    if (authHeader == "Basic aXFzZGF0YTppcXMxMQ==")
            //    {
            //        return true;
            //    }
            //}

            //return false;


            if (authHeader != null && authHeader.StartsWith("Basic"))
            {
                string encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
                Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));
                int seperatorIndex = usernamePassword.IndexOf(':');
                var username = usernamePassword.Substring(0, seperatorIndex);
                var password = usernamePassword.Substring(seperatorIndex + 1);
                if (Membership.ValidateUser(username, password)){
                    MembershipUser user = Membership.GetUser(username);
                    // check if user is locked out, if so unlock
                    if (user.IsLockedOut)
                    {
                        throw new Exception("User is locked out.");
                    }
                }
                else
                {
                    throw new Exception("Bad Credentials provided.");
                }
            }
            else {
                //Handle what happens if that isn't the case
                throw new Exception("The authorization header is either empty or isn't Basic.");
            }
        }

        public void getALLData(HttpContext context)
        {
            string connString = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ConnectionString;
            using (ReportingAppDataContext db = new ReportingAppDataContext(connString))
            {
                db.CommandTimeout = 0;
                string start = context.Request["start"] ?? "";
                string length = context.Request["length"] ?? "";

                if (context.Request["type"] == null) throw new Exception("No Type Parameter - Please provide the type parameter.");
                if (context.Request["year"] == null) throw new Exception("No Year Provided - Please provide the year parameter.");

                string type = context.Request["type"] ?? "finalselection";
                string year = context.Request["year"] ?? "2010";

                List<Report_SelectionDatapullingResult> vals = db.Report_SelectionDatapulling(year).ToList();

                if (start == "" && length == "")
                {
                    context.Response.Write(JsonConvert.SerializeObject(new ApiResponse { totalRecords = vals.Count, start = 0, length = vals.Count, type = type, data = vals.ToList() }));
                }
                else
                {
                    Int32 startIdx = Int32.Parse(start);
                    Int32 lengthIdx = Int32.Parse(length);
                    context.Response.Write(JsonConvert.SerializeObject(new ApiResponse { totalRecords = vals.Count, start = startIdx, length = lengthIdx, type = type, data = vals.Skip(startIdx).Take(lengthIdx).ToList() }));
                }

            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}