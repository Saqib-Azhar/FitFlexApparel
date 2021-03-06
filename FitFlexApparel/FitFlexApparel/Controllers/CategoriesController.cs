﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FitFlexApparel.Models;
using System.IO;

namespace FitFlexApparel.Controllers
{
    //[RequireHttps]
    public class CategoriesController : Controller
    {
        private FitflexApparelEntities db = new FitflexApparelEntities();

        // GET: Categories
        public ActionResult Index()
        {
            try
            {
                return View(db.Categories.Where(s => s.IsDeleted != true).ToList());
            }
            catch (Exception ex)
            {
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: Categories/Details/5
        public ActionResult Details(int? id)
        {
            try
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
            catch (Exception ex)
            {
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
                return RedirectToAction("Error", "Home");
            }
        }

        [Authorize(Roles = "Admin")]
        // GET: Categories/Create
        public ActionResult Create()
        {
            try
            {
                return View();

            }
            catch (Exception ex)
            {
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
                return RedirectToAction("Error", "Home");
            }
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Category_Name,Category_Description,Category_Image,Category_Slug,IsDeleted")] Category category, HttpPostedFileBase Category_Image)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (Category_Image != null)
                    {
                        string pic = System.IO.Path.GetFileName(Category_Image.FileName);
                        string path = System.IO.Path.Combine(
                                               Server.MapPath("~/UploadedImages"), pic);

                        Category_Image.SaveAs(path);

                        using (MemoryStream ms = new MemoryStream())
                        {
                            Category_Image.InputStream.CopyTo(ms);
                            byte[] array = ms.GetBuffer();
                        }
                        category.Category_Image = Category_Image.FileName;

                    }

                    category.Category_Slug = category.Category_Name.ToLower().Trim().Replace(' ', '-').Replace("'", "");
                    category.IsDeleted = false;
                    db.Categories.Add(category);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(category);

            }
            catch (Exception ex)
            {
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
                return RedirectToAction("Error", "Home");
            }
        }

        [Authorize(Roles = "Admin")]
        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            try
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
            catch (Exception ex)
            {
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
                return RedirectToAction("Error", "Home");
            }
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Category_Name,Category_Description,Category_Image,Category_Slug,IsDeleted")] Category category, HttpPostedFileBase Category_Image)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (Category_Image != null)
                    {
                        string pic = System.IO.Path.GetFileName(Category_Image.FileName);
                        string path = System.IO.Path.Combine(
                                               Server.MapPath("~/UploadedImages"), pic);

                        Category_Image.SaveAs(path);

                        using (MemoryStream ms = new MemoryStream())
                        {
                            Category_Image.InputStream.CopyTo(ms);
                            byte[] array = ms.GetBuffer();
                        }
                        category.Category_Image = Category_Image.FileName;

                    }
                    category.Category_Slug = category.Category_Name.ToLower().Trim().Replace(' ', '-').Replace("'","");
                    category.IsDeleted = false;
                    db.Entry(category).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(category);
            }
            catch (Exception ex)
            {
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
                return RedirectToAction("Error", "Home");
            }
        }

        [Authorize(Roles = "Admin")]
        // GET: Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            try
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
            catch (Exception ex)
            {
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
                return RedirectToAction("Error", "Home");
            }
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Category category = db.Categories.Find(id);
                db.Categories.Remove(category);
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


        [Authorize(Roles = "Admin")]
        public ActionResult SoftDelete(int id)
        {
            try
            {
                Category category = db.Categories.Find(id);
                category.IsDeleted = true;
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
