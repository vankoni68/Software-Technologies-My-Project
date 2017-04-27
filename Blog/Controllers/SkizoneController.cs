using Blog.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Blog.Controllers
{
    public class SkizoneController : Controller
    {
        // GET: Skizone
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            using (var db = new BlogDbContext())
            {
                var skizones = db.Skizones.Include(a => a.Author).ToList();

                return View(skizones);
            }

        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var db = new BlogDbContext())
            {
                var skizone = db.Skizones.Where(a => a.Id == id).Include(a => a.Author).FirstOrDefault();

                if (skizone == null)
                {
                    return HttpNotFound();
                }

                return View(skizone);
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
        public ActionResult Create(Skizone model, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                using (var db = new BlogDbContext())
                {
                    var authorId = User.Identity.GetUserId();
                    model.AuthorId = authorId;

                    if (image != null)
                    {
                        var allowedContentTypes = new[]
                        {
                            "image/jpeg", "image/jpg", "image/png"
                        };
                        if (allowedContentTypes.Contains(image.ContentType))
                        {
                            var imagesPath = "/Content/Images/";
                            var filename = image.FileName;
                            var uploadPath = imagesPath + filename;
                            var physicalPath = Server.MapPath(uploadPath);
                            image.SaveAs(physicalPath);
                            model.ImagePath = uploadPath;
                        }
                    }

                    db.Skizones.Add(model);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }

            return View(model);
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
                var skizone = db.Skizones.Where(a => a.Id == id).Include(a => a.Author).FirstOrDefault();

                if (!IsUserAuthorizedToEdit(skizone))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                if (skizone == null)
                {
                    return HttpNotFound();
                }

                return View(skizone);
            }

        }

        [Authorize]
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var db = new BlogDbContext())
            {
                var skizone = db.Skizones.Where(a => a.Id == id).Include(a => a.Author).FirstOrDefault();

                if (!IsUserAuthorizedToEdit(skizone))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                if (skizone == null)
                {
                    return HttpNotFound();
                }

                db.Skizones.Remove(skizone);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

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
                var skizone = db.Skizones.Where(a => a.Id == id).First();

                if (!IsUserAuthorizedToEdit(skizone))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                if (skizone == null)
                {
                    return HttpNotFound();
                }

                var model = new SkizoneViewModel();
                model.Id = skizone.Id;
                model.Name = skizone.Name;
                model.ElevationInfo = skizone.ElevationInfo;
                model.Slopes = skizone.Slopes;
                model.LiftTicket = skizone.LiftTicket;
                model.ImagePath = skizone.ImagePath;
                model.ContentInfo = skizone.ContentInfo;
                return View(model);
            }

        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(SkizoneViewModel model, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                using (var db = new BlogDbContext())
                {
                    var skizone = db.Skizones.FirstOrDefault(a => a.Id == model.Id);

                    if (!IsUserAuthorizedToEdit(skizone))
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                    }

                    if (image != null)
                    {
                        var allowedContentTypes = new[]
                        {
                            "image/jpeg", "image/jpg", "image/png"
                        };
                        if (allowedContentTypes.Contains(image.ContentType))
                        {
                            var imagesPath = "/Content/Images/";
                            var filename = image.FileName;
                            var uploadPath = imagesPath + filename;
                            var physicalPath = Server.MapPath(uploadPath);
                            image.SaveAs(physicalPath);
                            model.ImagePath = uploadPath;
                        }
                    }

                    skizone.Name = model.Name;
                    skizone.ElevationInfo = model.ElevationInfo;
                    skizone.Slopes = model.Slopes;
                    skizone.LiftTicket = model.LiftTicket;
                    skizone.ContentInfo = model.ContentInfo;
                    skizone.ImagePath = model.ImagePath;
                    db.Entry(skizone).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Details", new { id = model.Id });
                }
            }

            return View(model);
        }

        private bool IsUserAuthorizedToEdit(Skizone skizone)
        {
            bool isAdmin = this.User.IsInRole("Admin");
            bool isAuthor = skizone.IsAuthor(this.User.Identity.GetUserId());
            return isAdmin || isAuthor;
        }
    }
}