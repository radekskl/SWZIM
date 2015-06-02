using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SWZIM_WEBWeb.Helpers
{
    public class LayoutElementsHelper
    {
        public static int GetLEType(string input)
        {
            int result = 17; // Ogólny
            using (var db = new SWZIM_dbEntities())
            {
                var type = db.LayoutElementTypes.FirstOrDefault(x => x.Name.Equals(input));
                if (type != null)
                    result = type.Id;
            }
            return result;
        }

        public static bool InsertLayoutElements(List<LayoutElements> list)
        {
            try
            {
                using (var db = new SWZIM_dbEntities())
                {
                    foreach (var item in list)
                    {
                        db.LayoutElements.Add(item);
                    }
                    db.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            
        }
    }
}