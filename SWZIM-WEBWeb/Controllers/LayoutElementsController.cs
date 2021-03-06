﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SWZIM_WEBWeb;
using SWZIM_WEBWeb.Helpers;

namespace SWZIM_WEBWeb.Controllers
{
    public class LayoutElementsController : Controller
    {
        private SWZIM_dbEntities db = new SWZIM_dbEntities();

        // GET: LayoutElements
        [SharePointContextFilter]
        public ActionResult Index()
        {
            ViewData["sLE"] = db.LayoutElements.Include(l => l.LayoutElementTypes)
                .Include(l => l.Users)
                .Include(l => l.Layers)
                .Where(x=> x.LayoutElements1.Count == 0)
                .Where(x=> x.LayoutElementTypeId != 21) //lokalizacja
                .ToList();
            ViewData["pLE"] = db.LayoutElements
                .Include(l => l.LayoutElementTypes)
                .Include(l => l.Users)
                .Include(l => l.Layers)
                .Where(x=> x.LayoutElements1.Count > 0)
                .Where(x=> x.LayoutElementTypeId != 21) //lokalizacja
                .ToList();
            return View();
        }

        // GET: Layers
        [SharePointContextFilter]
        public ActionResult Layers(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewData["sLE"] = db.LayoutElements
                .Include(l => l.LayoutElementTypes)
                .Include(l => l.Users)
                .Include(l => l.Layers)
                .Where(x => x.LayoutElements1.Count == 0 && x.LayersId == id)
                .Where(x=> x.LayoutElementTypeId != 21) //lokalizacja
                .ToList();
            ViewData["pLE"] = db.LayoutElements
                .Include(l => l.LayoutElementTypes)
                .Include(l => l.Users)
                .Include(l => l.Layers)
                .Where(x => x.LayoutElements1.Count > 0 && x.LayersId == id)
                .Where(x=> x.LayoutElementTypeId != 21) //lokalizacja
                .ToList();
            return View();
        }

        // GET: Parts
        [SharePointContextFilter]
        public ActionResult Parts(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewData["sLE"] = db.LayoutElements
                .Include(l => l.LayoutElementTypes)
                .Include(l => l.Users)
                .Include(l => l.Layers)
                .Where(x => x.LayoutElements1.Count == 0 && x.LayoutElements2.Any(le=>le.Id == id))
                .Where(x=> x.LayoutElementTypeId != 21) //lokalizacja
                .ToList();
            ViewData["pLE"] = db.LayoutElements
                .Include(l => l.LayoutElementTypes)
                .Include(l => l.Users)
                .Include(l => l.Layers)
                .Where(x => x.LayoutElements1.Count > 0 && x.LayoutElements2.Any(le => le.Id == id))
                .Where(x=> x.LayoutElementTypeId != 21) //lokalizacja
                .ToList();
            return View();
        }

        //GET: AddAttribute
        [SharePointContextFilter]
        public ActionResult AddAttribute(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.ElementId = id;
            return View();
        }

        //GET: DeleteAttribute
        [SharePointContextFilter]
        public ActionResult DeleteAttribute(int? id, int? elId)
        {
            if (id == null || elId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var attr = db.LayoutElementAttributes.Find(id);
            db.LayoutElementAttributes.Remove(attr);
            db.SaveChanges();
            return RedirectToAction("Details", new { id = elId, SPHostUrl = SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri });
        }

        //POST: AddAttribute
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SharePointContextFilter]
        public ActionResult AddAttribute([Bind(Include = "Id,Name,Value,LayoutElementId")] LayoutElementAttributes lea)
        {
            if (ModelState.IsValid)
            {
                db.LayoutElementAttributes.Add(lea);
                db.SaveChanges();
                return RedirectToAction("Details", new { id=lea.LayoutElementId ,SPHostUrl = SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri });
            }

            return View(lea);
        }

        // GET: AddTo
        [SharePointContextFilter]
        public ActionResult AddTo(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.ElementList = new SelectList(db.LayoutElements, "Id", "Name");
            ViewBag.ElementId = id;
            
            return View();
        }

        // POST: AddTo
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SharePointContextFilter]
        public ActionResult AddTo(SWZIM_WEBWeb.Models.HelperViewModels.AddElementToElementViewModel model)
        {
            var el = db.LayoutElements.Find(model.LayoutElement1);
            var add = db.LayoutElements.Find(model.LayoutElement2);
            add.LayoutElements1.Add(el);
            db.SaveChanges();

            return RedirectToAction("Index", new { SPHostUrl = SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri });
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
                db.LayoutElementAttributes.Add(
                    new LayoutElementAttributes(){
                        Name = "base_Class",
                        Value = DataHelper.GenerateBaseClassId()
                    });
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
