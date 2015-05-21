using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using WpieSerwisInz.Models.BO.Core;

namespace WpieSerwisInz.Models.BO
{
    public class JunctionXml : DataCore
    {
        public int Id { get; set; }

        [Display(Name = "Xml")]
        public string SerializedXmlModel { get; set; }

        [Display(Name = "Information")]
        public string AdditionalInformation { get; set; }

        public virtual JunctionModel Junctiom { get; set; }
    }
}