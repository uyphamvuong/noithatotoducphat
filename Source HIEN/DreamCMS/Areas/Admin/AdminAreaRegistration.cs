using System.Web.Mvc;
using System.Web.Optimization;

namespace DreamCMS.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(context);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

    }
}