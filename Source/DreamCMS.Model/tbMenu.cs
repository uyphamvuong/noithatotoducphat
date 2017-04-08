using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DreamCMS.ModelsAdmin
{
    [Table("tbMenu")]
    public class tbMenu
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int tbMenuId { get; set; }

        [Required(ErrorMessage = "{0} không được để trống!")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "{0} từ {2} - {1} kí tự")]
        public string MenuName { get; set; }

        //[Required(ErrorMessage = "{0} không được để trống!")]
        [MaxLength(30)]
        public string Controller { get; set; }

        //[Required(ErrorMessage = "{0} không được để trống!")]
        [MaxLength(30)]
        public string Action { get; set; }

        [MaxLength(150)]
        public string QueryString { get; set; }

        [MaxLength(10)]
        public string Area { get; set; }

        [MaxLength(20)]
        public string ClassIcon { get; set; }

        public int? Order { get; set; }

        public bool IsController { get; set; }

        public bool IsMenu { get; set; }

        public Nullable<int> IdRoot { get; set; }
        [ForeignKey("IdRoot")]
        public virtual tbMenu tbMenuParrent { get; set; }

        public virtual ICollection<tbMenu> tbMenuChildrens { get; set; }

        public bool IsDisable { get; set; }

    }

}