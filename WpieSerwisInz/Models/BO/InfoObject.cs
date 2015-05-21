using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WpieSerwisInz.Models.BO
{
    public class InfoObject
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public string Name { get; set; }

        public string Information { get; set; }

        public string Controller { get; set; }

        public string Action { get; set; }

        public DateTime LastModificationDate { get; set; }
    }
}