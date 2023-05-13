using ElectroWeb.Models;
using ElectroWeb.Models.EntityFramework;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure.MappingViews;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElectroWeb.Controllers
{
    public class ProductController : Controller
    {
        private ApplicationDbContext dbContext = new ApplicationDbContext();
        // GET: AllProducts - Products Page
        public ActionResult Index(int? page)
        {
            var pageSize = 6;
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
        // GET: 1 Product - Product Detail Page
        public ActionResult ProductDetail(string alias, int id)
        {
            var item = dbContext.Products.Find(id);
            //if (item != null)
            //{
            //    dbContext.Products.Attach(item);
            //    //item.ViewCount = item.ViewCount + 1;
            //    //dbContext.Entry(item).Property(x => x.ViewCount).IsModified = true;
            //    dbContext.SaveChanges();
            //}

            return View(item);
        }


        //GET Products By Category - Products Page
        public ActionResult ProductsByCategory(string alias, int id, int? page)
        {
            var pageSize = 6;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<Product> items = dbContext.Products.Where(x => x.ProductCategoryID == id).ToList();
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            
            var cate = dbContext.ProductCategories.Find(id);
            if (cate != null)
            {
                ViewBag.CateName = cate.Title;
            }
            ViewBag.CateId = id;
            return View(items);
        }

        // GET News Products by Category Alias - Home Page
        public ActionResult ProductItemsByCateID(string alias)
        {
            var listProducts = dbContext.Products.Where(x=> x.ProductCategory.Alias == alias && x.IsHome && x.IsActive).Take(10).ToList();
            return PartialView(listProducts);
        }

        // GET Sale Products by Category Alias - Home Page
        public ActionResult ProductItemsSaleByCateID(string alias)
        {
            var listProducts = dbContext.Products.Where(x => x.ProductCategory.Alias == alias && x.IsSale && x.IsActive).Take(10).ToList();
            return PartialView(listProducts);
        }

        // GET All Sale Products by Category Alias - Home Page
        public ActionResult AllProductItemsSale(int top)
        {
            var listProducts = dbContext.Products.Where(x => x.IsSale && x.IsActive).OrderBy(e => e.Id).Skip(top).Take(3).ToList();
            return PartialView(listProducts);
        }
    }
}