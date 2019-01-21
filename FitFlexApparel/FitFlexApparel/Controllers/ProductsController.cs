using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FitFlexApparel.Models;
using Microsoft.AspNet.Identity;
using System.IO;

namespace FitFlexApparel.Controllers
{
    public class ProductsController : Controller
    {
        private FitflexApparelEntities db = new FitflexApparelEntities();

        // GET: Products
        public ActionResult Index()
        {
			try{
				var products = db.Products.Include(p => p.Brand).Include(p => p.SubCategory).Where(s => s.IsDeleted != true);
				return View(products.ToList());
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
		}

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
			try
			{
				if (id == null)
				{
					return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
				}
				Product product = db.Products.Find(id);
				if (product == null)
				{
					return HttpNotFound();
				}
				return View(product);
			
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
        }
        [HttpPost]
        public ActionResult NewReview(FormCollection fc)
        {
                var ProductId = fc["ProductId"];
            try
            {
                var Name = fc["Name"];
                var Email = fc["Email"];
                var UserId = User.Identity.GetUserId();
                var Review = fc["Review"];
                var Rating = fc["rating"];

                ProductReview reviewObj = new ProductReview();
                reviewObj.Created_At = DateTime.Now;
                reviewObj.IsDeleted = false;
                reviewObj.Rating = Convert.ToInt32(Rating);
                reviewObj.Review = Review;
                reviewObj.User_Id = UserId;
                reviewObj.Product_Id = Convert.ToInt32(ProductId);
                db.ProductReviews.Add(reviewObj);
                db.SaveChanges();

                var product = db.Products.Find(Convert.ToInt32(ProductId));
                product.Average_Rating = ((product.Average_Rating * product.Total_Ratings) + Convert.ToInt32(Rating)) / (product.Total_Ratings + 1);
                product.Total_Ratings++;
                db.SaveChanges();
                return Redirect("/Products/Display/" + ProductId);

            }
            catch (Exception ex)
            {
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);

            }
                return Redirect("/Products/Display/"+ProductId);
        }

        public ActionResult Display(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ViewBag.Reviews = db.ProductReviews.Where(s => s.Product_Id == id).ToList();
                Product product = db.Products.Find(id);
                if (product == null)
                {
                    return HttpNotFound();
                }
                return View(product);

            }
            catch (Exception ex)
            {
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
                return RedirectToAction("Error", "Home");
            }
        }
        
        public JsonResult AddColor(string colorCode, string colorName)
        {
            try
            {
                Color color = new Color();
                color.Color_Code = colorCode;
                color.Color_Name = colorName;
                db.Colors.Add(color);
                db.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult AddSize(string SizeCode, string SizeName)
        {
            try
            {
                Size size = new Size();
                size.Size_Code = SizeCode;
                size.Size_Name = SizeName;
                db.Sizes.Add(size);
                db.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        // GET: Products/Create
        public ActionResult Create()
        {
			try
            {
                ViewBag.ColorsAvailable = new SelectList(db.Colors, "Color_Code", "Color_Name"); 
                ViewBag.SizesAvailable = new SelectList(db.Sizes, "Size_Code", "Size_Name");
				ViewBag.Brand_Id = new SelectList(db.Brands.Where(s => s.IsDeleted != true), "Id", "Brand_Name");
				ViewBag.Subcategory_Id = new SelectList(db.SubCategories.Where(s => s.IsDeleted != true), "Id", "Subcategory_Name");
				return View();
				
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Product_Name,Product_Description,Product_Image1,Product_Image2,Product_Image3,Product_Image4,Product_Image5,Product_Overview,Subcategory_Id,Brand_Id,Product_Stock,Company_Profile,Original_Price,Average_Rating,Total_Ratings,Product_Slug,IsDeleted")] Product product, HttpPostedFileBase Product_Image1, HttpPostedFileBase Product_Image2, HttpPostedFileBase Product_Image3, HttpPostedFileBase Product_Image4, HttpPostedFileBase Product_Image5)
        {
			try
			{
				if (ModelState.IsValid)
                {
                    var priceList = Request.Form["PricesListField"];
                    List<ProductPrice> PricesList = new List<ProductPrice>();
                    if(priceList != null && priceList != "")
                    {
                        foreach(var item in priceList.Split('|'))
                        {
                            ProductPrice priceObj = new ProductPrice();
                            foreach (var obj in item.Split(','))
                            {
                                priceObj.IsDeleted = false;
                                var objProp = obj.Split(':');
                                if (objProp[0] == "Min")
                                    priceObj.Min = Convert.ToInt32(objProp[1]);
                                if (objProp[0] == "Max")
                                    priceObj.Max = Convert.ToInt32(objProp[1]);
                                if (objProp[0] == "Price")
                                    priceObj.Price = Convert.ToInt32(objProp[1]);
                                if (objProp[0] == "Discount")
                                    priceObj.Discount = Convert.ToInt32(objProp[1]);
                            }
                            if (priceObj.Price == null)
                                continue;
                            PricesList.Add(priceObj);
                        }
                    }
                    if (Product_Image1 != null)
                    {
                        string pic = System.IO.Path.GetFileName(Product_Image1.FileName);
                        string path = System.IO.Path.Combine(
                                               Server.MapPath("~/Uploadedimages/Products"), pic);

                        Product_Image1.SaveAs(path);

                        using (MemoryStream ms = new MemoryStream())
                        {
                            Product_Image1.InputStream.CopyTo(ms);
                            byte[] array = ms.GetBuffer();
                        }
                        product.Product_Image1 = "Products/" + Product_Image1.FileName;

                    }

                    if (Product_Image2 != null)
                    {
                        string pic = System.IO.Path.GetFileName(Product_Image2.FileName);
                        string path = System.IO.Path.Combine(
                                               Server.MapPath("~/Uploadedimages/Products"), pic);

                        Product_Image2.SaveAs(path);

                        using (MemoryStream ms = new MemoryStream())
                        {
                            Product_Image2.InputStream.CopyTo(ms);
                            byte[] array = ms.GetBuffer();
                        }
                        product.Product_Image2 = "Products/" + Product_Image2.FileName;

                    }

                    if (Product_Image3 != null)
                    {
                        string pic = System.IO.Path.GetFileName(Product_Image3.FileName);
                        string path = System.IO.Path.Combine(
                                               Server.MapPath("~/Uploadedimages/Products"), pic);

                        Product_Image3.SaveAs(path);

                        using (MemoryStream ms = new MemoryStream())
                        {
                            Product_Image3.InputStream.CopyTo(ms);
                            byte[] array = ms.GetBuffer();
                        }
                        product.Product_Image3 = "Products/" + Product_Image3.FileName;

                    }

                    if (Product_Image4 != null)
                    {
                        string pic = System.IO.Path.GetFileName(Product_Image4.FileName);
                        string path = System.IO.Path.Combine(
                                               Server.MapPath("~/Uploadedimages/Products"), pic);

                        Product_Image4.SaveAs(path);

                        using (MemoryStream ms = new MemoryStream())
                        {
                            Product_Image4.InputStream.CopyTo(ms);
                            byte[] array = ms.GetBuffer();
                        }
                        product.Product_Image4 = "Products/" + Product_Image4.FileName;

                    }

                    if (Product_Image5 != null)
                    {
                        string pic = System.IO.Path.GetFileName(Product_Image5.FileName);
                        string path = System.IO.Path.Combine(
                                               Server.MapPath("~/Uploadedimages/Products"), pic);

                        Product_Image5.SaveAs(path);

                        using (MemoryStream ms = new MemoryStream())
                        {
                            Product_Image5.InputStream.CopyTo(ms);
                            byte[] array = ms.GetBuffer();
                        }
                        product.Product_Image5 = "Products/" + Product_Image5.FileName;

                    }
                    var sizes = Request.Form["SizesAvailable"];
                    var colors = Request.Form["ColorsAvailable"];
                    product.Average_Rating = 0;
                    product.Total_Ratings = 0;
                    product.Product_Slug = product.Product_Name.ToLower().Trim().Replace(' ', '-').Replace("'", "-").Replace('"','-');
                    product.IsDeleted = false;
                    product.Sizes = sizes;
                    product.Colors = colors;
					db.Products.Add(product);
					db.SaveChanges();
                    foreach(var item in PricesList)
                    {
                        item.Product_Id = product.Id;
                        db.ProductPrices.Add(item);
                        db.SaveChanges();
                    }
					return RedirectToAction("Index");
				}

                ViewBag.Brand_Id = new SelectList(db.Brands.Where(s => s.IsDeleted != true), "Id", "Brand_Name", product.Brand_Id);
				ViewBag.Subcategory_Id = new SelectList(db.SubCategories.Where(s => s.IsDeleted != true), "Id", "Subcategory_Name", product.Subcategory_Id);
				return View(product);
				
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
			try
			{
				if (id == null)
				{
					return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
				}
				Product product = db.Products.Find(id);
				if (product == null)
				{
					return HttpNotFound();
				}
				ViewBag.Brand_Id = new SelectList(db.Brands.Where(s => s.IsDeleted != true), "Id", "Brand_Name", product.Brand_Id);
				ViewBag.Subcategory_Id = new SelectList(db.SubCategories.Where(s => s.IsDeleted != true), "Id", "Subcategory_Name", product.Subcategory_Id);
				return View(product);
				
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
		}

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Product_Name,Product_Description,Product_Image1,Product_Image2,Product_Image3,Product_Image4,Product_Image5,Product_Overview,Subcategory_Id,Brand_Id,Product_Stock,Company_Profile,Original_Price,Average_Rating,Total_Ratings,Product_Slug,IsDeleted")] Product product, HttpPostedFileBase Product_Image1, HttpPostedFileBase Product_Image2, HttpPostedFileBase Product_Image3, HttpPostedFileBase Product_Image4, HttpPostedFileBase Product_Image5)
        {
			try{
				if (ModelState.IsValid)
				{

                    if (Product_Image1 != null)
                    {
                        string pic = System.IO.Path.GetFileName(Product_Image1.FileName);
                        string path = System.IO.Path.Combine(
                                               Server.MapPath("~/Uploadedimages/Products"), pic);

                        Product_Image1.SaveAs(path);

                        using (MemoryStream ms = new MemoryStream())
                        {
                            Product_Image1.InputStream.CopyTo(ms);
                            byte[] array = ms.GetBuffer();
                        }
                        product.Product_Image1 = "Products/" + Product_Image1.FileName;

                    }

                    if (Product_Image2 != null)
                    {
                        string pic = System.IO.Path.GetFileName(Product_Image2.FileName);
                        string path = System.IO.Path.Combine(
                                               Server.MapPath("~/Uploadedimages/Products"), pic);

                        Product_Image2.SaveAs(path);

                        using (MemoryStream ms = new MemoryStream())
                        {
                            Product_Image2.InputStream.CopyTo(ms);
                            byte[] array = ms.GetBuffer();
                        }
                        product.Product_Image2 = "Products/" + Product_Image2.FileName;

                    }

                    if (Product_Image3 != null)
                    {
                        string pic = System.IO.Path.GetFileName(Product_Image3.FileName);
                        string path = System.IO.Path.Combine(
                                               Server.MapPath("~/Uploadedimages/Products"), pic);

                        Product_Image3.SaveAs(path);

                        using (MemoryStream ms = new MemoryStream())
                        {
                            Product_Image3.InputStream.CopyTo(ms);
                            byte[] array = ms.GetBuffer();
                        }
                        product.Product_Image3 = "Products/" + Product_Image3.FileName;

                    }

                    if (Product_Image4 != null)
                    {
                        string pic = System.IO.Path.GetFileName(Product_Image4.FileName);
                        string path = System.IO.Path.Combine(
                                               Server.MapPath("~/Uploadedimages/Products"), pic);

                        Product_Image4.SaveAs(path);

                        using (MemoryStream ms = new MemoryStream())
                        {
                            Product_Image4.InputStream.CopyTo(ms);
                            byte[] array = ms.GetBuffer();
                        }
                        product.Product_Image4 = "Products/" + Product_Image4.FileName;

                    }

                    if (Product_Image5 != null)
                    {
                        string pic = System.IO.Path.GetFileName(Product_Image5.FileName);
                        string path = System.IO.Path.Combine(
                                               Server.MapPath("~/Uploadedimages/Products"), pic);

                        Product_Image5.SaveAs(path);

                        using (MemoryStream ms = new MemoryStream())
                        {
                            Product_Image5.InputStream.CopyTo(ms);
                            byte[] array = ms.GetBuffer();
                        }
                        product.Product_Image5 = "Products/" + Product_Image5.FileName;

                    }

                    product.Product_Slug = product.Product_Name.ToLower().Trim().Replace(' ', '-').Replace("'", "-").Replace('"', '-');
                    db.Entry(product).State = EntityState.Modified;
					product.IsDeleted = false;
					db.SaveChanges();
					return RedirectToAction("Index");
				}
				ViewBag.Brand_Id = new SelectList(db.Brands.Where(s => s.IsDeleted != true), "Id", "Brand_Name", product.Brand_Id);
				ViewBag.Subcategory_Id = new SelectList(db.SubCategories.Where(s => s.IsDeleted != true), "Id", "Subcategory_Name", product.Subcategory_Id);
				return View(product);
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
			try
			{
				if (id == null)
				{
					return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
				}
				Product product = db.Products.Find(id);
				if (product == null)
				{
					return HttpNotFound();
				}
				return View(product);
			}
			catch(Exception ex)
			{
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
				return RedirectToAction("Error","Home");
			}
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
			try
			{
				Product product = db.Products.Find(id);
				db.Products.Remove(product);
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
				Product product = db.Products.Find(id);
                product.IsDeleted = true;
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
