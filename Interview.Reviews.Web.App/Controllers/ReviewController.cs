using DataModel.Library;
using Interview.Reviews.Web.App.DAL.Db_Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Interview.Reviews.Web.App.Controllers
{
    public class ReviewController : Controller
    {
        // GET: Review
        ReviewDbContext db = new ReviewDbContext();
        
        public ActionResult Index(string CompanyName)
        {
            //var obj = db.GetReviewsList().ToList();
            //return View(obj);

            List<ReviewModel> reviews;

            if (!string.IsNullOrEmpty(CompanyName))
            {
                reviews = db.SearchByCompanyName(CompanyName);
            }
            else
            {
                // If no company name is provided, fetch all reviews
                reviews = db.GetReviewsList();
            }
            return View(reviews);
        }
        
        public ActionResult AddReview()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                return View();
            }
          
        }

        [HttpGet]
        public ActionResult CreateReview()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult CreateReview(ReviewModel _data)
        {
           int _userId =  Convert.ToInt32(Session["UserId"]);
            _data.UserId = _userId;
             var obj =   db.InsertReview(_data);
            ModelState.Clear();

            return RedirectToAction("Index","Review");
        }

        [HttpGet]
        public ActionResult EditReview(int id)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {

                var data = db.GetReviewsList().Where(w => w.ReviewId == id).FirstOrDefault();
                return View(data);
            }
        }

        [HttpPost]
        public ActionResult EditReview(ReviewModel _review)
        {
            try
            {
                db.UpdateReview(_review);
            }
            catch (Exception ex)
            {
                ViewBag.msg = ex.Message;
            }
            return RedirectToAction("Dashboard","Admin");
        }
        
        public ActionResult DeleteReview(int id)
        {
            db.DeleteReviewByID(id);
            TempData["Message"] = "User has been deleted successfully.";
            return RedirectToAction("Dashboard","Admin");
        }
    }
}