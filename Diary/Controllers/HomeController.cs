using _17_33330_1_Mid_Lab.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _17_33330_1_Mid_Lab.Controllers
{
    public class HomeController : Controller
    {
        private readonly DiaryDBEntities context = new DiaryDBEntities();

        private readonly SelectList prioritySelectList = new SelectList(
            new List<SelectListItem>
            {
                new SelectListItem {Text = "High", Value = "1"},
                new SelectListItem {Text = "Moderate", Value = "2"},
                new SelectListItem {Text = "Low", Value = "3"},
            }, "Value", "Text", "1"
        );

        [NonAction]
        public bool Authorized()
        {
            return Session["username"] != null;
        }

        [HttpGet]
        public ActionResult Index()
        {
            if (!Authorized())
                return RedirectToAction("Login", "User");

            string username = Session["username"] as string;
            var user = context.Users
                .Where(u => u.username == username)
                .FirstOrDefault();

            ViewBag.username = username;

            var memories = context.Memories
                .Where(m => m.user_id == user.id)
                .OrderByDescending(m => m.created_on)
                .ThenBy(m => m.priority);
            return View(memories);
        }

        // GET: Memories/Details/5
        public ActionResult Details(int id)
        {
            if (!Authorized())
                return RedirectToAction("Login", "User");

            return View(context.Memories.Find(id));
        }

        // GET: Memories/Create
        public ActionResult Create()
        {
            if (!Authorized())
                return RedirectToAction("Login", "User");

            ViewBag.prioritySelectList = prioritySelectList;

            return View();
        }

        // POST: Memories/Create
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection collection)
        {
            if (!Authorized())
                return RedirectToAction("Login", "User");

            try
            {
                int user_id = int.Parse(Session["user_id"].ToString());

                Memory memory = new Memory
                {
                    title = collection["title"],
                    description = collection["description"],
                    created_on = DateTime.Now,
                    last_modified = DateTime.Now,
                    priority = int.Parse(collection["priority"]),
                    user_id = user_id,
                };

                context.Memories.Add(memory);
                context.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Memories/Edit/5
        public ActionResult Edit(int id)
        {
            if (!Authorized())
                return RedirectToAction("Login", "User");

            ViewBag.prioritySelectList = prioritySelectList;

            return View(context.Memories.Find(id));
        }

        // POST: Memories/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(int id, FormCollection collection)
        {
            if (!Authorized())
                return RedirectToAction("Login", "User");

            try
            {
                int user_id = int.Parse(Session["user_id"].ToString());

                var memory = context.Memories.Find(id);
                memory.title = collection["title"];
                memory.description = collection["description"];
                memory.last_modified = DateTime.Now;
                memory.priority = int.Parse(collection["priority"]);

                context.Entry(memory).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Memories/Delete/5
        public ActionResult Delete(int id)
        {
            if (!Authorized())
                return RedirectToAction("Login", "User");

            return View(context.Memories.Find(id));
        }

        // POST: Memories/Delete/5
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            if (!Authorized())
                return RedirectToAction("Login", "User");

            try
            {
                var memory = context.Memories.Find(id);
                
                context.Memories.Remove(memory);
                context.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}