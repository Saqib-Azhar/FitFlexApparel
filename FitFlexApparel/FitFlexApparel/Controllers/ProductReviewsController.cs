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
    public class ProductReviewsController : Controller
    {
        private FitflexApparelEntities db = new FitflexApparelEntities();

        // GET: ProductReviews
        public ActionResult Index()
        {
			try{
				var productReviews = db.ProductReviews.Include(p => p.AspNetUser);
				return View(productReviews.ToList());
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
		}

        // GET: ProductReviews/Details/5
        public ActionResult Details(int? id)
        {
			try
			{
				if (id == null)
				{
					return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
				}
				ProductReview productReview = db.ProductReviews.Find(id);
				if (productReview == null)
				{
					return HttpNotFound();
				}
				return View(productReview);
			
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
        }

        // GET: ProductReviews/Create
   //     public ActionResult Create()
   //     {
			//try{
			//	ViewBag.User_Id = new SelectList(db.AspNetUsers.Where(s => s.IsDeleted != true), "Id", "Email");
			//	return View();
				
			//}
			//catch(Exception ex)
			//{
   //             ExceptionManagerController.infoMessage(ex.Message);
   //             ExceptionManagerController.writeErrorLog(ex);
			//	return RedirectToAction("Error","Home");
			//}
   //     }

        // POST: ProductReviews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,User_Id,Product_Id,Review,Rating,Created_At,IsDeleted")] ProductReview productReview)
        {
			try
			{
				if (ModelState.IsValid)
				{
					productReview.IsDeleted = false;
					db.ProductReviews.Add(productReview);
					db.SaveChanges();
					return RedirectToAction("Index");
				}

				ViewBag.User_Id = new SelectList(db.AspNetUsers.Where(s => s.IsDeleted != true), "Id", "Email", productReview.User_Id);
				return View(productReview);
				
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
        }

        // GET: ProductReviews/Edit/5
  //      public ActionResult Edit(int? id)
  //      {
		//	try
		//	{
		//		if (id == null)
		//		{
		//			return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
		//		}
		//		ProductReview productReview = db.ProductReviews.Find(id);
		//		if (productReview == null)
		//		{
		//			return HttpNotFound();
		//		}
		//		ViewBag.User_Id = new SelectList(db.AspNetUsers.Where(s => s.IsDeleted != true), "Id", "Email", productReview.User_Id);
		//		return View(productReview);
				
		//	}
		//	catch(Exception ex)
		//	{
  //              ExceptionManagerController.infoMessage(ex.Message);
  //              ExceptionManagerController.writeErrorLog(ex);
		//		return RedirectToAction("Error","Home");
		//	}
		//}

        // POST: ProductReviews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,User_Id,Product_Id,Review,Rating,Created_At,IsDeleted")] ProductReview productReview)
        {
			try{
				if (ModelState.IsValid)
				{
					db.Entry(productReview).State = EntityState.Modified;
					productReview.IsDeleted = false;
					db.SaveChanges();
					return RedirectToAction("Index");
				}
				ViewBag.User_Id = new SelectList(db.AspNetUsers.Where(s => s.IsDeleted != true), "Id", "Email", productReview.User_Id);
				return View(productReview);
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
        }

        // GET: ProductReviews/Delete/5
        public ActionResult Delete(int? id)
        {
			try
			{
				if (id == null)
				{
					return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
				}
				ProductReview productReview = db.ProductReviews.Find(id);
				if (productReview == null)
				{
					return HttpNotFound();
				}
				return View(productReview);
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
        }

        // POST: ProductReviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
			try
			{
				ProductReview productReview = db.ProductReviews.Find(id);
				db.ProductReviews.Remove(productReview);
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
				ProductReview productReview = db.ProductReviews.Find(id);
                productReview.IsDeleted = true;
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
