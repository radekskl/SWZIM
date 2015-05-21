using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WpieSerwisInz.Models.BO.Core.CheckboxListBo;

namespace WpieSerwisInz.Models.BO.Wrapper
{
    public class EventWrapper
    {
        public EventModel Event { get; set; }

        public int EventType { get; set; }

        public List<LayerModel> AvailibleElement { get; set; }

        public List<LayerModel> SelectedElement { get; set; }

        public PostedService PostedService { get; set; }
    }
}