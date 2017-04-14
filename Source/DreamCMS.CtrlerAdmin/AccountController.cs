using DreamCMS.Authorize;
using DreamCMS.ModelsAdmin;
using DreamCMS.Upload;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DreamCMS.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        private DBAdmin db = new DBAdmin();

        // GET: Admin/Account
        [Auth]
        public ActionResult Index()
        {
            string userid = Auth.GetCookie("DUser");

            // Nếu là tài khoản thường
            if (userid != DDefault.SAdminID)
            {
                tbUser tbUser = db.tbUsers.Where(x => x.Username == userid).FirstOrDefault();
                if (tbUser == null)
                {
                    return HttpNotFound();
                }
                tbUser.Password = DreamCMS.Encrypt.DHash.Decrypt(tbUser.Password);
                return View("__Cms/Account/Profile", tbUser);
            }

            return View("__Cms/Account/Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth]
        public ActionResult Index([Bind(Include = "tbUserId,Username,Fullname,Password,IsDisable")] tbUser tbUser, HttpPostedFileBase file)
        {
            string userid = Auth.GetCookie("DUser");
            tbUser tbUserOld = db.tbUsers.Where(x => x.Username == userid).FirstOrDefault();
            if (tbUser == null)
            {
                return HttpNotFound();
            }            

            if (ModelState.IsValid)
            {

                tbUserOld.Fullname = tbUser.Fullname;
                tbUserOld.Password = DreamCMS.Encrypt.DHash.Encrypt(tbUser.Password);

                // AVATAR
                if (file != null)
                {
                    if (HttpPostedFileBaseExtensions.IsImage(file))
                    {
                        string ext = Path.GetExtension(file.FileName).ToLower();
                        //string pic = Path.GetFileName(file.FileName);
                        string path = Path.Combine(Server.MapPath("~/Areas/Admin/upload/avatar"), tbUserOld.Username + ext);
                        //delele old file
                        if (!string.IsNullOrEmpty(tbUserOld.AvatarUrl))
                        {
                            string fullPath = Path.Combine(Server.MapPath("~/Areas/Admin/upload/avatar"), tbUserOld.AvatarUrl);
                            if (System.IO.File.Exists(fullPath))
                            {
                                System.IO.File.Delete(fullPath);
                            }
                        }
                        // file is uploaded
                        file.SaveAs(path);

                        tbUserOld.AvatarUrl = tbUser.Username + ext;
                    }
                }
                
                db.SaveChanges();
                Auth.ReloadInfoUser();

                ViewBag.MsgResult = true;

                tbUserOld.Password = DreamCMS.Encrypt.DHash.Decrypt(tbUserOld.Password);
                return View("__Cms/Account/Profile", tbUserOld);
            }

            ViewBag.MsgResult = false;
            ViewBag.MsgText = "Cập nhật thông tin thất bại!!!";

            tbUserOld.Password = DreamCMS.Encrypt.DHash.Decrypt(tbUserOld.Password);
            return View("__Cms/Account/Profile", tbUserOld);
        }

        // GET: Admin/Login
        public ActionResult Login(string returnUrl)
        {
            if(returnUrl!=null)
                ViewBag.ReturnUrl = returnUrl;
            else if (Session["returnUrl"]!=null)
                ViewBag.ReturnUrl = Session["returnUrl"];
            return View("__Cms/Account/Login");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginOrForgotPasswordModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Auth.CheckLogin(model.LoginModel.Username,model.LoginModel.Password, model.LoginModel.RememberMe))
                {
                    return RedirectToLocal(returnUrl);
                }

                model.LoginModel.ErrorMsg = "Sai tài khoản hoặc mật khẩu.";
            }

            if (returnUrl != null)
                ViewBag.ReturnUrl = returnUrl;
            else if (Session["returnUrl"] != null)
                ViewBag.ReturnUrl = Session["returnUrl"];
            // If we got this far, something failed, redisplay form
            return View("__Cms/Account/Login", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            Auth.Logoff();
            return RedirectToAction("Login");
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Cpanel");
        }


    }
     
}