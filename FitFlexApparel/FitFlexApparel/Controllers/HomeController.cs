﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FitFlexApparel.Models;
using Microsoft.AspNet.Identity;

namespace FitFlexApparel.Controllers
{
    //[RequireHttps]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var db = new FitflexApparelEntities();
            List<Product> topProducts = new List<Models.Product>();
            try
            {
                topProducts = db.Products.Where(s=>s.IsDeleted != true).OrderByDescending(s=>s.Added_On).Take(10).ToList();
                ViewBag.NewArrivals = db.Products.Where(s => s.IsDeleted != true).OrderByDescending(s => s.Average_Rating).Take(10).ToList();
            }
            catch (Exception ex)
            {
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);

            }
            return View(topProducts);
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
            ViewBag.Categories = db.Categories.Where(s => s.IsDeleted != true).OrderBy(s => s.Category_Name).ToList();
            var userId = User.Identity.GetUserId();
            ViewBag.CartItems = db.Carts.Where(s => s.User_Id == userId).ToList();
            return PartialView();
        }

        public PartialViewResult Footer()
        {
            return PartialView();
        }

        public ActionResult Error()
        {
            return View();
        }

        public ActionResult ContactForm(FormCollection fc)
        {
            var db = new FitflexApparelEntities();
            var contact = new CustomerContactRequest();
            contact.Created_At = DateTime.Now;
            contact.Customer_Email = fc["Email"];
            contact.Customer_Name = fc["Name"];
            contact.Customer_Phone = fc["PhoneNo"];
            contact.Message = fc["Message"];
            contact.IsDeleted = false;
            db.CustomerContactRequests.Add(contact);
            db.SaveChanges();
            return RedirectToAction("Contact");
        }


        public ActionResult FAQ()
        {
            return View();
        }

        public ActionResult TermsAndConditions()
        {
            return View();
        }
    }
}