using ProjectBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectBlog.Utils
{
    public class CombosHelper : IDisposable
    {
        private static ProjectBlogContext db = new ProjectBlogContext();

        public static List<Category> GetCategory()
        {

            var dep = db.Categories.ToList();
            dep.Add(new Category
            {
                CategoryId = 0,
                Name = "[Selecione uma Categoria]"
            });

            return dep = dep.OrderBy(d => d.Name).ToList();
        }


        public static List<User> GetUser()
        {
            var dep = db.Users.ToList();
            return dep = dep.OrderBy(d => d.Name).ToList();
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}