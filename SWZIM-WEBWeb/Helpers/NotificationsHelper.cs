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
        public static string GetNotificationsCount()
        {
            // get notification where isRead false;
            return "5"; //do uzupełnienia z bazy
        }

        public static List<Notification> GetNotifications()
        {
            //do uzupełnienia z bazy

            //mock
            List<Notification> notifications = new List<Notification>();
            notifications.Add(new Notification(1, Enumerations.NotificationType.profile, Enumerations.NotificationLabel.warning, "Uzupełnij swój profil!", "1200"));
            notifications.Add(new Notification(2, Enumerations.NotificationType.book_alt, Enumerations.NotificationLabel.danger, "Wypadek przy ulicy Kaliskiego!", "10"));
            notifications.Add(new Notification(3, Enumerations.NotificationType.pin, Enumerations.NotificationLabel.success, "Korek przy ulicy Radiowej", "5"));
            return notifications;

        }


        public static List<Notification> GetNotificationById(int id)
        {
            //do uzupełnienia z bazy

            //mock
            List<Notification> notifications = new List<Notification>();
            notifications.Add(new Notification(1, Enumerations.NotificationType.profile, Enumerations.NotificationLabel.warning, "Uzupełnij swój profil!", "1200"));
            notifications.Add(new Notification(2, Enumerations.NotificationType.book_alt, Enumerations.NotificationLabel.danger, "Wypadek przy ulicy Kaliskiego!", "10"));
            notifications.Add(new Notification(3, Enumerations.NotificationType.pin, Enumerations.NotificationLabel.success, "Korek przy ulicy Radiowej", "5"));
            return notifications.Where(x => x.ID == id).ToList();

        }


        public static string GetMessageCount()
        {
            // get messages where isRead false;
            return "7"; //do uzupełnienia z bazy
        }
    }
}