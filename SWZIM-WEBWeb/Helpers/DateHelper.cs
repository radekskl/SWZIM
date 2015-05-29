using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SWZIM_WEBWeb.Helpers
{
    public class DateHelper
    {
        public static string HasOccured(DateTime dt)
        {
            var ts = DateTime.Now - dt;
            return ts.MakeReadable();
        }
    }
}