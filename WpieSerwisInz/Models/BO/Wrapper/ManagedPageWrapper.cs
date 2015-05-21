using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WpieSerwisInz.Models.BO.Wrapper
{
    public class ManagedPageWrapper
    {
        public ApplicationUser User { get; set; }
        public List<InfoObject> InfoObjects { get; set; }
    }
}