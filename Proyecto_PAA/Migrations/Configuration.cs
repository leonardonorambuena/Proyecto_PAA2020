namespace Proyecto_PAA.Migrations
{
    using Proyecto_PAA.Helpers;
    using Proyecto_PAA.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Proyecto_PAA.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Proyecto_PAA.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            if (!context.Roles.Any())
            {
                context.Roles.AddOrUpdate(new Role
                {
                    RoleName = StringHelper.ROLE_ADMINISTRATOR,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                });
                context.Roles.AddOrUpdate(new Role
                {
                    RoleName = StringHelper.ROLE_CLIENT,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                });

            }

           UserSeeder.Seeder(context);

           CategorySeeder.Seeder(context);


        }
    }
}
