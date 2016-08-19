using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BL
{
    public class XmlIOHelper
    {

        public static int Time()
        {
            int aantalMinuten;
            XmlDocument doc = new XmlDocument();
            doc.Load("Config.xml");
            XmlNode node = doc.DocumentElement.SelectSingleNode("/Settings/config/looptijd");
            string text = node.InnerText;
            return aantalMinuten = Convert.ToInt32(text);
        }

        private static string Key()
        {
            string keyValue;
            XmlDocument doc = new XmlDocument();
            doc.Load("Config.xml");
            XmlNode node = doc.DocumentElement.SelectSingleNode("/Settings/config/email");
            //string text = node.InnerText;
            return keyValue = node.InnerText.ToString();
        }

        private static string BasePath()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("Config.xml");
            XmlNode basePath = doc.DocumentElement.SelectSingleNode("/Settings/config/basePath");
            return basePath.InnerText.ToString();
        }

        private static string OpslagPath()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("Config.xml");
            XmlNode opslagPad = doc.DocumentElement.SelectSingleNode("/Settings/config/opslagPath");
            return opslagPad.InnerText.ToString();
        }

        private static string UitwijkPad()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("Config.xml");
            XmlNode uitwijkPad = doc.DocumentElement.SelectSingleNode("/Settings/config/uitwijkPad");
            return uitwijkPad.InnerText.ToString();
        }
    }
}
