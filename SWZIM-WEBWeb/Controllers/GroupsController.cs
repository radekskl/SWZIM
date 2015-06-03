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
    public class GroupsController : Controller
    {
        private SWZIM_dbEntities db = new SWZIM_dbEntities();

        // GET: Groups
        [SharePointContextFilter]
        public ActionResult Index()
        {
            return View(db.Groups.ToList());
        }

        // GET: Groups/Users/5
        [SharePointContextFilter]
        public ActionResult Users(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Groups groups = db.Groups.Find(id);
            ViewBag.GrpName = groups.Name;
            ViewBag.GrpId = groups.Id;
            if (groups == null)
            {
                return HttpNotFound();
            }
            return View(groups.Users.ToList());
        }

        // GET: AddUserFomGroup
        [SharePointContextFilter]
        public ActionResult AddUserForGroup(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var grp = db.Groups.Find(id);
            ViewBag.GrpName = grp.Name;
            ViewBag.GrpId = id;
            var users = new List<Users>();
            foreach (var item in db.Users)
            {
                if (!item.Groups.Any(x => x.Id == id))
                {
                    users.Add(item);
                }
            }
            ViewBag.NotInGrpUsers = new SelectList(users, "Id", "UserName");
            return View();
        }

        // POST: AddUserForGroup
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SharePointContextFilter]
        public ActionResult AddUserForGroup([Bind(Include = "GroupId,LayerId")] SWZIM_WEBWeb.Models.HelperViewModels.UserForGroupViewModel ufgvm)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Find(ufgvm.UserId);
                db.Groups.Find(ufgvm.GroupId).Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Group", new { id = ufgvm.GroupId, SPHostUrl = SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri });
            }

            //ViewBag.UserId = new SelectList(db.Users, "ID", "Email", notifications.UserId);
            return View(ufgvm);
        }

        // GET: DeleteUserFromGroup/5
        [SharePointContextFilter]
        public ActionResult DeleteUserFromGroup(int? id, int? groupId)
        {
            if (id == null || groupId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users user = db.Users.Find(id);
            db.Groups.Find(groupId).Users.Remove(user);
            db.SaveChanges();

            return RedirectToAction("Users", new { id = groupId, SPHostUrl = SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri });
        }


        // GET: Groups/Details/5
        [SharePointContextFilter]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Groups groups = db.Groups.Find(id);
            if (groups == null)
            {
                return HttpNotFound();
            }
            return View(groups);
        }

        // GET: Groups/Create
        [SharePointContextFilter]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Groups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SharePointContextFilter]
        public ActionResult Create([Bind(Include = "Id,Name")] Groups groups)
        {
            if (ModelState.IsValid)
            {
                db.Groups.Add(groups);
                db.SaveChanges();
                return RedirectToAction("Index", new { SPHostUrl = SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri });
            }

            return View(groups);
        }

        // GET: Groups/Edit/5
        [SharePointContextFilter]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Groups groups = db.Groups.Find(id);
            if (groups == null)
            {
                return HttpNotFound();
            }
            return View(groups);
        }

        // POST: Groups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SharePointContextFilter]
        public ActionResult Edit([Bind(Include = "Id,Name")] Groups groups)
        {
            if (ModelState.IsValid)
            {
                db.Entry(groups).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { SPHostUrl = SharePointContext.GetSPHostUrl(HttpContext.Request).AbsoluteUri });
            }
            return View(groups);
        }

        // GET: Groups/Delete/5
        [SharePointContextFilter]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Groups groups = db.Groups.Find(id);
            if (groups == null)
            {
                return HttpNotFound();
            }
            return View(groups);
        }

        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [SharePointContextFilter]
        public ActionResult DeleteConfirmed(int id)
        {
            Groups groups = db.Groups.Find(id);
            db.Groups.Remove(groups);
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
