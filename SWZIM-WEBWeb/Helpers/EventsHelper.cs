using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SWZIM_WEBWeb.Helpers
{
    public class EventsHelper
    {
        public static List<Events> GetLatestEventsForUser(int userId, int limit)
        {
            using (var db = new SWZIM_dbEntities())
            {
                var userGroup = db.Users.Find(userId).GroupId;
                return db.Events.Where(l => l.Groups.Any(g => g.Id == userGroup)).OrderByDescending(x=> x.CreatedAt).Take(limit).ToList();
            }
        }
    }
}