using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DreamCMS.Models
{
    [Table("Setting")]
    public class Setting
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int SettingId { get; set; }

        [MaxLength(20)]
        public string Key { get; set; }
        public string Value { get; set; }

        //GET KEY = bla bla bla
        public static string Get(string key)
        {
            DBFrontEnd db = new DBFrontEnd();
            Setting _v = db.Settings.SingleOrDefault(o => o.Key == key);
            if (_v != null)
            {
                if (_v.Value == null)
                    return "";
                else
                    return _v.Value;
            }
            return "";
        }

        //SET KEY = bla bla...
        public static void Set(string key, string value)
        {
            DBFrontEnd db = new DBFrontEnd();
            Setting _v = db.Settings.SingleOrDefault(o => o.Key == key);
            if (_v != null)
            {
                //UPDATE VALUE
                _v.Value = value;
            }
            else
            {
                //CREAT VALUE
                Setting _n = new Setting();
                _n.Key = key;
                _n.Value = value;
                db.Settings.Add(_n);
            }
            db.SaveChanges();
        }

        //REMOVE VALUE
        public static void Remove(string key)
        {
            DBFrontEnd db = new DBFrontEnd();
            Setting _v = db.Settings.SingleOrDefault(o => o.Key == key);
            if (_v != null)
            {
                db.Settings.Remove(_v);
                db.SaveChanges();
            }
        }
    }

    public class SettingModel
    {
        [ScaffoldColumn(false)]
        public string ErrorMessage { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Tên công ty")]
        public string CompanyName { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Thông tin liên hệ đầu trang")]
        public string TopContact { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Thông tin liên hệ cuối trang")]
        public string BottomContact { get; set; }        

        [DataType(DataType.Text)]
        [Display(Name = "Hình nền trang web")]
        public string BackgroundWebUrl { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Hình nền cuối trang")]
        public string BottomBackgroudUrl { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Link Logo công ty LỚN")]
        public string LogoUrlBig { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Link Logo công ty NHỎ")]
        public string LogoUrlSmall { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Link Video Youtube")]
        public string VideoYoutube { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Link Fanpage Facebook")]
        public string FacebookFanpage { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Email nhận thông báo liên hệ")]
        public string EmailReceiveContact { get; set; }     

        [DataType(DataType.MultilineText)]
        [Display(Name = "Ví trị Google Map")]
        public string GoogleMap { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Google Analytics")]
        public string GoogleAnalytics { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Plugin khác")]
        public string PluginOther { get; set; }

    }
}
