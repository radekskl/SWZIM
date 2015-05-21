using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PagedList;
using WpieSerwisInz.Models.BO;
using WpieSerwisInz.Models;

namespace WpieSerwisInz.Controllers
{
    public class JunctionController : Controller
    {
        private ApplicationDbContext DbContext = new ApplicationDbContext();

        // GET: /Junction/
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

            var junctions = DbContext.JunctionModelDbSet.ToList();

            if (!String.IsNullOrEmpty(searchString))
            {
                junctions = junctions.Where(s => s.Name != null && s.Name.ToUpper().Contains(searchString.ToUpper())).ToList();
            }
            switch (sortOrder)
            {
                case "name_desc":
                    junctions = junctions.OrderByDescending(s => s.Name).ToList();
                    break;
                case "date_desc":
                    junctions = junctions.OrderByDescending(s => s.LastModificationDate).ToList();
                    break;
                case "Date":
                    junctions = junctions.OrderBy(s => s.LastModificationDate).ToList();
                    break;
                default:  // Name ascending 
                    junctions = junctions.OrderBy(s => s.Name).ToList();
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(junctions.ToPagedList(pageNumber, pageSize));
        }

        // GET: /Junction/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JunctionModel junctionmodel = DbContext.JunctionModelDbSet.Find(id);
            if (junctionmodel == null)
            {
                return HttpNotFound();
            }
            return View(junctionmodel);
        }

        // GET: /Junction/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JunctionModel junctionmodel = DbContext.JunctionModelDbSet.Find(id);
            if (junctionmodel == null)
            {
                return HttpNotFound();
            }
            return View(junctionmodel);
        }

        // POST: /Junction/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Information,IsAccepted,Lat,Long,CreationDate,LastModificationDate")] JunctionModel junctionmodel)
        {
            junctionmodel.LastModificationDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                DbContext.Entry(junctionmodel).State = EntityState.Modified;
                DbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(junctionmodel);
        }

        // GET: /Junction/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JunctionModel junctionmodel = DbContext.JunctionModelDbSet.Find(id);
            if (junctionmodel == null)
            {
                return HttpNotFound();
            }
            return View(junctionmodel);
        }

        // POST: /Junction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            JunctionModel junctionmodel = DbContext.JunctionModelDbSet.Find(id);
            DbContext.JunctionModelDbSet.Remove(junctionmodel);
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

        private bool CheckExist(JunctionModel cross)
        {
            return DbContext.JunctionModelDbSet.Any(x => x.Lat == cross.Lat && x.Long == cross.Long);
        }

        public void AddCross(JunctionModel junction, string userId)
        {
            junction.CreationDate = DateTime.Now;
            junction.LastModificationDate = DateTime.Now;
            var Authors = DbContext.Users.ToList();
            junction.Author = Authors.FirstOrDefault(x => x.Id == userId);
            if (!CheckExist(junction))
            {
                DbContext.JunctionModelDbSet.Add(junction);
                DbContext.SaveChanges();
            }
        }
    }
}
