using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElectroWeb.Controllers
{
    public class UsersController : Controller
    {
        // GET: Users
        public ActionResult CheckOut()
        {
            return View();
        }
    }
}