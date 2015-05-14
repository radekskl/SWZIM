using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SWZIM_WEBWeb.Controllers
{
    public class HomeController : Controller
    {
        //[SharePointContextFilter]
        public ActionResult Index()
        {
            //User spUser = null;

            //var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);

            //using (var clientContext = spContext.CreateUserClientContextForSPHost())
            //{
            //    if (clientContext != null)
            //    {
            //        spUser = clientContext.Web.CurrentUser;

            //        clientContext.Load(spUser, user => user.Title);

            //        clientContext.ExecuteQuery();

                    ViewBag.UserName = "Jan Kowalski";
            //    }
            //}

            //IQueryable<Test> results;
            //using (var context = new SWZIM_dbEntities())
            //{
            //    results = context.Test
            //        .Where(o => o.Ocena > 2);

            //    if (results != null)
            //        ViewBag.Entries = results;
            //}

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
