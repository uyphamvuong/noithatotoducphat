using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DreamCMS.ModelsAdmin
{
    [Table("tbGroup")]
    public class tbGroup
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int tbGroupId { get; set; }

        [Required(ErrorMessage = "{0} không được để trống!")]
        [MaxLength(50, ErrorMessage="{0} không quá 50 kí tự")]
        [DataType(DataType.Text)]
        [Display(Name = "Tên nhóm")]
        //[Remote("IsNameAvailble", "Groups", "Admin", ErrorMessage = "{0} đã tồn tại!!!")]
        public string GroupName { get; set; }

        [MaxLength(200, ErrorMessage="{0} không quá 200 kí tự")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Diễn giải")]
        public string Description { get; set; }

        [Display(Name = "Khóa nhóm")]
        public bool IsDisable { get; set; }

    }

    public class tbUserInGroup
    {
        public tbUserInGroup(tbUser tbUser, int tbGroupId)
        {
            this.tbUser = tbUser;
            this.tbGroupId = tbGroupId;
        }

        public tbUser tbUser { get; set; }

        public int tbGroupId { get; set; }

        public bool IsIn
        {
            get
            {
                if (this.tbGroupId == 0) { return true; }
                DBAdmin db = new DBAdmin();
                if (db.tbGroupUsers.Where(x => x.tbGroupId == this.tbGroupId && x.tbUserId == tbUser.tbUserId).FirstOrDefault() != null)
                    return true;
                return false;
            }
        }
    }

    public class tbMenuInGroup
    {
        public tbMenuInGroup(tbMenu tbMenu, int tbGroupId)
        {
            this.tbMenu = tbMenu;
            this.tbGroupId = tbGroupId;
        }

        public tbMenu tbMenu { get; set; }

        public int tbGroupId { get; set; }

        public bool IsIn
        {
            get
            {
                if (this.tbGroupId == 0) { return true; }
                DBAdmin db = new DBAdmin();
                if (db.tbGroupMenus.Where(x => x.tbGroupId == this.tbGroupId && x.tbMenuId == tbMenu.tbMenuId).FirstOrDefault() != null)
                    return true;
                return false;
            }
        }
    }

}