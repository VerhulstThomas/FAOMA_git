using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Faoma4;
using BL;

namespace Faoma4.Controllers
{
    public class DocumentsController : Controller
    {
        //private FaomaModel db = new FaomaModel();

        DocumentenService ds = new DocumentenService();
        // GET: Documents
        public ActionResult Index()
        {
            // List<Document> documentenLijstVoorEenGebruiker = new List<Document>();
            //List<serverAccount>
            // alleen de documenten die bij een bepaalde gebruiker horen mogen getoond worden
            // daarvoor kijken we in de tabel contact_document om op  basis van het id van de document het id van het contact op te halen
            // dan halen we op basis van het id van het contact het id van de 

            //var docLijst = db.Document.ToList();
            //var contacten = db.contact_document.ToList();
            //var contacten_server = db.serveraccount_contacten.ToList();

            //foreach (var item in contacten_server)
            //{
            //    try
            //    {
            //        contacten_server.Where(x => x.serverAccountId == tmpServerAccountsId.tmpServerAccountId);

            //    }
            //    catch (Exception)
            //    {

            //        throw;
            //    }
            //}


            //foreach (var document in docLijst)
            //{
            //    // zoek het contact dat bij het document hoord
            //    var contactdocument = contacten.Where(x => x.documentId == document.id).SingleOrDefault();
            //    try
            //    {
            //        contacten_server.Where(x => x.serverAccountId == tmpServerAccountsId.tmpServerAccountId);
            //    }
            //    catch (Exception)
            //    {

            //        throw;
            //    }
            //}


            //hier moet een selectie gebeuren, enkel die van het ingelogde account

            // var onbewerkteContactenLijst = db.serveraccount_contacten.ToList();
            //var onbewerkteDocumentenLijst = db.Document.ToList();
            //List<serveraccount_contacten> bewerkteContactenLijst = new List<serveraccount_contacten>();
            //List<contact_document> teBewerkteDocumentenContactenLijst = new List<contact_document>();

            //foreach (var contact in onbewerkteContactenLijst)
            //{
            //    if (!(contact.serverAccountId == tmpServerAccountsId.tmpServerAccountId))
            //    {

            //    }
            //    else { bewerkteContactenLijst.Add(contact); }
            //}

            //foreach (var item in teBewerkteDocumentenContactenLijst)
            //{
            //    if (item.contactId)
            //    {

            //    }
            //}

            //List<contacten> doorTeGevenContacten = new List<contacten>();


            //db.contacten.ToList();
            //foreach (var bewerktContact in bewerkteContactenLijst)
            //{
            //    long tmp = bewerktContact.contactenId;
            //    foreach (var contact in db.contacten.ToList())
            //    {
            //        if (tmp == contact.id)
            //        {
            //            doorTeGevenContacten.Add(contact);
            //        }
            //    }


            //}

            // return View(db.contacten.ToList());
            //return View(doorTeGevenContacten);

            //List<contacten> doorTeGevenContacten2 = cs.contactsFromThisUser();

            // deze fout opgelost door verwijderen vqn model document uit faoma4 
            List<Document> list = ds.ListOfDocuments();

            return View(list);
        }

        // GET: Documents/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = ds.FindDocumentWithId(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            return View(document);
        }

        // GET: Documents/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Documents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "naam,verzendersEmail,link,datum,isBetaald,isOpgehaald")] Document document)
        {
            if (ModelState.IsValid)
            {
                //db.Document.Add(document);
                //db.SaveChanges();

                // Bij toevoegen van document moet ook de tabel  contacten_document worden aangevuld
                // http://stackoverflow.com/questions/5212751/how-can-i-get-id-of-inserted-entity-in-entity-framework
                // haal de id op van het juist toegevoegde document
                //contact_document cd = new contact_document();
                //db.Entry(document).GetDatabaseValues();
                //long id = document.id;
                //cd.documentId = id;

                

                //// haal op basis van het emailadres het contact op die het document verstuurde  
                //string email = document.verzendersEmail;
                //contacten contact = (from x in db.contacten
                //             where x.E_mail == email
                //             select x).FirstOrDefault();

                // als er geen id is, (omdat de email niet geregistreerd is, zet default id)
                
                 //contact = null;
                 //string y =z.ToString();
                

       
                //if (contact == null )
                //{
                //    // default id = 99999
                //    // default veranderd naar 1 => database heeft geen 99999 id
                //    cd.contactId = 1;
                //}
                //else {
                //    cd.contactId = contact.id;
                //}

                // vul de tabel in
                //db.contact_document.Add(cd);
                //db.SaveChanges();

                ds.CreateDocument(document);

                return RedirectToAction("Index");
            }

            return View(document);
        }

        // GET: Documents/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = ds.FindDocumentWithId(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            return View(document);
        }

        // POST: Documents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "id,naam,verzendersEmail,link,datum,isBetaald,isOpgehaald")] Document document)
        public ActionResult Edit(long? id,[Bind(Include = "isBetaald, naam")] Document document)

        {
            if (ModelState.IsValid)
            {
                ds.EditDocument(document,id);
                //Document origineelDocument = db.Document.Find(id);
                //origineelDocument.isBetaald = document.isBetaald;
                ////db.Entry(document).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(document);
        }

        // GET: Documents/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = ds.FindDocumentWithId(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            return View(document);
        }

        // POST: Documents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            ds.DeleteDocument(id);
            //Document document = db.Document.Find(id);
            //db.Document.Remove(document);
            //db.SaveChanges();
            return RedirectToAction("Index");
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
