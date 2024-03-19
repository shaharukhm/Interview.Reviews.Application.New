using DataModel.Library;
using Interview.Reviews.Web.App.DAL.Db_Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Interview.Reviews.Web.App.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        UserDbContext db = new UserDbContext();

        [HttpGet]
        public ActionResult UserProfile()
        {
            int id = Convert.ToInt32(Session["UserId"]);
            UserModel _user = db.GetUserDetails(id);
            if (_user == null)
            {
                return HttpNotFound();
            }

            return View("UserDetails", _user);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                var data = db.GetUserList().Where(w => w.UserId == id).FirstOrDefault();
                return View(data);
            }
        }

        [HttpPost]
        public ActionResult Edit(UserModel user)
        {
            try
            {
                db.EditUser(user);
            }
            catch (Exception ex)
            {

                ViewBag.msg = ex.Message;
            }

            return RedirectToAction("UserProfile");
        }

        public ActionResult Welcome()
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
        public ActionResult RegisterUser()
        {
            var Role = db.UserRoleList();
            ViewBag.list = new SelectList(Role, "UserStatusId", "Status");
            return View();
        }

        [HttpPost]
        public ActionResult RegisterUser(UserModel _data)
        {
            if (ModelState.IsValid)
            {
                db.RegisterUser(_data);
                return RedirectToAction("Login", "Login");
            }

            return View();
        }

        public ActionResult AllUserList()
        {
            var obj = db.GetUserList().ToList();
            return View(obj);
        }

        [HttpGet]
        public ActionResult UserDetails(int id)
        {
            if (id == 0)
            {
                id = Convert.ToInt32(Session["UserId"]);
            }
            UserModel _user = db.GetUserDetails(id);
            if (_user == null)
            {
                return HttpNotFound();
            }
            return View(new List<UserModel> { _user });
        }

        public ActionResult DeleteUser(int id)
        {
            db.DeleteUserByID(id);
            TempData["Message"] = "User has been deleted successfully.";
            return RedirectToAction("AllUserList");
        }

        /// <summary>
        /// //// work
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PromoteToGraduate()
        {
          

            int userId = (int)Session["UserId"];

          
            UserModel user = db.GetUserListToUpdate().Find(w => w.UserId == userId);
            ViewBag.NotApprovalComment = user.NotApprovalComment;
          
            return View(user);
        }
        [HttpPost]
        public ActionResult PromoteToGraduate(UserModel user)
        {
         
                try
                {
                    db.UpdateUserToGraduate(user);
                    ModelState.Clear();
                    return RedirectToAction("Index", "Login");
                }
                catch (Exception ex)
                {

                    ViewBag.msg = ex.Message;
                }

            return View();
        }
    }
}