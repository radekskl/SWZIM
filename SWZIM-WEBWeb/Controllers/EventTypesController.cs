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
    public class EventTypesController : Controller
    {
        private SWZIM_dbEntities db = new SWZIM_dbEntities();

        // GET: EventTypes
        [SharePointContextFilter]
        public ActionResult Index()
        {
            var eventTypes = db.EventTypes.Include(e => e.MarkerIcons);
            return View(eventTypes.ToList());
        }

        // GET: EventTypes/Details/5
        [SharePointContextFilter]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventTypes eventTypes = db.EventTypes.Find(id);
            if (eventTypes == null)
            {
                return HttpNotFound();
            }
            return View(eventTypes);
        }

        // GET: EventTypes/Create
        [SharePointContextFilter]
        public ActionResult Create()
        {
            ViewBag.MarkerIconId = new SelectList(db.MarkerIcons, "Id", "Name");
            return View();
        }

        // POST: EventTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SharePointContextFilter]
        public ActionResult Create([Bind(Include = "Id,Name,MarkerIconId")] EventTypes eventTypes)
        {
            if (ModelState.IsValid)
            {
                db.EventTypes.Add(eventTypes);
                db.SaveChanges();
                return RedirectToAction("Index", new { SPHostUrl = SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri });
            }

            ViewBag.MarkerIconId = new SelectList(db.MarkerIcons, "Id", "Name", eventTypes.MarkerIconId);
            return View(eventTypes);
        }

        // GET: EventTypes/Edit/5
        [SharePointContextFilter]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventTypes eventTypes = db.EventTypes.Find(id);
            if (eventTypes == null)
            {
                return HttpNotFound();
            }
            ViewBag.MarkerIconId = new SelectList(db.MarkerIcons, "Id", "Name", eventTypes.MarkerIconId);
            return View(eventTypes);
        }

        // POST: EventTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SharePointContextFilter]
        public ActionResult Edit([Bind(Include = "Id,Name,MarkerIconId")] EventTypes eventTypes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eventTypes).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { SPHostUrl = SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri });
            }
            ViewBag.MarkerIconId = new SelectList(db.MarkerIcons, "Id", "Name", eventTypes.MarkerIconId);
            return View(eventTypes);
        }

        // GET: EventTypes/Delete/5
        [SharePointContextFilter]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventTypes eventTypes = db.EventTypes.Find(id);
            if (eventTypes == null)
            {
                return HttpNotFound();
            }
            return View(eventTypes);
        }

        // POST: EventTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SharePointContextFilter]
        public ActionResult DeleteConfirmed(int id)
        {
            EventTypes eventTypes = db.EventTypes.Find(id);
            db.EventTypes.Remove(eventTypes);
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
