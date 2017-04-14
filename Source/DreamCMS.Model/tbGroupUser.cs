using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DreamCMS.ModelsAdmin
{
    [Table("tbGroupUser")]
    public class tbGroupUser
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int tbMemberGroupId { get; set; }

        public int tbGroupId { get; set; }
        [ForeignKey("tbGroupId")]
        public virtual tbGroup tbGroup { get; set; }

        public int tbUserId { get; set; }
        [ForeignKey("tbUserId")]
        public virtual tbUser tbUser { get; set; }        

        [MaxLength(20, ErrorMessage = "{0} từ {2} - {1} kí tự")]
        [ScaffoldColumn(false)]
        public string CreatedBy { get; set; }
        [ScaffoldColumn(false)]
        public DateTime? CreatedAt { get; set; }
    }
}