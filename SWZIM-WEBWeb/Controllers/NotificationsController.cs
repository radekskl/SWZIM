using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NotificationsHelper = SWZIM_WEBWeb.Helpers.NotificationsHelper;

namespace SWZIM_WEBWeb.Controllers
{
    public class NotificationsController : Controller
    {
        // GET: Notifications
        public ActionResult Index(Nullable<int> id)
        {
            if (id.HasValue)
                return View(NotificationsHelper.GetNotificationById(id.Value)); //pojedyncza notyfikacja
            return View(NotificationsHelper.GetNotifications()); //pokaz wszystkie
        }

    }
}