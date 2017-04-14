using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace DreamCMS
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            Response.Clear();

            HttpException httpException = exception as HttpException;

            if (httpException != null)
            {
                string action = "";

                switch (httpException.GetHttpCode())
                {
                    case 404:
                        // page not found
                        action = "HttpError404";
                        break;                        
                    case 400:
                        // Bad Request
                        action = "HttpError404";
                        break;
                    //case 500:
                    //    // server error
                    //    action = "HttpError500";
                    //    break;
                    //default:
                    //    action = "General";
                    //    break;
                }

                // clear error on server
                Server.ClearError();

                try
                {
                    Session["ErrorHttp"] = Request.Path != null ? Request.Path : exception.Message;
                    Response.Redirect(String.Format("~/Error/{0}", action));
                }
                catch
                {
                }
            }
        }

        //protected void Session_Start(object sender, EventArgs e)
        //{
        //    Session.Timeout = 300;
        //}
    }
}
