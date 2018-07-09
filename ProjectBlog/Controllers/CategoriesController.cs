using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjectBlog.Models;
using System.Security.Claims;
using ProjectBlog.Filters;

namespace ProjectBlog.Controllers
{
    [TypeAuthorization(new[] { UserType.Admin })]
    public class CategoriesController : Controller
    {
        private ProjectBlogContext db = new ProjectBlogContext();

        // GET: Categories
        public ActionResult Index()
        {
            return View(db.Categories.ToList());
        }

        // GET: Categories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                category = new Category
                {
                    Name = category.Name,
                    Create_time = DateTime.Now,
                    Update_Time = DateTime.Now,                    
                };
                db.Categories.Add(category);
                try
                {
                    db.SaveChanges();
                    TempData["MessagePanel"] = "Categoria cadastrada com sucesso";
                    return RedirectToAction("Index", "Categories");
                }
                catch (System.Exception ex)
                {

                    if (ex.InnerException != null &&
                       ex.InnerException.InnerException != null &&
                       ex.InnerException.InnerException.Message.Contains("_Index"))
                    {
                        ModelState.AddModelError(string.Empty, "Esta Categoria já esta Cadastrada!!");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }

                    //throw;
                    return View(category);
                }
            }

            return View(category);
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }            
            return View(category);
        }

        // POST: Categories/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                Category categories = db.Categories.Find(category.CategoryId);
                categories.Name = category.Name;
                categories.Update_Time = DateTime.Now;            
                db.Entry(categories).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                    TempData["MessagePanel"] = "Categoria cadastrada com sucesso";
                    return RedirectToAction("Index");
                }
                catch (System.Exception ex)
                {

                    if (ex.InnerException != null &&
                       ex.InnerException.InnerException != null &&                    
                       ex.InnerException.InnerException.Message.Contains("_Index"))

                    {
                        ModelState.AddModelError(string.Empty, "Esta Categoria já esta Cadastrada!!");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }

                    //throw;
                    return View(category);
                }
            }          
            return View(category);            
        }

        // GET: Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
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
                    ModelState.AddModelError(string.Empty, "Não é possível remover a categoria porque existe posts relacionadas a ele, primeiro remova ou altere os posts e volte a tentar!!");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }

                return View(category);
            }

            //Category category = db.Categories.Find(id);
            //db.Categories.Remove(category);
            //db.SaveChanges();
            //TempData["MessagePanel"] = "Categoria cadastrada com sucesso";
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
