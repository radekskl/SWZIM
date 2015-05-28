using SWZIM_WEBWeb.Consts;
using SWZIM_WEBWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SWZIM_WEBWeb.Helpers
{
    public class NotificationsHelper
    {
        public static int GetNotificationsCount(int userId)
        {
            IEnumerable<Notifications> results;

            using (var context = new SWZIM_dbEntities())
            {
                results = context.Notifications
                    .Where(o => o.UserId == userId && o.IsRead == false);

                return results.Count();
            }
            // get notification where isRead false;
        }

        public static IEnumerable<Notifications> GetNotifications(int userId, int getCount)
        {
            
            IEnumerable<Notifications> results;

            using (var context = new SWZIM_dbEntities())
            {
                results = context.Notifications
                    .Where(o => o.UserId == userId)
                    .OrderByDescending(a => a.Created);
                if (getCount == 0)
                {
                    results.ToList();
                }
                return results.ToList().Take(getCount);
            }
        }


        public static IEnumerable<Notifications> GetNotificationById(int id)
        {
            //do uzupełnienia z bazy
            IEnumerable<Notifications> results;
            var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext.Current);
            Microsoft.SharePoint.Client.User spUser = null;
            using (var clientContext = spContext.CreateUserClientContextForSPHost())
            {
                if (clientContext != null)
                {
                    spUser = clientContext.Web.CurrentUser;
                    clientContext.Load(spUser, user => user.Id);
                    clientContext.ExecuteQuery();
                }
            }

            using (var context = new SWZIM_dbEntities())
            {
                results = context.Notifications
                    .Where(o => o.UserId == spUser.Id);

                return results.ToList();
            }
            //mock
            //List<Notification> notifications = new List<Notification>();
            //notifications.Add(new Notification(1, Enumerations.NotificationType.profile, Enumerations.NotificationLabel.warning, "Uzupełnij swój profil!", "1200"));
            //notifications.Add(new Notification(2, Enumerations.NotificationType.book_alt, Enumerations.NotificationLabel.danger, "Wypadek przy ulicy Kaliskiego!", "10"));
            //notifications.Add(new Notification(3, Enumerations.NotificationType.pin, Enumerations.NotificationLabel.success, "Korek przy ulicy Radiowej", "5"));
            //return notifications.Where(x => x.ID == id).ToList();
        }


        public static string GetMessageCount()
        {

            // get messages where isRead false;
            return "7"; //do uzupełnienia z bazy
        }
    }
}