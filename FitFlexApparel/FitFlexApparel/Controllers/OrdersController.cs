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
using FitFlexApparel.Controllers;

namespace FitFlexApparel.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private FitflexApparelEntities db = new FitflexApparelEntities();

        // GET: Orders
        public ActionResult AllOrders()
        {
            try
            {
                var orderDetails = db.Orders.Include(o => o.OrderDetails);
                return View(orderDetails.ToList());
            }
            catch (Exception ex)
            {
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                OrderDetail orderDetail = db.OrderDetails.Find(id);
                if (orderDetail == null)
                {
                    return HttpNotFound();
                }
                return View(orderDetail);

            }
            catch (Exception ex)
            {
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
                return RedirectToAction("Error", "Home");
            }
        }

        public ActionResult Cart()
        {
            var userId = User.Identity.GetUserId();
            var cartDetails = db.Carts.Where(s => s.User_Id == userId);
            return View(cartDetails);
        }

        public ActionResult Checkout()
        {
            var userId = User.Identity.GetUserId();
            var cartDetails = db.Carts.Where(s => s.User_Id == userId);
            return View(cartDetails);
        }

        public ActionResult ConfirmOrder(FormCollection fc)
        {
            var TransactionObj = db.Database.BeginTransaction();
            try
            {
                Order order = new Order();
                var userId = User.Identity.GetUserId();
                var userObj = db.AspNetUsers.FirstOrDefault(s => s.Id == userId);
                var prevOrders = db.Orders.Where(s => s.User_Id == userId);
                order.Order_No = "o"+(prevOrders.Count()+1)+":"+userId;
                order.IsDeleted = false;
                order.Order_Status = 1;
                order.Order_Time = DateTime.Now;
                order.Payment_Method = Convert.ToInt32(fc["Payment_Method"]);
                order.Shipping_Address_City = fc["Shipping_Address_City"];
                order.Shipping_Address_Country = fc["Shipping_Address_Country"];
                order.Shipping_Address_Email = userObj.Email;
                order.Shipping_Address_Line1 = fc["Shipping_Address_Line1"]; 
                order.Shipping_Address_Line2 = fc["Shipping_Address_Line2"]; 
                order.Shipping_Address_PhoneNo = userObj.PhoneNumber;
                order.Shipping_Address_PostCode = fc["Shipping_Address_PostCode"];
                order.User_Name = userObj.DisplayName;

                var orderProducts = db.Carts.Where(s => s.User_Id == userId);
                double? totalAmount = 0;
                foreach(var item in orderProducts)
                {
                    totalAmount = totalAmount + item.Total_Price;
                }
                order.Total_Amount = totalAmount;
                order.Total_Products = orderProducts.Count();

                db.Orders.Add(order);
                db.SaveChanges();

                foreach (var item in orderProducts)
                {
                    OrderDetail orderDetail = new OrderDetail();
                    orderDetail.Order_Id = order.Id;
                    orderDetail.Product_Color = item.Color;
                    orderDetail.Product_Name = item.Product_Name;
                    orderDetail.Product_Price = item.Price_Per_Item;
                    orderDetail.Product_Quantity = Convert.ToString(item.Quantity);
                    orderDetail.Product_Size = item.Size;
                    orderDetail.Total_Price = item.Total_Price;
                    db.OrderDetails.Add(orderDetail);
                    db.SaveChanges();

                    
                }

                OrderActionHistory orderAction = new OrderActionHistory();
                orderAction.Order_Id = order.Id;
                orderAction.Order_Status = 1;
                orderAction.Updated_At = DateTime.Now;
                orderAction.Updated_By = userId;
                db.OrderActionHistories.Add(orderAction);
                db.SaveChanges();

                db.Carts.RemoveRange(orderProducts);
                db.SaveChanges();

                TransactionObj.Commit();


                try
                {
                    string message = "Your Order is Confirmed.<br>Order Details: ";
                    message = message + "<br><b>Order #:</b>" + order.Order_No;
                    message = message + "<br><b>Order Status:</b>" + order.OrderStatu.Order_Status;
                    message = message + "<br><b>Updated At:</b>" + orderAction.Updated_At;
                    message = message + "<br><b>Total Items:</b>" + order.Total_Products;
                    message = message + "<br><b>Total Price:</b>" + order.Total_Amount;
                    message = message + "<br><b>Items:</b>";
                    message = message + "<br><table border='1'><tr><td><b>Product Name</b></td><td><b>Quantity</b></td><td><b>Price</b></td></tr>";
                    foreach (var item in orderProducts)
                    {
                        message = message + "<tr><td>" + item.Product_Name + "</td><td>" + item.Quantity + "</td><td>" + item.Total_Price + "</td></tr>";
                    }
                    message = message + "</table><br><b>Thankyou for using our services, For any further query you can contact at info@fitflexapparel.com</b>";
                    MessagingController messagingController = new MessagingController();
                    messagingController.SendEmail("Order Update | Fitflex", message, order.Shipping_Address_Email, order.User_Name);

                }
                catch (Exception ex)
                {
                    ExceptionManagerController.infoMessage(ex.Message);
                    ExceptionManagerController.writeErrorLog(ex);
                }

            }
            catch (Exception ex)
            {
                TransactionObj.Dispose();
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);
                ViewBag.ErrorMessage = "Something went wrong! Please try again later.";
                return RedirectToAction("Checkout");
            }
            return RedirectToAction("Index","Home");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        #region OrderStages

        [Authorize(Roles = "Admin")]
        public ActionResult OrderProcessed(int? id)
        {
            try
            {
                var userId = User.Identity.GetUserId();

                var order = db.Orders.Find(id);
                order.Order_Status = 2;


                OrderActionHistory orderAction = new OrderActionHistory();
                orderAction.Order_Id = order.Id;
                orderAction.Order_Status = 2;
                orderAction.Updated_At = DateTime.Now;
                orderAction.Updated_By = userId;
                db.OrderActionHistories.Add(orderAction);
                db.SaveChanges();

                try
                {
                    string message = "Your Order is Processed.<br>Order Details: ";
                    message = message + "<br><b>Order #:</b>" + order.Order_No;
                    message = message + "<br><b>Order Status:</b>" + order.OrderStatu.Order_Status;
                    message = message + "<br><b>Updated At:</b>" + orderAction.Updated_At;
                    message = message + "<br><b>Total Items:</b>" + order.Total_Products;
                    message = message + "<br><b>Total Price:</b>" + order.Total_Amount;
                    message = message + "<br><b>Items:</b>";
                    message = message + "<br><table border='1'><tr><td><b>Product Name</b></td><td><b>Quantity</b></td><td><b>Price</b></td></tr>";
                    foreach (var item in order.OrderDetails)
                    {
                        message = message + "<tr><td>" + item.Product_Name + "</td><td>" + item.Product_Quantity + "</td><td>" + item.Total_Price + "</td></tr>";
                    }
                    message = message + "</table><br><b>Thankyou for using our services, For any further query you can contact at info@fitflexapparel.com</b>";
                    MessagingController messagingController = new MessagingController();
                    messagingController.SendEmail("Order Update | Fitflex", message, order.Shipping_Address_Email, order.User_Name);

                }
                catch (Exception ex)
                {
                    ExceptionManagerController.infoMessage(ex.Message);
                    ExceptionManagerController.writeErrorLog(ex);
                }
            }
            catch (Exception ex)
            {
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);

            }
            return RedirectToAction("AllOrders");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult OrderDispatched(int? id)
        {
            try
            {
                var userId = User.Identity.GetUserId();

                var order = db.Orders.Find(id);
                order.Order_Status = 3;


                OrderActionHistory orderAction = new OrderActionHistory();
                orderAction.Order_Id = order.Id;
                orderAction.Order_Status = 3;
                orderAction.Updated_At = DateTime.Now;
                orderAction.Updated_By = userId;
                db.OrderActionHistories.Add(orderAction);
                db.SaveChanges();
                try
                {
                    string message = "Your Order is Dispatched.<br>Order Details: ";
                    message = message + "<br><b>Order #:</b>" + order.Order_No;
                    message = message + "<br><b>Order Status:</b>" + order.OrderStatu.Order_Status;
                    message = message + "<br><b>Updated At:</b>" + orderAction.Updated_At;
                    message = message + "<br><b>Total Items:</b>" + order.Total_Products;
                    message = message + "<br><b>Total Price:</b>" + order.Total_Amount;
                    message = message + "<br><b>Items:</b>";
                    message = message + "<br><table border='1'><tr><td><b>Product Name</b></td><td><b>Quantity</b></td><td><b>Price</b></td></tr>";
                    foreach (var item in order.OrderDetails)
                    {
                        message = message + "<tr><td>" + item.Product_Name + "</td><td>" + item.Product_Quantity + "</td><td>" + item.Total_Price + "</td></tr>";
                    }
                    message = message + "</table><br><b>Thankyou for using our services, For any further query you can contact at info@fitflexapparel.com</b>";
                    MessagingController messagingController = new MessagingController();
                    messagingController.SendEmail("Order Update | Fitflex", message, order.Shipping_Address_Email, order.User_Name);

                }
                catch (Exception ex)
                {
                    ExceptionManagerController.infoMessage(ex.Message);
                    ExceptionManagerController.writeErrorLog(ex);
                }
            }
            catch (Exception ex)
            {
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);

            }
            return RedirectToAction("AllOrders");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult OrderCompleted(int? id)
        {
            try
            {
                var userId = User.Identity.GetUserId();

                var order = db.Orders.Find(id);
                order.Order_Status = 4;


                OrderActionHistory orderAction = new OrderActionHistory();
                orderAction.Order_Id = order.Id;
                orderAction.Order_Status = 4;
                orderAction.Updated_At = DateTime.Now;
                orderAction.Updated_By = userId;
                db.OrderActionHistories.Add(orderAction);
                db.SaveChanges();
                try
                {
                    string message = "Your Order is Completed.<br>Order Details: ";
                    message = message + "<br><b>Order #:</b>" + order.Order_No;
                    message = message + "<br><b>Order Status:</b>" + order.OrderStatu.Order_Status;
                    message = message + "<br><b>Updated At:</b>" + orderAction.Updated_At;
                    message = message + "<br><b>Total Items:</b>" + order.Total_Products;
                    message = message + "<br><b>Total Price:</b>" + order.Total_Amount;
                    message = message + "<br><b>Items:</b>";
                    message = message + "<br><table border='1'><tr><td><b>Product Name</b></td><td><b>Quantity</b></td><td><b>Price</b></td></tr>";
                    foreach (var item in order.OrderDetails)
                    {
                        message = message + "<tr><td>" + item.Product_Name + "</td><td>" + item.Product_Quantity + "</td><td>" + item.Total_Price + "</td></tr>";
                    }
                    message = message + "</table><br><b>Thankyou for using our services, For any further query you can contact at info@fitflexapparel.com</b>";
                    MessagingController messagingController = new MessagingController();
                    messagingController.SendEmail("Order Update | Fitflex", message, order.Shipping_Address_Email, order.User_Name);

                }
                catch (Exception ex)
                {
                    ExceptionManagerController.infoMessage(ex.Message);
                    ExceptionManagerController.writeErrorLog(ex);
                }
            }
            catch (Exception ex)
            {
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);

            }
            return RedirectToAction("AllOrders");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult RejectOrder(int? id)
        {
            try
            {
                var userId = User.Identity.GetUserId();

                var order = db.Orders.Find(id);
                order.Order_Status = 5;


                OrderActionHistory orderAction = new OrderActionHistory();
                orderAction.Order_Id = order.Id;
                orderAction.Order_Status = 5;
                orderAction.Updated_At = DateTime.Now;
                orderAction.Updated_By = userId;
                db.OrderActionHistories.Add(orderAction);
                db.SaveChanges();
                try
                {
                    string message = "Your Order is Canceled, Please Contact with Admin at info@fitflexapparel.com<br>Order Details: ";
                    message = message + "<br><b>Order #:</b>" + order.Order_No;
                    message = message + "<br><b>Order Status:</b>" + order.OrderStatu.Order_Status;
                    message = message + "<br><b>Updated At:</b>" + orderAction.Updated_At;
                    message = message + "<br><b>Total Items:</b>" + order.Total_Products;
                    message = message + "<br><b>Total Price:</b>" + order.Total_Amount;
                    message = message + "<br><b>Items:</b>";
                    message = message + "<br><table border='1'><tr><td><b>Product Name</b></td><td><b>Quantity</b></td><td><b>Price</b></td></tr>";
                    foreach (var item in order.OrderDetails)
                    {
                        message = message + "<tr><td>" + item.Product_Name + "</td><td>" + item.Product_Quantity + "</td><td>" + item.Total_Price + "</td></tr>";
                    }
                    message = message + "</table><br><b>Thankyou for using our services, For any further query you can contact at info@fitflexapparel.com</b>";
                    MessagingController messagingController = new MessagingController();
                    messagingController.SendEmail("Order Update | Fitflex", message, order.Shipping_Address_Email, order.User_Name);

                }
                catch (Exception ex)
                {
                    ExceptionManagerController.infoMessage(ex.Message);
                    ExceptionManagerController.writeErrorLog(ex);
                }
            }
            catch (Exception ex)
            {
                ExceptionManagerController.infoMessage(ex.Message);
                ExceptionManagerController.writeErrorLog(ex);

            }
            return RedirectToAction("AllOrders");
        }
        #endregion
    }
}
