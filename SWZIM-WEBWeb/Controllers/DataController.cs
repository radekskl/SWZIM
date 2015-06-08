using SWZIM_WEBWeb.Helpers;
using SWZIM_WEBWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SWZIM_WEBWeb.Controllers
{
    public class DataController : Controller
    {
        private SWZIM_dbEntities db = new SWZIM_dbEntities();

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
            List<XMIHelperModel.ProfilCADModel> listFromFile = DataHelper.ParseXMI(model.File);

            int choosedLayer = 1; // główny layer

            if (model.LayerId == 0)
            {
                choosedLayer = LayersHelper.InsertLayer(ViewBag.UserId);
            }
            else
                choosedLayer = model.LayerId;

            var listLEForDB = DataHelper.RefactorCADProfileToLayoutElements(listFromFile.Where(x=>x.Type == 0).ToList(),choosedLayer, ViewBag.UserId);
            var listEventsForDB = DataHelper.RefactorCADProfileToEvents(listFromFile.Where(x => x.Type == 1).ToList(), ViewBag.UserId);

            if(LayoutElementsHelper.InsertLayoutElements(listLEForDB) && EventsHelper.InsertEvents(listEventsForDB, choosedLayer))
                return RedirectToAction("Layers", "LayoutElements", new { id = choosedLayer, SPHostUrl = SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri });
            else
                return View();
        }

        // GET: Export
        [SharePointContextFilter]
        public ActionResult Export()
        {
            List<Layers> list = LayersHelper.GetLayersList(ViewBag.UserId);

            ViewBag.LayerId = new SelectList(list.OrderBy(x => x.Id).AsEnumerable(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SharePointContextFilter]
        public ActionResult Export(HelperViewModels.ImportExportFileViewModel model)
        {
            List<Layers> list = LayersHelper.GetLayersList(ViewBag.UserId);

            ViewBag.LayerId = new SelectList(list.OrderBy(x => x.Id).AsEnumerable(), "Id", "Name");
            var layer = db.Layers.Find(model.LayerId);

            var xmiTemp = DataHelper.GetXMIDocument(Server.MapPath("~/Helpers/Templates/xmiTemp.xmi"));

            xmiTemp = xmiTemp.Replace("{ModelName}", layer.Name);

            var packElemDict = DataHelper.GetPackageElementDict(layer.Id);
            xmiTemp = xmiTemp.Replace("{PackageElementList}", DataHelper.GetPackageElementList(packElemDict));

            var fileContent = Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(xmiTemp)).ToArray();
            string contentType = "application/vnd.xmi+xml";
            string fileName = layer.Name + ".xmi";

            return File(fileContent, contentType, fileName);
        }
    }
}