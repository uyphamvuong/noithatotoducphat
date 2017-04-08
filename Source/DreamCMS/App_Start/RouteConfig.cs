using System.Web.Mvc;
using System.Web.Routing;

namespace DreamCMS
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Giới thiệu
            routes.MapRoute(
                name: "GioiThieu",
                url: "gioi-thieu",
                defaults: new { controller = "Home", action = "ViewPages", titleid="gioi-thieu" }
            );

            // Giới thiệu
            routes.MapRoute(
                name: "LienHe",
                url: "lien-he",
                defaults: new { controller = "Home", action = "Contact" }
            );

            // Hoạt động
            routes.MapRoute(
                name: "Activities",
                url: "hoat-dong",
                defaults: new { controller = "Activity", action = "Index" }
            );

            routes.MapRoute(
                name: "ActivitiesDetail",
                url: "hoat-dong/{titleid}",
                defaults: new { controller = "Activity", action = "Detail", titleid = UrlParameter.Optional }
            );

            // Tin tức
            routes.MapRoute(
                name: "Newss",
                url: "tin-tuc",
                defaults: new { controller = "Activity", action = "IndexNews" }
            );

            routes.MapRoute(
                name: "NewssDetail",
                url: "tin-tuc/{titleid}",
                defaults: new { controller = "Activity", action = "DetailNews", titleid = UrlParameter.Optional }
            );

            // sản phẩm
            routes.MapRoute(
                name: "ProductRoot",
                url: "san-pham",
                defaults: new { controller = "Product", action = "Index" }
            );

            routes.MapRoute(
                name: "ProductGroup",
                url: "san-pham/{titleidgroup}",
                defaults: new { controller = "Product", action = "ViewGroup", titleidgroup = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ProductDetail",
                url: "san-pham/{titleidgroup}/{titleid}",
                defaults: new { controller = "Product", action = "ViewDetail", titleidgroup = UrlParameter.Optional, titleid = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "giohang",
               url: "gio-hang",
               defaults: new { controller = "ShoppingCart", action = "Index" }
           );
            // Default
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );            
        }
    }
}
