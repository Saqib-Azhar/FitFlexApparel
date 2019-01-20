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
    public class SubCategoriesController : Controller
    {
        private FitflexApparelEntities db = new FitflexApparelEntities();

        // GET: SubCategories
        public ActionResult Index()
        {
			try{
				var subCategories = db.SubCategories.Include(s => s.Category).Where(s => s.IsDeleted != true);
				return View(subCategories.ToList());
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
		}

        // GET: SubCategories/Details/5
        public ActionResult Details(int? id)
        {
			try
			{
				if (id == null)
				{
					return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
				}
				SubCategory subCategory = db.SubCategories.Find(id);
				if (subCategory == null)
				{
					return HttpNotFound();
				}
				return View(subCategory);
			
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
        }

        // GET: SubCategories/Create
        public ActionResult Create()
        {
			try{
				ViewBag.Category_Id = new SelectList(db.Categories.Where(s => s.IsDeleted != true), "Id", "Category_Name");
				return View();
				
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
        }

        // POST: SubCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Subcategory_Name,Subcategory_Description,Subcategory_Image,Category_Id,Subcategory_Slug,IsDeleted")] SubCategory subCategory)
        {
			try
			{
				if (ModelState.IsValid)
				{
					db.SubCategories.Add(subCategory);
					db.SaveChanges();
					return RedirectToAction("Index");
				}

				ViewBag.Category_Id = new SelectList(db.Categories.Where(s => s.IsDeleted != true), "Id", "Category_Name", subCategory.Category_Id);
				return View(subCategory);
				
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
        }

        // GET: SubCategories/Edit/5
        public ActionResult Edit(int? id)
        {
			try
			{
				if (id == null)
				{
					return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
				}
				SubCategory subCategory = db.SubCategories.Find(id);
				if (subCategory == null)
				{
					return HttpNotFound();
				}
				ViewBag.Category_Id = new SelectList(db.Categories.Where(s => s.IsDeleted != true), "Id", "Category_Name", subCategory.Category_Id);
				return View(subCategory);
				
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
		}

        // POST: SubCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Subcategory_Name,Subcategory_Description,Subcategory_Image,Category_Id,Subcategory_Slug,IsDeleted")] SubCategory subCategory)
        {
			try{
				if (ModelState.IsValid)
				{
					db.Entry(subCategory).State = EntityState.Modified;
					db.SaveChanges();
					return RedirectToAction("Index");
				}
				ViewBag.Category_Id = new SelectList(db.Categories.Where(s => s.IsDeleted != true), "Id", "Category_Name", subCategory.Category_Id);
				return View(subCategory);
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
        }

        // GET: SubCategories/Delete/5
        public ActionResult Delete(int? id)
        {
			try
			{
				if (id == null)
				{
					return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
				}
				SubCategory subCategory = db.SubCategories.Find(id);
				if (subCategory == null)
				{
					return HttpNotFound();
				}
				return View(subCategory);
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
        }

        // POST: SubCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
			try
			{
				SubCategory subCategory = db.SubCategories.Find(id);
				db.SubCategories.Remove(subCategory);
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

		
        public ActionResult SoftDelete(int id)
        {
            try
            {
				SubCategory subCategory = db.SubCategories.Find(id);
                subCategory.IsDeleted = true;
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
