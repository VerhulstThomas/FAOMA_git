using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace LoopEmailChecker
{
    public class MailSender
    {
        public void sendInlogErrorMessage(string emailadres)
        {
            try
            {
                MailMessage message = new MailMessage();
                message.From = new MailAddress("tfeindwerkcvo@gmail.com");

                message.To.Add(new MailAddress(emailadres));
                
                message.Subject = "Inlog mislukt";
                message.Body = "Het inloggen in de door u opgegeven account is mislukt, gelieve het emailadres en wachtwoord na te kijken in uw account bij Faoma";

                SmtpClient client = new SmtpClient();
                client.Send(message);
            }
            catch (Exception)
            {

                throw;
            }
           
        }
    }
}
