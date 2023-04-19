using ElectroWeb.Models;
using ElectroWeb.Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElectroWeb.Areas.admin.Controllers
{
    public class MenuController : Controller
    {
        // GET: admin/Menu
        private ApplicationDbContext dbContext = new ApplicationDbContext();
        public ActionResult Index()
        {
            var items = dbContext.Menus.ToList();
            return View(items);
        }
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Menu model )
        {
            if (ModelState.IsValid)
            {
                model.CreateDate = DateTime.Now;
                model.ModifierDate = DateTime.Now;
                model.Alias = ElectroWeb.Models.Common.FomatPath.FilterChar(model.Title);
                dbContext.Menus.Add(model);
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
             
        }
        public ActionResult Edit(int id)
        {
            var item = dbContext.Menus.Find(id);
            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Menu model)
        {
            if (ModelState.IsValid)
            {
                dbContext.Menus.Attach(model);
                model.ModifierDate = DateTime.Now;
                model.Alias = ElectroWeb.Models.Common.FomatPath.FilterChar(model.Title);
                dbContext.Entry(model).Property(x => x.Title).IsModified = true;
                dbContext.Entry(model).Property(x => x.Description).IsModified = true;
                dbContext.Entry(model).Property(x => x.Alias).IsModified = true;
                dbContext.Entry(model).Property(x => x.SeoTitle).IsModified = true;
                dbContext.Entry(model).Property(x => x.SeoDescription).IsModified = true;
                dbContext.Entry(model).Property(x => x.SeoKeywords).IsModified = true;
                dbContext.Entry(model).Property(x => x.Position).IsModified = true;
                dbContext.Entry(model).Property(x => x.ModifierDate).IsModified = true;
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);

        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = dbContext.Menus.Find(id);
            if(item != null)
            {
                dbContext.Menus.Remove(item);
                dbContext.SaveChanges();
                return Json(new {success = true});
            }

            return Json(new { success = false });
        }

        // Active Menu
        [HttpPost]
        public ActionResult IsActive(int id)
        {
            var item = dbContext.Menus.Find(id);
            if (item != null)
            {
                item.IsActive = !item.IsActive;
                dbContext.Entry(item).State = System.Data.Entity.EntityState.Modified;
                dbContext.SaveChanges();
                return Json(new { success = true, isActive = item.IsActive });
            }

            return Json(new { success = false });
        }
    }
}