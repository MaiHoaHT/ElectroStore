using System.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ElectroWeb.Models;
using WebGrease;
using ElectroWeb.Models.EntityFramework;
using PagedList;

namespace ElectroWeb.Areas.admin.Controllers
{
    public class NewsController : Controller
    {
        private ApplicationDbContext dbContext = new ApplicationDbContext();
        // GET: admin/News
        public ActionResult Index(string search,int? page)
        {
            // the number of news per 1 page
            var pageSize = 10;
            if(page == null)
            {
                page = 1;
            }
            IEnumerable<News> items = dbContext.News.OrderByDescending(x => x.Id);
            if (!string.IsNullOrEmpty(search))
            {
                items = items.Where(x => x.Alias.Contains(search) || x.Title.Contains(search));
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page): 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(News model)
        {
            if (ModelState.IsValid)
            {
                model.CreateDate = DateTime.Now;
                model.ModifierDate = DateTime.Now;
                model.MenuID = 6;
                model.Alias = ElectroWeb.Models.Common.FomatPath.FilterChar(model.Title);
                dbContext.News.Add(model);
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var item = dbContext.News.Find(id);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(News model)
        {
            if (ModelState.IsValid)
            {
                model.ModifierDate = DateTime.Now;
                model.Alias = ElectroWeb.Models.Common.FomatPath.FilterChar(model.Title);
                dbContext.News.Attach(model);
                dbContext.Entry(model).State = System.Data.Entity.EntityState.Modified;
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // Delete News
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = dbContext.News.Find(id);
            if (item != null)
            {
                dbContext.News.Remove(item);
                dbContext.SaveChanges();
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }

        // Active News
        [HttpPost]
        public ActionResult IsActive(int id)
        {
            var item = dbContext.News.Find(id);
            if (item != null)
            {
                item.IsActive = !item.IsActive;
                dbContext.Entry(item).State = System.Data.Entity.EntityState.Modified;
                dbContext.SaveChanges();
                return Json(new { success = true, isActive = item.IsActive });
            }

            return Json(new { success = false });
        }

        // Delete multiple news at once
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
                        var obj = dbContext.News.Find(Convert.ToInt32(item));
                        dbContext.News.Remove(obj);
                        dbContext.SaveChanges();
                    }
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}