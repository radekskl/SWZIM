﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SWZIM_WEBWeb.Controllers
{
    public class MapController : Controller
    {
        // GET: Map
        [SharePointContextFilter]
        public ActionResult Index()
        {
            return View();
        }

    }
}