using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WpieSerwisInz.Models.BO
{
    public class LayerElement
    {
        public int Id { get; set; }

        public string ElementName { get; set; }

        public string Information { get; set; }

        public virtual ICollection<LayerView> LayerViews { get; set; }

        public virtual ICollection<EventModel> EventModels { get; set; } 
    }
}