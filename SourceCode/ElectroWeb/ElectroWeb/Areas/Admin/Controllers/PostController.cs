using ElectroWeb.Models;
using ElectroWeb.Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElectroWeb.Areas.admin.Controllers
{
    public class PostController : Controller
    {
        private ApplicationDbContext dbContext = new ApplicationDbContext();
        // GET: admin/Post
        public ActionResult Index()
        {
            var items = dbContext.Posts.ToList();
            return View(items);
        }
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Post model)
        {
            if (ModelState.IsValid)
            {
                model.CreateDate = DateTime.Now;
                model.MenuID = 8;
                model.ModifierDate = DateTime.Now;
                model.Alias = ElectroWeb.Models.Common.FomatPath.FilterChar(model.Title);
                dbContext.Posts.Add(model);
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        public ActionResult Edit(int id)
        {
            var item = dbContext.Posts.Find(id);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Post model)
        {
            if (ModelState.IsValid)
            {
                model.ModifierDate = DateTime.Now;
                model.Alias = ElectroWeb.Models.Common.FomatPath.FilterChar(model.Title);
                dbContext.Posts.Attach(model);
                dbContext.Entry(model).State = System.Data.Entity.EntityState.Modified;
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = dbContext.Posts.Find(id);
            if (item != null)
            {
                dbContext.Posts.Remove(item);
                dbContext.SaveChanges();
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult IsActive(int id)
        {
            var item = dbContext.Posts.Find(id);
            if (item != null)
            {
                item.IsActive = !item.IsActive;
                dbContext.Entry(item).State = System.Data.Entity.EntityState.Modified;
                dbContext.SaveChanges();
                return Json(new { success = true, isAcive = item.IsActive });
            }

            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult DeleteAll(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var items = ids.Split(',');
                if (items != null && items.Any())
                {
                    foreach (var item in items)
                    {
                        var obj = dbContext.Posts.Find(Convert.ToInt32(item));
                        dbContext.Posts.Remove(obj);
                        dbContext.SaveChanges();
                    }
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}
