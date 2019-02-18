using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace FitFlexApparel.Controllers
{
    public class MessagingController : Controller
    {


        private static string SenderEmailId = WebConfigurationManager.AppSettings["DefaultEmailId"];
        private static string SenderEmailPassword = WebConfigurationManager.AppSettings["DefaultEmailPassword"];
        private static int SenderEmailPort = Convert.ToInt32(WebConfigurationManager.AppSettings["DefaultEmailPort"]);
        private static string SenderEmailHost = WebConfigurationManager.AppSettings["DefaultEmailHost"];

        [HttpPost]
        public JsonResult SendEmail(string EmailSubject, string EmailBody, string EmailTo, string EmailName)
        {

            try
            {
                System.Net.Mail.MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(SenderEmailHost);
                mail.From = new MailAddress(SenderEmailId, "Fitflex");
                mail.To.Add(EmailTo);
                mail.Subject = EmailSubject;
                mail.Body = EmailBody;
                mail.IsBodyHtml = true;
                SmtpServer.Port = SenderEmailPort;
                SmtpServer.Credentials = new System.Net.NetworkCredential(SenderEmailId, SenderEmailPassword);
                SmtpServer.EnableSsl = false;

                SmtpServer.Send(mail);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
    }
}