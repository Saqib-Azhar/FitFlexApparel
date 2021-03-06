﻿using System;
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
    [Authorize(Roles = "Admin")]
    //[RequireHttps]
    public class CustomerContactRequestsController : Controller
    {
        private FitflexApparelEntities db = new FitflexApparelEntities();

        // GET: CustomerContactRequests
        public ActionResult Index()
        {
			try{
				return View(db.CustomerContactRequests.Where(s => s.IsDeleted != true).ToList());
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
		}

        // GET: CustomerContactRequests/Details/5
        public ActionResult Details(int? id)
        {
			try
			{
				if (id == null)
				{
					return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
				}
				CustomerContactRequest customerContactRequest = db.CustomerContactRequests.Find(id);
				if (customerContactRequest == null)
				{
					return HttpNotFound();
				}
				return View(customerContactRequest);
			
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
        }

        // GET: CustomerContactRequests/Create
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

        // POST: CustomerContactRequests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Customer_Name,Customer_Email,Customer_Phone,Message,Created_At,IsDeleted")] CustomerContactRequest customerContactRequest)
        {
			try
			{
				if (ModelState.IsValid)
				{
					customerContactRequest.IsDeleted = false;
					db.CustomerContactRequests.Add(customerContactRequest);
					db.SaveChanges();
					return RedirectToAction("Index");
				}

				return View(customerContactRequest);
				
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
        }

        // GET: CustomerContactRequests/Edit/5
        public ActionResult Edit(int? id)
        {
			try
			{
				if (id == null)
				{
					return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
				}
				CustomerContactRequest customerContactRequest = db.CustomerContactRequests.Find(id);
				if (customerContactRequest == null)
				{
					return HttpNotFound();
				}
				return View(customerContactRequest);
				
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
		}

        // POST: CustomerContactRequests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Customer_Name,Customer_Email,Customer_Phone,Message,Created_At,IsDeleted")] CustomerContactRequest customerContactRequest)
        {
			try{
				if (ModelState.IsValid)
				{
					db.Entry(customerContactRequest).State = EntityState.Modified;
					customerContactRequest.IsDeleted = false;
					db.SaveChanges();
					return RedirectToAction("Index");
				}
				return View(customerContactRequest);
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
        }

        // GET: CustomerContactRequests/Delete/5
        public ActionResult Delete(int? id)
        {
			try
			{
				if (id == null)
				{
					return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
				}
				CustomerContactRequest customerContactRequest = db.CustomerContactRequests.Find(id);
				if (customerContactRequest == null)
				{
					return HttpNotFound();
				}
				return View(customerContactRequest);
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
        }

        // POST: CustomerContactRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
			try
			{
				CustomerContactRequest customerContactRequest = db.CustomerContactRequests.Find(id);
				db.CustomerContactRequests.Remove(customerContactRequest);
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
				CustomerContactRequest customerContactRequest = db.CustomerContactRequests.Find(id);
                customerContactRequest.IsDeleted = true;
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
