using DreamCMS.Encrypt;
using DreamCMS.ModelsAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DreamCMS.Authorize
{
    /// <summary>
    /// Class AuthAttribute gồm các hàm bảo mật, đăng nhập,...
    /// </summary>
    public class AuthAttribute : ActionFilterAttribute
    {

        public bool IsNotLayout { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filter)
        {
            string ActionName = filter.RouteData.Values["action"].ToString().ToLower();
            string ControllerName = filter.RouteData.Values["controller"].ToString().ToLower();
            string AreaName = filter.RouteData.DataTokens["area"] != null ? filter.RouteData.DataTokens["area"].ToString().ToLower() : "";
            string MethodName = filter.HttpContext.Request.HttpMethod;
            
            // GET AREAS
            string Areas = "";
            string[] arrT = filter.Controller.GetType().ToString().Split('.');
            if (arrT.Length >= 3) //DreamCMS.Areas.Admin.Controllers. ...
                if (arrT[1] == "Areas") { Areas = arrT[2]; }

            HttpCookie getCookie = HttpContext.Current.Request.Cookies[DDefault.NameCookieRemember];
            if (getCookie != null)
            {
                if (Auth.GetCookie("DUser") == null)
                {
                    Auth.SetCookie("DUser", DHash.Decrypt(getCookie.Value));                    
                }
                if (filter.HttpContext.Session["DUserName"] == null) { ReloadInfoUser(); }
            }
            string DUser = Auth.GetCookie("DUser");

            /// Kiểm tra các lỗi xảy ra
            /// Nếu không đúng phân vùng Area: Thông báo lỗi 404
            if (Areas != "" && AreaName != Areas.ToLower())
            {
                filter.HttpContext.Session["ErrorHttp"] = filter.HttpContext.Request.RawUrl;
                filter.Result = new RedirectResult(DDefault.Path404);
                return;
            }

            /// Nếu chưa đăng nhập
            /// Chuyển về trang đăng nhập 
            if (DUser == null)
            {
                filter.HttpContext.Session["returnUrl"] = filter.HttpContext.Request.RawUrl;
                filter.Result = new RedirectResult(DDefault.PathLogin);
                return;
            }

            /// Đặc quyền cho SUPERADMIN
            if (DUser == DDefault.SAdminID)
            {
                filter.HttpContext.Session["ErrorHttp"] = "";
                filter.HttpContext.Session["returnUrl"] = "";
                if(!IsNotLayout)
                {
                    IsNotLayout = false;
                    LoadBreadcrumb(filter);
                    LoadMenuLeft(filter);
                }                
                base.OnActionExecuting(filter);
                return;
            }

            DBAdmin db = new DBAdmin();
            int? GroupId = int.Parse(Auth.GetCookie("DGroupId")) as int?;
            tbMenuInGroup tbMenuInGroup = new tbMenuInGroup(db.tbMenus.Where(p => p.IsDisable == false && p.Controller.ToLower() == ControllerName && p.Action.ToLower() == ActionName).FirstOrDefault(), GroupId.Value);
            if (tbMenuInGroup.IsIn)
            {
                /// Trường hợp thỏa các điều kiện
                filter.HttpContext.Session["ErrorHttp"] = "";
                filter.HttpContext.Session["returnUrl"] = "";
                if (!IsNotLayout)
                {
                    IsNotLayout = false;
                    LoadBreadcrumb(filter);
                    LoadMenuLeft(filter);
                }
                base.OnActionExecuting(filter);
                return;
            }
            else
            {
                /// Nếu tài khoản không đủ quyền
                /// Chuyển về trang thông báo quyền truy cập hoặc 404
                HttpContext.Current.Session["ErrorHttp"] = filter.HttpContext.Request.RawUrl;
                filter.Result = new RedirectResult(DDefault.PathAccessDenied);
                return;
            }            
        }

        public static void ReloadInfoUser()
        {
            string DUser = Auth.GetCookie("DUser");
            if (DUser == DDefault.SAdminID)
            {
                HttpContext.Current.Session["DUserName"] = "Super Administrator";
                HttpContext.Current.Session["DUserAvatar"] = "defaultavatar.png";
                Auth.SetCookie("DGroupId", 0);
            }
            else
            {
                DBAdmin db = new DBAdmin();
                tbUser tbUser = db.tbUsers.Where(p => p.Username == DUser).FirstOrDefault();
                if (tbUser == null) { return; }
                HttpContext.Current.Session["DUserName"] = tbUser.Fullname;
                HttpContext.Current.Session["DUserAvatar"] = string.IsNullOrEmpty(tbUser.AvatarUrl) ? "defaultavatar.png" : tbUser.AvatarUrl;

                // tbGroupId
                tbGroupUser tbMemberGroup = db.tbGroupUsers.Where(x => x.tbUserId == tbUser.tbUserId).FirstOrDefault();
                if (tbMemberGroup == null) { Auth.SetCookie("DGroupId", -1);}
                else { Auth.SetCookie("DGroupId", tbMemberGroup.tbGroupId); }

            }
        }

        private static void LoadBreadcrumb(ActionExecutingContext filterContext)
        {
            string ActionName = filterContext.RouteData.Values["action"].ToString();
            string ControllerName = filterContext.RouteData.Values["controller"].ToString();
            string AreaName = filterContext.RouteData.DataTokens["area"] != null ? filterContext.RouteData.DataTokens["area"].ToString() : "";

            Dictionary<string, string> br = new Dictionary<string, string>();
            br.Add(AreaName, "/" + AreaName);
            br.Add(ControllerName, "/" + AreaName + "/" + ControllerName);
            if (ActionName != "Index")
                br.Add(ActionName, "/" + AreaName + "/" + ControllerName + "/" + ActionName);

            filterContext.Controller.ViewBag.Breadcrumb = br;
        }

        private static void LoadMenuLeft(ActionExecutingContext filterContext)
        {
            string ActionName = filterContext.RouteData.Values["action"].ToString();
            string ControllerName = filterContext.RouteData.Values["controller"].ToString();
            string AreaName = filterContext.RouteData.DataTokens["area"] != null ? filterContext.RouteData.DataTokens["area"].ToString() : "";

            int? GroupId = int.Parse(Auth.GetCookie("DGroupId")) as int?;

            DBAdmin db = new DBAdmin();

            tbMenu root = db.tbMenus.Where(p => p.MenuName == "Root").FirstOrDefault();

            List<tbMenuInGroup> ListMenu = new List<tbMenuInGroup>();
            if (root != null)
            {
                List<tbMenu> ListT = db.tbMenus.Where(p => p.IdRoot == root.tbMenuId && p.IsDisable == false && p.IsMenu == true).OrderBy(x=>x.Order).ToList();
                if (ListT != null) { ListT.ForEach(x => ListMenu.Add(new tbMenuInGroup(x, GroupId.Value))); }
            }

            filterContext.Controller.ViewBag.ListMenu = ListMenu;
        }
    }
}
