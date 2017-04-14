using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DreamCMS.ModelsAdmin
{
    [Table("tbGroupMenu")]
    public class tbGroupMenu
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int tbPermissonGroupId { get; set; }
        
        public int tbGroupId { get; set; }
        [ForeignKey("tbGroupId")]
        public virtual tbGroup tbGroup { get; set; }

        public int tbMenuId { get; set; }
        [ForeignKey("tbMenuId")]
        public virtual tbMenu tbMenu { get; set; }
        
    }
}