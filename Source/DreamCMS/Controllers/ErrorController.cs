using System.Web.Mvc;

namespace DreamCMS.Controllers
{
    public class ErrorController : Controller
    {
        // GET: /Error
        public ActionResult Index()
        {
            return Redirect("/");
        }

        // GET: /Error/HttpError404
        public ActionResult HttpError404()
        {
            return View();
        }

        public ActionResult HttpError500()
        {
            return View();
        }

        public ActionResult General()
        {
            return View();
        }

        public ActionResult AccessDenied()
        {
            return View();
        }

    }
}