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
    public class CartsController : Controller
    {
        private FitflexApparelEntities db = new FitflexApparelEntities();

        // GET: Carts
        public ActionResult Index()
        {
			try{
				var carts = db.Carts.Include(c => c.AspNetUser).Include(c => c.Product).Include(c => c.ProductSpecification).Where(s => s.IsDeleted != true);
				return View(carts.ToList());
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
		}

        // GET: Carts/Details/5
        public ActionResult Details(int? id)
        {
			try
			{
				if (id == null)
				{
					return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
				}
				Cart cart = db.Carts.Find(id);
				if (cart == null)
				{
					return HttpNotFound();
				}
				return View(cart);
			
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
        }

        // GET: Carts/Create
        public ActionResult Create()
        {
			try{
				ViewBag.User_Id = new SelectList(db.AspNetUsers.Where(s => s.IsDeleted != true), "Id", "Email");
				ViewBag.Product_Id = new SelectList(db.Products.Where(s => s.IsDeleted != true), "Id", "Product_Name");
				ViewBag.Product_Specifications_Id = new SelectList(db.ProductSpecifications.Where(s => s.IsDeleted != true), "Id", "Color");
				return View();
				
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
        }

        // POST: Carts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,User_Id,Product_Id,Product_Specifications_Id,Quantity,IsDeleted")] Cart cart)
        {
			try
			{
				if (ModelState.IsValid)
				{
					db.Carts.Add(cart);
					db.SaveChanges();
					return RedirectToAction("Index");
				}

				ViewBag.User_Id = new SelectList(db.AspNetUsers.Where(s => s.IsDeleted != true), "Id", "Email", cart.User_Id);
				ViewBag.Product_Id = new SelectList(db.Products.Where(s => s.IsDeleted != true), "Id", "Product_Name", cart.Product_Id);
				ViewBag.Product_Specifications_Id = new SelectList(db.ProductSpecifications.Where(s => s.IsDeleted != true), "Id", "Color", cart.Product_Specifications_Id);
				return View(cart);
				
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
        }

        // GET: Carts/Edit/5
        public ActionResult Edit(int? id)
        {
			try
			{
				if (id == null)
				{
					return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
				}
				Cart cart = db.Carts.Find(id);
				if (cart == null)
				{
					return HttpNotFound();
				}
				ViewBag.User_Id = new SelectList(db.AspNetUsers.Where(s => s.IsDeleted != true), "Id", "Email", cart.User_Id);
				ViewBag.Product_Id = new SelectList(db.Products.Where(s => s.IsDeleted != true), "Id", "Product_Name", cart.Product_Id);
				ViewBag.Product_Specifications_Id = new SelectList(db.ProductSpecifications.Where(s => s.IsDeleted != true), "Id", "Color", cart.Product_Specifications_Id);
				return View(cart);
				
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
		}

        // POST: Carts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,User_Id,Product_Id,Product_Specifications_Id,Quantity,IsDeleted")] Cart cart)
        {
			try{
				if (ModelState.IsValid)
				{
					db.Entry(cart).State = EntityState.Modified;
					db.SaveChanges();
					return RedirectToAction("Index");
				}
				ViewBag.User_Id = new SelectList(db.AspNetUsers.Where(s => s.IsDeleted != true), "Id", "Email", cart.User_Id);
				ViewBag.Product_Id = new SelectList(db.Products.Where(s => s.IsDeleted != true), "Id", "Product_Name", cart.Product_Id);
				ViewBag.Product_Specifications_Id = new SelectList(db.ProductSpecifications.Where(s => s.IsDeleted != true), "Id", "Color", cart.Product_Specifications_Id);
				return View(cart);
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
        }

        // GET: Carts/Delete/5
        public ActionResult Delete(int? id)
        {
			try
			{
				if (id == null)
				{
					return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
				}
				Cart cart = db.Carts.Find(id);
				if (cart == null)
				{
					return HttpNotFound();
				}
				return View(cart);
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
        }

        // POST: Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
			try
			{
				Cart cart = db.Carts.Find(id);
				db.Carts.Remove(cart);
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
				Cart cart = db.Carts.Find(id);
                cart.IsDeleted = true;
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
