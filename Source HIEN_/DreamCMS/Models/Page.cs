﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DreamCMS.Models
{
    [Table("Page")]
    public class Page
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [ScaffoldColumn(true)]
        public int PageId { get; set; }

        [Required(ErrorMessage = "{0} không được để trống!")]
        [DataType(DataType.Text)]
        [Display(Name = "Tiêu đề trang")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "{0} từ {2} - {1} kí tự")]
        public string Title { get; set; }

        [Display(Name = "SeoUrl")]
        [MaxLength(100, ErrorMessage = "{0} không quá {1} kí tự")]
        public string TitleId { get; set; }

        [Display(Name = "Mô tả ngắn")]
        [DataType(DataType.MultilineText)]
        [StringLength(500, MinimumLength = 5, ErrorMessage = "{0} từ {2} - {1} kí tự")]
        public string Intro { get; set; }

        [Display(Name = "Từ khóa")]
        [DataType(DataType.MultilineText)]
        [StringLength(150, MinimumLength = 5, ErrorMessage = "{0} từ {2} - {1} kí tự")]
        public string Keyword { get; set; }

        [DataType(DataType.Html)]
        [Display(Name = "Nội dung")]
        [Column(TypeName = "ntext")]
        public string Context { get; set; }

        [Display(Name = "Khóa trang")]
        public bool IsDisable { get; set; }

        [Display(Name = "Link hình đại diện")]
        [Column(TypeName = "varchar")]
        [MaxLength(200, ErrorMessage = "{0} không quá {1} kí tự")]
        public string ImgUrl { get; set; }
    }
}