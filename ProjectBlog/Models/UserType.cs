using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProjectBlog.Models
{   
    public enum UserType
    {
        Padrao,
        Admin
    }
}