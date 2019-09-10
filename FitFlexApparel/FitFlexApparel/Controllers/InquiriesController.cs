using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FitFlexApparel.Models;
using System.IO;

namespace FitFlexApparel.Controllers
{
    [Authorize(Roles ="Admin")]
    //[RequireHttps]
    public class InquiriesController : Controller
    {
        private FitflexApparelEntities db = new FitflexApparelEntities();

        // GET: Inquiries
        public ActionResult Index()
        {
			try{
				var inquiries = db.Inquiries.Include(i => i.Product);
				return View(inquiries.ToList());
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
		}

        // GET: Inquiries/Details/5
        public ActionResult Details(int? id)
        {
			try
			{
				if (id == null)
				{
					return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
				}
				Inquiry inquiry = db.Inquiries.Find(id);
				if (inquiry == null)
				{
					return HttpNotFound();
				}
				return View(inquiry);
			
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
        }

        // GET: Inquiries/Create
        [AllowAnonymous]
        public ActionResult Create(int? id)
        {
			try{
                ViewBag.ProdId = id;
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

        // POST: Inquiries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Product_Id,Product_Name,Number_Of_Samples,Payment_For_Sample,Payment_For_Shipment,First_Name,Last_Name,Business_Email,Company_Name,Country_Code,Area_Code,Telephone_Number,Ext_Number,Company_Website,Business_Address,City,State_Province,Message,Image_File_1,Image_File_2,Image_File_3,Image_File_4,Image_File_5")] Inquiry inquiry, HttpPostedFileBase Image_File_1, HttpPostedFileBase Image_File_2, HttpPostedFileBase Image_File_3, HttpPostedFileBase Image_File_4, HttpPostedFileBase Image_File_5)
        {
			try
			{
				if (ModelState.IsValid)
                {
                    if (Image_File_1 != null)
                    {
                        string pic = System.IO.Path.GetFileName(Image_File_1.FileName);
                        string path = System.IO.Path.Combine(
                                               Server.MapPath("~/Uploadedimages/Inquiries"), pic);

                        Image_File_1.SaveAs(path);

                        using (MemoryStream ms = new MemoryStream())
                        {
                            Image_File_1.InputStream.CopyTo(ms);
                            byte[] array = ms.GetBuffer();
                        }
                        inquiry.Image_File_1 = "Inquiries/" + Image_File_1.FileName;

                    }

                    if (Image_File_2 != null)
                    {
                        string pic = System.IO.Path.GetFileName(Image_File_2.FileName);
                        string path = System.IO.Path.Combine(
                                               Server.MapPath("~/Uploadedimages/Inquiries"), pic);

                        Image_File_2.SaveAs(path);

                        using (MemoryStream ms = new MemoryStream())
                        {
                            Image_File_2.InputStream.CopyTo(ms);
                            byte[] array = ms.GetBuffer();
                        }
                        inquiry.Image_File_2 = "Inquiries/" + Image_File_2.FileName;

                    }

                    if (Image_File_3 != null)
                    {
                        string pic = System.IO.Path.GetFileName(Image_File_3.FileName);
                        string path = System.IO.Path.Combine(
                                               Server.MapPath("~/Uploadedimages/Inquiries"), pic);

                        Image_File_3.SaveAs(path);

                        using (MemoryStream ms = new MemoryStream())
                        {
                            Image_File_3.InputStream.CopyTo(ms);
                            byte[] array = ms.GetBuffer();
                        }
                        inquiry.Image_File_3 = "Inquiries/" + Image_File_3.FileName;

                    }

                    if (Image_File_4 != null)
                    {
                        string pic = System.IO.Path.GetFileName(Image_File_4.FileName);
                        string path = System.IO.Path.Combine(
                                               Server.MapPath("~/Uploadedimages/Inquiries"), pic);

                        Image_File_4.SaveAs(path);

                        using (MemoryStream ms = new MemoryStream())
                        {
                            Image_File_4.InputStream.CopyTo(ms);
                            byte[] array = ms.GetBuffer();
                        }
                        inquiry.Image_File_4 = "Inquiries/" + Image_File_4.FileName;

                    }

                    if (Image_File_5 != null)
                    {
                        string pic = System.IO.Path.GetFileName(Image_File_5.FileName);
                        string path = System.IO.Path.Combine(
                                               Server.MapPath("~/Uploadedimages/Inquiries"), pic);

                        Image_File_5.SaveAs(path);

                        using (MemoryStream ms = new MemoryStream())
                        {
                            Image_File_5.InputStream.CopyTo(ms);
                            byte[] array = ms.GetBuffer();
                        }
                        inquiry.Image_File_5 = "Inquiries/" + Image_File_5.FileName;

                    }
                    try
                    {
                        inquiry.Product_Name = db.Products.FirstOrDefault(s => s.Id == inquiry.Product_Id).Product_Name;
                    }
                    catch (Exception)
                    {
                    }
                    db.Inquiries.Add(inquiry);
					db.SaveChanges();
                    return Redirect("/Products/Display/" + inquiry.Product_Id);
                }

				ViewBag.Product_Id = new SelectList(db.Products.Where(s => s.IsDeleted != true), "Id", "Product_Name", inquiry.Product_Id);
				return View(inquiry);
				
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
        }

        // GET: Inquiries/Edit/5
        public ActionResult Edit(int? id)
        {
			try
			{
				if (id == null)
				{
					return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
				}
				Inquiry inquiry = db.Inquiries.Find(id);
				if (inquiry == null)
				{
					return HttpNotFound();
				}
				ViewBag.Product_Id = new SelectList(db.Products.Where(s => s.IsDeleted != true), "Id", "Product_Name", inquiry.Product_Id);
				return View(inquiry);
				
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
		}

        // POST: Inquiries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Product_Id,Product_Name,Number_Of_Samples,Payment_For_Sample,Payment_For_Shipment,First_Name,Last_Name,Business_Email,Company_Name,Country_Code,Area_Code,Telephone_Number,Ext_Number,Company_Website,Business_Address,City,State_Province,Message,Image_File_1,Image_File_2,Image_File_3,Image_File_4,Image_File_5,Custom_Field1,Custom_Field2,Custom_Field3")] Inquiry inquiry)
        {
			try{
				if (ModelState.IsValid)
				{
					db.Entry(inquiry).State = EntityState.Modified;
					db.SaveChanges();
					return RedirectToAction("Index");
				}
				ViewBag.Product_Id = new SelectList(db.Products.Where(s => s.IsDeleted != true), "Id", "Product_Name", inquiry.Product_Id);
				return View(inquiry);
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
        }

        // GET: Inquiries/Delete/5
        public ActionResult Delete(int? id)
        {
			try
			{
				if (id == null)
				{
					return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
				}
				Inquiry inquiry = db.Inquiries.Find(id);
				if (inquiry == null)
				{
					return HttpNotFound();
				}
				return View(inquiry);
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
        }

        // POST: Inquiries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
			try
			{
				Inquiry inquiry = db.Inquiries.Find(id);
				db.Inquiries.Remove(inquiry);
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
				Inquiry inquiry = db.Inquiries.Find(id);
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
