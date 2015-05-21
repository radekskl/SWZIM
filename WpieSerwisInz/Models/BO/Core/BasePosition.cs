using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WpieSerwisInz.Models.BO.Core
{
    public class BasePosition : DataCore
    {
        [Display(Name = "Lattitude")]
        public string Lat { get; set; }

        [Display(Name = "Longtitude")]
        public string Long { get; set; }
    }
}