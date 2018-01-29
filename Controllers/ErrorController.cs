using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChamadosPro.Controllers
{
    public class ErrorController : Controller
    {
        [AllowAnonymous]
        public ActionResult Error404()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult Error500()
        {
            return View();
        }
    }
}