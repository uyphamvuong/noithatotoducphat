using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DreamCMS.ModelsAdmin
{
    [Table("tbUser")]
    public class tbUser
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [ScaffoldColumn(true)]
        public int tbUserId { get; set; }

        [Required(ErrorMessage = "{0} không được để trống!")]
        [DataType(DataType.Text)]
        [Display(Name = "Tên tài khoản")]
        [Column(TypeName = "varchar")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "{0} từ {2} - {1} kí tự")]
        //[Remote("IsNameAvailble", "Users", "Admin", ErrorMessage = "{0} đã tồn tại!!!")]
        public string Username { get; set; }

        [Required(ErrorMessage = "{0} không được để trống!")]
        [DataType(DataType.Text)]
        [Display(Name = "Tên đầy đủ")]
        [MaxLength(30, ErrorMessage = "{0} không quá {1} kí tự")]
        public string Fullname { get; set; }

        [Required(ErrorMessage = "{0} không được để trống!")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        [Column(TypeName = "varchar")]
        [StringLength(50, MinimumLength=6, ErrorMessage = "{0} từ {2} - {1} kí tự")]
        public string Password { get; set; }

        [Display(Name = "Khóa tài khoản")]
        public bool IsDisable { get; set; }

        [Display(Name = "Link hình đại diện")]
        [Column(TypeName="varchar")]
        [MaxLength(200, ErrorMessage="{0} không quá {1} kí tự")]
        public string AvatarUrl { get; set; }
    }
}