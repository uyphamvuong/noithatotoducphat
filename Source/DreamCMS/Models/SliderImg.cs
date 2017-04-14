using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DreamCMS.Models
{
    [Table("SliderImg")]
    public class SliderImg
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SliderImgId { get; set; }

        [Required(ErrorMessage = "{0} không được để trống!")]
        [DataType(DataType.Text)]
        [Display(Name = "Tiêu đề của hình ảnh")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "{0} từ {2} - {1} kí tự")]
        public string TitleImg {get;set;}

        [Display(Name = "Ưu tiên")]
        public int? Order { get; set; }

        [Display(Name = "Link hình")]
        [Column(TypeName = "varchar")]
        [MaxLength(200, ErrorMessage = "{0} không quá {1} kí tự")]
        public string ImgUrl { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Link website")]
        [MaxLength(200, ErrorMessage = "{0} không quá {1} kí tự")]
        [RegularExpression(@"(http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?", ErrorMessage = "Định dạng link không phù hợp!!")]
        public string Link { get; set; }

    }
}
