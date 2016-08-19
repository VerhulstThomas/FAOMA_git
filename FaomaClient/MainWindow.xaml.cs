using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using Spire.Pdf;
using System.Drawing.Printing;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using DAL;
using Faoma4;
using BL;

namespace FaomaClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {

        private static FaomaModel db = new FaomaModel();
        //private static xmlHelpers.XmlIOHelper xm = new xmlHelpers.XmlIOHelper();
        //List<string> log = new List<string>();
        //private static List<string> logs = new List<string>();
        private static Timer timer = new Timer(OnTick, null, TimeSpan.FromMinutes(1), TimeSpan.FromSeconds(60));

        // na de functie OnTick staat de log ind e xml dus kan ze her gelezen en evrwerkt worden
        //List<string> teTonenLogging = logging();


        public MainWindow()
        {
            InitializeComponent();


            XmlDocument doc = new XmlDocument();
            //doc.Load("f:\\FaomaSettings.xml");
            //string xmlPad = "C:\\Users\\thomas\\Documents\\visual studio 2015\\Projects\\FaomaClient\\FaomaClient\\FaomaSettings.xml";
            doc.Load("C:\\Users\\thomas\\documents\\visual studio 2015\\Projects\\FaomaTaskClient\\FaomaTaskClient\\xmlHelpers\\Config.xml");
            //doc.Load("Config.xml");
            XmlNode node = doc.DocumentElement.SelectSingleNode("/Settings/config/opslagPath");
            string text = node.InnerText;
            tbLocatie.Text = text;

            //XmlNode node2 = doc.DocumentElement.SelectSingleNode("/Settings/config/opslagPath");
            XmlNode node2 = doc.DocumentElement.SelectSingleNode("/Settings/config/looptijd");
            tbLoop.Text = node2.InnerText;

            XmlNode email = doc.DocumentElement.SelectSingleNode("/Settings/config/email");
            //tbLocatie.Text = 
            if (email.InnerText == "")
            {
                tbEmail.Text = "geef je te beheren eail in";
            }
            else
            {
                tbEmail.Text = email.InnerText;

            }
            XmlNode basePad = doc.DocumentElement.SelectSingleNode("/Settings/config/basePath");
            tbBasePad.Text = basePad.InnerText;

            XmlNode uitwijkPad = doc.DocumentElement.SelectSingleNode("/Settings/config/uitwijkPad");
            tbUitwijkPad.Text = uitwijkPad.InnerText;


            /////////////// Add old log when starting
            List<string> oldLog = logging();

            foreach (var item in oldLog)
            {
                lbLog.Items.Add(item);
            }



        }


        private static void OnTick(object state)
        {
            //counter++;
            //return "r";
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
                //MainWindow mainWindow = new MainWindow();
                //mainWindow.Show();
            }
            int minutes = Time();
            timer.Change(TimeSpan.FromMinutes(minutes), TimeSpan.FromMinutes(minutes));







        }

        

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // tbText.Text = counter.ToString();
        }

        private void setTb()
        {
            //tbText.Text = counter.ToString();


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
                    lbLog.Items.Add(item);
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
                        foreach (var printer in PrinterSettings.InstalledPrinters)
                        {
                            Console.WriteLine(printer);
                        }

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

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void lbLog_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btOpslaan_Click(object sender, RoutedEventArgs e)
        {



            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(tbLocatie.Text));

            string xmlPad = "C:\\Users\\thomas\\documents\\visual studio 2015\\Projects\\FaomaTaskClient\\FaomaTaskClient\\xmlHelpers\\Config.xml";
            // maak een xml document om de gegevens in op te slaan
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = Encoding.UTF8;
            settings.Indent = true;

            using (XmlWriter writer = XmlWriter.Create(xmlPad, settings))
            {
                //string hash = tbEmail.Text;
                //long hash2 = hash.GetHashCode();
                writer.WriteStartDocument();
                writer.WriteStartElement("Settings");

                writer.WriteStartElement("config");
                writer.WriteStartElement("email");
                writer.WriteString(tbEmail.Text);
                writer.WriteEndElement();

                writer.WriteStartElement("looptijd");
                writer.WriteString(tbLoop.Text);
                writer.WriteEndElement();

                writer.WriteStartElement("opslagPath");
                writer.WriteString(tbLocatie.Text);
                writer.WriteEndElement();

                writer.WriteStartElement("basePath");
                writer.WriteString(tbBasePad.Text);
                writer.WriteEndElement();

                writer.WriteStartElement("uitwijkPad");
                writer.WriteString(tbUitwijkPad.Text);
                writer.WriteEndElement();


                writer.WriteEndDocument();
                writer.Flush();
                writer.Close();
            }


            // lbLog.Items.Add()
            //listBox1.Items.Add(((TextBox)sender).Text);
        }

        private void btLog_Click(object sender, RoutedEventArgs e)
        {
            //  lbLog
            List<string> teTonenLogging = logging();
            if (teTonenLogging.Count() > 0)
            {
                foreach (var item in teTonenLogging)
                {
                    lbLog.Items.Add(item);
                }
            }
        }

        //public static void updateLog()
        //{
        //    List<string> oldLog = logging();

        //    foreach (var item in oldLog)
        //    {
        //        lbLog.Items.Add(item);
        //    }
        //}
    }
}

