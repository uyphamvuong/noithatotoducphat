/*
 * 
 * 
 * UPDATE DATABASE FRONTEND:
 *      Update-Database -ConfigurationTypeName DreamCMS.Migrations.Configuration -Verbose
 *     
 * 
 * UPDATE DATABASE ADMIN:
 *      Update-Database -ConfigurationTypeName DreamCMS.Cms.Migrations.Configuration -Verbose
 * 
*/

namespace DreamCMS.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<DreamCMS.Models.DBFrontEnd>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            MigrationsDirectory = @"Migrations";
            ContextKey = "DBFrontEnd";
        }

        protected override void Seed(DreamCMS.Models.DBFrontEnd context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
