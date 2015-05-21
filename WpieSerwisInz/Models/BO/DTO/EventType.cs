using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WpieSerwisInz.Models.BO
{
    public class EventType
    {
        public int Id { get; set; }

        [Display(Name = "Event name")]
        public string EventName { get; set; }

        [Display(Name = "Information")]
        public string EventInformation { get; set; }

        [Display(Name = "Assigned Image")]
        public string AssignedImage { get; set; }

        public virtual ICollection<EventModel> EventModels { get; set; } 
    }
}