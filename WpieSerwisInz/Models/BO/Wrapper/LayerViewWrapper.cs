using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WpieSerwisInz.Models.BO.Core.CheckboxListBo;

namespace WpieSerwisInz.Models.BO.Wrapper
{
    public class LayerViewWrapper
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Information { get; set; }
        public List<LayerModel> AvailibleElement { get; set; }
        public List<LayerModel> SelectedElement { get; set; }
        public PostedService PostedService { get; set; }
    }
}