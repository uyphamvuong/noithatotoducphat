using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DreamCMS.Models
{
    [Table("Order")]
    public class Order
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [ScaffoldColumn(true)]
        public int OrderId { get; set; }

        [Required(ErrorMessage = "{0} không được để trống!")]
        [DataType(DataType.Text)]
        [Display(Name = "Họ Tên")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "{0} từ {2} - {1} kí tự")]
        public string Name { get; set; }

        [Display(Name = "Số điện thoại")]
        [MaxLength(100, ErrorMessage = "{0} không quá {1} kí tự")]
        public string SDT { get; set; }

        [Display(Name = "Email")]
        [MaxLength(100, ErrorMessage = "{0} không quá {1} kí tự")]
        public string Email { get; set; }

        [Display(Name = "Địa Chỉ")]
        [MaxLength(300, ErrorMessage = "{0} không quá {1} kí tự")]
        public string Adress { get; set; }

        [Display(Name = "Thời gian")]
        public DateTime Date { get; set; }

        [Display(Name = "Ghi chú")]
        [DataType(DataType.MultilineText)]
        [StringLength(500, MinimumLength = 5, ErrorMessage = "{0} từ {2} - {1} kí tự")]
        public string Des { get; set; }

        [InverseProperty("Order")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        [ScaffoldColumn(false)]
        public bool IsSeen { get; set; }

    }
}