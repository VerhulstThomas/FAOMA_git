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
using DAL;

namespace Faoma4.Controllers
{
    public class contactensController : Controller
    {
        //private FaomaModel db = new FaomaModel();
        ContactenService cs = new ContactenService();
        // GET: contactens
        public ActionResult Index()
        {
            //hier moet een selectie gebeuren, enkel die van het ingelogde account

            //var onbewerkteContactenLijst = db.serveraccount_contacten.ToList();
            //List<serveraccount_contacten> bewerkteContactenLijst = new List<serveraccount_contacten>();

            //foreach (var contact in onbewerkteContactenLijst)
            //{
            //    if (!(contact.serverAccountId == tmpServerAccountsId.tmpServerAccountId))
            //    {

            //    }
            //    else { bewerkteContactenLijst.Add(contact); }
            //}

            //List<contacten> doorTeGevenContacten = new List<contacten>();
            ////db.contacten.ToList();
            //foreach (var bewerktContact in bewerkteContactenLijst)
            //{
            //    long tmp =bewerktContact.contactenId;
            //    foreach (var contact in db.contacten.ToList())
            //    {
            //        if (tmp == contact.id)
            //        {
            //            doorTeGevenContacten.Add(contact);
            //        }
            //    }
                

            //}
            //ContactenService c = new ContactenService();
            List<contacten> doorTeGevenContacten2 = cs.contactsFromThisUser();
           // return View(db.contacten.ToList());
            return View(doorTeGevenContacten2);
        }

        // GET: contactens/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            contacten contacten = cs.FindContactWithId (id);
            if (contacten == null)
            {
                return HttpNotFound();
            }
            return View(contacten);
        }

        // GET: contactens/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: contactens/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Bedrijfsnaam,E_mail")] contacten contacten)
        {
            if (ModelState.IsValid)
            {
                ContactenService cs = new ContactenService();
                cs.createContact(contacten);
                //db.contacten.Add(contacten);
                //db.SaveChanges();

                //bij toevoegen contact moet ook de colom serveraccount_contacten worden aangevuld
                // http://stackoverflow.com/questions/5212751/how-can-i-get-id-of-inserted-entity-in-entity-framework
                //serveraccount_contacten sc = new serveraccount_contacten();
                //db.Entry(contacten).GetDatabaseValues();
                //long id = contacten.id;
                //sc.contactenId = id;
                //sc.serverAccountId = 3;// hardcoaded, moet eigenlijk aan login hangen
                //zoals hierboven, toegepast
                //sc.serverAccountId = tmpServerAccountsId.tmpServerAccountId;
                //db.serveraccount_contacten.Add(sc);
                //db.SaveChanges();
                ///
                return RedirectToAction("Index");
            }

            return View(contacten);
        }

        // GET: contactens/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            contacten contacten = cs.FindContactWithId(id);
            if (contacten == null)
            {
                return HttpNotFound();
            }
            return View(contacten);
        }

        // POST: contactens/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Bedrijfsnaam,E_mail")] contacten contacten)
        {
            if (ModelState.IsValid)
            {
                cs.EditContact(contacten);
                //db.Entry(contacten).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(contacten);
        }

        // GET: contactens/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            contacten contacten = cs.FindContactWithId(id);
            if (contacten == null)
            {
                return HttpNotFound();
            }
            return View(contacten);
        }

        // POST: contactens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            //contacten contacten = db.contacten.Find(id);
            //serveraccount_contacten
            // hier komt een foutmelding, in serverAccount_contacten moet deze ook verwijderd worden
            //db.contact_document()



            // serveraccount_contacten sc = (from x in db.serveraccount_contacten
            //                     where x.contactenId == contacten.id
            //                     select x).FirstOrDefault();

            //db.serveraccount_contacten.Remove(sc);



            //db.contacten.Remove(contacten);
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
