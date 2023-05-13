using ElectroWeb.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElectroWeb.Controllers
{
    public class CartController : Controller
    {
        // DbContext
        private ApplicationDbContext dbContext = new ApplicationDbContext();
        // GET: Cart
        public ActionResult Index()
        {
            Cart cart = (Cart)Session["Cart"];
            if (cart != null && cart.Items.Any())
            {
                ViewBag.CheckCart = cart;
            }
            return View();
        }

        // Show the num of items cart
        public ActionResult ShowCountNum()
        {
            Cart cart = (Cart)Session["Cart"];
            if (cart != null)
            {
                return Json(new { Count = cart.Items.Count }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Count = 0 }, JsonRequestBehavior.AllowGet);
        }

        // Add Product Items to Cart
        [HttpPost]
        public ActionResult AddToCart(int id, int quantity)
        {
            
            var code = new { Success = false, msg = "", code = -1, Count = 0 };
            var checkProduct = dbContext.Products.FirstOrDefault(x => x.Id == id);
            if (checkProduct != null)
            {
                Cart cart = (Cart)Session["Cart"];
                if (cart == null)
                {
                    cart = new Cart();
                }
                CartItems item = new CartItems
                {
                    ProductId = checkProduct.Id,
                    ProductName = checkProduct.Name,
                    CategoryName = checkProduct.ProductCategory.Title,
                    Alias = checkProduct.Alias,
                    Quantity = quantity
                };
                if (checkProduct.ProductImage.FirstOrDefault(x => x.IsDefault) != null)
                {
                    item.ProductImg = checkProduct.ProductImage.FirstOrDefault(x => x.IsDefault).Image;
                }
                item.Price = checkProduct.Price;
                if (checkProduct.PriceSale > 0)
                {
                    item.Price = (decimal)checkProduct.PriceSale;
                }
                item.TotalPrice = item.Quantity * item.Price;
                cart.AddToCart(item, quantity);
                Session["Cart"] = cart;
                code = new { Success = true, msg = "Đã thêm vào giỏ hàng!", code = 1, Count = cart.Items.Count };
            }
            return Json(code);
        }

        // Show Cart Items - Cart Page
        public ActionResult Show_Cart_Items()
        {
            Cart cart = (Cart)Session["Cart"];
            if (cart != null && cart.Items.Any())
            {
                return PartialView(cart.Items);
            }
            return PartialView();
        }

        // Update Num Of Product Item In Cart
        [HttpPost]
        public ActionResult Update(int id, int quantity)
        {
            Cart cart = (Cart)Session["Cart"];
            if (cart != null)
            {
                cart.UpdateQuantity(id, quantity);
                return Json(new { Success = true });
            }
            return Json(new { Success = false });
        }
        [HttpPost]

        // Delete item in cart
        public ActionResult Delete(int id)
        {
            var code = new { Success = false, msg = "", code = -1, Count = 0 };

            Cart cart = (Cart)Session["Cart"];
            if (cart != null)
            {
                var checkProduct = cart.Items.FirstOrDefault(x => x.ProductId == id);
                if (checkProduct != null)
                {
                    cart.Remove(id);
                    code = new { Success = true, msg = "", code = 1, Count = cart.Items.Count };
                }
            }
            return Json(code);
        }
        //Delete All Products In Cart
        [HttpPost]
        public ActionResult DeleteAll()
        {
            Cart cart = (Cart)Session["Cart"];
            if (cart != null)
            {
                cart.ClearCart();
                return Json(new { Success = true });
            }
            return Json(new { Success = false });
        }
        // Check out page
        public ActionResult CheckOut()
        {
            Cart cart = (Cart)Session["Cart"];
            if (cart != null && cart.Items.Any())
            {
                ViewBag.CheckCart = cart;
            }
            return View();
        }
        // Show Cart Items - Cart Page
        public ActionResult Show_Checkout_Items()
        {
            Cart cart = (Cart)Session["Cart"];
            if (cart != null && cart.Items.Any())
            {
                return PartialView(cart.Items);
            }
            return PartialView();
        }
    }
}