using System;
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

        public class AddEventViewModel
        {
            public Events Event { get; set; }
            public int LayerId { get; set; }
        }
    }
}