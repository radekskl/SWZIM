using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SWZIM_WEBWeb.Controllers
{
    public class MapController : Controller
    {
        // GET: Map
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult FirstLook(int? height, int? width)
        {
            ViewData["height"] = height ?? 0;
            ViewData["width"] = width ?? 0;
            return View();
        }
    }
}