using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ElectroWeb
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "Products",
                url: "san-pham",
                defaults: new { controller = "Product", action = "Index", alias = UrlParameter.Optional },
                namespaces: new[] { "ElectroWeb.Controllers" }
            );
            routes.MapRoute(
                name: "ProductCategory",
                url: "danh-muc/{alias}-{id}",
                defaults: new { controller = "Product", action = "ProductsByCategory", id = UrlParameter.Optional },
                namespaces: new[] { "ElectroWeb.Controllers" }
            );
            routes.MapRoute(
                name: "DetailProduct",
                url: "chi-tiet-san-pham/{alias}-prod{id}",
                defaults: new { controller = "Product", action = "ProductDetail", alias = UrlParameter.Optional },
                namespaces: new[] { "ElectroWeb.Controllers" }
            );
            routes.MapRoute(
                name: "Home",
                url: "trang-chu",
                defaults: new { controller = "Home", action = "Index", alias = UrlParameter.Optional },
                namespaces: new[] { "ElectroWeb.Controllers" }
            );
            routes.MapRoute(
                name: "Contacts",
                url: "lien-he",
                defaults: new { controller = "Contacts", action = "Index", alias = UrlParameter.Optional },
                namespaces: new[] { "ElectroWeb.Controllers" }
            );
            routes.MapRoute(
                 name: "Cart",
                 url: "gio-hang",
                 defaults: new { controller = "Cart", action = "Index", alias = UrlParameter.Optional },
                 namespaces: new[] { "ElectroWeb.Controllers" }
             );
            routes.MapRoute(
                 name: "Checkout",
                 url: "thanh-toan",
                 defaults: new { controller = "Cart", action = "Checkout", alias = UrlParameter.Optional },
                 namespaces: new[] { "ElectroWeb.Controllers" }
             );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "ElectroWeb.Controllers" }
            );
        }
    }
}
