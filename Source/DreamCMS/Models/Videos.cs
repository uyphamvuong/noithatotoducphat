﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DreamCMS.Models
{
    [Table("Video")]
    public class Video
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [ScaffoldColumn(true)]
        public int VideoId { get; set; }

        [Required(ErrorMessage = "{0} không được để trống!")]
        [DataType(DataType.Text)]
        [Display(Name = "Tiêu đề Video")]
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

        [Required(ErrorMessage = "{0} không được để trống!")]
        [DataType(DataType.Text)]
        [Display(Name = "Link Video")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "{0} từ {2} - {1} kí tự")]
        public string Link { get; set; }

        [Display(Name = "Link hình đại diện")]
        [Column(TypeName = "varchar")]
        [MaxLength(200, ErrorMessage = "{0} không quá {1} kí tự")]
        public string ImgUrl { get; set; }

        [Display(Name = "IdText")]
        [Column(TypeName = "varchar")]
        public string IdText { get; set; }

        [Display(Name = "Khóa nội dung")]
        public bool IsDisable { get; set; }

    }
}
