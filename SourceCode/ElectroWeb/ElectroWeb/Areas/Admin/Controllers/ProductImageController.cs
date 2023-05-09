using ElectroWeb.Models.EntityFramework;
using ElectroWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebGrease;

namespace ElectroWeb.Areas.admin.Controllers
{
    public class ProductImageController : Controller
    {
        // GET: admin/ProductImage
        private ApplicationDbContext dbContext = new ApplicationDbContext();
        public ActionResult Index(int id)
        {
            ViewBag.ProductID = id;
            var items = dbContext.ProductImages.Where(x => x.ProductID == id).ToList();
            return View(items);
        }

        [HttpPost]
        public ActionResult AddImage(int productId, string url)
        {
            dbContext.ProductImages.Add(new ProductImage
            {
                ProductID = productId,
                Image = url,
                IsDefault = false
            });
            dbContext.SaveChanges();
            return Json(new { Success = true });
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = dbContext.ProductImages.Find(id);
            dbContext.ProductImages.Remove(item);
            dbContext.SaveChanges();
            return Json(new { success = true });
        }
        [HttpPost]
        public ActionResult DeleteAll()
        {
            var item = dbContext.ProductImages.ToList();
            dbContext.ProductImages.RemoveRange(item);
            dbContext.SaveChanges();
            return Json(new { success = true });
        }

        // Active Menu
        [HttpPost]
        public ActionResult DefaultImage(int id)
        {
            var item = dbContext.ProductImages;
            var itemClick = item.Find(id);
            var oldDefault = item.Where(e => e.IsDefault == true).ToList();
            if (itemClick != null)
            {
                oldDefault.ForEach(e => e.IsDefault = false);
                itemClick.IsDefault = true;
                dbContext.Entry(itemClick).State = System.Data.Entity.EntityState.Modified;
                dbContext.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}