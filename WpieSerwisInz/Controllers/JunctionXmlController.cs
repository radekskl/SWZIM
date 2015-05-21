using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;
using System.Xml;
using Microsoft.AspNet.Identity;
using PagedList;
using WpieSerwisInz.Logic.CADXml;
using WpieSerwisInz.Models.BO;
using WpieSerwisInz.Models;
using WpieSerwisInz.Models.BO.CADXml;
using WpieSerwisInz.Models.BO.Wrapper;

namespace WpieSerwisInz.Controllers
{
    public class JunctionXmlController : Controller
    {
        private ApplicationDbContext DbContext = new ApplicationDbContext();

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

            var junctions = DbContext.JunctionXmlDbSet.ToList();

            if (!String.IsNullOrEmpty(searchString))
            {
                junctions = junctions.Where(s => s.Junctiom.Name != null && s.Junctiom.Name.ToUpper().Contains(searchString.ToUpper())).ToList();
            }
            switch (sortOrder)
            {
                case "name_desc":
                    junctions = junctions.OrderByDescending(s => s.Junctiom.Name).ToList();
                    break;
                case "date_desc":
                    junctions = junctions.OrderByDescending(s => s.LastModificationDate).ToList();
                    break;
                case "Date":
                    junctions = junctions.OrderBy(s => s.LastModificationDate).ToList();
                    break;
                default:  // Name ascending 
                    junctions = junctions.OrderBy(s => s.Junctiom.Name).ToList();
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(junctions.ToPagedList(pageNumber, pageSize));
        }


        [HttpPost]
        public ActionResult PrepareDownload(int? id)
        {
            Session["DownloadId"] = id;
            return Json(new { Success = true });
        }


        public ActionResult Download(int? id)
        {
            JunctionXml junctionxml = DbContext.JunctionXmlDbSet.Find(id);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(junctionxml.SerializedXmlModel);
            System.Text.Encoding enc = System.Text.Encoding.UTF8;
            MemoryStream str = new MemoryStream(enc.GetBytes(doc.OuterXml));
            return File(str, "text/xml", junctionxml.Junctiom.Name + ".xml");
        }


        // GET: /JunctionXml/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JunctionXml junctionxml = DbContext.JunctionXmlDbSet.Find(id);
            topology topologyDB = new topology();
            CadLogic cad = new CadLogic();
            bool isUploadFile = false;
            try
            {
                if (!(junctionxml == null || String.IsNullOrEmpty(junctionxml.SerializedXmlModel)))
                {
                    isUploadFile = true;
                    topologyDB = cad.DeserializeXmlTopology(junctionxml.SerializedXmlModel);
                }
            }
            catch
            {
                isUploadFile = false;
            }
            JunctionXmlWrapper junctionWrapper = cad.GetTopologyWrapper(topologyDB);
            junctionWrapper.Xml = junctionxml;
            junctionWrapper.IsUploadFile = isUploadFile;

            if (junctionxml == null)
            {
                return HttpNotFound();
            }
            return View(junctionWrapper);
        }

        // GET: /JunctionXml/Create
        public ActionResult Create(int id)
        {
            JunctionXml junction = new JunctionXml();
            junction.Junctiom = DbContext.JunctionModelDbSet.FirstOrDefault(x => x.Id == id);
            return View(junction);
        }

        // POST: /JunctionXml/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(JunctionXml junctionxml)
        {
            int junctionId = Int32.Parse(Session["junctionId"].ToString());
            Session["junctionId"] = null;
            CadLogic cad = new CadLogic();
            junctionxml.Junctiom = DbContext.JunctionModelDbSet.FirstOrDefault(x => x.Id == junctionId);
            var Authors = DbContext.Users.ToList();
            junctionxml.Author = Authors.FirstOrDefault(x => x.Id == User.Identity.GetUserId());
            junctionxml.CreationDate = DateTime.Now;
            junctionxml.LastModificationDate = DateTime.Now;

            if (Request != null && Request.Files != null && Request.Files.Count > 0 &&
                    Request.Files[0].ContentLength != 0)
            {
                HttpPostedFileBase file = Request.Files[0];
                string result = new StreamReader(file.InputStream).ReadToEnd();
                try
                {
                    topology topologyDB = null;
                    if (!(result == null || String.IsNullOrEmpty(result)))
                    {

                        topologyDB = cad.DeserializeXmlTopology(result);
                    }

                    if (topologyDB != null)
                    {
                        junctionxml.SerializedXmlModel = result;
                    }
                    else
                    {
                        junctionxml.SerializedXmlModel = String.Empty;
                    }

                    if (ModelState.IsValid)
                    {
                        DbContext.JunctionXmlDbSet.Add(junctionxml);
                        DbContext.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                catch
                {
                    //Error
                }
            }

            return View(junctionxml);
        }

        // GET: /JunctionXml/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JunctionXml junctionxml = DbContext.JunctionXmlDbSet.Find(id);
            topology topologyDB = new topology();
            CadLogic cad = new CadLogic();
            bool isUploadFile = false;
            try
            {
                if (!(junctionxml == null || String.IsNullOrEmpty(junctionxml.SerializedXmlModel)))
                {
                    isUploadFile = true;
                    topologyDB = cad.DeserializeXmlTopology(junctionxml.SerializedXmlModel);
                }
            }
            catch
            {
                isUploadFile = false;
            }
            JunctionXmlWrapper junctionWrapper = cad.GetTopologyWrapper(topologyDB);
            junctionWrapper.Xml = junctionxml;
            junctionWrapper.IsUploadFile = isUploadFile;

            if (junctionxml == null)
            {
                return HttpNotFound();
            }
            return View(junctionWrapper);
        }

        // POST: /JunctionXml/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string submit, JunctionXmlWrapper junctionxmlWrapper)
        {
            CadLogic cad = new CadLogic();
            if (submit == "Upload")
            {
                JunctionXmlWrapper junctionWrapper = junctionxmlWrapper;
                JunctionXml junctionToSave = DbContext.JunctionXmlDbSet.Find(junctionxmlWrapper.Xml.Id);
                if (Request != null && Request.Files != null && Request.Files.Count > 0 &&
                    Request.Files[0].ContentLength != 0)
                {
                    HttpPostedFileBase file = Request.Files[0];
                    string result = new StreamReader(file.InputStream).ReadToEnd();
                    try
                    {
                        topology topologyDB = null;
                        if (!(result == null || String.IsNullOrEmpty(result)))
                        {
                            topologyDB = cad.DeserializeXmlTopology(result);
                        }

                        junctionToSave.LastModificationDate = DateTime.Now;

                        if (topologyDB != null)
                        {
                            junctionToSave.SerializedXmlModel = result;
                        }

                        if (ModelState.IsValid)
                        {
                            DbContext.Entry(junctionToSave).State = EntityState.Modified;
                            DbContext.SaveChanges();
                        }
                        junctionWrapper = cad.GetTopologyWrapper(topologyDB);
                        junctionWrapper.Xml = junctionToSave;
                        junctionWrapper.IsUploadFile = true;
                    }
                    catch
                    {
                        //Error
                    }
                }
                return View(junctionWrapper);

            }
            else
            {


                JunctionXml junctionToSave = DbContext.JunctionXmlDbSet.Find(junctionxmlWrapper.Xml.Id);
                topology topToSave = cad.GetTopology(junctionxmlWrapper);
                string modifiedXml = cad.SerializeXmlTopology(topToSave);


                junctionToSave.LastModificationDate = DateTime.Now;
                junctionToSave.SerializedXmlModel = modifiedXml;
                if (ModelState.IsValid)
                {
                    DbContext.Entry(junctionToSave).State = EntityState.Modified;
                    DbContext.SaveChanges();
                    return RedirectToAction("Details", new { id = junctionToSave.Id });
                }
                return View(junctionToSave);
            }

        }

        // GET: /JunctionXml/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JunctionXml junctionxml = DbContext.JunctionXmlDbSet.Find(id);
            if (junctionxml == null)
            {
                return HttpNotFound();
            }
            return View(junctionxml);
        }

        // POST: /JunctionXml/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            JunctionXml junctionxml = DbContext.JunctionXmlDbSet.Find(id);
            DbContext.JunctionXmlDbSet.Remove(junctionxml);
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
