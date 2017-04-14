using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DreamCMS.Encrypt;
using DreamCMS.ModelsAdmin;

namespace DreamCMS.Authorize
{
    /// <summary>
    /// Class Auth gồm các hàm bảo mật, đăng nhập,...
    /// </summary>
    public class Auth
    {
        public static int _Auth = 0;

        #region # Properties #

        /// <summary>
        /// Thuộc tính trả path redirect khi bị lỗi
        /// </summary>
        public static string PathRedirect
        {
            get
            {
                switch (_Auth)
                {
                    case 0: // Not permission (AccessDenied)
                        return DDefault.PathAccessDenied;
                    case -2: // Login
                        return DDefault.PathLogin;
                    case -1: // 404 Area Admin
                        return DDefault.Path404;
                    default: //-1: Page Not Found
                        return DDefault.Path404;
                }
            }
        }

        /// <summary>
        /// Lấy IP
        /// </summary>
        /// <param name="request">ct.ControllerContext.HttpContext.Request đối với Controller</param>
        /// <returns></returns>
        public static string IPAddress
        {
            get
            {
                string ip;
                try
                {
                    ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                    if (!string.IsNullOrEmpty(ip))
                    {
                        if (ip.IndexOf(",") > 0)
                        {
                            string[] ipRange = ip.Split(',');
                            ip = ipRange[ipRange.Length - 1];
                        }
                    }
                    else
                    {
                        ip = HttpContext.Current.Request.UserHostAddress;
                    }
                }
                catch { ip = null; }
                return ip;
            }
        }

        #endregion

        #region # Function #

        /// <summary>
        /// Hàm check có đúng là area hay ko
        /// </summary>
        /// <param name="ct">Control hiện tại (this)</param>
        /// <param name="area">Area hiện tại</param>
        /// <returns>int</returns>
        public static bool Not404(Controller ct, string area)
        {
            string AreaName = ct.ControllerContext.RouteData.DataTokens["area"]!=null?ct.ControllerContext.RouteData.DataTokens["area"].ToString():"";

            if (AreaName == area) { return true; }

            HttpContext.Current.Session["ErrorHttp"] = ct.Request.RawUrl;
            return false;
        }

        /// <summary>
        /// Hàm check quyền của control Admin
        /// </summary>
        /// <param name="ct">Control hiện tại (this)</param>
        /// <returns>int</returns>
        public static int IsUse(Controller ct, string area = "")
        {
            string ActionName = ct.ControllerContext.RouteData.Values["action"].ToString().ToLower();
            string ControllerName = ct.ControllerContext.RouteData.Values["controller"].ToString().ToLower();
            string AreaName = ct.ControllerContext.RouteData.DataTokens["area"] != null ? ct.ControllerContext.RouteData.DataTokens["area"].ToString().ToLower() : "";
            string MethodName = ct.ControllerContext.HttpContext.Request.HttpMethod;
            //string Ip = IPAddress;

            HttpCookie getCookie = HttpContext.Current.Request.Cookies[DDefault.NameCookieRemember];
            if (getCookie != null) {
                if (GetCookie("DUser") == null)
                {
                    SetCookie("DUser", DHash.Decrypt(getCookie.Value));
                    if (HttpContext.Current.Session["DUserName"] == null) { ReloadInfoUser(); }
                }
            }
            string DUser = GetCookie("DUser");            

            /// Kiểm tra các lỗi xảy ra
            /// Nếu không đúng phân vùng Area: Thông báo lỗi 404
            if (area!="" && AreaName != area.ToLower())
            {
                HttpContext.Current.Session["ErrorHttp"] = ct.Request.RawUrl;
                _Auth = -1;
                return -1;
            }

            /// Nếu chưa đăng nhập
            /// Chuyển về trang đăng nhập 
            if(DUser==null)
            {
                HttpContext.Current.Session["returnUrl"] = ct.Request.RawUrl;
                _Auth = -2;
                return -2;
            }

            /// Đặc quyền cho SUPERADMIN
            if (DUser == DDefault.SAdminID)
            {
                HttpContext.Current.Session["ErrorHttp"] = "";
                HttpContext.Current.Session["returnUrl"] = "";
                _Auth = 1;
                LoadBreadcrumb(ct);
                LoadMenuLeft(ct);
                return 1;
            }

            DBAdmin db = new DBAdmin();
            int? GroupId = int.Parse(GetCookie("DGroupId")) as int?;
            tbMenuInGroup tbMenuInGroup = new tbMenuInGroup(db.tbMenus.Where(p => p.IsDisable == false && p.Controller.ToLower() == ControllerName && p.Action.ToLower() == ActionName).FirstOrDefault(), GroupId.Value);
            if (tbMenuInGroup.IsIn)
            {
                /// Trường hợp thỏa các điều kiện
                HttpContext.Current.Session["ErrorHttp"] = "";
                HttpContext.Current.Session["returnUrl"] = "";
                _Auth = 1;
                LoadBreadcrumb(ct);
                LoadMenuLeft(ct);
                return 1;
            }
            else
            {
                /// Nếu tài khoản không đủ quyền
                /// Chuyển về trang thông báo quyền truy cập hoặc 404
                HttpContext.Current.Session["ErrorHttp"] = ct.Request.RawUrl;
                _Auth = 0;
                return 0;

            }
        }

        public static void ReloadInfoUser()
        {
            string DUser = GetCookie("DUser");
            if (DUser == DDefault.SAdminID)
            {
                HttpContext.Current.Session["DUserName"] = "Super Administrator";
                HttpContext.Current.Session["DUserAvatar"] = "defaultavatar.png";
                SetCookie("DGroupId", 0);
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
                if (tbMemberGroup == null) { SetCookie("DGroupId", -1); }
                else { SetCookie("DGroupId", tbMemberGroup.tbGroupId); }
            
            }
        }

        /// <summary>
        /// Hàm kiểm tra dành cho đăng nhập
        /// <para>Được sử riêng cho trang Login</para>
        /// </summary>
        /// <param name="username">Tài khoản</param>
        /// <param name="password">Mật khẩu</param>
        /// <param name="remember">Ghi nhớ tài khoản bằng Cookie</param>
        /// <returns>bool</returns>
        public static bool CheckLogin(string username, string password, bool remember = false)
        {
            if (username == DDefault.SAdminID && password == DDefault.SAdminPW)
            {
                //Setup quyền cho Supper Admin
                SetCookie("DUser", DDefault.SAdminID);
                ReloadInfoUser();

                if (remember)
                {
                    SetCookie(DDefault.NameCookieRemember, username, 24*DDefault.DayCookiesLogin);
                }
                else
                {
                    ClearCookie(DDefault.NameCookieRemember);
                }
                return true;
            }

            //Check tài khoàn đăng nhập = database thông thường
            else
            {
                DBAdmin db = new DBAdmin();
                password = DHash.Encrypt(password);
                tbUser tbUser = db.tbUsers.Where(p => p.Username == username && p.Password == password).FirstOrDefault();
                if (tbUser == null)
                {
                    return false;
                }

                SetCookie("DUser", tbUser.Username);
                ReloadInfoUser();

                if (remember)
                {
                    SetCookie(DDefault.NameCookieRemember, username);
                }
                else
                {
                    ClearCookie(DDefault.NameCookieRemember);
                }                
                return true;
            }
        }

        /// <summary>
        /// Hàm đăng xuất
        /// <para>Xóa session và cookie DUser</para>
        /// </summary>
        public static void Logoff()
        {
            _Auth = 0;
            ClearCookie("DUser");
            ClearCookie("DGroupId");
            ClearCookie("CKFinder_Path");
            ClearCookie(DDefault.NameCookieRemember);            
        }

        /// <summary>
        /// Hàm tạo menu path
        /// </summary>
        /// <param name="ct">Controller hiện tại</param>
        private static void LoadBreadcrumb(Controller ct)
        {
            string ActionName = ct.ControllerContext.RouteData.Values["action"].ToString();
            string ControllerName = ct.ControllerContext.RouteData.Values["controller"].ToString();
            string AreaName = ct.ControllerContext.RouteData.DataTokens["area"] != null ? ct.ControllerContext.RouteData.DataTokens["area"].ToString() : "";

            Dictionary<string, string> br = new Dictionary<string, string>();
            br.Add(AreaName, "/" + AreaName);
            br.Add(ControllerName, "/" + AreaName + "/" + ControllerName);
            if(ActionName!="Index")
                br.Add(ActionName, "/" + AreaName + "/" + ControllerName + "/" + ActionName);

            ct.ViewBag.Breadcrumb = br;
        }

        private static void LoadMenuLeft(Controller ct)
        {
            string ActionName = ct.ControllerContext.RouteData.Values["action"].ToString();
            string ControllerName = ct.ControllerContext.RouteData.Values["controller"].ToString();
            string AreaName = ct.ControllerContext.RouteData.DataTokens["area"] != null ? ct.ControllerContext.RouteData.DataTokens["area"].ToString() : "";
            int? GroupId = int.Parse(GetCookie("DGroupId")) as int?;

            DBAdmin db = new DBAdmin();

            tbMenu root = db.tbMenus.Where(p=>p.MenuName == "Root").FirstOrDefault();

            List<tbMenuInGroup> ListMenu = new List<tbMenuInGroup>();
            if (root != null)
            {
                List<tbMenu> ListT = db.tbMenus.Where(p => p.IdRoot == root.tbMenuId && p.IsDisable == false && p.IsMenu == true).OrderBy(x => x.Order).ToList();
                if (ListT != null) { ListT.ForEach(x => ListMenu.Add(new tbMenuInGroup(x, GroupId.Value))); }
            }
            
            ct.ViewBag.ListMenu = ListMenu;
        }

        public static void SetCookie(string key, object value, int timehour = 5)
        {
            // create a cookie
            HttpCookie newCookie = new HttpCookie(key, DHash.Encrypt(value.ToString()));
            if (timehour == 5) { timehour = DDefault.HourCookiesSession; }
            newCookie.Expires = DateTime.Now.AddHours(timehour);
            newCookie.HttpOnly = true;
            HttpContext.Current.Response.Cookies.Add(newCookie);
        }
        public static void ClearCookie(string key)
        {
            // clear cookie
            HttpCookie clearCookie = HttpContext.Current.Response.Cookies[key];
            if (clearCookie != null) { clearCookie.Expires = DateTime.Today.AddDays(-1); }
        }
        public static string GetCookie(string key)
        {
            HttpCookie getCookie = HttpContext.Current.Request.Cookies[key];
            if (getCookie != null) { return DHash.Decrypt(getCookie.Value); }
            else { return null; }
        }
        #endregion

    }
}
