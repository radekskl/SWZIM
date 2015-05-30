using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SWZIM_WEBWeb;

namespace SWZIM_WEBWeb.Controllers
{
    public class LayersController : Controller
    {
        private SWZIM_dbEntities db = new SWZIM_dbEntities();

        // GET: Layers
        [SharePointContextFilter]
        public ActionResult Index()
        {
            return View(db.Layers.ToList());
        }

        // GET: Group
        [SharePointContextFilter]
        public ActionResult Group(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var grp = db.Groups.Find(id);
            if (grp == null)
            {
                return HttpNotFound();
            }
            ViewBag.GrpName = grp.Name;
            ViewBag.GrpId = id;
            var layers = grp.Layers;

            if (layers == null)
            {
                return HttpNotFound();
            }
            return View(layers.ToList());
        }

        // GET: AddLayerToGroup
        [SharePointContextFilter]
        public ActionResult AddLayerToGroup(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var grp = db.Groups.Find(id);
            ViewBag.GrpName = grp.Name;
            ViewBag.GrpId = id;
            var layers = new List<Layers>();
            foreach (var item in db.Groups)
            {
                if (grp.Id != id)
                {
                    layers.AddRange(item.Layers);
                }
            }
            ViewBag.NotInGrpLayers = new SelectList(layers, "Id", "Name");
           
            return View();
        }

        // GET: Layers/Details/5
        [SharePointContextFilter]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Layers layers = db.Layers.Find(id);
            if (layers == null)
            {
                return HttpNotFound();
            }
            return View(layers);
        }

        // GET: Layers/Create
        [SharePointContextFilter]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Layers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SharePointContextFilter]
        public ActionResult Create([Bind(Include = "Id,Name,Description")] Layers layers)
        {
            if (ModelState.IsValid)
            {
                db.Layers.Add(layers);
                db.SaveChanges();
                return RedirectToAction("Index", new { SPHostUrl = SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri });
            }

            return View(layers);
        }

        // GET: Layers/Edit/5
        [SharePointContextFilter]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Layers layers = db.Layers.Find(id);
            if (layers == null)
            {
                return HttpNotFound();
            }
            return View(layers);
        }

        // POST: Layers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SharePointContextFilter]
        public ActionResult Edit([Bind(Include = "Id,Name,Description")] Layers layers)
        {
            if (ModelState.IsValid)
            {
                db.Entry(layers).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { SPHostUrl = SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri });
            }
            return View(layers);
        }

        // GET: Layers/Delete/5
        [SharePointContextFilter]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Layers layers = db.Layers.Find(id);
            if (layers == null)
            {
                return HttpNotFound();
            }
            return View(layers);
        }

        // POST: Layers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SharePointContextFilter]
        public ActionResult DeleteConfirmed(int id)
        {
            Layers layers = db.Layers.Find(id);
            db.Layers.Remove(layers);
            db.SaveChanges();
            return RedirectToAction("Index", new { SPHostUrl = SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
