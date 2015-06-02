﻿using SWZIM_WEBWeb.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SWZIM_WEBWeb.Controllers
{
    public class DataController : Controller
    {
        // GET: Data
        public ActionResult Index()
        {
            return View();
        }

        // GET: Import
        [SharePointContextFilter]
        public ActionResult Import()
        {
            List<Layers> list = LayersHelper.GetLayersList(ViewBag.UserId);
            list.Add(new Layers(){ Id = 0, Name = "Nowa"});
            ViewBag.LayerId = list.OrderBy(l => l.Id).ToList(); 

            return View();
        }

        // GET: Export
        [SharePointContextFilter]
        public ActionResult Export()
        {
            return View();
        }
    }
}