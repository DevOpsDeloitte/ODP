using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.IO;
using System.Configuration;
using ODPTaxonomyUtility_TT;

namespace ExcelReportCleaner
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string sitesFolders = ConfigurationManager.AppSettings["sitesFolders"].ToString();
                string deleteExcelFilePathBase = ConfigurationManager.AppSettings["deleteExcelFilePathBase"].ToString();
                string timeSpanMinutes = ConfigurationManager.AppSettings["timeSpanMinutes"].ToString();
                int timeSpan = 0;
                if (Int32.TryParse(timeSpanMinutes, out timeSpan))
                {
                    DeleteReports(sitesFolders, deleteExcelFilePathBase, timeSpan);
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        static void DeleteReports(string sitesFolders, string deleteExcelFilePathBase, int timeSpan)
        {
            string[] folders = sitesFolders.Split(',');
            string path = null;
            string[] dirs = null;

            foreach (string folder in folders)
            {
                path = deleteExcelFilePathBase + folder + @"\Reports\";
                if (Directory.Exists(path))
                {
                    dirs = Directory.GetFiles(path);
                    foreach (string dir in dirs)
                    {
                        DateTime creationTime = File.GetCreationTime(dir);
                        DateTime currentTime = DateTime.Now;
                        TimeSpan ts = currentTime - creationTime;

                        if (ts.TotalMinutes > timeSpan)
                        {
                            File.Delete(dir);
                        }

                    }
                }
            }            
        }

        static void LogError(Exception ex)
        {
            string path = ConfigurationManager.AppSettings["logFilePath"].ToString();
            if (Directory.Exists(path))
            {
                string fileName = @"error_" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Month.ToString() +
                       "_" + DateTime.Now.Date.Day +
                   "_" + DateTime.Now.Hour.ToString() + "_" + DateTime.Now.Minute.ToString() + "_" + DateTime.Now.Second.ToString() + ".txt";
                string input = ex.ToString();

                Utils.CreateFile(path, fileName, input);
            }

        }
    }

}
