using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using S22.Imap;
using System.Net.Mail;
using System.IO;
using System.Diagnostics;

using Faoma4;
//using Faoma4.DAL;

namespace LoopEmailChecker
{
    // service https://www.youtube.com/watch?v=uM9o8GsO_u4
    public class LoopUtils
    {
        private static FaomaModel db = new FaomaModel();

        //aan deze lijst worden de accounts toegevoegd als ze gemaakt worden NA de opstart
        public static List<serverAccount> newAccounts = db.serverAccount.ToList();

        // deze methode geeft de niewue accounts terug
        public static List<serverAccount> getNewAccounts()
        {
            // maak een kopie zodat deze methode de oorspronkelijke lijst kan leegmaken
            List<serverAccount> copyNewAccounts = newAccounts;

            for (int i = 0; i < newAccounts.Count; i++)
            {
                newAccounts.RemoveAt(i);
            }

           
            return copyNewAccounts;
         }

        // deze methode wordt voorlopig opgeroepen ind e create van document
        public static string getAllUnseenMails(List<serverAccount> teVerwerkenAccounts)
        {

                foreach (var account in teVerwerkenAccounts)
                {
                string email2 =account.beheerdersEmail;
                    // deze moet in het path verwerkt worden
                    string accountId = Convert.ToString(account.id);

                    try
                    {
                        // using (ImapClient client = new ImapClient("imap.gmail.com", 993, "tfeindwerkcvo@gmail.com", "TFeindwerkCVO1", AuthMethod.Login, true))
                        using (ImapClient client = new ImapClient("imap.gmail.com", 993, account.teBeherenEmail, account.teBeherenEmailPW, AuthMethod.Login, true))

                        {
                            // Returns a collection of identifiers of all mails matching the specified search criteria.
                            IEnumerable<uint> uids = client.Search(SearchCondition.Unseen());
                            // Download mail messages from the default mailbox. also add System.Net.Mail;
                            IEnumerable<MailMessage> messages = client.GetMessages(uids);


                            foreach (var email in messages)
                            {

                                string path = accountId + "\\";
                                //long id;
                                //AttachmentCollection atc = new AttachmentCollection;
                                var itemAttachments = email.Attachments;

                                if (itemAttachments.Count > 0)
                                {

                                    DocumentHelperService dh = new DocumentHelperService();
                                    //bepaal het pat (bedrijfsnaam of default)
                                    // string emailt = email.Sender.ToString();// sender is null?
                                    string verzendersmMail = email.From.Address.ToString();
                                    //string t = "testen";
                                    path += dh.getPathName(verzendersmMail);

                                    foreach (var attachment in itemAttachments)
                                    {
                                        //om het path uniek te maken wordyt de aam van het attatchment toegevoegd
                                        //path += "\\"+attachment.Name;

                                        //Guid g = new Guid();
                                        Guid g = Guid.NewGuid();
                                        // sub voegt een "-" toe + karacters 0-9 van de guid .
                                        string sub = "-" + g.ToString().Substring(0, 9);
                                        // extentie haalt de 4 laatste karakters van de attachmentnaam op bvb ".pdf"
                                        string extentie = attachment.Name.Substring(attachment.Name.Length - 4, 4);
                                        // bestandnaamZonderExtentie bewaard tijdelijk de bestandsnaam zonder extentie
                                        string bestandnaamZonderExtentie = attachment.Name.Substring(0, attachment.Name.Length - 4);
                                        // de uiteindelijke filename is een combinatie van de 3 vorige
                                        string fileName = bestandnaamZonderExtentie + sub + extentie;


                                        // deze data moet worden weggeschreven naar een directory
                                        var data = attachment.ContentStream;
                                        wegschrijven(path, fileName, data);
                                        //OUDwegschrijven(path , attachment.Name, data);

                                        // opslaan van het document, het id van het document wordt teruggegeven, wort hieronder gebrikt voor koppeling contact-document
                                        long docId = dh.saveDocument(attachment.Name, verzendersmMail, path + "\\" + fileName);
                                        //OUDlong docId =dh.saveDocument(attachment.Name, verzendersmMail, path + "\\" + attachment.Name);

                                        // koppel docId aan contactId
                                        dh.koppelDocumentAanContact(verzendersmMail, docId);

                                    }

                                //return null;
                                }
                           // return null;
                            }
                            // het tijdstip van de laatste check wordt weer aangepast
                            account.lastCheked = DateTime.Now;
                            // het account is nagekeken en mag weer van de lijst verwijderd worden
                            //mag niet in foreach lus
                           // teVerwerkenAccounts.Remove(account);

                      
                        //    if (messages.Count() > 0)
                        //{
                        //    return "er zijn " + messages.Count() + " berichten gevonden";
                        //}
                        //else {
                        //    return "er zijn geen nieuwe brichten gevonden";
                        //}

                    }
                    
                    }
                    catch (Exception ex)
                    {
                    // als het inloggen mislukt wordt er een mail verstuurd
                    MailSender ms = new MailSender();
                    ms.sendInlogErrorMessage(account.beheerdersEmail);
                        
                        InvalidCredentialsException ive = new InvalidCredentialsException();
                        if (ex.GetType().Equals(ive.GetType()))
                        {
                        return null;
                        }
                        return "er is iest mis";
                        //throw;

                    }
           
                }
           
            return null;
        }

        // deze methde verwerkt de bestanden
        public static void wegschrijven(string folderName, string fileName, Stream bestand)
        {
           
            var dir = "c:\\Users\\thomas\\Desktop\\Eindwerk\\gmailBestandenASP\\" + folderName;
          
            // Als de folder niet bestaat, maak die dan eerst aan en schrijf het bestand weg
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
          
                CopyStream(bestand, dir + "\\" + fileName);

            

            }
            else
            {
                // anders, schrijf het bestand direct weg
                CopyStream(bestand, dir + "\\" + fileName);
            }
        }

        public static void CopyStream(Stream bestand, string path)
        {
            //http://stackoverflow.com/questions/411592/how-do-i-save-a-stream-to-a-file-in-c
            // poging op basis van verschillende posts in de link hierboven

            using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                bestand.CopyTo(fileStream);               
            }
        }
    }
}
