using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

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

        public static List<LayoutElements> GetSearchForUser(int userId, string value)
        {
            using (var db = new SWZIM_dbEntities())
            {
                var userGroup = db.Users.Find(userId).GroupId;
                var layers = db.Groups.Find(userGroup).Layers;
                var result = new List<LayoutElements>();
                foreach (var l in layers)
                {
                    result.AddRange(l.LayoutElements.Where(e => e.Name.Contains(value) || e.Description.Contains(value)));
                }
                return result.ToList();
            }
        }
    }
}