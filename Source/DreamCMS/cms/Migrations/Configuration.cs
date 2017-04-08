namespace DreamCMS.Cms.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<DreamCMS.ModelsAdmin.DBAdmin>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            MigrationsDirectory = @"Cms\Migrations";
            ContextKey = "DBAdmin";
        }

        protected override void Seed(DreamCMS.ModelsAdmin.DBAdmin context)
        {

        }
    }
}
