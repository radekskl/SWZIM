using Microsoft.SharePoint.Client;
using SWZIM_WEBWeb.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SWZIM_WEBWeb.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        [SharePointContextFilter]
        public ActionResult Index()
        {
            var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);
            using (var clientContext = spContext.CreateAppOnlyClientContextForSPAppWeb())
            {
                GroupsHelper.CreateGroups(clientContext);
            }
            return View();
        }
    }
}