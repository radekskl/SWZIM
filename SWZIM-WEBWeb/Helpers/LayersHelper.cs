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
                var userGroup = db.Users.Find(userId).Groups;
                var layerGroups = new List<Layers>();
                foreach (var item in userGroup)
                {
                    layerGroups.AddRange(item.Layers);
                }
                return layerGroups.Distinct().ToList();
                               
            }
        }

        public static List<LayoutElements> GetSearchForUser(int userId, string value)
        {
            using (var db = new SWZIM_dbEntities())
            {
                //var userGroup = db.Users.Find(userId).Groups;
                //var layerGroups = new List<Layers>();
                //foreach (var item in userGroup)
                //{
                //    layerGroups.AddRange(item.Layers);
                //}
                //var result = new List<LayoutElements>();
                //foreach (var item in layerGroups)
                //{
                //    result.AddRange(item.LayoutElements.Where(e => e.Name.Contains(value) || e.Description.Contains(value)));
                //}
                //return result.ToList();
                return db.LayoutElements.Include(le=>le.Layers).Where(e => e.Name.Contains(value) || e.Description.Contains(value)).ToList();
            }
        }

        public static int InsertLayer(int userId, string modelId)
        {
            int result = -1;
            using (var db = new SWZIM_dbEntities())
            {
                Layers l = new Layers();
                l.Name = "Warstwa " + DateTime.Now.ToShortTimeString();
                l.Description = modelId;
                db.Layers.Add(l);
                var grp = db.Groups.Where(g => g.Users.Count(u => u.ID == userId) > 0).ToList();
                foreach (var item in grp)
                {
                    item.Layers.Add(l);
                }
                db.SaveChanges();
                result = l.Id;
            }
            return result;
        }
    }
}