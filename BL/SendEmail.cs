using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Email
    {
        public bool SendEmail(MailAddress toAddress, string subject, string body)
        {
            MailAddress fromAddress = new MailAddress("tfeindwerkcvo@gmail.com", "FAOMA_Auto_service");
            string fromPassword = "TFeindwerkCVO1";
            string smtpHost = "smtp.gmail.com";
            int smtpPort = 587;//gmail port

            try
            {
                var smtp = new SmtpClient
                {
                    Host = smtpHost,
                    Port = smtpPort,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                })
                {
                    smtp.Send(message);
                }
                return true;
            }
            catch (Exception err)
            {
                //Elmah.ErrorSignal.FromCurrentContext().Raise(err);
                return false;
            }

        }
    }
}
