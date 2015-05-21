using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WpieSerwisInz.Models.BO.Core.CheckboxListBo
{
    public class LayerModel
    {
        public bool IsSelected { get; set; }

        public int Id { get; set; }

        public string ElementName { get; set; }

        public string Information { get; set; }
    }
}