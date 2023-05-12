using ElectroWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElectroWeb.Controllers
{
    public class ProductController : Controller
    {
        private ApplicationDbContext dbContext = new ApplicationDbContext();
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ProductItemsByCateID(string alias)
        {
            var listProducts = dbContext.Products.Where(x=> x.ProductCategory.Alias == alias && x.IsHome && x.IsActive).Take(10).ToList();
            //var listProducts = dbContext.Products.Where(x => x.IsHome && x.IsActive).Take(10).ToList();
            return PartialView(listProducts);
        }

        public ActionResult ProductItemsSaleByCateID(string alias)
        {
            var listProducts = dbContext.Products.Where(x => x.ProductCategory.Alias == alias && x.IsSale && x.IsActive).Take(10).ToList();
            //var listProducts = dbContext.Products.Where(x => x.IsHome && x.IsActive).Take(10).ToList();
            return PartialView(listProducts);
        }
        public ActionResult AllProductItemsSale(int top)
        {
            var listProducts = dbContext.Products.Where(x => x.IsSale && x.IsActive).OrderBy(e => e.Id).Skip(top).Take(3).ToList();
            return PartialView(listProducts);
        }
    }
}