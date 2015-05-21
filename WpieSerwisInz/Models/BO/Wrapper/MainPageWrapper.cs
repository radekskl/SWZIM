using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WpieSerwisInz.Models.BO.Core;
using WpieSerwisInz.Models.BO.Core.CheckboxListBo;

namespace WpieSerwisInz.Models.BO.Wrapper
{
    public class MainPageWrapper
    {
        public string Lat { get; set; }

        public string Long { get; set; }

        public List<Marker> MarkerList { get; set; }

        public PagedList.IPagedList<Marker> MarkerListShow { get; set; } 

        public EventWrapper EventModel { get; set; }

        public JunctionModel JunctionModel { get; set; }

        public List<LayerModel> AvailibleElement { get; set; }

        public List<LayerModel> SelectedElement { get; set; }

        public PostedService PostedService { get; set; }
    }
}