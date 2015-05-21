using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WpieSerwisInz.Models.BO.Core;

namespace WpieSerwisInz.Models.BO
{
    public class EventModel : BasePosition
    {
        public int Id { get; set; }

        [Display(Name = "Name")]
        public string EventName { get; set; }

        [Display(Name = "Information")]
        public string EventInformation { get; set; }

        public virtual EventType EventTypes { get; set; }

        public virtual ICollection<LayerElement> LayerElements { get; set; } 
    }
}