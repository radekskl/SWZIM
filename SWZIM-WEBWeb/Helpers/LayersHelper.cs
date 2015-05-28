using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SWZIM_WEBWeb.Helpers
{
    public class LayersHelper
    {
        public static List<Layers> GetLayersList()
        {
            using (var db = new SWZIM_dbEntities())
            {
                return db.Layers.ToList();
            }
        }
    }
}