﻿using System;
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
    public class NotificationsController : Controller
    {
        private SWZIM_dbEntities db = new SWZIM_dbEntities();

        // GET: Notifications
        [SharePointContextFilter]
        public ActionResult Index()
        {
            var notifications = db.Notifications.Include(n => n.Users);
            return View(notifications.ToList());
        }

        // GET: Notifications/Details/5
        [SharePointContextFilter]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notifications notifications = db.Notifications.Find(id);
            if (notifications == null)
            {
                return HttpNotFound();
            }
            notifications.IsRead = true;
            db.SaveChanges();
            return View(notifications);
        }

        // GET: Notifications/Create
        [SharePointContextFilter]
        public ActionResult Create()
        {
            var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);
            using (var clientContext = spContext.CreateAppOnlyClientContextForSPAppWeb())
            {
                //Microsoft.SharePoint.Client.UserCollection spUsers = clientContext.Web.SiteUsers;
                //clientContext.Load(spUsesr, user => user.Title, user => user.Id, user => user.Email);
                //clientContext.ExecuteQuery();
            }
            ViewBag.UserList = new SelectList(db.Users, "ID", "Email");
            return View();
        }

        // POST: Notifications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SharePointContextFilter]
        public ActionResult Create([Bind(Include = "Label,Body")] Notifications notifications)
        {
            if (ModelState.IsValid)
            {
                
                notifications.IsRead = false;
                notifications.UserId = ViewBag.UserId; //dla kogo
                notifications.Created = DateTime.Now;
                db.Notifications.Add(notifications);
                db.SaveChanges();
                return RedirectToAction("Index", new { SPHostUrl = SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri });
            }

            ViewBag.UserId = new SelectList(db.Users, "ID", "Email", notifications.UserId);
            return View(notifications);
        }

        // GET: Notifications/Edit/5
        [SharePointContextFilter]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notifications notifications = db.Notifications.Find(id);
            if (notifications == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.Users, "ID", "Email", notifications.UserId);
            return View(notifications);
        }

        // GET: Notifications/Delete/5
        [SharePointContextFilter]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notifications notifications = db.Notifications.Find(id);
            if (notifications == null)
            {
                return HttpNotFound();
            }
            return View(notifications);
        }

        // POST: Notifications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SharePointContextFilter]
        public ActionResult DeleteConfirmed(int id)
        {
            Notifications notifications = db.Notifications.Find(id);
            db.Notifications.Remove(notifications);
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
