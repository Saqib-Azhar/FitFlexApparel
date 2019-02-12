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
    public class ContributionRequestsController : Controller
    {
        private FitflexApparelEntities db = new FitflexApparelEntities();

        // GET: ContributionRequests
        public ActionResult Index()
        {
            try
            {
                var contributionRequests = db.ContributionRequests.Include(c => c.ContributionRequestsType);
                return View(contributionRequests.ToList());
            }
            catch (Exception ex)
            {
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: ContributionRequests/Details/5
        public ActionResult Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ContributionRequest contributionRequest = db.ContributionRequests.Find(id);
                if (contributionRequest == null)
                {
                    return HttpNotFound();
                }
                contributionRequest.Is_Read = true;
                db.SaveChanges();
                return View(contributionRequest);

            }
            catch (Exception ex)
            {

                ContributionRequest contributionRequest = db.ContributionRequests.Find(id);
                if (contributionRequest == null)
                {
                    return HttpNotFound();
                }
                return View(contributionRequest);
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: ContributionRequests/Create
        public ActionResult Create(int? type = 1)
        {
            try
            {
                switch(type)
                {
                    case 1:
                        ViewBag.Request_Type = "Bulk Importer";
                        break;
                    case 2:
                        ViewBag.Request_Type = "Brand Owners";
                        break;
                    case 3:
                        ViewBag.Request_Type = "Online Stores";
                        break;
                    case 4:
                        ViewBag.Request_Type = "Sports Clubs";
                        break;
                }
                ViewBag.Request_Id = type;
                ViewBag.Request_Type_Id = new SelectList(db.ContributionRequestsTypes, "Id", "Request_Type", type);
                return View();

            }
            catch (Exception ex)
            {
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
                return RedirectToAction("Error", "Home");
            }
        }

        // POST: ContributionRequests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Requested_At,Is_Read,Name,Email,Contact_No,Country,Message,Request_Type_Id")] ContributionRequest contributionRequest)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    contributionRequest.Is_Read = false;
                    contributionRequest.Requested_At = DateTime.Now;
                    db.ContributionRequests.Add(contributionRequest);
                    db.SaveChanges();
                    return RedirectToAction("Create");
                }

                ViewBag.Request_Type_Id = new SelectList(db.ContributionRequestsTypes, "Id", "Request_Type", contributionRequest.Request_Type_Id);
                return View(contributionRequest);

            }
            catch (Exception ex)
            {
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: ContributionRequests/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ContributionRequest contributionRequest = db.ContributionRequests.Find(id);
                if (contributionRequest == null)
                {
                    return HttpNotFound();
                }
                ViewBag.Request_Type_Id = new SelectList(db.ContributionRequestsTypes, "Id", "Request_Type", contributionRequest.Request_Type_Id);
                return View(contributionRequest);

            }
            catch (Exception ex)
            {
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
                return RedirectToAction("Error", "Home");
            }
        }

        // POST: ContributionRequests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Requested_At,Is_Read,Name,Email,Contact_No,Country,Message,Request_Type_Id")] ContributionRequest contributionRequest)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(contributionRequest).State = EntityState.Modified;

                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.Request_Type_Id = new SelectList(db.ContributionRequestsTypes, "Id", "Request_Type", contributionRequest.Request_Type_Id);
                return View(contributionRequest);
            }
            catch (Exception ex)
            {
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: ContributionRequests/Delete/5
        public ActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ContributionRequest contributionRequest = db.ContributionRequests.Find(id);
                if (contributionRequest == null)
                {
                    return HttpNotFound();
                }
                return View(contributionRequest);
            }
            catch (Exception ex)
            {
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
                return RedirectToAction("Error", "Home");
            }
        }

        // POST: ContributionRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                ContributionRequest contributionRequest = db.ContributionRequests.Find(id);
                db.ContributionRequests.Remove(contributionRequest);
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
