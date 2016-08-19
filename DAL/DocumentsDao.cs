using Faoma4;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DocumentsDao
    {
        private FaomaModel db = new FaomaModel();

        public IList<Document> ListOfDocuments()
        {
            
            List<Document> onbewerkteDocumentenLijst = db.Document.ToList();
            db.Dispose();
            //List<contact_document> teBewerkteDocumentenContactenLijst = new List<contact_document>();

            return onbewerkteDocumentenLijst;

        }

        public Document FindDocumentWithId(long? id)
        {
            try
            {
                Document document = db.Document.Find(id);
                db.Dispose();
                return document;
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public void EditDocument(Document document, long? id)
        {
            try
            {
                //db.Entry(contact).State = EntityState.Modified;
                //db.SaveChanges();
                //db.Dispose();
                Document origineelDocument = db.Document.Find(id);
                origineelDocument.isBetaald = document.isBetaald;
                db.Entry(document).State = EntityState.Modified;
                db.SaveChanges();
                db.Dispose();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void DeleteDocument(long id)
        {
            try
            {
                Document document = db.Document.Find(id);
                db.Document.Remove(document);
                db.SaveChanges();
                db.Dispose();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void CreateDocument(Document document)
        {
            try
            {
                db.Document.Add(document);
                db.SaveChanges();

                // Bij toevoegen van document moet ook de tabel  contacten_document worden aangevuld
                // http://stackoverflow.com/questions/5212751/how-can-i-get-id-of-inserted-entity-in-entity-framework
                // haal de id op van het juist toegevoegde document
                contact_document cd = new contact_document();
                db.Entry(document).GetDatabaseValues();
                long id = document.id;
                cd.documentId = id;

                // haal op basis van het emailadres het contact op die het document verstuurde  
                string email = document.verzendersEmail;
                contacten contact = (from x in db.contacten
                                     where x.E_mail == email
                                     select x).FirstOrDefault();

                if (contact == null)
                {
                    // default id = 99999
                    // default veranderd naar 1 => database heeft geen 99999 id
                    cd.contactId = 1;
                }
                else {
                    cd.contactId = contact.id;
                }

                // vul de tabel in
                db.contact_document.Add(cd);
                db.SaveChanges();

                db.Dispose();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
