using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DreamCMS.Models
{
    [Table("GroupProduct")]
    public class GroupProduct
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GroupProductId { get; set; }

        [Required(ErrorMessage = "{0} không được để trống!")]
        [DataType(DataType.Text)]
        [Display(Name = "Tên nhóm sản phẩm")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "{0} từ {2} - {1} kí tự")]
        public string GroupName {get;set;}

        [Display(Name = "SeoUrl")]
        [MaxLength(100, ErrorMessage = "{0} không quá {1} kí tự")]
        public string SeoUrl { get; set; }

        [Display(Name = "Ưu tiên")]
        public int? Order { get; set; }

        [Display(Name = "Khóa nhóm")]
        public bool IsDisable { get; set; }

        [Display(Name = "Link hình đại diện")]
        [Column(TypeName = "varchar")]
        [MaxLength(200, ErrorMessage = "{0} không quá {1} kí tự")]
        public string ImgUrl { get; set; }

        [InverseProperty("GroupProduct")]
        public virtual ICollection<Product> Products { get; set; }

        public Nullable<int> IdRoot { get; set; }
        [ForeignKey("IdRoot")]
        public virtual GroupProduct GroupProductParrent { get; set; }

        public virtual ICollection<GroupProduct> GroupProductChildrens { get; set; }

        [ScaffoldColumn(false)]
        public int CountNode { get; set; }
    }
}
