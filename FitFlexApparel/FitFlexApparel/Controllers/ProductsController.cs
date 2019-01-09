using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FitFlexApparel.Models;

namespace FitFlexApparel.Controllers
{
    public class ProductsController : Controller
    {
        private FitflexApparelEntities db = new FitflexApparelEntities();

        // GET: Products
        public ActionResult Index()
        {
			try{
				var products = db.Products.Include(p => p.Brand).Include(p => p.SubCategory).Where(s => s.IsDeleted != true);
				return View(products.ToList());
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
		}

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
			try
			{
				if (id == null)
				{
					return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
				}
				Product product = db.Products.Find(id);
				if (product == null)
				{
					return HttpNotFound();
				}
				return View(product);
			
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
        }

        // GET: Products/Create
        public ActionResult Create()
        {
			try{
				ViewBag.Brand_Id = new SelectList(db.Brands.Where(s => s.IsDeleted != true), "Id", "Brand_Name");
				ViewBag.Subcategory_Id = new SelectList(db.SubCategories.Where(s => s.IsDeleted != true), "Id", "Subcategory_Name");
				return View();
				
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Product_Name,Product_Description,Product_Image1,Product_Image2,Product_Image3,Product_Image4,Product_Image5,Product_Overview,Subcategory_Id,Brand_Id,Product_Stock,Company_Profile,Original_Price,Average_Rating,Total_Ratings,Product_Slug,IsDeleted")] Product product)
        {
			try
			{
				if (ModelState.IsValid)
				{
					db.Products.Add(product);
					db.SaveChanges();
					return RedirectToAction("Index");
				}

				ViewBag.Brand_Id = new SelectList(db.Brands.Where(s => s.IsDeleted != true), "Id", "Brand_Name", product.Brand_Id);
				ViewBag.Subcategory_Id = new SelectList(db.SubCategories.Where(s => s.IsDeleted != true), "Id", "Subcategory_Name", product.Subcategory_Id);
				return View(product);
				
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
			try
			{
				if (id == null)
				{
					return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
				}
				Product product = db.Products.Find(id);
				if (product == null)
				{
					return HttpNotFound();
				}
				ViewBag.Brand_Id = new SelectList(db.Brands.Where(s => s.IsDeleted != true), "Id", "Brand_Name", product.Brand_Id);
				ViewBag.Subcategory_Id = new SelectList(db.SubCategories.Where(s => s.IsDeleted != true), "Id", "Subcategory_Name", product.Subcategory_Id);
				return View(product);
				
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
		}

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Product_Name,Product_Description,Product_Image1,Product_Image2,Product_Image3,Product_Image4,Product_Image5,Product_Overview,Subcategory_Id,Brand_Id,Product_Stock,Company_Profile,Original_Price,Average_Rating,Total_Ratings,Product_Slug,IsDeleted")] Product product)
        {
			try{
				if (ModelState.IsValid)
				{
					db.Entry(product).State = EntityState.Modified;
					db.SaveChanges();
					return RedirectToAction("Index");
				}
				ViewBag.Brand_Id = new SelectList(db.Brands.Where(s => s.IsDeleted != true), "Id", "Brand_Name", product.Brand_Id);
				ViewBag.Subcategory_Id = new SelectList(db.SubCategories.Where(s => s.IsDeleted != true), "Id", "Subcategory_Name", product.Subcategory_Id);
				return View(product);
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
        }

        // POST: Products/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SoftDelete(int id)
        {
            try
            {
                Product product = db.Products.Find(id);
                product.IsDeleted = true;
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


        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
			try
			{
				if (id == null)
				{
					return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
				}
				Product product = db.Products.Find(id);
				if (product == null)
				{
					return HttpNotFound();
				}
				return View(product);
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
			try
			{
				Product product = db.Products.Find(id);
				db.Products.Remove(product);
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
