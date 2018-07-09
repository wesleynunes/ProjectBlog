using ProjectBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectBlog.Utils
{
    public class Duplicates
    {
        public static bool CheckEmail(string email, int id)
        {
            using (ProjectBlogContext db = new ProjectBlogContext())
            {
                var DoesExistmail = (from u in db.Users
                                   where u.Email == email
                                   where u.UserId != id
                                   select u).FirstOrDefault();
                if (DoesExistmail != null)
                    return true;
                else
                    return false;
            }
        }
    }
}