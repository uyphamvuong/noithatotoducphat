using System.Web.Mvc;

namespace DreamCMS.Areas.Admin
{
    internal static class FilterConfig
    {
        internal static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}