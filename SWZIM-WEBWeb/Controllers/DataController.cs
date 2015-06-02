using SWZIM_WEBWeb.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SWZIM_WEBWeb.Controllers
{
    public class DataController : Controller
    {
        // GET: Data
        public ActionResult Index()
        {
            return View();
        }

        // GET: Import
        [SharePointContextFilter]
        public ActionResult Import()
        {
            List<Layers> list = LayersHelper.GetLayersList(ViewBag.UserId);
            list.Add(new Layers(){ Id = 0, Name = "Nowa"});

            ViewBag.LayerId = new SelectList(list.OrderBy(x => x.Id).AsEnumerable(), "Id", "Name");

            return View();
        }

        // POST: Import
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SharePointContextFilter]
        public ActionResult Import(SWZIM_WEBWeb.Models.HelperViewModels.ImportExportFileViewModel model)
        {
            List<Layers> list = LayersHelper.GetLayersList(ViewBag.UserId);
            list.Add(new Layers() { Id = 0, Name = "Nowa" });

            ViewBag.LayerId = new SelectList(list.OrderBy(x => x.Id).AsEnumerable(), "Id", "Name");
            ViewBag.Results = "tutaj co wylapiemy z z pliku";
            return View();
        }

        // GET: Export
        [SharePointContextFilter]
        public ActionResult Export()
        {
            return View();
        }
    }
}