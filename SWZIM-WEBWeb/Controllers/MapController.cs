using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SWZIM_WEBWeb.Controllers
{
    public class MapController : Controller
    {

        private SWZIM_dbEntities db = new SWZIM_dbEntities();
        // GET: Map
        [SharePointContextFilter]
        public ActionResult Index()
        {
            return View();
        }

        [SharePointContextFilter]
        public ActionResult Layout(int? id)
        {
            if (id != null)
            {
                return View();
            }
            else
                return RedirectToAction("Error", "Home");
        }

        public JsonResult GetAllElementsForLayout(int id)
        {
            return Json(db.LayoutElements.Where(le => le.LayersId == id).ToList());
        }

        public JsonResult GetAllEventsForLayout(int id)
        {
            var groups = db.Groups.Where(g => g.Layers.Any(l => l.Id == id));
            // do poprawy
            return Json(groups);
        }

    }
}