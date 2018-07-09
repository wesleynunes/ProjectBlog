﻿using PagedList;
using ProjectBlog.Filters;
using ProjectBlog.Models;
using ProjectBlog.Utils;
using ProjectBlog.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web.Mvc;

namespace ProjectBlog.Controllers
{
    public class PostsController : Controller
    {
        private ProjectBlogContext db = new ProjectBlogContext();


        public ActionResult Error()
        {
            return View();
        }

        public ActionResult PostCategory(PostViewModel viewmodel)
        {
            List<PostViewModel> PostByCategoryAdd = new List<PostViewModel>();

            var PostByCategory = (from p in db.Posts
                                  join c in db.Categories on p.PostId equals c.CategoryId
                                  select new
                                  {
                                      c.Name,
                                  }).ToList();

            foreach (var item in PostByCategory)
            {
                PostViewModel CategoryP = new PostViewModel(); //ViewModel

                CategoryP.Name = item.Name;

                PostByCategoryAdd.Add(CategoryP);
            }
            //Retorna as informações para View
            return View(PostByCategoryAdd);

        }

        public ActionResult Test(string busca = "", int pagina = 1)
        {

            List<PostViewModel> PostAdd = new List<PostViewModel>();

            var PostView = (from p in db.Posts
                            where p.Title.Contains(busca)
                            orderby p.PostId descending
                            select new
                            {
                                p.PostId,
                                p.Title,
                                p.Content,
                                p.Create_time,
                                p.Update_time,
                                p.Tag,
                                p.Image,
                                p.CategoryId,
                                p.UserId,
                                p.Users,
                                p.Categories,
                            }).ToPagedList(pagina, 7);
                      
            ViewBag.Busca = busca;      

            foreach (var item in PostView)
            {
                PostViewModel PostC = new PostViewModel(); //ViewModel

                PostC.PostId = item.PostId;
                PostC.Title = item.Title;
                PostC.Image = item.Image;
                PostC.Users = item.Users;
                PostC.Categories = item.Categories;
                PostC.Create_time = item.Create_time;
                PostAdd.Add(PostC);
            }
            //Retorna as informações para View
            return View(PostAdd);
        }
        

        public ActionResult PostByCategory(PostViewModel viewmodel, int? categoryid)
        {

            List<PostViewModel> PostCategoryAdd = new List<PostViewModel>();

            var PostByCategory = (from p in db.Posts
                                  join c in db.Categories on p.PostId equals c.CategoryId
                                  where c.CategoryId == categoryid
                                  select new
                                  {
                                      c.Name,
                                      p.Title,
                                      p.Image,
                                      p.Create_time,
                                      p.Users,
                                  }).ToList();


            foreach (var item in PostByCategory)
            {
                PostViewModel PostCategory = new PostViewModel(); //ViewModel
                PostCategory.Name = item.Name;
                PostCategory.Title = item.Title;
                PostCategory.Image = item.Image;
                PostCategory.Create_time = item.Create_time;
                PostCategory.Users = item.Users;
                PostCategoryAdd.Add(PostCategory);
            }
            //Retorna as informações para View
            return View(PostCategoryAdd);
        }

        
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

                       

        // GET: Posts
        public ActionResult Index(string busca = "", int pagina = 1)
        {
            var posts = db.Posts.Where(p => p.Title.Contains(busca))
                                      .OrderByDescending(p => p.PostId)
                                      .ToPagedList(pagina, 7);

            ViewBag.Busca = busca;

            return View(posts);
        }

        [TypeAuthorization(new[] { UserType.Admin })]
        public ActionResult List(string busca = "", int pagina = 1)
        {

            var posts = db.Posts.Where(p => p.Title.Contains(busca))
                                            .OrderByDescending(p => p.PostId)
                                            .ToPagedList(pagina, 7);

            ViewBag.Busca = busca;

            return View(posts);

            //var posts = db.Posts.Include(p => p.Categorias).Include(p => p.Usuarios);
            //var postDesc = posts.OrderByDescending(a => a.PostId);
            //return View(postDesc.ToList());
        }


        // GET: Posts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // GET: Posts/Create
        [TypeAuthorization(new[] { UserType.Admin })]
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(CombosHelper.GetCategory(), "CategoryId", "Name");
            ViewBag.UserId = new SelectList(db.Users, "UserId", "Name");
            return View();
        }

        // POST: Posts/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Post post)
        {
            // pega usuario logado
            var identity = User.Identity as ClaimsIdentity;
            var login = identity.Claims.FirstOrDefault(c => c.Type == "Login").Value;
            var userLoggedIn = db.Users.FirstOrDefault(u => u.Login == login);


            if (ModelState.IsValid)
            {

                post = new Post
                {
                    Title = post.Title,
                    Content = post.Content,
                    Create_time = DateTime.Now,
                    Update_time = DateTime.Now,                
                    Tag = post.Tag,
                    ImageFile = post.ImageFile,
                    CategoryId = post.CategoryId,
                    UserId = userLoggedIn.UserId // pega usuario logado
                };
                db.Posts.Add(post);
                db.SaveChanges();
                if (post.ImageFile != null)
                {

                    var pic = string.Empty;
                    var folder = "~/Content/image/post";
                    var file = string.Format("{0}.jpg", post.PostId);

                    var response = FilesHelper.UploadImage(post.ImageFile, folder, file);
                    if (response)
                    {
                        pic = string.Format("{0}/{1}", folder, file);
                        post.Image = pic;
                        db.Entry(post).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }

                TempData["MessagePanel"] = "Post realizado com sucesso";

                return RedirectToAction("List");
            }

            ViewBag.CategoryId = new SelectList(CombosHelper.GetCategory(), "CategoryId", "Name");
            //ViewBag.UserId = new SelectList(db.Users, "UserId", "Login", post.UserId);
            return View(post);
        }

        // GET: Posts/Edit/5
        [TypeAuthorization(new[] { UserType.Admin })]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(CombosHelper.GetCategory(), "CategoryId", "Name", post.CategoryId);
            ViewBag.UserId = new SelectList(CombosHelper.GetUser(), "UserId", "Name", post.UserId);
            return View(post);
        }

        // POST: Posts/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Post post)
        {
            //// pega usuario logado
            //var identity = User.Identity as ClaimsIdentity;
            //var login = identity.Claims.FirstOrDefault(c => c.Type == "Login").Value;
            //var userLoggedIn = db.Users.FirstOrDefault(u => u.Login == login);
            
            if (ModelState.IsValid)
            {

                Post posts = db.Posts.Find(post.PostId);

                posts.Title = post.Title;
                posts.Content = post.Content;
                posts.Update_time = DateTime.Now;
                posts.Tag = post.Tag;
                posts.CategoryId = post.CategoryId;
                posts.UserId = post.UserId;// Pega usuario da ViewBag.UsuarioId opção de escolha               
                //posts.UserId = userLoggedIn.UserId; // pega usuario logado                
                posts.ImageFile = post.ImageFile;

                if (posts.ImageFile != null)
                {
                    var pic = string.Empty;
                    var folder = "~/Content/image/post";
                    var file = string.Format("{0}.jpg", posts.PostId);

                    var response = FilesHelper.UploadImage(posts.ImageFile, folder, file);
                    if (response)
                    {
                        pic = string.Format("{0}/{1}", folder, file);
                        posts.Image = pic;
                    }
                }               
                db.Entry(posts).State = EntityState.Modified;
                db.SaveChanges();

                TempData["MessagePanel"] = "Post editado com sucesso";

                return RedirectToAction("List");
            }
            
            ViewBag.CategoryId = new SelectList(CombosHelper.GetCategory(), "CategoryId", "Name", post.CategoryId);
            ViewBag.UserId = new SelectList(CombosHelper.GetUser(), "UserId", "Login", post.UserId);
            return View(post);
        }

        // GET: Posts/Delete/5
        [TypeAuthorization(new[] { UserType.Admin })]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            db.SaveChanges();

            TempData["MessagePanel"] = "Post deletado com sucesso";

            return RedirectToAction("List");
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
