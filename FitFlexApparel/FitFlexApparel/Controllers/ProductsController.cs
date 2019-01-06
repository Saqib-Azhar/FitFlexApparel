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
            var products = db.Products.Include(p => p.Brand).Include(p => p.SubCategory);
            return View(products.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
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

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.Brand_Id = new SelectList(db.Brands, "Id", "Brand_Name");
            ViewBag.Subcategory_Id = new SelectList(db.SubCategories, "Id", "Subcategory_Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Product_Name,Product_Description,Product_Image1,Product_Image2,Product_Image3,Product_Image4,Product_Image5,Product_Overview,Subcategory_Id,Brand_Id,Product_Stock,Company_Profile,Original_Price,Average_Rating,Total_Ratings,Product_Slug")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Brand_Id = new SelectList(db.Brands, "Id", "Brand_Name", product.Brand_Id);
            ViewBag.Subcategory_Id = new SelectList(db.SubCategories, "Id", "Subcategory_Name", product.Subcategory_Id);
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.Brand_Id = new SelectList(db.Brands, "Id", "Brand_Name", product.Brand_Id);
            ViewBag.Subcategory_Id = new SelectList(db.SubCategories, "Id", "Subcategory_Name", product.Subcategory_Id);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Product_Name,Product_Description,Product_Image1,Product_Image2,Product_Image3,Product_Image4,Product_Image5,Product_Overview,Subcategory_Id,Brand_Id,Product_Stock,Company_Profile,Original_Price,Average_Rating,Total_Ratings,Product_Slug")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Brand_Id = new SelectList(db.Brands, "Id", "Brand_Name", product.Brand_Id);
            ViewBag.Subcategory_Id = new SelectList(db.SubCategories, "Id", "Subcategory_Name", product.Subcategory_Id);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
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
