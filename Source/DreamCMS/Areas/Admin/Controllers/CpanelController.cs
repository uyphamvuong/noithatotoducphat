using DreamCMS.Authorize;
using DreamCMS.Config;
using DreamCMS.Models;
using System.Web.Mvc;
using System.Linq;

namespace DreamCMS.Areas.Admin.Controllers
{
    public class CpanelController : Controller
    {

        private DBFrontEnd db = new DBFrontEnd();

        // GET: Admin/Cpanel
        [Auth]
        public ActionResult Index()
        {
            if (Auth.GetCookie("DUser") == DDefault.SAdminID && !DConfig.IsCheck) { return RedirectToAction("Index","Config"); }

            ViewBag.CountIsSeenContact = db.Contacts.Where(x => x.IsSeen == false).Count();
            ViewBag.CountIsSeenOrder = db.Orders.Where(x => x.IsSeen == false).Count();

            return View();
        }

        [Auth]
        public ActionResult Settings()
        {
            SettingModel sm = new SettingModel();
            sm.BackgroundWebUrl = Setting.Get("BackgroundWebUrl");
            sm.BottomBackgroudUrl = Setting.Get("BottomBackgroudUrl");
            sm.BottomContact = Setting.Get("BottomContact");
            sm.CompanyName = Setting.Get("CompanyName");
            sm.FacebookFanpage = Setting.Get("FacebookFanpage");
            sm.EmailReceiveContact = Setting.Get("EmailReceiveContact");
            sm.GoogleAnalytics = Setting.Get("GoogleAnalytics");
            sm.LogoUrlBig = Setting.Get("LogoUrlBig");
            sm.LogoUrlSmall = Setting.Get("LogoUrlSmall");
            sm.TopContact = Setting.Get("TopContact");
            sm.VideoYoutube = Setting.Get("VideoYoutube");
            sm.PluginOther = Setting.Get("PluginOther");
            sm.GoogleMap = Setting.Get("GoogleMap");

            ViewBag.IsSuccess = false;
            return View(sm);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [Auth]
        public ActionResult Settings(SettingModel sm)
        {
            sm.ErrorMessage = "Cập nhật dữ liệu THẤT BẠI";
            ViewBag.IsSuccess = false;

            if(ModelState.IsValid)
            {
                Setting.Set("BackgroundWebUrl", sm.BackgroundWebUrl);
                Setting.Set("BottomBackgroudUrl", sm.BottomBackgroudUrl);
                Setting.Set("BottomContact", sm.BottomContact);
                Setting.Set("CompanyName", sm.CompanyName);
                Setting.Set("FacebookFanpage", sm.FacebookFanpage);
                Setting.Set("EmailReceiveContact", sm.EmailReceiveContact);
                Setting.Set("GoogleAnalytics", sm.GoogleAnalytics);
                Setting.Set("LogoUrlBig", sm.LogoUrlBig);
                Setting.Set("LogoUrlSmall", sm.LogoUrlSmall);
                Setting.Set("TopContact", sm.TopContact);
                Setting.Set("VideoYoutube", sm.VideoYoutube);
                Setting.Set("PluginOther", sm.PluginOther);
                Setting.Set("GoogleMap", sm.GoogleMap);

                sm.ErrorMessage = "Cập nhật dữ liệu THÀNH CÔNG";
                ViewBag.IsSuccess = true;
            }

            return View(sm);
        }

    }
}