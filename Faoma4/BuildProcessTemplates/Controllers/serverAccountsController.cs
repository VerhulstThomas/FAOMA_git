using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Faoma4;
using System.Web.Security;
    using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;



namespace Faoma4.Controllers
{
    public class serverAccountsController : Controller
    {
        
        private FaomaModel db = new FaomaModel();

        // GET: serverAccounts
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            return View(db.serverAccount.ToList());
        }

        // GET: serverAccounts/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            serverAccount serverAccount = db.serverAccount.Find(id);
            if (serverAccount == null)
            {
                return HttpNotFound();
            }
            return View(serverAccount);
        }

        // GET: serverAccounts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: serverAccounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "username,password,teBeherenEmail,teBeherenEmailPW,looptijd,beheerdersEmail,lastCheked")] serverAccount serverAccount
        public ActionResult Create([Bind(Include = "teBeherenEmail,teBeherenEmailPW,looptijd,beheerdersEmail")] serverAccount serverAccount)
        {

            // omdat we het "serverAccount" pas maken na de registratie, kunnen we de koppeling tussen de twee 
            // maken door het id van de actieve gebruiker in te geven als username
            // validatie zal dan waarschijnlijk aangepast moeten worden
            //var currentUser = Membership.GetUser(User.Identity.Name);
            //string username = currentUser.UserName //** get UserName
            //Guid userID = (Guid)currentUser.ProviderUserKey; //** get user 
            //currentUser.ProviderUserKey;
            //string id=User.Identity.Name;

            //serverAccount.username = tempId.tmpId; 
            // voeg ook de datum toe als eerste waarde voor lastCheked
            //serverAccount.lastCheked = DateTime.Now;

            //Guid g = Guid.NewGuid();
            //serverAccount.password = g.ToString();
            //http://stackoverflow.com/questions/15333513/why-modelstate-isvalid-always-return-false-in-mvc
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            // username was een vereist veld in het model, dat gaf fout dat Modelstate false was

            if (ModelState.IsValid)
            {
                


                db.serverAccount.Add(serverAccount);
                db.SaveChanges();


                // hier doen we nel als bij de document controller
                // Bij toevoegen van document moet ook de tabel  contacten_document worden aangevuld
                // http://stackoverflow.com/questions/5212751/how-can-i-get-id-of-inserted-entity-in-entity-framework
                // haal de id op van het juist toegevoegde document
                //serverAccount cd;//= new serverAccount();
                db.Entry(serverAccount).GetDatabaseValues();
                long id = serverAccount.id;


                serverAccount sAccount = db.serverAccount.Find(id);
                //cd.id = id;
//cd = (from x in db.serverAccount
                      // x.id == id
                      //select x).FirstOrDefault();

                sAccount.username = tempId.tmpId;
                // voeg ook de datum toe als eerste waarde voor lastCheked
                sAccount.lastCheked = DateTime.Now;

                Guid g = Guid.NewGuid();
                sAccount.password = g.ToString();

                // vul de tabel 
                //db.serverAccount.Add(cd);
                db.SaveChanges();
              








                return RedirectToAction("Index");
            }

            return View(serverAccount);
        }

        // GET: serverAccounts/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            serverAccount serverAccount = db.serverAccount.Find(id);
            if (serverAccount == null)
            {
                return HttpNotFound();
            }
            return View(serverAccount);
        }

        // POST: serverAccounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(long? id,[Bind(Include = "teBeherenEmail,teBeherenEmailPW,looptijd,beheerdersEmail")] serverAccount serverAccount)
        {
            if (ModelState.IsValid)
            {
                serverAccount origineelServerAccount = db.serverAccount.Find(id);
                origineelServerAccount.teBeherenEmail = serverAccount.teBeherenEmail;
                origineelServerAccount.teBeherenEmailPW = serverAccount.teBeherenEmailPW;
                origineelServerAccount.looptijd = serverAccount.looptijd;
                origineelServerAccount.beheerdersEmail = serverAccount.beheerdersEmail;
               // db.Entry(serverAccount).State = EntityState.Modified;
                db.SaveChanges();

                // redirect verzet van Index naar Details om niet in de lijst tercht te komen,
                // maar enkel het eigen accoutn te zien
                // @Html.ActionLink("Details", "Details", new { id = tmpServerAccountsId.tmpServerAccountId }) |
                return RedirectToAction("Details", new { id = tmpServerAccountsId.tmpServerAccountId });
            }
            return View(serverAccount);
        }

        // GET: serverAccounts/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            serverAccount serverAccount = db.serverAccount.Find(id);
            if (serverAccount == null)
            {
                return HttpNotFound();
            }
            return View(serverAccount);
        }

        // POST: serverAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            serverAccount serverAccount = db.serverAccount.Find(id);
            db.serverAccount.Remove(serverAccount);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
