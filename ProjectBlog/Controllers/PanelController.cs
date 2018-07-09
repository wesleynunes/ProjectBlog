using ProjectBlog.Filters;
using ProjectBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectBlog.Controllers
{
    [TypeAuthorization(new[] { UserType.Admin})]
    public class PanelController : Controller
    {
        // GET: Panel
        public ActionResult Index()
        {
            return View();
        }
    }
}