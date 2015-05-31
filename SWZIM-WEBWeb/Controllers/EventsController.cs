using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SWZIM_WEBWeb;
using Microsoft.SharePoint.Client;
using SWZIM_WEBWeb.Helpers;

namespace SWZIM_WEBWeb.Controllers
{
    public class EventsController : Controller
    {
        private SWZIM_dbEntities db = new SWZIM_dbEntities();

        // GET: Events
        [SharePointContextFilter]
        public ActionResult Index()
        {
            //var events = db.Events.Include(e => e.EventTypes).Include(e => e.Users);
            var events = EventsHelper.GetLatestEventsForUser(ViewBag.UserId, 100);
            return View(events);
        }

        // GET: Events/Details/5
        [SharePointContextFilter]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Events events = db.Events.Find(id);
            if (events == null)
            {
                return HttpNotFound();
            }
            return View(events);
        }

        // GET: Events/Create
        [SharePointContextFilter]
        public ActionResult Create()
        {
            ViewBag.EventTypeId = new SelectList(db.EventTypes, "Id", "Name");
            ViewBag.AddedBy = new SelectList(db.Users, "ID", "Email");

            //int uid = ViewBag.UserId;
            //Users u = db.Users.Where(x => x.ID == uid).First();
            var layers = db.Layers.AsEnumerable();
            ViewBag.Layers = new SelectList(layers, "Id", "Name");

            //grupy SP
            //var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);
            //using (var clientContext = spContext.CreateAppOnlyClientContextForSPAppWeb())
            //{
            //    var groups = clientContext.LoadQuery(clientContext.Web.SiteGroups.Where(x => x.PrincipalType == Microsoft.SharePoint.Client.Utilities.PrincipalType.SharePointGroup));
            //    clientContext.ExecuteQuery();
                
            //    ViewBag.Groups = new SelectList(groups, "Id", "Title");
            //}
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SharePointContextFilter]
        public ActionResult Create([Bind(Include = "Id,Name,Latitude,Longitude,Description,EventTypeId,Status,LayerId")] SWZIM_WEBWeb.Models.HelperViewModels.AddEventViewModel events)
        {
            if (ModelState.IsValid)
            {
                events.Event.AddedBy = ViewBag.UserId;
                events.Event.CreatedAt = DateTime.Now;

                var layer = db.Layers.Find(events.LayerId);
                events.Event.Layers.Add(layer);

                db.Events.Add(events.Event);
                db.SaveChanges();
                return RedirectToAction("Index", new { SPHostUrl = SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri });
            }

            ViewBag.EventTypeId = new SelectList(db.EventTypes, "Id", "Name", events.Event.EventTypeId);
           // ViewBag.AddedBy = new SelectList(db.Users, "ID", "Email", events.AddedBy);
            return View(events);
        }

        // GET: Events/Edit/5
        [SharePointContextFilter]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Events events = db.Events.Find(id);
            if (events == null)
            {
                return HttpNotFound();
            }
            ViewBag.EventTypeId = new SelectList(db.EventTypes, "Id", "Name", events.EventTypeId);
            ViewBag.AddedBy = new SelectList(db.Users, "ID", "Email", events.AddedBy);
            return View(events);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SharePointContextFilter]
        public ActionResult Edit([Bind(Include = "Id,Name,Latitude,Longitude,Description,EventTypeId,Status")] Events events)
        {
            if (ModelState.IsValid)
            {
                db.Entry(events).State = System.Data.Entity.EntityState.Modified;
                events.AddedBy = ViewBag.UserId;
                events.CreatedAt = DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("Index", new { SPHostUrl = SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri });
            }
            ViewBag.EventTypeId = new SelectList(db.EventTypes, "Id", "Name", events.EventTypeId);
            //ViewBag.AddedBy = new SelectList(db.Users, "ID", "Email", events.AddedBy);
            return View(events);
        }

        // GET: Events/Delete/5
        [SharePointContextFilter]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Events events = db.Events.Find(id);
            if (events == null)
            {
                return HttpNotFound();
            }
            return View(events);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SharePointContextFilter]
        public ActionResult DeleteConfirmed(int id)
        {
            Events events = db.Events.Find(id);
            foreach (var item in events.Layers)
            {
                db.Layers.Remove(item);
            }
            
            db.Events.Remove(events);
            db.SaveChanges();
            return RedirectToAction("Index");
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
