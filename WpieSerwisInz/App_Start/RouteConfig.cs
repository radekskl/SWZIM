using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WpieSerwisInz
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
           
            #region AdminUsers
            routes.MapRoute(
                "AdminUsers",
                "{controller}/UserView/{action}",
                new
                {
                    controller = "Admin",
                    action = UrlParameter.Optional
                });

            routes.MapRoute(
                "AdminUsersPar",
                "{controller}/UserView/{action}/{id}",
                new
                {
                    controller = "Admin",
                    action = UrlParameter.Optional,
                    id = UrlParameter.Optional
                });
            #endregion

            #region LayerElement
            routes.MapRoute(
                "AdminLayerElement",
                "{controller}/LayerElement/{action}",
                new
                {
                    controller = "Admin",
                    action = UrlParameter.Optional
                });

            routes.MapRoute(
                "AdminLayerElementPar",
                "{controller}/LayerElement/{action}/{id}",
                new
                {
                    controller = "Admin",
                    action = UrlParameter.Optional,
                    id = UrlParameter.Optional
                });
            #endregion

            #region LayerView
            routes.MapRoute(
                "AdminLayerView",
                "{controller}/LayerView/{action}",
                new
                {
                    controller = "Admin",
                    action = UrlParameter.Optional
                });

            routes.MapRoute(
                "AdminLayerViewPar",
                "{controller}/LayerView/{action}/{id}",
                new
                {
                    controller = "Admin",
                    action = UrlParameter.Optional,
                    id = UrlParameter.Optional
                });
            #endregion

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );


           

        }
    }
}
