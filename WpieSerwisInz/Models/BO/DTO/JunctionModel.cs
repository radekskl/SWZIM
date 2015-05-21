using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WpieSerwisInz.Models.BO.Core;

namespace WpieSerwisInz.Models.BO
{
    public class JunctionModel : BasePosition
    {
        public int Id { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Information")]
        public string Information { get; set; }

        [Display(Name = "Accepted")]
        public bool IsAccepted { get; set; }

        public virtual ICollection<JunctionXml> JunctionXmlList { get; set; }
    }
}