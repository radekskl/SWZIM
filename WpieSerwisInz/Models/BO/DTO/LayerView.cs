using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WpieSerwisInz.Models.BO
{
    public class LayerView
    {
        public int Id { get; set; }

        public string LayerName { get; set; }

        public string LayerInformation { get; set; }

        public virtual ICollection<LayerElement> LayerElements { get; set; }

        //Tu ew do zmiany na IdentityUser 
    }
}