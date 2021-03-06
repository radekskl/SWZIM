﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SWZIM_WEBWeb.Models
{
    public class HelperViewModels
    {
        public class SearchViewModel
        {
            public Events Event { get; set; }
            public LayoutElements LayoutElement { get; set; }
        }

        public class LayerForGroupViewModel
        {
            public int GroupId { get; set; }
            public int LayerId { get; set; }
        }

        public class UserForGroupViewModel
        {
            public int GroupId { get; set; }
            public int UserId { get; set; }
        }

        public class AddEventViewModel
        {
            public Events Event { get; set; }
            public int LayerId { get; set; }
        }

        public class AddElementToElementViewModel
        {
            public int LayoutElement1 { get; set; }
            public int LayoutElement2 { get; set; }
        }

        public class ImportExportFileViewModel
        {
            public int LayerId { get; set; }
            public HttpPostedFileBase File { get; set; }
        }
    }
}