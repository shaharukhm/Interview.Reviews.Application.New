using DataModel.Library;
using Interview.Reviews.Web.App.DAL.Db_Context;
using System.Linq;
using System.Web.Mvc;

namespace Interview.Reviews.Web.App.Controllers
{
    public class LoginController : Controller
    {
        private readonly UserDbContext userDbContext;
        
        public LoginController()
        {
            userDbContext = new UserDbContext();
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public ActionResult LogOut()
        {
            //Clear session
            Session.Abandon(); 
            Session.Clear();
            
            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public ActionResult Login(UserModel _user)
        {
           var data = userDbContext.GetUserList().Where(w => w.Email == _user.Email && w.Password == _user.Password).FirstOrDefault();

            if (data != null)
            {
                Session["UserId"] = data.UserId;
                Session["Role"] = data.UserStatusId;
                Session["Name"] = data.FullName;

                ModelState.Clear();
                return RedirectToAction("Index", "Login");
            }
            else
            {
                ViewBag.Message = "Please Enter Valid Email or Password...";
                ModelState.Clear();
                return View();
            }
        }

        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        } 
        
        [HttpPost]
        public ActionResult ForgotPassword(UserModel _user)
        {
            var data = userDbContext.GetUserList().Where(w => w.Email == _user.Email ).FirstOrDefault();
            if (data != null)
            {
                Session["Email"] = _user.Email;
                return RedirectToAction("UpdatePassword", "Login");
            }
            else
            {
                ViewBag.Message = "Please enter a valid email address.";
                ModelState.Clear();
                return View();
            }
            
        }
        
        [HttpGet]
        public ActionResult UpdatePassword()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult UpdatePassword(UpdatePasswordModel _user, string newPassword)
        {
            if (_user.NewPassword == _user.ConfirmPassword )
            {
                string email = (string)Session["Email"];
                if (ModelState.IsValid)
                {
                    userDbContext.UpdatePassword(email, newPassword);
                    ModelState.Clear();
                    return RedirectToAction("Login", "Login");
                }
            }
            else
            {
                ViewBag.Message = "Password must be matched...";
                ModelState.Clear();
                return View();
            }
            return View();
        }
    }
}