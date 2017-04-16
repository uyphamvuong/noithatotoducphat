using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DreamCMS.Models
{
    [Table("OrderDetail")]
    public class OrderDetail
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [ScaffoldColumn(true)]
        public int OrderDetailId { get; set; }        

        [Display(Name = "Nhóm Đơn Hàng")]
        public int OrderId { get; set; }


        [Display(Name = "ID Nhóm Đơn Hàng")]
        public int ProductId { get; set; }

        [Display(Name = "Link hình sản phẩm")]
        [Column(TypeName = "varchar")]
        public string ImgUrl { get; set; }

        [Display(Name = "Tên sản phẩm")]
        [Column(TypeName = "varchar")]
        [MaxLength(200, ErrorMessage = "{0} không quá {1} kí tự")]
        public string ProductName { get; set; }


        [Display(Name = "Đơn hàng")]
        [InverseProperty("OrderDetails")]
        public Order Order { get; set; }


    }
}