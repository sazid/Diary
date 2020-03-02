using _17_33330_1_Mid_Lab.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _17_33330_1_Mid_Lab.Controllers
{
    public class UploadController : Controller
    {
        private readonly DiaryDBEntities context = new DiaryDBEntities();

        [NonAction]
        public bool Authorized()
        {
            return Session["username"] != null;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return RedirectToAction("Add");
        }

        [HttpGet]
        public ActionResult Add()
        {
            if (!Authorized())
                return RedirectToAction("Login", "User");

            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Add(Upload upload)
        {
            if (!Authorized())
                return RedirectToAction("Login", "User");

            try
            {
                string fileName = DateTime.Now.ToString("yyyyMMddmmssfff");
                string extension = Path.GetExtension(upload.File.FileName);

                // New file name with extension
                fileName += extension;

                // Set the file path relative to the root directory of the app
                upload.path = $"~/Uploads/{fileName}";

                // Get the absolute file path
                string filePath = Path.Combine(Server.MapPath("~/Uploads/"), fileName);

                // Save the file at the absolute path
                upload.File.SaveAs(filePath);

                // Save into db
                context.Uploads.Add(upload);
                context.SaveChanges();

                ModelState.Clear();
                TempData["Message"] = "File uploaded successfully";
            }
            catch
            {
                TempData["Message"] = "Failed to upload file";
            }
            return View();
        }
    }
}