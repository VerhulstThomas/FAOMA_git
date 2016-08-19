using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Faoma4;
using DAL;
using S22.Imap;
using System.Net.Mail;

namespace BL
{
    public class ServerAccountService : IServerAccountService
    {
        private ServerAccountDao sDao = new ServerAccountDao();
        public void CreateServerAccount(serverAccount serverAccount)
        {
            sDao.CreateServerAccount(serverAccount);
            //throw new NotImplementedException();
        }

        public void DeleteServerAccount(long id)
        {
            sDao.DeleteServerAccount(id);
            //throw new NotImplementedException();
        }

       

        //public void EditServerAccount()
        //{
        //     sDao.EditServerAccount();
        //    throw new NotImplementedException();
        //}

        public void EditServerAccount(serverAccount serverAccount, long? id)
        {
            sDao.EditServerAccount(serverAccount, id);
            //throw new NotImplementedException(serverAccount, id);
        }

        public serverAccount FindServerAccountWithId(long? id)
        {
            return sDao.FindServerAccountWithId(id);
            //throw new NotImplementedException();
        }

        public IList<serverAccount> ListOfServerAccounts()
        {
            return sDao.ListOfServerAccounts().ToList();
        }

        public void TestLoginTeBeherenAccount(serverAccount serverAccount)
        {
            // test check if it is possible to login in given account
            // if not send warningmail tot beheerdersemail
            //string teBeherenEmail = serverAccount.teBeherenEmail;
            //string teBeherenEmailPw = 
            
            string body = "We hebben geprobeerd in te loggen op het door jou ingegeven te beheren e-mailadres, maar dat is niet gelukt /n gelieve na te kijken of u de instellingen in Gmail juist heeft aangepast om FAOMA toegang te geven tot je account";

            try
            {
                using (ImapClient client = new ImapClient("imap.gmail.com", 993, serverAccount.teBeherenEmail, serverAccount.teBeherenEmailPW, AuthMethod.Login, true))
                {
                   // try
                   // {
                       // client.Login(serverAccount.teBeherenEmail, serverAccount.teBeherenEmailPW, AuthMethod.Login);
                    client.GetMailboxInfo();
                    //string naam = "";
                    //naam =client.GetMailboxInfo().Name;
                    //client.Logout();
                    if (client.GetMailboxInfo().Name.Length <2)
                    {
                        MailAddress m = new MailAddress(serverAccount.beheerdersEmail);
                        //Email
                        Email e = new Email();
                        e.SendEmail(m, "testinlog niet gelukt", body);
                    }
                   
                 

                }
            }
            catch (Exception)
            {

                //if inlog fails send e-mail
                MailAddress m = new MailAddress(serverAccount.beheerdersEmail);
                //Email
                Email e = new Email();
                e.SendEmail(m, "testinlog niet gelukt", body);
               // throw;
            }
        }

        public void SendEditConfirmaiton(serverAccount serverAccount)
        {
            string body = "u heeft de gegevens van u FAOMA account succesvol aangepast";
            MailAddress m = new MailAddress(serverAccount.beheerdersEmail);
            //Email
            Email e = new Email();
            e.SendEmail(m, "FAOMA gegevens gewijzigd", body);
            //test
            
        }
    }
}
