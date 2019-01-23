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

namespace FitFlexApparel.Controllers
{
    [Authorize]
    public class WishlistsController : Controller
    {
        private FitflexApparelEntities db = new FitflexApparelEntities();

        // GET: Wishlists
        public ActionResult Index()
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var wishlists = db.Wishlists.Where(s => s.User_Id == userId);
                List<Product> products = new List<Product>();
                foreach (var item in wishlists)
                {
                    products.Add(item.Product);
                }
                return View(products.ToList());
            }
            catch (Exception ex)
            {
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
                return RedirectToAction("Error", "Home");
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public JsonResult AddToWishlist(int? id)
        {
            try
            {
                if(!User.Identity.IsAuthenticated)
                {
                    return Json("LoginError", JsonRequestBehavior.AllowGet);
                }
                var userId = User.Identity.GetUserId();


                Wishlist checkWishlist = db.Wishlists.FirstOrDefault(s => s.User_Id == userId && s.Product_Id == id);
                if (checkWishlist != null)
                {
                    return Json("Already in Wishlist", JsonRequestBehavior.AllowGet);
                }

                Wishlist wishlistObj = new Wishlist();
                wishlistObj.Added_At = DateTime.Now;
                wishlistObj.Product_Id = id;
                wishlistObj.User_Id = userId;
                db.Wishlists.Add(wishlistObj);
                db.SaveChanges();
                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
                return Json("Something went wrong!", JsonRequestBehavior.AllowGet);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public JsonResult RemoveFromWishlist(int? id)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return Json("LoginError", JsonRequestBehavior.AllowGet);
                }
                var userId = User.Identity.GetUserId();
                Wishlist wishlistObj = db.Wishlists.FirstOrDefault(s => s.User_Id == userId && s.Product_Id == id);
                db.Wishlists.Remove(wishlistObj);
                db.SaveChanges();
                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
                return Json("Something went wrong!", JsonRequestBehavior.AllowGet);
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
