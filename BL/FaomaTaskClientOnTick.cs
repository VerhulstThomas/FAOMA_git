using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Faoma4;
using System.Threading;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using Spire.Pdf;
using System.Text.RegularExpressions;

namespace BL
{
    public class FaomaTaskClientOnTick
    {

        private static FaomaModel db = new FaomaModel();
        //private static xmlHelpers.XmlIOHelper xm = new xmlHelpers.XmlIOHelper();
        //List<string> log = new List<string>();
        //private static List<string> logs = new List<string>();
        private static Timer timer = new Timer(OnTick, null, TimeSpan.FromMinutes(1), TimeSpan.FromSeconds(60));

        // na de functie OnTick staat de log ind e xml dus kan ze her gelezen en evrwerkt worden
        //List<string> teTonenLogging = logging();





        private static void OnTick(object state)
        {
            Boolean logChanged = false;

            string logtekst = "";
            //setTb();
            //int i =xm.Time()
            //Console.WriteLine("de tijd is " + Time());

            // zoek op basiss van het paswoord van het te beheren emailaccount het id van het serveraccount
            // dit kan aan het path geplakt worden om de documenten te kunenn vinden
            string key = Key();
            long id;
            serverAccount sa = (from x in db.serverAccount
                                where x.teBeherenEmail.Equals(key)
                                select x).FirstOrDefault();
            id = sa.id;
            ///long id = 20003;
            // poging met linq
            // List<string> linksVanOpTeHalenBestanden = null;
            //linksVanOpTeHalenBestanden.Add( db.Document.Select(x => x.link.ToString().Where(y=>(x.link.Contains(id.ToString()))&&(x.isOpgehaald.Equals("0")))).ToString());
            //string[] fileArray = Directory.GetFiles(@"c:\Dir\", "*.jpg", SearchOption.AllDirectories).Where(FileInfo.;

            //long id = 20003;
            //var linksVanOpTeHalenBestanden ;
            //var linksVanOpTeHalenBestanden=db.Document.SelectMany(x => x.link.ToString().Where(y => (x.link.Contains(id.ToString())) && (x.isOpgehaald.Equals("0")))).ToList().ToString();


            // lijst van alle links van de bestanden die nog niet zijn opgehaald en die de id van dit account in hun link hebben

            List<string> linksVanOpTeHalenBestanden = (from p in db.Document
                                                       where p.link.Contains(id.ToString())
                                                       where p.isOpgehaald.ToString().Equals("0")                //== someISIN
                                                       select p.link).ToList();
            // deze list is ook op te kuisen
            List<string> bestandsNamen = (from p in db.Document
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
                //logtekst += tmp + " ";
                opgekuisteBestandsNamen.Add(tmp);
            }
            List<string> tePrintenPaden = new List<string>();
            //List<string> opgekuisteLinks = new List<string>();
            if (!(linksVanOpTeHalenBestanden.Count == 0))
            {
                ///linksVanOpTeHalenBestanden.ToList<string>();

                List<string> padenVanOpTeHalenBestanden = new List<string>();

                foreach (string link in linksVanOpTeHalenBestanden)
                {
                    // correctie voor fout in database probleem: wat met spaties in titel van pdf
                    string tmp = string.Join("", link.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
                    // het bestand staat op basepath + tmp
                    string nieuwPad = BasePath() + "\\" + tmp;
                    padenVanOpTeHalenBestanden.Add(nieuwPad);
                }

                int counter = 0;
                foreach (string pad in padenVanOpTeHalenBestanden)
                {

                    if (File.Exists(pad))
                    {            //from    //to
                        File.Copy(@pad, @OpslagPath() + "\\" + opgekuisteBestandsNamen[counter]);
                        tePrintenPaden.Add(@OpslagPath() + "\\" + opgekuisteBestandsNamen[counter]);
                        //counter++;
                        //logtekst += "is opgehaald: ja ";
                    }
                    counter++;
                }
                //alle documenten worden als opgehaald gemarkeerd in de database
                foreach (var document in db.Document.Where(x => x.link.Contains(id.ToString()) && x.isOpgehaald.ToString().Equals("0")))
                {
                    document.isOpgehaald = 1;

                }
                try
                {
                    // fout pending dependencys : oplossing OnTick async maken en 
                    // hier wachten + async maken
                    //await db.SaveChangesAsync();
                    db.SaveChanges();
                }
                catch (Exception)
                {

                    throw;
                }


            }

            int counter2 = -1;
            // printen van al de files in de dir
            foreach (string tePrintenPad in tePrintenPaden)
            {
                counter2++;
                // tijd voor logging
                string t = DateTime.Now.ToLongTimeString();
                try
                {
                    // PrinterSettings.InstalledPrinters.g


                    print(tePrintenPad);
                    // counter2++
                    logtekst += opgekuisteBestandsNamen[counter2] + " is afgedrukt: ja " + t + "*";
                    //logPerFile(logtekst);
                    //log.Add("Printen geslaagd")

                }
                catch (Exception ex)
                {
                    // als het bestand niet geprint kan worden, moet het gekopieerd wordne naar een ander pad
                    File.Copy(@tePrintenPad, @UitwijkPad() + "\\" + opgekuisteBestandsNamen[counter2]);
                    logtekst += opgekuisteBestandsNamen[counter2] + " is afgedrukt: nee " + t + " zie " + UitwijkPad() + "*";
                    //logPerFile(logtekst);
                }
            }

            if (logtekst.Length > 0)
            {
                logPerFile(logtekst);
                logChanged = true;
            }

            // op het eidne moet de dir opslagPath leeggemaakt worden
            //clearDirectory(OpslagPath());


            //return null;

            db.SaveChanges();
            // should open a new instansce if log has changed
            if (logChanged == true)
            {
                //   MainWindow mainWindow = new MainWindow();
                // m//ainWindow.Show();
            }
            int minutes = Time();
            timer.Change(TimeSpan.FromMinutes(minutes), TimeSpan.FromMinutes(minutes));

        }
        public static int Time()
        {
            int aantalMinuten;
            XmlDocument doc = new XmlDocument();
            //doc.Load("Config.xml");
            doc.Load("C:\\Users\\thomas\\documents\\visual studio 2015\\Projects\\FaomaTaskClient\\FaomaTaskClient\\xmlHelpers\\Config.xml");

            XmlNode node = doc.DocumentElement.SelectSingleNode("/Settings/config/looptijd");
            string text = node.InnerText;
            return aantalMinuten = Convert.ToInt32(text);
        }

        private static string Key()
        {
            string keyValue;
            XmlDocument doc = new XmlDocument();
            //doc.Load("Config.xml");
            doc.Load("C:\\Users\\thomas\\documents\\visual studio 2015\\Projects\\FaomaTaskClient\\FaomaTaskClient\\xmlHelpers\\Config.xml");

            XmlNode node = doc.DocumentElement.SelectSingleNode("/Settings/config/email");
            //string text = node.InnerText;
            return keyValue = node.InnerText.ToString();
        }

        private static string BasePath()
        {
            XmlDocument doc = new XmlDocument();
            //doc.Load("Config.xml");
            doc.Load("C:\\Users\\thomas\\documents\\visual studio 2015\\Projects\\FaomaTaskClient\\FaomaTaskClient\\xmlHelpers\\Config.xml");

            XmlNode basePath = doc.DocumentElement.SelectSingleNode("/Settings/config/basePath");
            return basePath.InnerText.ToString();
        }

        private static string OpslagPath()
        {
            XmlDocument doc = new XmlDocument();
            //doc.Load("Config.xml");
            doc.Load("C:\\Users\\thomas\\documents\\visual studio 2015\\Projects\\FaomaTaskClient\\FaomaTaskClient\\xmlHelpers\\Config.xml");

            XmlNode opslagPad = doc.DocumentElement.SelectSingleNode("/Settings/config/opslagPath");
            return opslagPad.InnerText.ToString();
        }

        private static string UitwijkPad()
        {
            XmlDocument doc = new XmlDocument();
            //doc.Load("Config.xml");
            doc.Load("C:\\Users\\thomas\\documents\\visual studio 2015\\Projects\\FaomaTaskClient\\FaomaTaskClient\\xmlHelpers\\Config.xml");

            XmlNode uitwijkPad = doc.DocumentElement.SelectSingleNode("/Settings/config/uitwijkPad");
            //uitwijkPad.InnerText.ToString();
            return uitwijkPad.InnerText.ToString();
        }

        //deze methode vult de logxml
        private static void logPerFile(string log)
        {
            XmlDocument doc = new XmlDocument();
            //doc.Load("Config.xml");
            string pad = "C:\\Users\\thomas\\documents\\visual studio 2015\\Projects\\FaomaTaskClient\\FaomaTaskClient\\xmlHelpers\\Log.xml";

            //XmlNode lastLog = doc.DocumentElement.SelectSingleNode("/logging/lastlog");

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = Encoding.UTF8;
            settings.Indent = true;

            using (XmlWriter writer = XmlWriter.Create(pad, settings))
            {
                XElement root = new XElement("lastLog");
                //string hash = tbEmail.Text;
                //long hash2 = hash.GetHashCode();
                writer.WriteStartDocument();
                writer.WriteStartElement("logging");

                writer.WriteStartElement("lastLog");
                //root.Add(new XAttribute("lastLog", log));
                writer.WriteString(log);
                writer.WriteEndElement();



                //lastLog

            }
        }

        public List<string> logging()
        {
            XmlDocument doc = new XmlDocument();
            //doc.Load("Config.xml");
            doc.Load("C:\\Users\\thomas\\documents\\visual studio 2015\\Projects\\FaomaTaskClient\\FaomaTaskClient\\xmlHelpers\\Log.xml");
            XmlNode logPad = doc.DocumentElement.SelectSingleNode("/logging/lastLog");
            string originalString = logPad.InnerText.ToString();
            string[] parts = Regex.Split(originalString, @"(?<=[*])");
            return parts.ToList();
        }

        public void update()
        {
            //  lbLog
            List<string> teTonenLogging = logging();
            if (teTonenLogging.Count() > 0)
            {
                foreach (var item in teTonenLogging)
                {
                    //lbLog.Items.Add(item);
                }
            }

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
                        //foreach (var printer in PrinterSettings.InstalledPrinters)
                        //{
                        //    Console.WriteLine(printer);
                        //}

                        doc2 = doc;
                        //    doc.PrintDocument.Print();


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


