using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElectroWeb.Controllers
{
    public class ProductsController : Controller
    {
        // GET: Products
        public ActionResult _ProductDetail()
        {
            return View();
        }

        public ActionResult _ListLaptop()
        {
            return View();
        }
    }
}