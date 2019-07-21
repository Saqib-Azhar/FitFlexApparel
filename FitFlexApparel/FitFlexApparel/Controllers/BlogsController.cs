using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FitFlexApparel.Models;
using Microsoft.AspNet.Identity;
using System.IO;

namespace FitFlexApparel.Controllers
{
    [RequireHttps]
    public class BlogsController : Controller
    {
        private FitflexApparelEntities db = new FitflexApparelEntities();
        [Authorize(Roles = "Admin")]

        public ActionResult Index()
        {
			try
            {
				return View(db.Blogs.Where(s => s.IsDeleted != true).ToList());
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
		}

        [AllowAnonymous]
        public ActionResult AllBlogs()
        {
            try
            {
                return View(db.Blogs.Where(s => s.IsDeleted != true).ToList());
            }
            catch (Exception ex)
            {
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
                return RedirectToAction("Error", "Home");
            }
        }

        [Authorize(Roles ="Admin")]
        // GET: Blogs/Details/5
        public ActionResult Details(int? id)
        {
			try
			{
				if (id == null)
				{
					return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
				}
				Blog blog = db.Blogs.Find(id);
				if (blog == null)
				{
					return HttpNotFound();
				}
				return View(blog);
			
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
        }

        [Authorize(Roles = "Admin")]
        // GET: Blogs/Create
        public ActionResult Create()
        {
			try{
				return View();
				
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
        }


        // POST: Blogs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Blog_Name,Written_On,Edited_On,IsAvailable,Created_By,Blogger_Name,Blog1,IsDeleted")] Blog blog, HttpPostedFileBase Image)
        {
			try
			{
				if (ModelState.IsValid)
                {
                    if (Image != null)
                    {
                        string pic = System.IO.Path.GetFileName(Image.FileName);
                        string path = System.IO.Path.Combine(
                                               Server.MapPath("~/Uploadedimages/Blogs"), pic);

                        Image.SaveAs(path);

                        using (MemoryStream ms = new MemoryStream())
                        {
                            Image.InputStream.CopyTo(ms);
                            byte[] array = ms.GetBuffer();
                        }
                        blog.Image = "Blogs/" + Image.FileName;

                    }
                    var userId = User.Identity.GetUserId();
                    blog.Written_On = DateTime.Now;
                    blog.Created_By = userId;
					blog.IsDeleted = false;
					db.Blogs.Add(blog);
					db.SaveChanges();
					return RedirectToAction("Index");
				}

				return View(blog);
				
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
        }

        [Authorize(Roles = "Admin")]
        // GET: Blogs/Edit/5
        public ActionResult Edit(int? id)
        {
			try
			{
				if (id == null)
				{
					return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
				}
				Blog blog = db.Blogs.Find(id);
				if (blog == null)
				{
					return HttpNotFound();
				}
				return View(blog);
				
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
		}

        // POST: Blogs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Blog_Name,Edited_On,IsAvailable,Blogger_Name,Blog1,IsDeleted")] Blog blog, HttpPostedFileBase Image)
        {
			try{
				if (ModelState.IsValid)
                {
                    if (Image != null)
                    {
                        string pic = System.IO.Path.GetFileName(Image.FileName);
                        string path = System.IO.Path.Combine(
                                               Server.MapPath("~/Uploadedimages/Blogs"), pic);

                        Image.SaveAs(path);

                        using (MemoryStream ms = new MemoryStream())
                        {
                            Image.InputStream.CopyTo(ms);
                            byte[] array = ms.GetBuffer();
                        }
                        blog.Image = "Blogs/" + Image.FileName;

                    }
                    blog.Edited_On = DateTime.Now;
					db.Entry(blog).State = EntityState.Modified;
					blog.IsDeleted = false;
					db.SaveChanges();
					return RedirectToAction("Index");
				}
				return View(blog);
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
        }

        [Authorize(Roles = "Admin")]
        // GET: Blogs/Delete/5
        public ActionResult Delete(int? id)
        {
			try
			{
				if (id == null)
				{
					return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
				}
				Blog blog = db.Blogs.Find(id);
				if (blog == null)
				{
					return HttpNotFound();
				}
				return View(blog);
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
        }

        // POST: Blogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
			try
			{
				Blog blog = db.Blogs.Find(id);
				db.Blogs.Remove(blog);
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
        }

        [Authorize(Roles = "Admin")]
        public ActionResult SoftDelete(int id)
        {
            try
            {
				Blog blog = db.Blogs.Find(id);
                blog.IsDeleted = true;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
                return RedirectToAction("Error", "Home");
            }
        }

        [AllowAnonymous]
        public ActionResult Read(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Blog blog = db.Blogs.Find(id);
                if (blog == null)
                {
                    return HttpNotFound();
                }
                blog.Views = blog.Views++;
                db.SaveChanges();
                return View(blog);
            }
            catch (Exception ex)
            {
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);

                var blog = db.Blogs.FirstOrDefault(s => s.Id == id);
                return View(blog);
            }
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
