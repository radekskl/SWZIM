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
    public class LayoutElementTypesController : Controller
    {
        private SWZIM_dbEntities db = new SWZIM_dbEntities();

        // GET: LayoutElementTypes
        [SharePointContextFilter]
        public ActionResult Index()
        {
            var layoutElementTypes = db.LayoutElementTypes.Include(l => l.MarkerIcons);
            return View(layoutElementTypes.ToList());
        }

        // GET: LayoutElementTypes/Details/5
        [SharePointContextFilter]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LayoutElementTypes layoutElementTypes = db.LayoutElementTypes.Find(id);
            if (layoutElementTypes == null)
            {
                return HttpNotFound();
            }
            return View(layoutElementTypes);
        }

        // GET: LayoutElementTypes/Create
        [SharePointContextFilter]
        public ActionResult Create()
        {
            ViewBag.MarkerIconId = new SelectList(db.MarkerIcons, "Id", "Name");
            return View();
        }

        // POST: LayoutElementTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SharePointContextFilter]
        public ActionResult Create([Bind(Include = "Id,Name,MarkerIconId")] LayoutElementTypes layoutElementTypes)
        {
            if (ModelState.IsValid)
            {
                db.LayoutElementTypes.Add(layoutElementTypes);
                db.SaveChanges();
                return RedirectToAction("Index", new { SPHostUrl = SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri });
            }

            ViewBag.MarkerIconId = new SelectList(db.MarkerIcons, "Id", "Name", layoutElementTypes.MarkerIconId);
            return View(layoutElementTypes);
        }

        // GET: LayoutElementTypes/Edit/5
        [SharePointContextFilter]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LayoutElementTypes layoutElementTypes = db.LayoutElementTypes.Find(id);
            if (layoutElementTypes == null)
            {
                return HttpNotFound();
            }
            ViewBag.MarkerIconId = new SelectList(db.MarkerIcons, "Id", "Name", layoutElementTypes.MarkerIconId);
            return View(layoutElementTypes);
        }

        // POST: LayoutElementTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SharePointContextFilter]
        public ActionResult Edit([Bind(Include = "Id,Name,MarkerIconId")] LayoutElementTypes layoutElementTypes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(layoutElementTypes).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { SPHostUrl = SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri });
            }
            ViewBag.MarkerIconId = new SelectList(db.MarkerIcons, "Id", "Name", layoutElementTypes.MarkerIconId);
            return View(layoutElementTypes);
        }

        // GET: LayoutElementTypes/Delete/5
        [SharePointContextFilter]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LayoutElementTypes layoutElementTypes = db.LayoutElementTypes.Find(id);
            if (layoutElementTypes == null)
            {
                return HttpNotFound();
            }
            return View(layoutElementTypes);
        }

        // POST: LayoutElementTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SharePointContextFilter]
        public ActionResult DeleteConfirmed(int id)
        {
            LayoutElementTypes layoutElementTypes = db.LayoutElementTypes.Find(id);
            db.LayoutElementTypes.Remove(layoutElementTypes);
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
