using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace SWZIM_WEBWeb.Helpers
{
    public class EventsHelper
    {
        public static List<Events> GetLatestEventsForUser(int userId, int limit)
        {
            using (var db = new SWZIM_dbEntities())
            {
                var userGroup = db.Users.Find(userId).Groups;
                var layerGroups = new List<Layers>();
                foreach (var item in userGroup)
	            {
		            layerGroups.AddRange(item.Layers);
	            }
                var result = new List<Events>();
                foreach (var item in layerGroups)
                {
                    result.AddRange(item.Events);
                }
                return result.OrderByDescending(x=> x.CreatedAt).Take(limit).ToList();
            }
        }

        public static List<Events> GetSearchForUser(int userId, string value)
        {
            using (var db = new SWZIM_dbEntities())
            {
                var userGroup = db.Users.Find(userId).Groups;
                var layerGroups = new List<Layers>();
                foreach (var item in userGroup)
                {
                    layerGroups.AddRange(item.Layers);
                }
                var result = new List<Events>();
                foreach (var item in layerGroups)
                {
                    result.AddRange(item.Events);
                }
                return result.OrderByDescending(x => x.CreatedAt).Where(e => e.Name.Contains(value) || e.Description.Contains(value)).ToList();
            }
        }
    }
}