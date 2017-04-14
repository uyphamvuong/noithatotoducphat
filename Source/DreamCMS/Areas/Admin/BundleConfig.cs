using System.Web.Optimization;

namespace DreamCMS.Areas.Admin
{
    internal static class BundleConfig
    {
        internal static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/admin/css_global").Include(                      
                      "~/Areas/Admin/assets/plugins/bootstrap/css/bootstrap.min.css",
                      "~/Areas/Admin/assets/plugins/uniform/css/uniform.default.css"));

            bundles.Add(new StyleBundle("~/admin/css_theme").Include(
                      "~/Areas/Admin/assets/css/style-metronic.css",
                      "~/Areas/Admin/assets/css/style.css",
                      "~/Areas/Admin/assets/css/style-responsive.css",
                      "~/Areas/Admin/assets/css/plugins.css",
                      "~/Areas/Admin/assets/css/themes/default.css",
                      "~/Areas/Admin/assets/css/custom.css"));

            bundles.Add(new ScriptBundle("~/admin/js_core").Include(
                      "~/Areas/Admin/assets/plugins/jquery-1.10.2.min.js",
                      "~/Areas/Admin/assets/plugins/jquery-migrate-1.2.1.min.js",
                      "~/Areas/Admin/assets/plugins/jquery-ui/jquery-ui-1.10.3.custom.min.js",
                      "~/Areas/Admin/assets/plugins/bootstrap/js/bootstrap.min.js",
                      "~/Areas/Admin/assets/plugins/bootstrap-hover-dropdown/bootstrap-hover-dropdown.min.js",
                      "~/Areas/Admin/assets/plugins/jquery-slimscroll/jquery.slimscroll.min.js",
                      "~/Areas/Admin/assets/plugins/jquery.blockui.min.js",
                      "~/Areas/Admin/assets/plugins/jquery.cokie.min.js",
                      "~/Areas/Admin/assets/plugins/uniform/jquery.uniform.min.js"));

            bundles.Add(new ScriptBundle("~/admin/js_core_app").Include(
                      "~/Areas/Admin/assets/scripts/core/app.js"));

        }
    }
}