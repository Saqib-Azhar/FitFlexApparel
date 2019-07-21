using System;
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
    [RequireHttps]
    public class BrandsController : Controller
    {
        private FitflexApparelEntities db = new FitflexApparelEntities();

        // GET: Brands
        public ActionResult Index()
        {
			try{
				return View(db.Brands.Where(s => s.IsDeleted != true).ToList());
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
		}

        // GET: Brands/Details/5
        public ActionResult Details(int? id)
        {
			try
			{
				if (id == null)
				{
					return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
				}
				Brand brand = db.Brands.Find(id);
				if (brand == null)
				{
					return HttpNotFound();
				}
				return View(brand);
			
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
        }

        [Authorize(Roles = "Admin")]
        // GET: Brands/Create
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

        // POST: Brands/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Brand_Name,Brand_Logo,Brand_Description,IsDeleted")] Brand brand, HttpPostedFileBase Brand_Logo)
        {
			try
			{
				if (ModelState.IsValid)
				{

                    if (Brand_Logo != null)
                    {
                        string pic = System.IO.Path.GetFileName(Brand_Logo.FileName);
                        string path = System.IO.Path.Combine(
                                               Server.MapPath("~/UploadedImages"), pic);

                        Brand_Logo.SaveAs(path);

                        using (MemoryStream ms = new MemoryStream())
                        {
                            Brand_Logo.InputStream.CopyTo(ms);
                            byte[] array = ms.GetBuffer();
                        }
                        brand.Brand_Logo = Brand_Logo.FileName;

                    }
                    
					brand.IsDeleted = false;
					db.Brands.Add(brand);
					db.SaveChanges();
					return RedirectToAction("Index");
				}

				return View(brand);
				
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
        }

        [Authorize(Roles = "Admin")]
        // GET: Brands/Edit/5
        public ActionResult Edit(int? id)
        {
			try
			{
				if (id == null)
				{
					return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
				}
				Brand brand = db.Brands.Find(id);
				if (brand == null)
				{
					return HttpNotFound();
				}
				return View(brand);
				
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
		}

        // POST: Brands/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Brand_Name,Brand_Logo,Brand_Description,IsDeleted")] Brand brand, HttpPostedFileBase Brand_Logo)
        {
			try{
				if (ModelState.IsValid)
                {
                    if (Brand_Logo != null)
                    {
                        string pic = System.IO.Path.GetFileName(Brand_Logo.FileName);
                        string path = System.IO.Path.Combine(
                                               Server.MapPath("~/UploadedImages"), pic);

                        Brand_Logo.SaveAs(path);

                        using (MemoryStream ms = new MemoryStream())
                        {
                            Brand_Logo.InputStream.CopyTo(ms);
                            byte[] array = ms.GetBuffer();
                        }
                        brand.Brand_Logo = Brand_Logo.FileName;

                    }
                    brand.IsDeleted = false;
                    db.Entry(brand).State = EntityState.Modified;
					db.SaveChanges();
					return RedirectToAction("Index");
				}
				return View(brand);
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
        }

        [Authorize(Roles = "Admin")]
        // GET: Brands/Delete/5
        public ActionResult Delete(int? id)
        {
			try
			{
				if (id == null)
				{
					return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
				}
				Brand brand = db.Brands.Find(id);
				if (brand == null)
				{
					return HttpNotFound();
				}
				return View(brand);
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
        }

        // POST: Brands/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
			try
			{
				Brand brand = db.Brands.Find(id);
				db.Brands.Remove(brand);
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
				Brand brand = db.Brands.Find(id);
                brand.IsDeleted = true;
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
