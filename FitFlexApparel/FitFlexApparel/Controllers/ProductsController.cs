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
                ViewBag.ProductPrices = db.ProductPrices.Where(s => s.Product_Id == id).ToList();
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

                ViewBag.ColorsAvailable = new SelectList(db.Colors, "Color_Code", "Color_Name");
                ViewBag.SizesAvailable = new SelectList(db.Sizes, "Size_Code", "Size_Name");
                ViewBag.Brand_Id = new SelectList(db.Brands.Where(s => s.IsDeleted != true), "Id", "Brand_Name");
                ViewBag.Subcategory_Id = new SelectList(db.SubCategories.Where(s => s.IsDeleted != true), "Id", "Subcategory_Name");
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
        public ActionResult Edit([Bind(Include = "Id,Product_Name,Product_Description,Product_Overview,Subcategory_Id,Brand_Id,Product_Stock,Company_Profile,Original_Price,Average_Rating,Total_Ratings,Product_Slug,IsDeleted")] Product product, HttpPostedFileBase Product_Image1, HttpPostedFileBase Product_Image2, HttpPostedFileBase Product_Image3, HttpPostedFileBase Product_Image4, HttpPostedFileBase Product_Image5)
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

        #region CartManagement
        [HttpPost]
        public JsonResult AddToCart(int productId, int selectedQuantity, string selectedColor, string selectedSize)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("LoginError", JsonRequestBehavior.AllowGet);
                }
                var userId = User.Identity.GetUserId();
                var findObj = db.Carts.FirstOrDefault(s => s.Product_Id == productId && s.User_Id == userId && s.Color == selectedColor && s.Size == selectedSize);
                if(findObj != null)
                {
                    findObj.Quantity = findObj.Quantity + selectedQuantity;
                    var productPriceObj1 = db.ProductPrices.FirstOrDefault(s => s.Product_Id == productId && s.Min <= findObj.Quantity && (s.Max >= findObj.Quantity || s.Max == null));
                    findObj.Price_Per_Item = productPriceObj1.Price;
                    findObj.Total_Price = findObj.Price_Per_Item * findObj.Quantity;
                    db.SaveChanges();
                    var res1 = SyncCart();
                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
                var cart = new Cart();
                cart.Product_Id = productId;
                cart.Quantity = selectedQuantity;
                cart.Size = selectedSize;
                cart.Color = selectedColor;
                cart.User_Id = userId;
                var productPriceObj = db.ProductPrices.FirstOrDefault(s => s.Product_Id == productId && s.Min <= selectedQuantity && (s.Max >= selectedQuantity || s.Max == null));
                cart.Price_Per_Item = productPriceObj.Price;
                cart.Total_Price = cart.Price_Per_Item * selectedQuantity;
                cart.Added_At = DateTime.Now;
                var productObj = db.Products.FirstOrDefault(s => s.Id == productId);
                if (productObj.Product_Image1 != null)
                    cart.Image = productObj.Product_Image1;
                else if (productObj.Product_Image2 != null)
                    cart.Image = productObj.Product_Image2;
                else if (productObj.Product_Image3 != null)
                    cart.Image = productObj.Product_Image3;
                else if (productObj.Product_Image4 != null)
                    cart.Image = productObj.Product_Image4;
                else if (productObj.Product_Image5 != null)
                    cart.Image = productObj.Product_Image5;
                db.Carts.Add(cart);
                db.SaveChanges();
                var res = SyncCart();
                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }
        }

        public bool SyncCart()
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var cartItems = db.Carts.Where(s => s.User_Id == userId);
                double? totalPrice = 0;
                foreach(var item in cartItems)
                {
                    totalPrice = totalPrice + item.Total_Price;
                }
                var totalItems = cartItems.Count();


                string objCartListString = "TotalItems:" + totalItems + ",TotalPrice:" + totalPrice;


                Response.Cookies["CartCookie"].Value = objCartListString;

                Response.Cookies["CartCookie"].Expires = DateTime.Now.AddYears(30);

                return true;
            }
            catch (Exception ex)
            {
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
                return false;
            }
        }



        public ActionResult RemoveFromCart(int? id)
        {
            string url = Request.UrlReferrer.AbsoluteUri;
            try
            {
                var cartItem = db.Carts.FirstOrDefault(s => s.Id == id);
                db.Carts.Remove(cartItem);
                db.SaveChanges();
                var res = SyncCart();
            }
            catch (Exception ex)
            {
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
            }
            return Redirect(url);
        }



        public ActionResult UpdateCartQuantity(int? id, int? quantity)
        {
            string url = Request.UrlReferrer.AbsoluteUri;
            try
            {
                var cartItem = db.Carts.FirstOrDefault(s => s.Id == id);
                cartItem.Quantity = quantity;

                var priceObj = db.ProductPrices.FirstOrDefault(s => s.Product_Id == cartItem.Product_Id && s.Min <= cartItem.Quantity && (s.Max == null || s.Max >= cartItem.Quantity));
                cartItem.Price_Per_Item = priceObj.Price;
                cartItem.Total_Price = cartItem.Price_Per_Item * cartItem.Quantity;

                db.SaveChanges();
                var res = SyncCart();
            }
            catch (Exception ex)
            {
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
            }
            return Redirect(url);
        }

        #endregion


        #region Search

        public ActionResult Search(FormCollection fc)
        {
            List<Product> productsList = new List<Product>();
            try
            {
                string searchedString = fc["Query"];
                var queries = searchedString.Split(' ');
                var selectedCategory = Convert.ToInt32(fc["SelectedCategory"]);
                ViewBag.SearchedQuery = searchedString;
                if (selectedCategory == 0)
                {
                    productsList = db.Products.Where(s => s.Product_Name.Contains(searchedString) && s.IsDeleted != true).ToList();
                    if(productsList.Count == 0)
                    {
                        foreach(var item in queries)
                        {
                            productsList.AddRange(db.Products.Where(s => s.Product_Name.Contains(item) && s.IsDeleted != true).ToList());
                        }
                    }
                }

                else
                {
                    productsList = db.Products.Where(s => s.Product_Name.Contains(searchedString) && s.SubCategory.Category_Id == selectedCategory && s.IsDeleted != true).ToList();
                    if (productsList.Count == 0)
                    {
                        foreach (var item in queries)
                        {
                            productsList.AddRange(db.Products.Where(s => s.Product_Name.Contains(item) && s.SubCategory.Category_Id == selectedCategory && s.IsDeleted != true).ToList());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);

            }
            return View(productsList);
        }


        #endregion



    }
}
