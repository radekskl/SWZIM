using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SWZIM_WEBWeb.Helpers
{
    public class RouteHelper
    {
        public static string GetControllerAndAction(string controller, string action)
        {
            return string.Format("data-ctrl=\"{0}\" data-action=\"{1}\"", controller, action);
        }
    }
}