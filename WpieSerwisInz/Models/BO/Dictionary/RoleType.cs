using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WpieSerwisInz.Models.BO.Dictionary
{
    public class RoleType
    {
        /// <summary>
        /// System Admin
        /// </summary>
        public static string Admin = "Administrator";
        /// <summary>
        /// User With confirmed Email Address (Or Confirmed by Admin)
        /// </summary>
        public static string Confirmed = "Confirmed";
        /// <summary>
        /// User who can Insert and modify models.
        /// </summary>
        public static string Developer = "Developer";
        /// <summary>
        /// All user after registration.
        /// </summary>
        public static string Unconfirmed = "Unconfirmed";
    }
}