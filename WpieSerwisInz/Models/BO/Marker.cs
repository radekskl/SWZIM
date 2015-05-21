using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using WpieSerwisInz.Models.BO.Core;

namespace WpieSerwisInz.Models.BO
{
    public class Marker
    {
        public bool IsEvent { get; set; }

        public string Title { get; set; }

        public string Information { get; set; }

        public int Id { get; set; }

        public string AssignedImage { get; set; }

        public int EventType { get; set; }

        public DateTime LastModificationDate { get; set; }

        public string Lat { get; set; }

        public string Long { get; set; }
    }
}