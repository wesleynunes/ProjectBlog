using ProjectBlog.Models;
using ProjectBlog.Utils;
using ProjectBlog.ViewModels;
using System;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace ProjectBlog.Controllers
{
    public class AuthenticationController : Controller
    {
        private ProjectBlogContext db = new ProjectBlogContext();

        public ActionResult Login(string ReturnUrl)
        {
            var viewmodel = new LoginViewModel
            {
                UrlRetorno = ReturnUrl
            };

            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel viewmodel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewmodel);
            }

            var user = db.Users.FirstOrDefault(u => u.Login == viewmodel.Login);

            if (user == null)
            {
                ModelState.AddModelError("Login", "Usurio incorreto");
                return View(viewmodel);
            }

            if (user.Password != Hash.GerarHash(viewmodel.Password))
            {
                ModelState.AddModelError("Password", "Senha incorreta");
                return View(viewmodel);
            }

            // Validação de usuarios ativos e desativados
            if (user.ActiveUser == false)
            {
                ModelState.AddModelError("ActiveUser", "Usuario não tem permissão");
                return View(viewmodel);
            }

            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim("Login", user.Login),
                new Claim(ClaimTypes.Role, user.Types.ToString())
            }, "ApplicationCookie");

            Request.GetOwinContext().Authentication.SignIn(identity);

            if (!String.IsNullOrWhiteSpace(viewmodel.UrlRetorno) || Url.IsLocalUrl(viewmodel.UrlRetorno))
                return Redirect(viewmodel.UrlRetorno);
            else
                return RedirectToAction("Index", "Posts");
        }

        [Authorize]
        public ActionResult Logout()
        {
            Request.GetOwinContext().Authentication.SignOut("ApplicationCookie");
            return RedirectToAction("Login", "Authentication");
        }
    }
}
