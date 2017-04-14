using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DreamCMS.Models
{
    [Table("Contact")]
    public class Contact
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [ScaffoldColumn(true)]
        public int ContactId { get; set; }

        [Required(ErrorMessage = "{0} không được để trống!")]
        [DataType(DataType.Text)]
        [Display(Name = "Họ và tên")]
        [StringLength(70, MinimumLength = 3, ErrorMessage = "{0} từ {2} - {1} kí tự")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "{0} không được để trống!")]
        [DataType(DataType.Text)]
        [Display(Name = "Địa chỉ")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "{0} từ {2} - {1} kí tự")]
        public string Address { get; set; }

        [Required(ErrorMessage = "{0} không được để trống!")]
        [DataType(DataType.Text)]
        [Display(Name = "Điện thoại")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "{0} từ {2} - {1} kí tự")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "{0} không được để trống!")]
        [DataType(DataType.Text)]
        [Display(Name = "Email")]
        [RegularExpression(@"^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$", ErrorMessage="Bạn đã nhập sai định dạng email!!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} không được để trống!")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Nội dung")]
        [StringLength(300, MinimumLength = 3, ErrorMessage = "{0} từ {2} - {1} kí tự")]
        public string Content { get; set; }

        [ScaffoldColumn(false)]
        public bool IsSeen { get; set; }
    }
}
