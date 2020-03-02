using _17_33330_1_Mid_Lab.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _17_33330_1_Mid_Lab.Controllers
{
    public class UserController : Controller
    {
        private readonly DiaryDBEntities context = new DiaryDBEntities();

        [HttpGet]
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Login(User user)
        {
            try
            {
                var logged_user = context.Users
                   .Where(u => u.username == user.username && u.password == user.password)
                   .FirstOrDefault();

                if (logged_user != null)
                {
                    Session["username"] = logged_user.username;
                    Session["user_id"] = logged_user.id;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Message = "Incorrect credentials.";
                    return View();
                }
            }
            catch
            {
                ViewBag.Message = "Failed to log in.";
                return View();
            }
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Register(User user)
        {
            try
            {
                context.Users.Add(user);
                context.SaveChanges();

                Session["username"] = user.username;
                Session["user_id"] = user.id;

                return RedirectToRoute("Index", "Home");
            }
            catch
            {
                ViewBag.Message = "Failed to create user.";
                return View();
            }
        }
    }
}