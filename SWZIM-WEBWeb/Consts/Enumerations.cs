using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SWZIM_WEBWeb.Consts
{
    public class Enumerations
    {
        public enum NotificationType
        {
            profile,
            pin,
            book_alt,
            like
            // więcej w css
        }


        public enum NotificationLabel
        {
            warning,
            danger,
            success
            // więcej w css
        }

        public static List<GroupItem> Groups { 
            get {
                List<GroupItem> retList = new List<GroupItem>();
                retList.Add(new GroupItem() { Title = "Administratorzy SWZIM", Description = "Grupa zawierająca osoby administrujące systemem" });
                retList.Add(new GroupItem() { Title = "Policja SWZIM", Description = "Grupa zawierająca osoby należące do służby: Policja" });
                retList.Add(new GroupItem() { Title = "Straż pożarna SWZIM", Description = "Grupa zawierająca osoby należące do służby: Straż pożarna" });
                retList.Add(new GroupItem() { Title = "Pogotowie ratunkowe SWZIM", Description = "Grupa zawierająca osoby należące do służby: Pogotowie ratunkowe" });
                retList.Add(new GroupItem() { Title = "Urzędnicy SWZIM", Description = "Grupa zawierająca osoby będące urzędnikami" });
                retList.Add(new GroupItem() { Title = "Użytkownicy SWZIM", Description = "Grupa zawierająca użytkowników bez specjalnych praw" });

                return retList;
            } 
        }
    }

    public class GroupItem
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}