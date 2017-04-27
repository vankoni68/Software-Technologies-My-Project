using Blog.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Blog.Controllers.Admin
{
    public class CategoryController : Controller
    {
        // GET: Category
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            using (var db = new BlogDbContext())
            {
                var categories = db.Categories.ToList();
                return View(categories);
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                using (var db = new BlogDbContext())
                {
                    db.Categories.Add(category);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }

            return View(category);
        }

        [Authorize]
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var db = new BlogDbContext())
            {
                var category = db.Categories.Where(c => c.Id == id).FirstOrDefault();

                if (!IsUserAuthorizedToEdit(category))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                if (category == null)
                {
                    return HttpNotFound();
                }

                return View(category);
            }

        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                using (var db = new BlogDbContext())
                {
                    if (!IsUserAuthorizedToEdit(category))
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                    }

                    db.Entry(category).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }

            return View(category);
        }

        [Authorize]
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var db = new BlogDbContext())
            {
                var category = db.Categories.Where(c => c.Id == id).FirstOrDefault();

                if (!IsUserAuthorizedToEdit(category))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                if (category == null)
                {
                    return HttpNotFound();
                }

                return View(category);
            }

        }

        [Authorize]
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int? id)
        {            
                using (var db = new BlogDbContext())
                {
                    var category = db.Categories.Where(c => c.Id == id).FirstOrDefault();
                    var categorySkizones = category.Skizones.ToList();

                    foreach (var skizone in categorySkizones)
                    {
                        db.Skizones.Remove(skizone);
                    }
                    db.Categories.Remove(category);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }       
        }

        private bool IsUserAuthorizedToEdit(Category category)
        {
            bool isAdmin = this.User.IsInRole("Admin");
            return isAdmin;
        }
    }
}