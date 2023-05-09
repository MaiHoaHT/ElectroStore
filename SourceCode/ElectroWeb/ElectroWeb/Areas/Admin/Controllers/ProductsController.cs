using ElectroWeb.Models;
using ElectroWeb.Models.EntityFramework;
using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElectroWeb.Areas.admin.Controllers
{
    public class ProductsController : Controller
    {
        private ApplicationDbContext dbContext = new ApplicationDbContext();
        // GET: admin/Products
        public ActionResult Index(int? page)
        {
            var pageSize = 10;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<Product> items = dbContext.Products.OrderByDescending(x => x.Id);
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }
        public ActionResult Add()
        {
            ViewBag.ProductCategory = new SelectList(dbContext.ProductCategories.ToList(), "Id", "Title");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Product model, List<string> Images, List<int> rDefault)
        {
            if (ModelState.IsValid)
            {
                if (Images != null && Images.Count > 0)
                {
                    for (int i = 0; i < Images.Count; i++)
                    {
                        if (i + 1 == rDefault[0])
                        {
                            model.Image = Images[i];
                            model.ProductImage.Add(new ProductImage
                            {
                                ProductID = model.Id,
                                Image = Images[i],
                                IsDefault = true
                            });
                        }
                        else
                        {
                            model.ProductImage.Add(new ProductImage
                            {
                                ProductID = model.Id,
                                Image = Images[i],
                                IsDefault = false
                            });
                        }
                    }
                }
                model.CreateDate = DateTime.Now;
                model.ModifierDate = DateTime.Now;
                if (string.IsNullOrEmpty(model.SeoTitle))
                {
                    model.SeoTitle = model.Name;
                }
                if (string.IsNullOrEmpty(model.Alias))
                    model.Alias = ElectroWeb.Models.Common.FomatPath.FilterChar(model.Name);
                dbContext.Products.Add(model);
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductCategory = new SelectList(dbContext.ProductCategories.ToList(), "Id", "Title");
            return View(model);
        }
        public ActionResult Edit(int id)
        {
            ViewBag.ProductCategoryList = new SelectList(dbContext.ProductCategories.ToList(), "Id", "Title");
            var item = dbContext.Products.Find(id);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product model)
        {
            if (ModelState.IsValid)
            {
                model.ModifierDate = DateTime.Now;
                model.Alias = ElectroWeb.Models.Common.FomatPath.FilterChar(model.Name);
                dbContext.Products.Attach(model);
                dbContext.Entry(model).State = System.Data.Entity.EntityState.Modified;
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductCategoryList = new SelectList(dbContext.ProductCategories.ToList(), "Id", "Title");
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = dbContext.Products.Find(id);
            if (item != null)
            {
                var checkImg = item.ProductImage.Where(x => x.ProductID == item.Id);
                if (checkImg != null)
                {
                        dbContext.ProductImages.RemoveRange(checkImg);
                        dbContext.Products.Remove(item);
                        dbContext.SaveChanges();
                    return Json(new { success = true });
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult IsActive(int id)
        {
            var item = dbContext.Products.Find(id);
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
        public ActionResult IsHome(int id)
        {
            var item = dbContext.Products.Find(id);
            if (item != null)
            {
                item.IsHome = !item.IsHome;
                dbContext.Entry(item).State = System.Data.Entity.EntityState.Modified;
                dbContext.SaveChanges();
                return Json(new { success = true, IsHome = item.IsHome });
            }

            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult IsSale(int id)
        {
            var item = dbContext.Products.Find(id);
            if (item != null)
            {
                item.IsSale = !item.IsSale;
                dbContext.Entry(item).State = System.Data.Entity.EntityState.Modified;
                dbContext.SaveChanges();
                return Json(new { success = true, IsSale = item.IsSale });
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
                        var obj = dbContext.Products.Find(Convert.ToInt32(item));
                        dbContext.Products.Remove(obj);
                        dbContext.SaveChanges();
                    }
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

    }
}