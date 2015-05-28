using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SWZIM_WEBWeb.Helpers
{
    public class LayersHelper
    {
        public static List<Layers> GetLayersList(int userId)
        {
            using (var db = new SWZIM_dbEntities())
            {
                return db.Layers.Where(l=> l.Groups.All(g=> g.Users.Where(u => u.ID == userId).Count() > 0)).ToList();
            }
        }
    }
}