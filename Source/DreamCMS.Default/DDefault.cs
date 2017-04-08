using System.Web.Hosting;

namespace DreamCMS
{
    /// <summary>
    /// Giá trị mặc định của DreamCMS mà khó thay đổi
    /// </summary>
    public class DDefault
    {
        #region # DATABASE #
        public static string
            DefaultConnection = "DefaultConnection";
        #endregion

        #region # Path #
        static public string
            Path404 = "/Error/HttpError404",
            PathLogin = "/Admin/Account/Login",
            PathAccessDenied = "/Error/AccessDenied",

            PathFileConfig = HostingEnvironment.ApplicationPhysicalPath + "/cms/DreamCMS.xml";
        #endregion

        #region # Account SuperAdmin #
        static public string
            SAdminID = "sadmin",
            SAdminPW = "sadmin123";
        #endregion

        #region # Cookies #
        static public string
            NameCookieRemember = "DreamCMS_RememberMe";       

        static public int
            DayCookiesLogin = 3,
            HourCookiesSession = 5;
        #endregion

        #region # Paging #
        public static int DefaultPageSize = 7;
        #endregion

    }
}