using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SWZIM_WEBWeb.Models;
using SWZIM_WEBWeb.Consts;

namespace SWZIM_WEBWeb.Models
{
    public class Notification
    {
        public int ID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Enumerations.NotificationType Type { get; set; } //do zebrania typu oraz użycia jako ikona, np. typ=profile -> klasa icon_profile
        public Enumerations.NotificationLabel Label { get; set; } //do ustawienia koloru pod ikoną jako klasy, np. label=primary -> klasa label_primary (niebieska)
        public string Body { get; set; }
        public DateTime Created { get; set; }

        public Notification(int id, Enumerations.NotificationType type, Enumerations.NotificationLabel label,  string body, string created)
        {
            this.ID = id;
            this.Label = label;
            this.Type = type;
            this.Body = body;
            this.Created = DateTime.Now;
            //TimeSpan.FromMinutes(double.Parse(timeAgo)); // w minutach
        }
    }
}
