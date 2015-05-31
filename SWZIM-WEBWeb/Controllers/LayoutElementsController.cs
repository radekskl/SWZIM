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
    public class LayoutElementsController : Controller
    {
        private SWZIM_dbEntities db = new SWZIM_dbEntities();

        // GET: LayoutElements
        [SharePointContextFilter]
        public ActionResult Index()
        {
            ViewData["sLE"] = db.LayoutElements.Include(l => l.LayoutElementTypes).Include(l => l.Users).Include(l => l.Layers).Where(x=> x.LayoutElements1.Count == 0).ToList();
            ViewData["pLE"] = db.LayoutElements.Include(l => l.LayoutElementTypes).Include(l => l.Users).Include(l => l.Layers).Where(x=> x.LayoutElements1.Count > 0).ToList();
            return View();
        }

        // GET: LayoutElements
        [SharePointContextFilter]
        public ActionResult Parts(int? id)
        {
            ViewData["sLE"] = db.LayoutElements.Include(l => l.LayoutElementTypes).Include(l => l.Users).Include(l => l.Layers).Where(x => x.LayoutElements1.Count == 0 && x.LayoutElements2.Any(le=>le.Id == id)).ToList();
            ViewData["pLE"] = db.LayoutElements.Include(l => l.LayoutElementTypes).Include(l => l.Users).Include(l => l.Layers).Where(x => x.LayoutElements1.Count > 0 && x.LayoutElements2.Any(le => le.Id == id)).ToList();
            return View();
        }

        // GET: LayoutElements/Details/5
        [SharePointContextFilter]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LayoutElements layoutElements = db.LayoutElements.Find(id);
            if (layoutElements == null)
            {
                return HttpNotFound();
            }
            return View(layoutElements);
        }

        // GET: LayoutElements/Create
        [SharePointContextFilter]
        public ActionResult Create()
        {
            ViewBag.LayoutElementTypeId = new SelectList(db.LayoutElementTypes, "Id", "Name");
            //ViewBag.UserId = new SelectList(db.Users, "ID", "Email");
            ViewBag.LayersId = new SelectList(db.Layers, "Id", "Name");
            return View();
        }

        // POST: LayoutElements/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SharePointContextFilter]
        public ActionResult Create([Bind(Include = "Id,Name,Description,Longitude,Latitude,LayoutElementTypeId,LayersId")] LayoutElements layoutElements)
        {
            if (ModelState.IsValid)
            {
                layoutElements.UserId = ViewBag.UserId;
                db.LayoutElements.Add(layoutElements);
                db.SaveChanges();
                return RedirectToAction("Index", new { SPHostUrl = SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri });
            }

            ViewBag.LayoutElementTypeId = new SelectList(db.LayoutElementTypes, "Id", "Name", layoutElements.LayoutElementTypeId);
            //ViewBag.UserId = new SelectList(db.Users, "ID", "Email", layoutElements.UserId);
            ViewBag.LayersId = new SelectList(db.Layers, "Id", "Name", layoutElements.LayersId);
            return View(layoutElements);
        }

        // GET: LayoutElements/Edit/5
        [SharePointContextFilter]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LayoutElements layoutElements = db.LayoutElements.Find(id);
            if (layoutElements == null)
            {
                return HttpNotFound();
            }
            ViewBag.LayoutElementTypeId = new SelectList(db.LayoutElementTypes, "Id", "Name", layoutElements.LayoutElementTypeId);
            //ViewBag.UserId = new SelectList(db.Users, "ID", "Email", layoutElements.UserId);
            ViewBag.LayersId = new SelectList(db.Layers, "Id", "Name", layoutElements.LayersId);
            return View(layoutElements);
        }

        // POST: LayoutElements/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SharePointContextFilter]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,Longitude,Latitude,LayoutElementTypeId,UserId,LayersId")] LayoutElements layoutElements)
        {
            if (ModelState.IsValid)
            {
                db.Entry(layoutElements).State = System.Data.Entity.EntityState.Modified;
                layoutElements.UserId = ViewBag.UserId;
                db.SaveChanges();
                return RedirectToAction("Index", new { SPHostUrl = SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri });
            }
            ViewBag.LayoutElementTypeId = new SelectList(db.LayoutElementTypes, "Id", "Name", layoutElements.LayoutElementTypeId);
            //ViewBag.UserId = new SelectList(db.Users, "ID", "Email", layoutElements.UserId);
            ViewBag.LayersId = new SelectList(db.Layers, "Id", "Name", layoutElements.LayersId);
            return View(layoutElements);
        }

        // GET: LayoutElements/Delete/5
        [SharePointContextFilter]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LayoutElements layoutElements = db.LayoutElements.Find(id);
            if (layoutElements == null)
            {
                return HttpNotFound();
            }
            return View(layoutElements);
        }

        // POST: LayoutElements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SharePointContextFilter]
        public ActionResult DeleteConfirmed(int id)
        {
            LayoutElements layoutElements = db.LayoutElements.Find(id);
            foreach (var item in layoutElements.LayoutElements1.ToList())
            {
                layoutElements.LayoutElements1.Remove(item);
            }
            foreach (var item in layoutElements.LayoutElements2.ToList())
            {
                layoutElements.LayoutElements2.Remove(item);
            }
            db.LayoutElements.Remove(layoutElements);
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
