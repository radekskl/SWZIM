using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WpieSerwisInz.Models.BO.Core;

namespace WpieSerwisInz.Models.BO.Wrapper
{
    public class LayerTypeWrapper
    {
        public string UserId { get; set; }

        public CustomRadioButtonList RadioButtons { get; set; }
    }
}