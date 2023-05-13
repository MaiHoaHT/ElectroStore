using ElectroWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElectroWeb.Controllers
{
    public class MenuController : Controller
    {
        private ApplicationDbContext dbContext = new ApplicationDbContext();

        // GET: Menu
        public ActionResult Index()
        {
            return View();
        }
        // Main menu 
        public ActionResult MenuTop()
        {
            // Get menu - order by position
            var menuItems = dbContext.Menus.OrderBy(x => x.Position).ToList();
            return PartialView("_MenuTop", menuItems);
        }
        public ActionResult MenuLeft(int? id)
        {
            if (id != null)
            {
                ViewBag.CateId = id;
            }
            // Get menu - order by position
            var menuItems = dbContext.ProductCategories.OrderBy(x => x.Id).ToList();
            return PartialView("_MenuLeft", menuItems);
        }

        // Menu Product Category
        public ActionResult MenuProductCategory()
        {
            var itemsPC = dbContext.ProductCategories.ToList();
            return PartialView("_MenuProductCategory", itemsPC);
        }
        // Menu Product New Category
        public ActionResult MenuNewProdCate()
        {
            var itemsPC = dbContext.ProductCategories.ToList();
            return PartialView("_MenuNewProdCate", itemsPC);
        }
        // Menu Product Sale Category
        public ActionResult MenuSaleProdCate()
        {
            var itemsPC = dbContext.ProductCategories.ToList();
            return PartialView("_MenuSaleProdCate", itemsPC);
        }
    }
}