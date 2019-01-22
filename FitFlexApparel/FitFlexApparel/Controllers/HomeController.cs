using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FitFlexApparel.Models;
using Microsoft.AspNet.Identity;

namespace FitFlexApparel.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public PartialViewResult Header()
        {
            var db = new FitflexApparelEntities();
            ViewBag.SubCategories = db.SubCategories.Where(s => s.IsDeleted != true).ToList();
            ViewBag.Categories = db.Categories.Where(s => s.IsDeleted != true).ToList();
            return PartialView();
        }

        public PartialViewResult AccountAndCart()
        {
            var db = new FitflexApparelEntities();
            var userId = User.Identity.GetUserId();
            ViewBag.CartItems = db.Carts.Where(s => s.User_Id == userId).ToList();
            return PartialView();
        }
    }
}