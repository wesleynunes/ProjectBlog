using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjectBlog.Models;
using ProjectBlog.ViewModels;
using ProjectBlog.Utils;
using System.Security.Claims;
using ProjectBlog.Filters;

namespace ProjectBlog.Controllers
{
    public class UsersController : Controller
    {
        private ProjectBlogContext db = new ProjectBlogContext();

        // GET: Users
        [TypeAuthorization(new[] { UserType.Admin })]
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: Users/Details/5
        [TypeAuthorization(new[] { UserType.Admin })]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }


        public ActionResult Register()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel viewmodel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewmodel);
            }


            if (db.Users.Count(u => u.Login == viewmodel.Login) > 0)
            {
                ModelState.AddModelError("Login", "Esse login já está em uso");
                return View(viewmodel);
            }


            if (db.Users.Count(u => u.Email == viewmodel.Email) > 0)
            {
                ModelState.AddModelError("Email", "Esse Email já está em uso");
                return View(viewmodel);
            }


            User NewUser  = new User
            {
                Name = viewmodel.Name,
                Login = viewmodel.Login,
                Email = viewmodel.Email,
                Password = Hash.GerarHash(viewmodel.Password),
                Create_time = DateTime.Now,
                Update_Time = DateTime.Now,
                Last_Login = DateTime.Now,
                ActiveUser = viewmodel.ActiveUser,
                Types = viewmodel.Types,
            };

            db.Users.Add(NewUser);
            db.SaveChanges();

            TempData["RegistrationMessage"] = "Cadastro realizado com sucesso. Efetue login.";

            return RedirectToAction("Login", "Authentication");

        }

        // GET: Users/Create
        [TypeAuthorization(new[] { UserType.Admin })]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserViewModel viewmodel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewmodel);
            }

            if (db.Users.Count(u => u.Login == viewmodel.Login) > 0)
            {
                ModelState.AddModelError("Login", "Esse login já está em uso");
                return View(viewmodel);
            }

            if (db.Users.Count(u => u.Email == viewmodel.Email) > 0)
            {
                ModelState.AddModelError("Email", "Este email já está em uso");
                return View(viewmodel);
            }


            User NewUser = new User
            {  
                Name = viewmodel.Name,
                Login = viewmodel.Login,
                Email = viewmodel.Email,
                Password = Hash.GerarHash(viewmodel.Password),
                Create_time = DateTime.Now,
                Update_Time = DateTime.Now,
                Last_Login = DateTime.Now,
                ActiveUser = viewmodel.ActiveUser,
                Types = viewmodel.Types
            };

            db.Users.Add(NewUser);
            db.SaveChanges();

            TempData["MessagePanel"] = "Cadastro realizado com sucesso";

            return RedirectToAction("Index", "Users");
        }

        // GET: Users/Edit/5
        [TypeAuthorization(new[] { UserType.Admin })]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user)
        {
            if (!Duplicates.CheckEmail(user.Email, user.UserId))
            {
                User users = db.Users.Find(user.UserId);
                users.Name = user.Name;
                users.Email = user.Email;
                users.ActiveUser = user.ActiveUser;
                users.Types = user.Types;
                users.Update_Time = DateTime.Now;
                db.Entry(users).State = EntityState.Modified;
                db.SaveChanges();
                TempData["MessagePanel"] = "Usuário atualizado com sucesso";
            }
            else
            {
                ModelState.AddModelError("Email", "Este email já está em uso");
                return View(user);
            }

            return RedirectToAction("Index", "Users");
        }


        // GET: Usuario/EditPassword 
        [TypeAuthorization(new[] { UserType.Admin })]
        public ActionResult EditPassword(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }


        // POST: Usuario/EditPassword    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPassword(User user)
        {
            // validar senha com mesmo de sem caracteres
            if (user.Password == null || user.Password.Length < 6)
            {
                return View(user);
            }

            User users = db.Users.Find(user.UserId);
            users.Password = Hash.GerarHash(user.Password);
            users.Update_Time = DateTime.Now;
            db.Entry(users).State = EntityState.Modified;
            db.SaveChanges();

            TempData["MessagePanel"] = "Senha alterada com sucesso";

            return RedirectToAction("Index", "Users");
        }



        // alterar senha
        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }


        //alterar senha 
        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel viewmodel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var identity = User.Identity as ClaimsIdentity;
            var login = identity.Claims.FirstOrDefault(c => c.Type == "Login").Value;

            var user = db.Users.FirstOrDefault(u => u.Login == login);

            if (Hash.GerarHash(viewmodel.CurrentPassword) != user.Password)
            {
                ModelState.AddModelError("CurrentPassword", "Senha incorreta");
                return View();
            }

            user.Password = Hash.GerarHash(viewmodel.NewPassword);
            db.Entry(user).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            TempData["Message"] = "Senha alterada com sucesso";

            return RedirectToAction("Index", "Posts");
        }


        // GET: Users/Delete/5
        [TypeAuthorization(new[] { UserType.Admin })]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            User user = db.Users.Find(id);
            db.Users.Remove(user);
            try
            {
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (System.Exception ex)
            {
                if (ex.InnerException != null &&
                       ex.InnerException.InnerException != null &&
                       ex.InnerException.InnerException.Message.Contains("REFERENCE"))
                {
                    ModelState.AddModelError(string.Empty, "Não é possível remover o usuario porque existe posts relacionadas a ele, primeiro remova ou altere os posts e volte a tentar!!");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }

                return View(user);
            }

            //User user = db.Users.Find(id);
            //db.Users.Remove(user);
            //db.SaveChanges();
            //TempData["MessagePanel"] = "Usuário deletado com sucesso";
            //return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
