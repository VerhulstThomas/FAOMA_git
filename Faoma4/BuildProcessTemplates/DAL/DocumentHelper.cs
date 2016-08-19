using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Faoma4.DAL
{
    public class DocumentHelper
    {
        private FaomaModel db = new FaomaModel();

        // deze methode haalt op basis van het emailadres het contactId op,
        //als er geen is, komt er "1" als default

        public long getContactId(string email)
        {
            // DBcontext db = enw d
            // Document document = new Document();
            // Bij toevoegen van document moet ook de tabel  contacten_document worden aangevuld
            // http://stackoverflow.com/questions/5212751/how-can-i-get-id-of-inserted-entity-in-entity-framework
            // haal de id op van het juist toegevoegde document
            contact_document cd = new contact_document();

            // db.Entry(document).GetDatabaseValues();
            // long id = document.id;
            // cd.documentId = id;



            // haal op basis van het emailadres het contact op die het document verstuurde  
            //string email = document.verzendersEmail;
            contacten contact = (from x in db.contacten
                                 where x.E_mail == email
                                 select x).FirstOrDefault();

            // als er geen id is, (omdat de email niet geregistreerd is, zet default id)


            if (contact == null)
            {
                // default veranderd naar 1 => database heeft geen 99999 
                // contact niet gekend
                return cd.contactId = 1;
            }
            else {
                return cd.contactId = contact.id;
            }

            // vul de tabel in
            // db.contact_document.Add(cd);
            // db.SaveChanges();
        }

        // nodig voor te bepalen in welke folder het document moet worden opgeslagen
        public string getPathName(string email)
        {
            contacten contact = (from x in db.contacten
                                 where x.E_mail == email
                                 select x).FirstOrDefault();

            if (contact == null)
            {
                // default veranderd naar 1 => database heeft geen 99999 
                // contact niet gekend
                return "default";
            }
            else {
                return contact.Bedrijfsnaam;
            }
        }

        public long saveDocument(string naam, string email, string link)
        {

            Document document = new Document();
            document.naam = naam;
            document.verzendersEmail = email;
            document.link = link;
            document.isBetaald = 0;
            document.isOpgehaald = 0;
            document.datum = DateTime.Now;

            // save in database
            db.Document.Add(document);
            db.SaveChanges();

            // haal id op om door te geven aan tabel contact_document
            db.Entry(document).GetDatabaseValues();
            long documentId = document.id;
            return documentId;
            //contact_document cd = new contact_document();

            //cd.documentId = id;
        }

        public void createContact_Document(long id)
        {
            contact_document cd = new contact_document();

            cd.documentId = id;
        }

        // deze methode zoekt op basis van de email de id van de leverancier en koppelt dat aan de documentId 
        // in de tabel contact_document. 1 is de default voor als er met het emailadres geen id gevonden kan worden 
        public void koppelDocumentAanContact(string email, long documentId)
        {
            // haal de id op van het juist toegevoegde document
            contact_document cd = new contact_document();
            // koppel het doorgekregen documentid
            cd.documentId = documentId;

            // haal op basis van het emailadres het contact op die het document verstuurde  
            //string email = document.verzendersEmail;
            contacten contact = (from x in db.contacten
                                 where x.E_mail == email
                                 select x).FirstOrDefault();

            // als er geen id is, (omdat de email niet geregistreerd is, zet default id)
            if (contact == null)
            {
                // default veranderd naar 1 => database heeft geen 99999 
                // contact niet gekend
                cd.contactId = 1;
            }
            else {
                cd.contactId = contact.id;
            }
            // vul de tabel in
            db.contact_document.Add(cd);
            db.SaveChanges();
            // ik was de dispose vergeten en FaomaTasclient gaf daar een foutmelding op als het bestand wordt opgehaald
            db.Dispose();
        }
    }
}