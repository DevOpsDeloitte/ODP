using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Net.Mail;
using System.IO;
using System.Configuration;
using System.Security.Cryptography;
using System.Linq;

namespace ODPTaxonomyUtility_TT
{
    public class Utils
    {
        
        public static void LogError(Exception ex)
        {
            string path = HttpContext.Current.Request.PhysicalApplicationPath.ToString() + @"log\";
            string fileName = @"error_" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Month.ToString() +
                   "_" + DateTime.Now.Date.Day +
               "_" + DateTime.Now.Hour.ToString() + "_" + DateTime.Now.Minute.ToString() + "_" + DateTime.Now.Second.ToString() + ".txt";
            string input = ex.ToString();

            CreateFile(path, fileName, input);


        }

        public static void LogError(string text)
        {
            string path = HttpContext.Current.Request.PhysicalApplicationPath.ToString() + @"log\";
            string fileName = @"error_" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Month.ToString() +
                   "_" + DateTime.Now.Date.Day +
               "_" + DateTime.Now.Hour.ToString() + "_" + DateTime.Now.Minute.ToString() + "_" + DateTime.Now.Second.ToString() + ".txt";
            string input = text;

            CreateFile(path, fileName, input);


        }


        public static void CreateFile(string path, string fileName, string input, Encoding enc)
        {
            FileStream fileStream;
            StreamWriter streamWriter;
            string dirPath;
            if (Directory.Exists(path))
            {
                dirPath = path + fileName;
                fileStream = new FileStream(dirPath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
                streamWriter = new StreamWriter(fileStream, enc);
                streamWriter.Write(input);
                streamWriter.WriteLine();
                streamWriter.Flush();
                streamWriter.Close();
            }


        }

        public static void CreateFile(string path, string fileName, string input)
        {
            FileStream fileStream;
            StreamWriter streamWriter;
            string dirPath;
            if (Directory.Exists(path))
            {
                dirPath = path + fileName;
                fileStream = new FileStream(dirPath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
                streamWriter = new StreamWriter(fileStream);
                streamWriter.Write(input);
                streamWriter.WriteLine();
                streamWriter.Flush();
                streamWriter.Close();
            }


        }

        private static bool validateRegEx(string value, string pattern)
        {
            bool isValid = false;
            Match paramMatch;
            paramMatch = Regex.Match(value, pattern);

            if (paramMatch.Success) isValid = true;

            return isValid;
        }

        public static void SendEmail(string recipientEmail, string senderEmail, string subject, string message, string attachmentPath, string ccEmailAddress = null)
        {
            string smtpServer = ConfigurationManager.AppSettings["EmailServer"];
            SmtpClient smptServerClient = new SmtpClient(smtpServer);
            //MailMessage messageToBeSent = new MailMessage(recipientEmail, senderEmail, subject, message);
            MailMessage messageToBeSent = new MailMessage();
            messageToBeSent.From = new MailAddress(senderEmail);

            string pattern = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
            string[] emails = recipientEmail.Split(';');
            bool isValid = false;

            foreach (string s in emails)
            {

                if (s.Length > 1)
                {
                    isValid = validateRegEx(s, pattern);
                    if (isValid)
                    {
                        messageToBeSent.To.Add(s);
                    }
                }
            }

            if (ccEmailAddress != null)
            {
                messageToBeSent.CC.Add(ccEmailAddress);
            }

            if (File.Exists(attachmentPath))
            {
                Attachment attach = new Attachment(attachmentPath);
                messageToBeSent.Attachments.Add(attach);
                messageToBeSent.Subject = subject;
                messageToBeSent.Body = message;
                messageToBeSent.IsBodyHtml = true;

                smptServerClient.Send(messageToBeSent);
                attach.Dispose();
            }


        }


        public static void SendEmail(string recipientEmail, string senderEmail, string subject, string message, string ccEmailAddress = null)
        {
            string smtpServer = ConfigurationManager.AppSettings["EmailServer"];
            SmtpClient smptServerClient = new SmtpClient(smtpServer);
            //MailMessage messageToBeSent = new MailMessage(recipientEmail, senderEmail, subject, message);
            MailMessage messageToBeSent = new MailMessage();
            messageToBeSent.From = new MailAddress(senderEmail);

            string pattern = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
            string[] emails = recipientEmail.Split(';');
            bool isValid = false;

            foreach (string s in emails)
            {

                if (s.Length > 1)
                {
                    isValid = validateRegEx(s, pattern);
                    if (isValid)
                    {
                        messageToBeSent.To.Add(s);
                    }
                }
            }

            if (ccEmailAddress != null)
            {
                messageToBeSent.CC.Add(ccEmailAddress);
            }
            messageToBeSent.Subject = subject;
            messageToBeSent.Body = message;
            messageToBeSent.IsBodyHtml = true;

            smptServerClient.Send(messageToBeSent);
        }

        public static bool IsGZipSupported()
        {
            string AcceptEncoding = HttpContext.Current.Request.Headers["Accept-Encoding"];
            if (AcceptEncoding == null)
            {
                return false;
            }
            else
            {
                if (AcceptEncoding.Contains("gzip") || AcceptEncoding.Contains("deflate"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }

        public static void GZipEncodePage()
        {
            HttpResponse Response = HttpContext.Current.Response;

            if (IsGZipSupported())
            {
                string AcceptEncoding = HttpContext.Current.Request.Headers["Accept-Encoding"];
                if (AcceptEncoding.Contains("deflate"))
                {
                    Response.Filter = new System.IO.Compression.DeflateStream(Response.Filter,
                                               System.IO.Compression.CompressionMode.Compress);
                    Response.AppendHeader("Content-Encoding", "deflate");
                }
                else
                {
                    Response.Filter = new System.IO.Compression.GZipStream(Response.Filter,
                                              System.IO.Compression.CompressionMode.Compress);
                    Response.AppendHeader("Content-Encoding", "gzip");
                }
            }

            // Allow proxy servers to cache encoded and unencoded versions separately
            Response.AppendHeader("Vary", "Content-Encoding");
        }


    }
}
