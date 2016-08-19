using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Faoma4.Controllers
{
    public class BeheerController : Controller
    {
        // GET: Beheer
        public ActionResult Index()
        {
            return View();
        }

        // GET: Login
        public ActionResult AccountBeheer()
        {
            //var vm = new LoginViewModel();
            return View();
        }
    }
}