using DataModel.Library;
using Interview.Reviews.Web.App.DAL.Db_Context;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Interview.Reviews.Web.App.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        UserDbContext db = new UserDbContext();

        public ActionResult GetAdminDetailById()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            int userId = Convert.ToInt32(Session["UserId"]);

            // Use the user ID to fetch user details
            UserModel user = db.GetUserDetails(userId);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(new List<UserModel> { user });
        }
        
        public ActionResult Dashboard()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                int TotalUsers = db.GetUserCount("tblUser");
                int TotalGraduates = db.GetGraduatesCount("tblUser");
                int TotalAdmins = db.GetAdminCount("tblUser");
                int TotalReviews = db.GetReviewCount("tblReview");

                int TotalPromotionRequests = db.GetPromotionRequestCount("tblUser");
                ViewBag.TotalUsers = TotalUsers;
                ViewBag.TotalGraduates = TotalGraduates;
                ViewBag.TotalAdmins = TotalAdmins;
                ViewBag.TotalReviews = TotalReviews;
                ViewBag.TotalPromotions = TotalPromotionRequests;
                return View();
            }
        }


        public ActionResult CheckPromotionRequests()
          {
         // Your logic to check for promotion requests and get the total count
            int totalPromotions = db.GetPromotionRequestCount("tblUser"); // Implement this method as per your requirements

            // Return the total count as JSON
            return Json(new { totalPromotions }, JsonRequestBehavior.AllowGet);
        }
        /// 
        /// controller for Partial views 
        ///

        public ActionResult GetPromotionsList()
        {
            var promotion = db.GetPromotionsFromDatabase();
            return PartialView("_Promotions", promotion);
        }

        public ActionResult GetGraduateList()
        {
               var Grad = db.GetGraduatesFromDatabase();// Get students data from database using ADO.NET
                return PartialView("_Graduates", Grad);
        }

        public ActionResult GetAdminList()
        {
            var Grad = db.GetAdminsFromDatabase();// Get students data from database using ADO.NET
            return PartialView("_Admins", Grad);
        }

        public ActionResult GetUserList()
        {
            var users = db.GetUsersFromDatabase();// Get students data from database using ADO.NET
            return PartialView("_Users", users);
        }
        public ActionResult GetReviewList()
        {
            var review = db.GetReviewsFromDatabase();// Get students data from database using ADO.NET
            return PartialView("_Reviews", review);
        }

        public ActionResult UpdatePromotionApproval(int UserId)
        {
            try
            {
                UserModel data = new UserModel();
                var _user = db.GetUserList().Find(w => w.UserId == UserId);

                db.UpdatePromotionApproved(_user);
                ModelState.Clear();
                return RedirectToAction("Dashboard", "Admin");
            }
            catch (Exception ex)
            {

                ViewBag.msg = ex.Message;
            }

            return View();
        }

        [HttpGet]
        public ActionResult UpdateRejection(int UserId)
        {
            try
            {
                UserModel data = new UserModel();
                var _user = db.GetUserListToUpdate().Find(w => w.UserId == UserId);
                return View(_user);
               
            }
            catch (Exception ex)
            {

                ViewBag.msg = ex.Message;
            }

            return View();
        }

        [HttpPost]
        public ActionResult UpdateRejection(UserModel _user)
        {
            try
            {
                

                db.UpdateRejection(_user);
                ModelState.Clear();
                return RedirectToAction("Dashboard", "Admin");
            }
            catch (Exception ex)
            {

                ViewBag.msg = ex.Message;
            }

            return View();
        }
    }
}