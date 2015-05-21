using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PagedList;
using WpieSerwisInz.Logic.BusinessLogic;
using WpieSerwisInz.Models.BO;
using WpieSerwisInz.Models;
using WpieSerwisInz.Models.BO.Core.CheckboxListBo;
using WpieSerwisInz.Models.BO.Wrapper;

namespace WpieSerwisInz.Controllers
{
    public class EventController : Controller
    {
        private ApplicationDbContext DbContext = new ApplicationDbContext();

        // GET: /Event/
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;

            var events = DbContext.EventModelDbSet.ToList();

            if (!String.IsNullOrEmpty(searchString))
            {
                events = events.Where(s => s.EventName != null && s.EventName.ToUpper().Contains(searchString.ToUpper())).ToList();
            }
            switch (sortOrder)
            {
                case "name_desc":
                    events = events.OrderByDescending(s => s.EventName).ToList();
                    break;
                case "date_desc":
                    events = events.OrderByDescending(s => s.LastModificationDate).ToList();
                    break;
                case "Date":
                    events = events.OrderBy(s => s.LastModificationDate).ToList();
                    break;
                default:  // Name ascending 
                    events = events.OrderBy(s => s.EventName).ToList();
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(events.ToPagedList(pageNumber, pageSize));
        }

        // GET: /Event/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventModel eventmodel = DbContext.EventModelDbSet.Find(id);
            if (eventmodel == null)
            {
                return HttpNotFound();
            }
            return View(eventmodel);
        }

        public void AddEvent(EventWrapper newEvent, string userId)
        {
            List<LayerElement> layerElements = new List<LayerElement>();
            if (newEvent.PostedService != null)
            {
                layerElements = LayerLogic.GetPostedService(newEvent.PostedService);
            }
            EventModel evModel = new EventModel();
            evModel = newEvent.Event;
            evModel.LastModificationDate = DateTime.Now;
            evModel.CreationDate = DateTime.Now;
            evModel.LayerElements = new Collection<LayerElement>();
            foreach (LayerElement elem in layerElements)
            {
                evModel.LayerElements.Add(DbContext.LayerElementDbSet.First(x => x.Id == elem.Id));
            }
            var Authors = DbContext.Users.ToList();
            evModel.Author = Authors.FirstOrDefault(x => x.Id == userId);
            evModel.EventTypes = DbContext.EventTypeDbSet.First(x => x.Id == newEvent.EventType);
            if (!CheckExist(newEvent))
            {
                DbContext.EventModelDbSet.Add(evModel);
                DbContext.SaveChanges();
            }
        }

        private bool CheckExist(EventWrapper newEvent)
        {
            return DbContext.EventModelDbSet.Any(x => x.Lat == newEvent.Event.Lat && x.Long == newEvent.Event.Long);
        }

        // GET: /Event/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventModel eventmodel = DbContext.EventModelDbSet.Find(id);
            EventWrapper eventWrapper = new EventWrapper();
            eventWrapper.Event = eventmodel;
            eventWrapper.EventType = eventmodel.EventTypes.Id;
            LayerViewWrapper lay = LayerLogic.GetLayerView();
            eventWrapper.AvailibleElement = lay.AvailibleElement;
            eventWrapper.SelectedElement = LayerLogic.GetListLayer(eventmodel.LayerElements.ToList(), true);
            if (eventmodel == null)
            {
                return HttpNotFound();
            }
            return View(eventWrapper);
        }

        // POST: /Event/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EventWrapper eventmodel)
        {
            if (ModelState.IsValid)
            {
                var toSave = LayerLogic.GetPostedService(eventmodel.PostedService);
                EventModel evModel = DbContext.EventModelDbSet.FirstOrDefault(x => x.Id == eventmodel.Event.Id);
                evModel.EventName = eventmodel.Event.EventName;
                evModel.EventInformation = eventmodel.Event.EventInformation;
                evModel.Lat = eventmodel.Event.Lat;
                evModel.Long = eventmodel.Event.Long;
                evModel.EventTypes = DbContext.EventTypeDbSet.First(x => x.Id == eventmodel.EventType);
                evModel.LayerElements.Clear();
                evModel.LastModificationDate = DateTime.Now;
                foreach (LayerElement lmod in toSave)
                {
                    evModel.LayerElements.Add(DbContext.LayerElementDbSet.First(x => x.Id == lmod.Id));
                }

                DbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(eventmodel);
        }

        // GET: /Event/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventModel eventmodel = DbContext.EventModelDbSet.Find(id);
            if (eventmodel == null)
            {
                return HttpNotFound();
            }
            return View(eventmodel);
        }

        // POST: /Event/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EventModel eventmodel = DbContext.EventModelDbSet.Find(id);
            DbContext.EventModelDbSet.Remove(eventmodel);
            DbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                DbContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
