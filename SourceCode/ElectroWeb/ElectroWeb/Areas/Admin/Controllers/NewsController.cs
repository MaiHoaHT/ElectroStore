using System.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ElectroWeb.Models;
using WebGrease;

namespace ElectroWeb.Areas.admin.Controllers
{
    public class NewsController : Controller
    {
        private ApplicationDbContext dbContext = new ApplicationDbContext();
        // GET: admin/News
        public ActionResult Index()
        {
            var items = dbContext.News.OrderByDescending(x => x.Id).ToList();
            return View(items);
        }
        public ActionResult Add()
        {
            return View();
        }
    }
}