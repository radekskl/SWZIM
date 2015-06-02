using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SWZIM_WEBWeb.Models
{
    public class XMIHelperModel
    {
        public class LatLong
        {
            public decimal Latitude { get; set; }
            public decimal Longitude { get; set; }
        }

        public class ProfilCADModel
        {
            public string Name { get; set; }
            public string ClassName { get; set; }
            public Dictionary<string, string> Attributes { get; set; }
            public LatLong Coordinates { get; set; }

        }
    }
}