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
    public class WishlistsController : Controller
    {
        private FitflexApparelEntities db = new FitflexApparelEntities();

        // GET: Wishlists
        public ActionResult Index()
        {
			try{
				var wishlists = db.Wishlists.Include(w => w.AspNetUser).Include(w => w.Product);
				return View(wishlists.ToList());
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
		}

        // GET: Wishlists/Details/5
        public ActionResult Details(int? id)
        {
			try
			{
				if (id == null)
				{
					return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
				}
				Wishlist wishlist = db.Wishlists.Find(id);
				if (wishlist == null)
				{
					return HttpNotFound();
				}
				return View(wishlist);
			
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
        }

        // GET: Wishlists/Create
        public ActionResult Create()
        {
			try{
				ViewBag.User_Id = new SelectList(db.AspNetUsers.Where(s => s.IsDeleted != true), "Id", "Email");
				ViewBag.Product_Id = new SelectList(db.Products.Where(s => s.IsDeleted != true), "Id", "Product_Name");
				return View();
				
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
        }

        // POST: Wishlists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,User_Id,Product_Id,Added_At")] Wishlist wishlist)
        {
			try
			{
				if (ModelState.IsValid)
				{
					db.Wishlists.Add(wishlist);
					db.SaveChanges();
					return RedirectToAction("Index");
				}

				ViewBag.User_Id = new SelectList(db.AspNetUsers.Where(s => s.IsDeleted != true), "Id", "Email", wishlist.User_Id);
				ViewBag.Product_Id = new SelectList(db.Products.Where(s => s.IsDeleted != true), "Id", "Product_Name", wishlist.Product_Id);
				return View(wishlist);
				
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
        }

        // GET: Wishlists/Edit/5
        public ActionResult Edit(int? id)
        {
			try
			{
				if (id == null)
				{
					return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
				}
				Wishlist wishlist = db.Wishlists.Find(id);
				if (wishlist == null)
				{
					return HttpNotFound();
				}
				ViewBag.User_Id = new SelectList(db.AspNetUsers.Where(s => s.IsDeleted != true), "Id", "Email", wishlist.User_Id);
				ViewBag.Product_Id = new SelectList(db.Products.Where(s => s.IsDeleted != true), "Id", "Product_Name", wishlist.Product_Id);
				return View(wishlist);
				
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
		}

        // POST: Wishlists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,User_Id,Product_Id,Added_At")] Wishlist wishlist)
        {
			try{
				if (ModelState.IsValid)
				{
					db.Entry(wishlist).State = EntityState.Modified;
					db.SaveChanges();
					return RedirectToAction("Index");
				}
				ViewBag.User_Id = new SelectList(db.AspNetUsers.Where(s => s.IsDeleted != true), "Id", "Email", wishlist.User_Id);
				ViewBag.Product_Id = new SelectList(db.Products.Where(s => s.IsDeleted != true), "Id", "Product_Name", wishlist.Product_Id);
				return View(wishlist);
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
        }

        // GET: Wishlists/Delete/5
        public ActionResult Delete(int? id)
        {
			try
			{
				if (id == null)
				{
					return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
				}
				Wishlist wishlist = db.Wishlists.Find(id);
				if (wishlist == null)
				{
					return HttpNotFound();
				}
				return View(wishlist);
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
        }

        // POST: Wishlists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
			try
			{
				Wishlist wishlist = db.Wishlists.Find(id);
				db.Wishlists.Remove(wishlist);
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
                Wishlist wishlist = db.Wishlists.Find(id);
                db.Wishlists.Remove(wishlist);
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
