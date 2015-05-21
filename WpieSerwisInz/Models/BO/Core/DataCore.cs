using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WpieSerwisInz.Models.BO.Core
{
    public class DataCore
    {
        [Display(Name = "Create Date")]
        public DateTime CreationDate { get; set; }

        [Display(Name = "Modification Date")]
        public DateTime LastModificationDate { get; set; }

        public virtual ApplicationUser Author { get; set; }
    }
}