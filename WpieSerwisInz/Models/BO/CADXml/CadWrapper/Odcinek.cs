using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WpieSerwisInz.Models.BO.CADXml.CadWrapper
{
    public class Odcinek
    {
        public string Signcode { get; set; }

        public string LatStart { get; set; }

        public string LongStart { get; set; }

        public string LatEnd { get; set; }

        public string LongEnd { get; set; }

        public string Id { get; set; }
    }
}