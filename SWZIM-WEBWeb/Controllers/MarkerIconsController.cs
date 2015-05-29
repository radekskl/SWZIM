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
    public class MarkerIconsController : Controller
    {
        private SWZIM_dbEntities db = new SWZIM_dbEntities();

        // GET: MarkerIcons
        [SharePointContextFilter]
        public ActionResult Index()
        {
            return View(db.MarkerIcons.ToList());
        }

        // GET: MarkerIcons/Details/5
        [SharePointContextFilter]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MarkerIcons markerIcons = db.MarkerIcons.Find(id);
            if (markerIcons == null)
            {
                return HttpNotFound();
            }
            return View(markerIcons);
        }

        // GET: MarkerIcons/Create
        [SharePointContextFilter]
        public ActionResult Create()
        {
            return View();
        }

        // POST: MarkerIcons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SharePointContextFilter]
        public ActionResult Create([Bind(Include = "Id,Name,Url")] MarkerIcons markerIcons)
        {
            if (ModelState.IsValid)
            {
                db.MarkerIcons.Add(markerIcons);
                db.SaveChanges();
                return RedirectToAction("Index", new { SPHostUrl = SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri });
            }

            return View(markerIcons);
        }

        // GET: MarkerIcons/Edit/5
        [SharePointContextFilter]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MarkerIcons markerIcons = db.MarkerIcons.Find(id);
            if (markerIcons == null)
            {
                return HttpNotFound();
            }
            return View(markerIcons);
        }

        // POST: MarkerIcons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SharePointContextFilter]
        public ActionResult Edit([Bind(Include = "Id,Name,Url")] MarkerIcons markerIcons)
        {
            if (ModelState.IsValid)
            {
                db.Entry(markerIcons).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index", new { SPHostUrl = SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri });
        }

        // GET: MarkerIcons/Delete/5
        [SharePointContextFilter]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MarkerIcons markerIcons = db.MarkerIcons.Find(id);
            if (markerIcons == null)
            {
                return HttpNotFound();
            }
            return View(markerIcons);
        }

        // POST: MarkerIcons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SharePointContextFilter]
        public ActionResult DeleteConfirmed(int id)
        {
            MarkerIcons markerIcons = db.MarkerIcons.Find(id);
            db.MarkerIcons.Remove(markerIcons);
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
