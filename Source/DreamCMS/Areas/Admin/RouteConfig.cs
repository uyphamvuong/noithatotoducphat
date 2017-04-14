using System.Web.Mvc;

namespace DreamCMS.Areas.Admin
{
    internal static class RouteConfig
    {
        internal static void RegisterRoutes(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { area = "Admin", controller = "Cpanel", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}