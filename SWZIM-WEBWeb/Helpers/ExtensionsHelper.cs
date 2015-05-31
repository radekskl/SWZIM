using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SWZIM_WEBWeb.Helpers
{
    public static class ExtensionsHelper
    {
        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : string.Format("{0}{1}", value.Substring(0, maxLength), "...");
        }

        /// <summary>
        ///     Produkuje ciąg znaków typu 2 dni 1 godz 20min
        /// </summary>
        /// <param name="value">timespan</param>
        /// <returns></returns>
        public static string MakeReadable(this TimeSpan value)
        {
            if (value == null) return string.Empty;
            if (value.Days == 1) { return "1 dzień"; }
            if (value.Days > 0) { return string.Format("{0} dni", value.Days); }
            else return string.Format("{0} {1}", (value.Hours > 0) ? string.Format("{0} godz", value.Hours) : "", (value.Minutes > 0) ? string.Format("{0} min", value.Minutes) : "");
        }

        public static string GetAllParentLayoutElements(this ICollection<LayoutElements> value)
        {
            string result = "";
            
            foreach (var item in value)
            {
                result += item.Name + ",";
            }
            result = result.Substring(0, result.Length - 1);
            return result;
        }
    }
}