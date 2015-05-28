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
                var userGroup = db.Users.Find(userId).GroupId;
                return db.Layers.Where(l => l.Groups.Any(g => g.Id == userGroup)).ToList();
                               
            }
        }
    }
}