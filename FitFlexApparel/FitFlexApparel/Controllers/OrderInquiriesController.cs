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
    //[RequireHttps]
    public class OrderInquiriesController : Controller
    {
        private FitflexApparelEntities db = new FitflexApparelEntities();

        // GET: OrderInquiries
        public ActionResult Index()
        {
			try{
				return View(db.Custom_Table_1.ToList());
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
		}

        // GET: OrderInquiries/Details/5
        public ActionResult Details(int? id)
        {
			try
			{
				if (id == null)
				{
					return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
				}
                CustomizedInquiry model = new CustomizedInquiry();
                model.CustomInquiry = new Custom_Table_1();
                model.InquiryItems = new List<List<Custom_Table_2>>();
                Custom_Table_1 custom_Table_1 = db.Custom_Table_1.Find(id);

				if (custom_Table_1 == null)
				{
					return HttpNotFound();
				}
                model.CustomInquiry = custom_Table_1;
                var inquiryId = Convert.ToString(model.CustomInquiry.Id);
                var inquiriesList = db.Custom_Table_2.Where(s => s.Column_10 == inquiryId).ToList();
                var cartProdIdsList = inquiriesList.Select(s => s.Column_2).Distinct();

                List<List<Custom_Table_2>> inqList = new List<List<Custom_Table_2>>();

                foreach (var prodId in cartProdIdsList)
                {
                    var cartProdList = inquiriesList.Where(s => s.Column_2 == prodId).OrderBy(s => s.Column_3);
                    var cartProdColorList = cartProdList.Select(s => s.Column_3).Distinct();
                    foreach (var color in cartProdColorList)
                    {
                        var cartList = cartProdList.Where(s => s.Column_3 == color).ToList();
                        inqList.Add(cartList);
                    }
                }
                model.InquiryItems = inqList;
                return View(model);
			
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
        }

        // GET: OrderInquiries/Delete/5
        public ActionResult Delete(int? id)
        {
			try
			{
				if (id == null)
				{
					return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
				}
				Custom_Table_1 custom_Table_1 = db.Custom_Table_1.Find(id);
				if (custom_Table_1 == null)
				{
					return HttpNotFound();
				}
				return View(custom_Table_1);
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
        }

        // POST: OrderInquiries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
			try
			{
				Custom_Table_1 custom_Table_1 = db.Custom_Table_1.Find(id);
				db.Custom_Table_1.Remove(custom_Table_1);
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
				Custom_Table_1 custom_Table_1 = db.Custom_Table_1.Find(id);
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
