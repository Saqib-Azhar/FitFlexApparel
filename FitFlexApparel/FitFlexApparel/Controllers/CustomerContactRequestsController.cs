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
    public class CustomerContactRequestsController : Controller
    {
        private FitflexApparelEntities db = new FitflexApparelEntities();

        // GET: CustomerContactRequests
        public ActionResult Index()
        {
            return View(db.CustomerContactRequests.ToList());
        }

        // GET: CustomerContactRequests/Details/5
        public ActionResult Details(int? id)
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

        // GET: CustomerContactRequests/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CustomerContactRequests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Customer_Name,Customer_Email,Customer_Phone,Message,Created_At")] CustomerContactRequest customerContactRequest)
        {
            if (ModelState.IsValid)
            {
                db.CustomerContactRequests.Add(customerContactRequest);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customerContactRequest);
        }

        // GET: CustomerContactRequests/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: CustomerContactRequests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Customer_Name,Customer_Email,Customer_Phone,Message,Created_At")] CustomerContactRequest customerContactRequest)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customerContactRequest).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customerContactRequest);
        }

        // GET: CustomerContactRequests/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: CustomerContactRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CustomerContactRequest customerContactRequest = db.CustomerContactRequests.Find(id);
            db.CustomerContactRequests.Remove(customerContactRequest);
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
