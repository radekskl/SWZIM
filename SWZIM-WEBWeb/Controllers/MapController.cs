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
            var list = db.LayoutElements.Where(le => le.Layers.Id == id && le.Latitude != null && le.Longitude != null && le.LayoutElementTypeId != 21).ToList()
                .Select(x => new
                {
                    id = x.Id,
                    lon = x.Longitude,
                    lat = x.Latitude,
                    name = x.Name,
                    desc = x.Description,
                    addedBy = x.UserId,
                    icon = x.LayoutElementTypes.MarkerIcons.Url,
                    points = x.LayoutElements1.Select(y => new { lat = y.Latitude, lon = y.Longitude }).ToList()
                }).ToList();
            foreach (var item in  db.LayoutElements.Where(le => le.Layers.Id == id && le.Latitude != null && le.Longitude != null && le.LayoutElements1.Count > 0))
	        {
		        foreach (var itx in item.LayoutElements1)
	            {
		            list.RemoveAll(x=> x.id == itx.Id);
	            }
	        }
            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllEventsForLayout(int id)
        {
            var events = db.Events.Where(e => e.Layers.Any(l => l.Id == id));
            // do poprawy
            return Json(events.Select(x => new { id = x.Id, lon = x.Longitude, lat = x.Latitude, name = x.Name, desc = x.Description,
                                    addedBy = x.AddedBy, status = x.Status, eType = x.EventTypes.Name, icon = x.EventTypes.MarkerIcons.Url
            }).ToList(), JsonRequestBehavior.AllowGet);
        }

    }
}