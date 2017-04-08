using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace DreamCMS.Models
{

    #region # Mẫu required #

    // Enable-Migrations
    // Mẫu sẵn về required
    // [RegularExpression("([0-9]{1,2})", ErrorMessage = "{0} phải là số và có độ dài 1 hoặc 2 kí tự!")]
    // [RegularExpression("([0-9]{1,4},[0-9]{1,4})", ErrorMessage = "{0} phải có định dạng: 0000,0000")]


    #endregion

    #region # DataDBContext #
    public class DBFrontEnd : DbContext
    {
        public DBFrontEnd()
            : base(DDefault.DefaultConnection)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

        public DbSet<Page> Pages { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<News> Newss { get; set; }

        public DbSet<SliderImg> SliderImgs { get; set; }

        public DbSet<GroupProduct> GroupProducts { get; set; }

        public DbSet<Product> Products { get; set; }
       
        public DbSet<Setting> Settings { get; set; }

        public DbSet<Partner> Partners { get; set; }

        public DbSet<Banner> Banners { get; set; }

        public DbSet<SliderLeft> SliderLefts { get; set; }

        public DbSet<Contact> Contacts { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

    }
    #endregion

}