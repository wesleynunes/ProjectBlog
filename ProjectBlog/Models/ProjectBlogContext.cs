using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace ProjectBlog.Models
{
    public class ProjectBlogContext : DbContext
    {
        public ProjectBlogContext() : base("BlogConnection")
        {
        }

        //DESABILITAR CASCATAS
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

        public System.Data.Entity.DbSet<ProjectBlog.Models.User> Users { get; set; }

        public System.Data.Entity.DbSet<ProjectBlog.Models.Category> Categories { get; set; }

        public System.Data.Entity.DbSet<ProjectBlog.Models.Post> Posts { get; set; }
    }
}