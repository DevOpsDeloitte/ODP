using System;
using System.Web;
using Newtonsoft.Json;
using ODPTaxonomyDAL_TT;
using System.Configuration;
using ODPTaxonomyUtility_TT;

namespace ODPTaxonomyWebsite.Evaluation.Handlers
{
   
    public class RedirectionMessage : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.QueryString["userName"] != null)
            {
                string userName = context.Request.QueryString["userName"].ToString().Trim();                

                if (String.IsNullOrEmpty(userName))
                {
                    context.Response.ContentType = "text/plain";
                    context.Response.Write(JsonConvert.SerializeObject(new { success = false }));
                    return;
                }

                if (context.Request.QueryString["abstractId"] != null)
                {
                    int id = -1;
                    string abstractIdToCheck =  context.Request.QueryString["abstractId"].ToString().Trim();
                    if (String.IsNullOrEmpty(abstractIdToCheck))
                    {
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(JsonConvert.SerializeObject(new { success = false, message = "abstractId parameter does not have value" }));
                        return;
                    }

                    if(Int32.TryParse(abstractIdToCheck, out id))
                    {
                        string connString = null;
                        try
                        {
                            connString = ConfigurationManager.ConnectionStrings["ODPTaxonomy"].ToString();
                            string abstractData = Common.GetAbstarctIdForTeamMember(connString, userName);
                            string[] arr = abstractData.Split(',');
                            int status = (int)Common.GetAbstractStatus(connString, id);

                            context.Response.ContentType = "text/plain";
                            context.Response.Write(JsonConvert.SerializeObject(new { status = status, abstractId = arr[0], abstractTitle = arr[1], success = true }));
                        }
                        catch(Exception ex)
                        {
                            Utils.LogError(ex);
                            context.Response.ContentType = "text/plain";
                            context.Response.Write(JsonConvert.SerializeObject(new { success = false, message = ex.Message }));
                            return;
                        }
                    }
                    else
                    {
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(JsonConvert.SerializeObject(new { success = false, message = "abstractId is incorrect format" }));
                        return;
                    }
                }
                else
                {
                    context.Response.ContentType = "text/plain";
                    context.Response.Write(JsonConvert.SerializeObject(new { success = false, message = "no abstractId parameter passed" }));
                    return;
                }               
                
            }
            else
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write(JsonConvert.SerializeObject(new { success = false, message = "no userName parameter passed" }));
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