using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DreamCMS.Models
{
    [Table("Product")]
    public class Product
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "{0} không được để trống!")]
        [DataType(DataType.Text)]
        [Display(Name = "Tên sản phẩm")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "{0} từ {2} - {1} kí tự")]
        public string ProductName { get; set; }

        [Display(Name = "SeoUrl")]
        [MaxLength(100, ErrorMessage = "{0} không quá {1} kí tự")]
        public string SeoUrl { get; set; }

        [Display(Name = "Link hình sản phẩm")]
        [Column(TypeName = "varchar")]
        [MaxLength(200, ErrorMessage = "{0} không quá {1} kí tự")]
        public string ImgUrl { get; set; }

        [Display(Name = "Ưu tiên")]
        public int? Order { get; set; }

        [Display(Name = "Giá sản phẩm")]
        [MaxLength(20, ErrorMessage = "{0} không quá {1} kí tự")]
        public string Price{ get; set; }

        [Display(Name = "Khóa sản phẩm")]
        public bool IsDisable { get; set; }

        [Display(Name = "Khuyến mãi sản phẩm")]
        public bool IsKM{ get; set; }

        [Display(Name = "Nhóm sản phẩm")]
        public int GroupProductId{get;set;}

        [Display(Name="Nhóm sản phẩm")]
        [InverseProperty("Products")]
        public GroupProduct GroupProduct { get; set; }

        [Display(Name = "Mô tả ngắn")]
        [DataType(DataType.MultilineText)]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "{0} từ {2} - {1} kí tự")]
        public string Intro { get; set; }

        [Display(Name = "Từ khóa")]
        [DataType(DataType.MultilineText)]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "{0} từ {2} - {1} kí tự")]
        public string Keyword { get; set; }

        [Display(Name = "Chi tiết")]
        [MaxLength(5000, ErrorMessage = "{0} không quá {1} kí tự")]
        public string Des { get; set; }


        // gio hang san pham la duy nhat, ko co noi bang, chi co don hang tro den no thoi

    }
}
