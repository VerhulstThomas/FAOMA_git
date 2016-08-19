using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using AE.Net.Mail;
using S22.Imap;
using System.Net.Mail;
using System.IO;
using System.Security.Permissions;
using Faoma4.DAL;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Xml;
using Spire.Pdf;

namespace Faoma4
{
    public class Mailk
    {
        private static FaomaModel db = new FaomaModel();
        // laad alle accounts
        private static List<serverAccount> accounts = db.serverAccount.ToList();
        static string bericht = "";
        // in deze lijst komen de accounts die volgends hun tijd aan de beurt zijn om nagekeken te worden
        // gaf fout De objectverwijzing is niet op een exemplaar van een object ingesteld.
        // omdat het niet geinstantieerds was
        //private static List<serverAccount> teVerwerkenAccounts = new List<serverAccount>();
        private static List<serverAccount> teVerwerkenAccounts = accounts;

        // goede documentatie http://smiley22.github.io/S22.Imap/Documentation/
        //http://smiley22.github.io/S22.Imap/Documentation/ de goede?
        // Connect to the IMAP server. The 'true' parameter specifies to use SSL
        // which is important (for Gmail at least)
        //string username= "tfeindwerkcvo@gmail.com";
        //string password= "TFeindwerkCVO1";

        public string connectionTest()
        {
            try
            {
                string hostname = "imap.gmail.com",
                  username = "tfeindwerkcvo@gmail.com", password = "TFeindwerkCVO1";
                // The default port for IMAP over SSL is 993.
                using (ImapClient client = new ImapClient(hostname, 993, username, password, AuthMethod.Login, true))
                {
                    return"We are connected!";
                }
            }
            catch (Exception)
            {

                throw;
            }
           
        }
        // deze methode wordt voorlopig opgeroepen ind e create van document
        public static string getAllUnseenMails(){


            //string hostname = "imap.gmail.com",
            //  username = "tfeindwerkcvo@gmail.com", password = "TFeindwerkCVO1";

            //do
            //{
                foreach (var account in teVerwerkenAccounts)
                {
                    // deze moet in het path verwerkt worden
                    string accountId =  Convert.ToString(account.id);

                    try
                    {
                       // using (ImapClient client = new ImapClient("imap.gmail.com", 993, "tfeindwerkcvo@gmail.com", "TFeindwerkCVO1", AuthMethod.Login, true))
                        //using (ImapClient client = new ImapClient("imap.gmail.com", 993, account.beheerdersEmail, account.teBeherenEmailPW, AuthMethod.Login, true))
                    using (ImapClient client = new ImapClient("imap.gmail.com", 993, account.teBeherenEmail, account.teBeherenEmailPW, AuthMethod.Login, true))

                    {
                        // Returns a collection of identifiers of all mails matching the specified search criteria.
                        IEnumerable<uint> uids = client.Search(SearchCondition.Unseen());
                            // Download mail messages from the default mailbox. also add System.Net.Mail;
                            IEnumerable<MailMessage> messages = client.GetMessages(uids);


                            foreach (var email in messages)
                            {

                                string path = accountId+"\\";
                                //long id;
                                //AttachmentCollection atc = new AttachmentCollection;
                                var itemAttachments = email.Attachments;

                                if (itemAttachments.Count > 0)
                                {

                                    DocumentHelper dh = new DocumentHelper();
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


                                }

                            }
                            // het tijdstip van de laatste check wordt weer aangepast
                            account.lastCheked = DateTime.Now;
                            // het account is nagekeken en mag weer van de lijst verwijderd worden
                            teVerwerkenAccounts.Remove(account);

                            if (messages.Count() > 0)
                            {
                                return "er zijn " + messages.Count() + " berichten gevonden";
                            }
                            else {
                                return "er zijn geen nieuwe brichten gevonden";
                            }
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        InvalidCredentialsException ive = new InvalidCredentialsException();
                        if (ex.GetType().Equals(ive.GetType()))
                        {

                        }
                        //return "er is iest mis";
                        //throw;

                    }
                }
            return "";
            //} while (true);

        }

        // deze methode krijgt een foldernaam (default of een bedrijfsnaam) en een  datastream (het bestand)
        // als de directory nog niet bestaat wordt ze gecreert
        // het bestand wordt in de juiste folder weggeschreven
        public static void wegschrijven(string folderName,string fileName, Stream bestand)
        {
            //http://stackoverflow.com/questions/30013448/c-sharp-create-dir-in-appdata
            // poging 1: string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), folderName);
            // poging 2: string path = Path.Combine(Environment.GetFolderPath(Environment.s ), folderName);
            // http://stackoverflow.com/questions/2574587/writing-data-to-app-data
            //string path = HttpContext.Current.Server.MapPath(@"~/App_Data/");
            // defaultAppPool toevoegen https://www.iis.net/learn/manage/configuring-security/application-pool-identities
            //var path = @"App_Data"+ folderName;


            var dir = "c:\\Users\\thomas\\Desktop\\Eindwerk\\gmailBestandenASP\\"+folderName;
                       // C:\Users\thomas\Desktop\Eindwerk\gmailBestandenASP
            //var path = Faoma4.UserAppDataPath

            // Als de folder niet bestaat, maak die dan eerst aan en schrijf het bestand weg
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
                //bestand.Seek(0, SeekOrigin.Begin);
                //myOtherObject.InputStream.Seek(0, SeekOrigin.Begin);
                // myOtherObject.InputStream.CopyTo(fileStream);

                //Stream fs = new FileStream(path, );
                //FileStream fs = new FileStream();
                //bestand.CopyTo(bestand);
                // fileStream.Close();
               // Stream output = new Stream();
                //CopyStream2(bestand, Stream output =null);
                CopyStream(bestand, dir +"\\"+fileName);

                // TODO moet ik hier bestand ook nog sluiten en disposen?

            }
            else
            {
                // anders, schrijf het bestand direct weg
                CopyStream(bestand, dir + "\\" + fileName);
            }
        }

        private static void CopyStream2(Stream bestand, Stream stream, object output)
        {
            throw new NotImplementedException();
        }

        public static void CopyStream(Stream bestand, string path)
        {
            //http://stackoverflow.com/questions/411592/how-do-i-save-a-stream-to-a-file-in-c
            // poging op basis van verschillende posts in d elink hierboven
            
             //FileIOPermission f2 = new FileIOPermission(FileIOPermissionAccess.AllAccess, "C:\\Users\\thomas\\Desktop\\Eindwerk\\gmailBestandenASP\\test");
            using (var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                
                
                //bestand.Seek(0, SeekOrigin.Begin);
                bestand.CopyTo(fileStream);
                //fileStream.Dispose();
                //fileStream.Close();
            }
        }
        //http://stackoverflow.com/questions/411592/how-do-i-save-a-stream-to-a-file-in-c
        public static void CopyStream2(Stream input, Stream output)
        {
            byte[] buffer = new byte[8 * 1024];
            int len;
            while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, len);
            }
        }



        public void CopyStream3(Stream stream, string destPath)
        {
            using (var fileStream = new FileStream(destPath, FileMode.Create, FileAccess.Write))
            {
                stream.CopyTo(fileStream);
            }
        }


        public static string StartAllAccountLoops()
        {
            return "niet gebruikt";

            //// info over timercallback
            ////https://msdn.microsoft.com/en-us/library/system.threading.timercallback%28v=vs.110%29.aspx
            //// haal een lijst op met alle geregistreerde accounts
            //// om de 500 minuten refrech voor het ophalen van niewe accounts
            //List<serverAccount> l = new List<serverAccount>();
            ////string x = "r";
            ////start parallelle loops
            //AutoResetEvent autoEvent = new AutoResetEvent(false);

            ////TimerCallback tcb = tester();
            //TestCallBack t = new TestCallBack(accounts);
            //TimerCallback tecb = t.CheckStatus;
            //// na een minuut, elke minuut?
            //Timer stateTimer = new Timer(tecb, autoEvent, 1000, 1000);
            ////StatusChecker statusChecker = new StatusChecker(10);
            ////List<serverAccount> teVerwerkenAccounts;
            ////accounts = l;
            //int counter = 1;
            
            //teVerwerkenAccounts= t.teVerwerkenAccounts;
            //getAllUnseenMails();
            //return bericht="we zijn hier";
            ////do { 
            //////int startin = 60 - DateTime.Now.Second;
            //TestCallBack tc = new TestCallBack(int c =3)
            ////// kijk elke minuut na welke account getchekcked moeten worden
            ////// als startin >500 zet terug op 0
            ////int startin = 60 - DateTime.Now.Second;
            ////if (counter<501)
            ////{
            ////        counter++;
            ////        //http://stackoverflow.com/questions/1329900/net-event-every-minute-on-the-minute-is-a-timer-the-best-option
            ////        var t = new System.Threading.Timer(tester(accounts),
            ////         null, startin * 1000, 60000);
            ////}
            ////else { counter = 0; }
                //int counter = 0;
                //////if (counter <500)
                //////{



                //////    foreach (serverAccount account in accounts)
                //////    {
                //////        int looptijd = account.looptijd;
                //////        string paswoord = account.teBeherenEmailPW;
                //////        string teBeherenEmail = account.teBeherenEmail;

                //////        serverAccount tmp = account; // Make temporary
                //////        Thread myThread = new Thread(() => initializeLoop(teBeherenEmail, paswoord, looptijd));
                //////        myThread.Start();
                //////    }
                //////}

            //} while (true);

            ////Parallel.ForEach(accounts, account =>
            ////{
            ////    int looptijd = account.looptijd;
            ////    string paswoord = account.teBeherenEmailPW;
            ////    string teBeherenEmail = account.teBeherenEmail;

            ////    while (true)
            ////    {
            ////        Thread.Sleep(60 * looptijd * 1000);
            ////        //Console.WriteLine("*** calling MyMethod *** ");
            ////        //MyMethod();
            ////        x= initializeLoop(teBeherenEmail, paswoord);

            ////    }


            ////    //Trend trend = SampleUtilities.Fit(account.Balance);
            ////    //double prediction = trend.Predict(
            ////    //                 account.Balance.Length + NumberOfMonths);
            ////    //account.ParPrediction = prediction;
            ////    //account.ParWarning = prediction < account.Overdraft;
            ////})
            ////;
            //return x;
        }

       

        //private static TimerCallback tester(List<serverAccount> accounts,AutoResetEvent autoevent)
        private static string tester( )

        {
            // kijkt voor elk account of het tijd is om de mailbox te tchecken
            // als dat het geval is wordt het account aan de lijst "teVerwerkenAccounts" toegevoegd
            
            try
            {

           
            foreach (serverAccount account in accounts)
            {
                // zet lastchecked van accounts waar dat null is
                if (account.lastCheked== null)
                {
                    account.lastCheked = DateTime.Now.AddMinutes(-10);
                }

                int looptijd = account.looptijd;
                //DateTime dt = new DateTime();

                DateTime dt = (DateTime)account.lastCheked;
                dt.AddMinutes(looptijd);

                if (DateTime.Now> dt)
                {
                    teVerwerkenAccounts.Add(account);
                }

               // if (looptijd % teller ==0)
               // {
                    //account.lastCheked
                //    teVerwerkenAccounts.Add(account);
                //}
                //string paswoord = account.teBeherenEmailPW;
                //string teBeherenEmail = account.teBeherenEmail;

                //serverAccount tmp = account; // Make temporary
                // ZO START EEN NIEUWE TREATH   
                //Thread myThread = new Thread(() => initializeLoop(teBeherenEmail, paswoord, looptijd));
                //myThread.Start();
            }
                bericht = "accounts zijn opgestart";
                //autoevent.Set();
                
            }
            catch (Exception e)
            {
                //tNotImplementedException("opgestarrt");
                throw new NotImplementedException("opgestarrt",e);
                //throw e("dc")
            }
            return bericht;
            //return "accounts zijn ";
            // notimplemetedexception not handelded by user => text in gezet
            //throw new NotImplementedException("opgestarrt");
        }

        public static string initializeLoop(string teBeherenEmail, string teBeherenEmailPaswoord, int loopTijd)
        {
            // op basis van de lijst "teVerwerkenAccounts gaat deze methode de mailboxen controlleren
            Stopwatch sw = new Stopwatch();
            //sw.Start();
            bool s = true;
            //while (sw.Elapsed < TimeSpan.FromSeconds(loopTijd * 60
            do
            {
                sw.Start();
                //sw.IsRunning
                //do something
                //string hostname = "imap.gmail.com",
                //  username = "tfeindwerkcvo@gmail.com", password = "TFeindwerkCVO1";
                try
                {




                    using (ImapClient client = new ImapClient("imap.gmail.com", 993, teBeherenEmail, teBeherenEmailPaswoord, AuthMethod.Login, true))
                    {
                        // Returns a collection of identifiers of all mails matching the specified search criteria.
                        IEnumerable<uint> uids = client.Search(SearchCondition.Unseen());
                        // Download mail messages from the default mailbox. also add System.Net.Mail;
                        IEnumerable<MailMessage> messages = client.GetMessages(uids);


                        foreach (var email in messages)
                        {

                            string path = "";
                            long id;
                            //AttachmentCollection atc = new AttachmentCollection;
                            var itemAttachments = email.Attachments;

                            if (itemAttachments.Count > 0)
                            {

                                DocumentHelper dh = new DocumentHelper();
                                //bepaal het pat (bedrijfsnaam of default)
                                // string emailt = email.Sender.ToString();// sender is null?
                                string verzendersmMail = email.From.Address.ToString();
                                //string t = "testen";
                                path = dh.getPathName(verzendersmMail);

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


                            }
                            //long lt =Convert.ToInt64(loopTijd);
                            int d = Convert.ToInt32( sw.ElapsedMilliseconds);
                            int slaapTijd =(loopTijd *1000) -d*60;
                            //long millisec = 1000;
                            //long sec = 60;
                            Thread.Sleep(slaapTijd);

                        }

                        if (messages.Count() > 0)
                        {
                            return "er zijn " + messages.Count() + " berichten gevonden";
                        }
                        else {
                            return "er zijn geen nieuwe brichten gevonden";
                        }
                        //Thread.Sleep(2);
                    }
                    //      return "";
                    //Thread.Sleep(2);

                }
                catch (Exception ex)
                {
                    InvalidCredentialsException ive = new InvalidCredentialsException();
                    if (ex.GetType().Equals(ive.GetType()))
                    {

                    }
                    return "er is iest mis";
                    //throw;
                }
                
            }
            while (s);
            //return"";
        }
        public static List<string>testLinq()
        {
            long id = 20003;
            //var linksVanOpTeHalenBestanden ;
            //var linksVanOpTeHalenBestanden=db.Document.SelectMany(x => x.link.ToString().Where(y => (x.link.Contains(id.ToString())) && (x.isOpgehaald.Equals("0")))).ToList().ToString();
            


           List<string> linksVanOpTeHalenBestanden = (from p in db.Document
                                        where p.link.Contains(id.ToString())
                                        where p.isOpgehaald.ToString().Equals("0")                //== someISIN
                                            select p.link).ToList();
            // deze list is ook op te kuisen
            List<string>bestandsNamen  = (from p in db.Document
                                          where p.link.Contains(id.ToString())
                                          where p.isOpgehaald.ToString().Equals("0")                //== someISIN
                                          select p.naam).ToList();
            // voor kopie pasten
            List<string> opgekuisteBestandsNamen = new List<string>();

            foreach (string bestandsNaam in bestandsNamen)
            {
                // om bestandsnaam zeker uniek te maken guid toevoegen
                //Guid guid = new Guid();
                Guid g;
                g = Guid.NewGuid();
                string tmp = g.ToString();
                tmp += string.Join("", bestandsNaam.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
                
                opgekuisteBestandsNamen.Add(tmp);
            }

            List<string> tePrintenPaden = new List<string>();
            //List<string> opgekuisteLinks = new List<string>();
            if (!(linksVanOpTeHalenBestanden.Count==0))
            {
                //return linksVanOpTeHalenBestanden.ToList<string>();
                List<string> padenVanOpTeHalenBestanden = new List<string>();
                foreach (string link in linksVanOpTeHalenBestanden)
                {
                    string tmp = string.Join("", link.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
                    // tmp bewaren voor gebruik bij copy paste
                    //opgekuisteLinks.Add(tmp);
                    // het bestand staat op basepath + tmp
                    string nieuwPad = BasePath() + "\\" + tmp;
                    padenVanOpTeHalenBestanden.Add(nieuwPad);
                }
                
                int counter = 0;
                foreach (string pad in padenVanOpTeHalenBestanden)
                {
                    
                    if (File.Exists(pad))
                    {            //from    //to
                        File.Copy(@pad, @OpslagPath()+"\\"+opgekuisteBestandsNamen[counter]);
                        tePrintenPaden.Add(@OpslagPath() + "\\" + opgekuisteBestandsNamen[counter]);
                        //counter++;
                    }
                    counter++;
                }
                //alle documenten wordne als opgehaald gemarkeerd in de database
                foreach (var document in db.Document.Where(x => x.link.Contains(id.ToString())&& x.isOpgehaald.ToString().Equals("0")))
                {
                    document.isOpgehaald = 1;
                    //some.status = true;
                }
               
                //db.SubmitChanges();
                db.SaveChanges();


               

            }
            int counter2 = -1;
            // printen van al de files in de dir
            foreach (string tePrintenPad in tePrintenPaden)
            {
                counter2++;
                try
                {
                    // tijdelijk om aantal prints te beparen
                   // if (counter2<2)
                   // {
                        print(tePrintenPad);
                       // counter2++;
                   // }
                    //counter2++;
                 

                }
                catch (Exception)
                {
                    // als het bestand niet geprint kan worden, moet het gekopieerd wordne naar een ander pad
                    File.Copy(@tePrintenPad, @UitwijkPad() + "\\" + opgekuisteBestandsNamen[counter2]);
                    //counter2++;
                    //throw null;
                }
               
                
            }

            // op het eidne moet de dir opslagPath leeggemaakt worden
            clearDirectory(OpslagPath());

            return null;
        }
        // varaibel maken van pad waar xml staat moet nog gebeuren
        private static string BasePath()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("f:\\FaomaSettings.xml");
            XmlNode basePath = doc.DocumentElement.SelectSingleNode("/Settings/config/basePath");
            return basePath.InnerText.ToString();
        }
        private static string OpslagPath()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("f:\\FaomaSettings.xml");
            XmlNode opslagPad = doc.DocumentElement.SelectSingleNode("/Settings/config/opslagPath");
            return opslagPad.InnerText.ToString();
        }


        private static string UitwijkPad()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("f:\\FaomaSettings.xml");
            XmlNode uitwijkPad = doc.DocumentElement.SelectSingleNode("/Settings/config/uitwijkPad");
            return uitwijkPad.InnerText.ToString();
        }

        public static void print(string path)
        {
            // http://www.e-iceblue.com/Tutorials/Spire.PDF/Spire.PDF-Program-Guide/Document-Operation/How-to-print-PDF-files-in-C.html
            // free for commercial use http://www.e-iceblue.com/Introduce/free-pdf-component.html#.Vy74E4SLTIU
         

            try
            {
                
                if (path.EndsWith(".pdf"))
                {
                    PdfDocument doc = new PdfDocument();
                   
                    doc.LoadFromFile(path);
                    // met deze link kan het mss nog beter http://www.e-iceblue.com/Tutorials/Spire.PDF/Spire.PDF-Program-Guide/Set-Transparency-for-PDF-Image-in-C-VB.NET.html
                    PdfDocument doc2 = new PdfDocument();
                    try
                    {
                        doc2 = doc;
                        doc.PrintDocument.Print();

                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
               
            }
            catch (Exception)
            {

                throw;
            }
           

        }

        public static void clearDirectory(string pad)
        {
            DirectoryInfo dir = new DirectoryInfo(pad);

            foreach (FileInfo file in dir.GetFiles())
            {
                try
                {
                    file.Delete();

                }
                catch (Exception)
                {

                   // throw;
                }
            }
            foreach (DirectoryInfo dirInfo in dir.GetDirectories())
            {
                try
                {
                    dirInfo.Delete(true);

                }
                catch (Exception)
                {

                    //throw;
                }
            }

        }
    }
    
}