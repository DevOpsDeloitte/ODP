using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;

namespace ODPTaxonomyDAL_TT
{
    public enum Status
    {
        Active = 1,
        InActive = 2,
        Deleted = 3

    }

    public enum TeamType
    {
        Coder = 1,
        ODPStaff = 2
    }

    

    public static class Common
    {
        
        static Common()
        {
            RoleNames = new Dictionary<string, string>();
            RoleNames.Add("admin", "Admin");
            RoleNames.Add("coder", "Coder");
            RoleNames.Add("coderSup", "CoderSupervisor");
            RoleNames.Add("odp", "ODPStaffMember");
            RoleNames.Add("odpSup", "ODPStaffSupervisor");
        }

        public static Dictionary<string, string> RoleNames
        {
            set;
            get;
        }

        public static Guid GetCurrentUserId(string connString, string userCurrentName)
        {
            Guid userCurrentId = Guid.Empty;
            using (DataDataContext db = new DataDataContext(connString))
            {
                var matches = (from u in db.tbl_aspnet_Users
                               where u.UserName == userCurrentName
                               select u).FirstOrDefault();
                userCurrentId = matches.UserId;
            }

            return userCurrentId;
        }

        public static string GetTeamCode(string initials)
        {
            string teamCode = "";
            string format = "yyyyMMdd_HHmmss";
            string sDateTime = DateTime.Now.ToString(format);
            StringBuilder sb = new StringBuilder();

            sb.Append(initials);
            sb.Append(sDateTime);
            teamCode = sb.ToString();

            return teamCode;
        }
    }
}
