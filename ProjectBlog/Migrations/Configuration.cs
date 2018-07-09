namespace ProjectBlog.Migrations
{
    using ProjectBlog.Models;
    using ProjectBlog.Utils;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ProjectBlog.Models.ProjectBlogContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "ProjectBlog.Models.ProjectBlogContext";
        }

        protected override void Seed(ProjectBlog.Models.ProjectBlogContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            
            var user = new List<User>
            {
                new User {
                    Name = "Admin",
                    Login = "admin",
                    Email = "admin@admin.com",
                    Password = Hash.GerarHash("1234567"),
                    Create_time = DateTime.Now,
                    Update_Time = DateTime.Now,
                    Last_Login = DateTime.Now,
                    ActiveUser = true,
                    Types = UserType.Admin},
            };

            user.ForEach(s => context.Users.AddOrUpdate(p => p.Name, s));
            context.SaveChanges();
        }
    }
}
