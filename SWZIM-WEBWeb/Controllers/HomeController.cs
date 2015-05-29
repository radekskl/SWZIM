using Microsoft.SharePoint.Client;
using SWZIM_WEBWeb.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SWZIM_WEBWeb.Controllers
{
    public class HomeController : Controller
    {
        [SharePointContextFilter]
        public ActionResult Index()
        {
            var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);

            using (var clientContext = spContext.CreateUserClientContextForSPHost())
            {
                if (clientContext != null)
                {
                   // sharepoint CSOM here if needed
                }
            }

            return View();
        }

        [SharePointContextFilter]
        public ActionResult Search(string input)
        {
            ViewBag.TextToFind = input;
            ViewData["events"] = EventsHelper.GetSearchForUser(ViewBag.UserId, input);
            ViewData["layoutElements"] = LayersHelper.GetSearchForUser(ViewBag.UserId, input);

            return View();
        }

        [SharePointContextFilter]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [SharePointContextFilter]
        public ActionResult About()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [SharePointContextFilter]
        public ActionResult Controls()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [SharePointContextFilter]
        public ActionResult Error()
        {
            ViewBag.Message = "Wystąpił błąd.";

            return View();
        }
    }
}
