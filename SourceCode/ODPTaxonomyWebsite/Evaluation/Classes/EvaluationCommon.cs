using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ODPTaxonomyWebsite.Evaluation
{

    //public class Comments
    //{
    //    public List<TeamUser> IQCoders;
    //    public List<TeamUser> ODPCoders;
    //    public TeamUser IQConsensusUser;
    //    public TeamUser ODPConsensusUser;

    //    public Comments()
    //    {
    //        this.IQCoders = new List<TeamUser>();
    //        this.ODPCoders = new List<TeamUser>();
    //        this.IQConsensusUser = new TeamUser();
    //        this.ODPConsensusUser = new TeamUser();
    //    }
    //}

    //public class TeamUser // Team user class
    //{
    //    public Guid UserId {get;set;}
    //    public int TeamId { get; set; }
    //    public string TeamType { get; set; }
    //    public string UserName { get; set; }
    //    public string UserFirstName { get; set; }
    //    public string UserLastName { get; set; }
    //    public int UserSubmissionID { get; set; }
    //    public string UserComment { get; set; }
    //}

    //public class ComparisonTeamUser
    //{
    //    public Guid? UserId { get; set; }
    //    public int? TeamId { get; set; }
    //    public string TeamType { get; set; }
    //    public int ComparisonSubmissionID { get; set; }
    //    public string UserComment { get; set; }
    //}

    public class EvaluationCommon
    {

        public static bool checkCorrectEvaluation(string applicationID, HttpRequest Request)
        {
            // This is an added security check for B types.
            if (applicationID.ToLower().Contains("_b"))
            {
                if (Request.Url.Segments.Last().ToLower().Contains("b.aspx"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else // Not a B type eval.
            {
                if (Request.Url.Segments.Last().ToLower().Contains("b.aspx"))
                {
                    return false; // regular evaluations should not land on this page.
                }
                else
                {
                    return true;
                }
            }

        }

    }
}