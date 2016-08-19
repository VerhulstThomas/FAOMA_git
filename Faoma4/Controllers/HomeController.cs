using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Faoma4.DAL;
using Faoma4.Helpers;


namespace Faoma4.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            // we halen het id op van de ingelogde gebruiker

             //var id =System.Web.HttpContext.Current.User.Identity.GetUserId();
            //var id=null;
            if ( (tempId.tmpId  == null) )
            {
                tempId.tmpId  = System.Web.HttpContext.Current.User.Identity.GetUserId();

                //serverAccount sa = new serverAccount
                // als er een ingelogde gebruiker is, ze hier het id in tmpId
                //tempId.tmpId = id;
                // en omdat: serverAccounts.userName= id ; // zie iid hierboven,
                // kunnen we van serverAccount ook het id halen
                ServerAccountServerAaccountsHelper ssah = new ServerAccountServerAaccountsHelper(tempId.tmpId);
                long serverAccountsId =ssah.serverAccountsId();
                // we vullen tmpServerAccountId om het op te kunnen halen als nodig
                tmpServerAccountsId.tmpServerAccountId = serverAccountsId;
            }
            // omgekeerde naamgeving
            if (IsStartup.isStartup == false)
            {
                // uitgezet gebeurd nu door service
                //Mailk.StartAllAccountLoops();
                IsStartup.isStartup = true;
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult AccountBeheer()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Edit(string id)
        {
            return RedirectToAction("Edit", "ServerAccounts",id);

        //    //return View();
      }
        public ActionResult EmailVoorbereiden()
        {
            return View();
        }
    }
}