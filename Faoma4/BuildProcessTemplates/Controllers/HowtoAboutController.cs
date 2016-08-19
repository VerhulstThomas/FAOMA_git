using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Faoma4.Controllers
{
    public class HowtoAboutController : Controller
    {
        // GET: HowtoAbout
        public ActionResult Index()
        {
            return View();
        }
        // GET: info
        public ActionResult EmailVoorbereiden()
        {
            return View();
        }
    }
}