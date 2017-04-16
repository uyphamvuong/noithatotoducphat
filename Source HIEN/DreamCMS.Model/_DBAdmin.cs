using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace DreamCMS.ModelsAdmin
{
    public class DBAdmin : DbContext
    {
        public DBAdmin()
            : base(DDefault.DefaultConnection)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

        public DbSet<tbUser> tbUsers { get; set; }
        public DbSet<tbGroup> tbGroups { get; set; }
        public DbSet<tbGroupUser> tbGroupUsers { get; set; }
        public DbSet<tbMenu> tbMenus { get; set; }
        public DbSet<tbGroupMenu> tbGroupMenus { get; set; }

    }
}
