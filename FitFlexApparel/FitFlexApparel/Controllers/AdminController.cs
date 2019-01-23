using FitFlexApparel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FitFlexApparel.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        private FitflexApparelEntities db = new FitflexApparelEntities();
        // GET: Admin
        public ActionResult Dashboard()
        {
            return View();
        }

        public PartialViewResult _Users()
        {
            return PartialView("../AspNetUsers/Index", db.AspNetUsers.ToList());
        }

        public PartialViewResult _Categories()
        {
            return PartialView();
        }

        public PartialViewResult _SubCategories()
        {
            return PartialView();
        }

        public PartialViewResult _Brands()
        {
            return PartialView();
        }

        public PartialViewResult _Products()
        {
            return PartialView();
        }

        public PartialViewResult _ProductReviews()
        {
            return PartialView();
        }

        public PartialViewResult _Messages()
        {
            return PartialView();
        }

        public PartialViewResult _Blogs()
        {
            return PartialView();
        }
        
    }
}