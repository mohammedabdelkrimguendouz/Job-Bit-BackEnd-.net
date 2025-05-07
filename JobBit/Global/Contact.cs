
using EASendMail;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.EventLog;
using JobBit_Business;

namespace JobBit.Global
{
    public class Contact
    {
        static public bool SendEmail(string ToEmail, string Subject, string Body)
        {
            string FromEmail = "jobbit.contact@gmail.com";
            string Password = "aubc emlx imhb glyz";
            try
            {
                SmtpMail Mail = new SmtpMail("TryIt");
                Mail.From = FromEmail;// إميل الشخص الذي يرسل الرسالة 
                Mail.To = ToEmail; // إميل الشخص الذي يستقبل الرسالة
                Mail.Subject = Subject;// موضوع الرسالة  
                Mail.TextBody = Body;// محتوى الرسالة 

                SmtpServer Server = new SmtpServer("smtp.gmail.com");// smtp.live.com  ------> @hotmail///smtp.gmail.com---->@gmail
                Server.User = FromEmail;// إميل الشخص الذي يرسل الرسالة
                Server.Password = Password;  // كلمة السر  
                Server.Port = 465;  //  25 or 587 or 465
                Server.ConnectType = SmtpConnectType.ConnectSSLAuto;

                SmtpClient smtp = new SmtpClient();
                smtp.SendMail(Server, Mail);
            }
            catch (Exception Ex)
            {
                clsEventLog.WriteEvent($" Message : {Ex.Message} \n\n Source : {Ex.Source} \n\n Target Site :  {Ex.TargetSite} \n\n Stack Trace :  {Ex.StackTrace}", EventLogEntryType.Error);
                return false;
            }
            return true;
        }
    }
}
