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
    [Authorize(Roles = "Admin")]
    [RequireHttps]
    public class ColorsController : Controller
    {
        private FitflexApparelEntities db = new FitflexApparelEntities();

        // GET: Colors
        public ActionResult Index()
        {
			try{
				return View(db.Colors.ToList());
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
		}

        // GET: Colors/Details/5
        public ActionResult Details(int? id)
        {
			try
			{
				if (id == null)
				{
					return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
				}
				Color color = db.Colors.Find(id);
				if (color == null)
				{
					return HttpNotFound();
				}
				return View(color);
			
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
        }

        // GET: Colors/Create
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

        // POST: Colors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Color_Name,Color_Code")] Color color)
        {
			try
			{
				if (ModelState.IsValid)
                {
                    var colorString = color.Color_Code.Split('#');
                    color.Color_Code = colorString[1];
                    db.Colors.Add(color);
					db.SaveChanges();
					return RedirectToAction("Index");
				}

				return View(color);
				
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
        }

        // GET: Colors/Edit/5
        public ActionResult Edit(int? id)
        {
			try
			{
				if (id == null)
				{
					return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
				}
				Color color = db.Colors.Find(id);
				if (color == null)
				{
					return HttpNotFound();
                }
                return View(color);
				
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
		}

        // POST: Colors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Color_Name,Color_Code")] Color color)
        {
			try{
				if (ModelState.IsValid)
				{
                    var colorString = color.Color_Code.Split('#');
                    color.Color_Code = colorString[1];

					db.Entry(color).State = EntityState.Modified;
					db.SaveChanges();
					return RedirectToAction("Index");
				}
				return View(color);
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
        }

        // GET: Colors/Delete/5
        public ActionResult Delete(int? id)
        {
			try
			{
				if (id == null)
				{
					return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
				}
				Color color = db.Colors.Find(id);
				if (color == null)
				{
					return HttpNotFound();
				}
				return View(color);
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
        }

        // POST: Colors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
			try
			{
				Color color = db.Colors.Find(id);
				db.Colors.Remove(color);
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
				Color color = db.Colors.Find(id);
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
