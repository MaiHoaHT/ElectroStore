using ElectroWeb.Models;
using ElectroWeb.Models.Common;
using ElectroWeb.Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Configuration;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckOut(OrderInfoViewModel req)
        {
            var code = new { Success = false, Code = -1, Url = "" };
            if (ModelState.IsValid)
            {
                Cart cart = (Cart)Session["Cart"];
                if (cart != null)
                {
                    Order order = new Order();
                    order.CustomerName = req.CustomerName;
                    order.Phone = req.Phone;
                    order.Address = req.Address;
                    order.Email = req.Email;
                    order.Status = 1;//chưa thanh toán / 2/đã thanh toán, 3/Hoàn thành, 4/hủy
                    cart.Items.ForEach(x => order.OrderDetails.Add(new OrderDetail
                    {
                        ProductID = x.ProductId,
                        Quantity = x.Quantity,
                        Price = x.Price
                    }));
                    order.TotalAmount = cart.Items.Sum(x => (x.Price * x.Quantity));
                    order.Payment = req.Payment;
                    order.CreateDate = DateTime.Now;
                    order.ModifierDate = DateTime.Now;
                    Random rd = new Random();
                    order.CodeOrder = "DH" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9);
                    //order.E = req.CustomerName;
                    dbContext.Orders.Add(order);
                    dbContext.SaveChanges();
                    //send email for customer
                    //var strSanPham = "";
                    //var thanhtien = decimal.Zero;
                    //var TongTien = decimal.Zero;
                    //foreach (var sp in cart.Items)
                    //{
                    //    strSanPham += "<tr>";
                    //    strSanPham += "<td>" + sp.ProductName + "</td>";
                    //    strSanPham += "<td>" + sp.Quantity + "</td>";
                    //    strSanPham += "<td>" + FomatPath.FormatNumber(sp.TotalPrice, 0) + "</td>";
                    //    strSanPham += "</tr>";
                    //    thanhtien += sp.Price * sp.Quantity;
                    //}
                    //TongTien = thanhtien;
                    //string contentCustomer = System.IO.File.ReadAllText(Server.MapPath("~/Content/templates/send2.html"));
                    //contentCustomer = contentCustomer.Replace("{{MaDon}}", order.CodeOrder);
                    //contentCustomer = contentCustomer.Replace("{{SanPham}}", strSanPham);
                    //contentCustomer = contentCustomer.Replace("{{NgayDat}}", DateTime.Now.ToString("dd/MM/yyyy"));
                    //contentCustomer = contentCustomer.Replace("{{TenKhachHang}}", order.CustomerName);
                    //contentCustomer = contentCustomer.Replace("{{Phone}}", order.Phone);
                    //contentCustomer = contentCustomer.Replace("{{Email}}", req.Email);
                    //contentCustomer = contentCustomer.Replace("{{DiaChiNhanHang}}", order.Address);
                    //contentCustomer = contentCustomer.Replace("{{ThanhTien}}", ElectroWeb.Models.Common.FomatPath.FormatNumber(thanhtien, 0));
                    //contentCustomer = contentCustomer.Replace("{{TongTien}}", FomatPath.FormatNumber(TongTien, 0));
                    //SendContact.SendMail("Electro Store", "Đơn hàng #" + order.CodeOrder, contentCustomer.ToString(), req.Email);

                    //string contentAdmin = System.IO.File.ReadAllText(Server.MapPath("~/Content/templates/send1.html"));
                    //contentAdmin = contentAdmin.Replace("{{MaDon}}", order.CodeOrder);
                    //contentAdmin = contentAdmin.Replace("{{SanPham}}", strSanPham);
                    //contentAdmin = contentAdmin.Replace("{{NgayDat}}", DateTime.Now.ToString("dd/MM/yyyy"));
                    //contentAdmin = contentAdmin.Replace("{{TenKhachHang}}", order.CustomerName);
                    //contentAdmin = contentAdmin.Replace("{{Phone}}", order.Phone);
                    //contentAdmin = contentAdmin.Replace("{{Email}}", req.Email);
                    //contentAdmin = contentAdmin.Replace("{{DiaChiNhanHang}}", order.Address);
                    //contentAdmin = contentAdmin.Replace("{{ThanhTien}}", FomatPath.FormatNumber(thanhtien, 0));
                    //contentAdmin = contentAdmin.Replace("{{TongTien}}", FomatPath.FormatNumber(TongTien, 0));
                    //SendContact.SendMail("ShopOnline", "Đơn hàng mới #" + order.CodeOrder, contentAdmin.ToString(), ConfigurationManager.AppSettings["EmailAdmin"]);
                    cart.ClearCart();
                    code = new { Success = true, Code = req.Payment, Url = "" };
                    //var url = "";
                    //if (req.Payment == 2)
                    //{
                    //    var url = UrlPayment(req.TypePaymentVN, order.Code);
                    //    code = new { Success = true, Code = req.TypePayment, Url = url };
                    //}

                    //code = new { Success = true, Code = 1, Url = url };
                    return RedirectToAction("CheckOutSuccess");
                }
            }
            return Json(code);
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
        // Customer information form order
        public ActionResult Checkout_OrderInfor()
        {
            return PartialView();
        }
        public ActionResult CheckOutSuccess()
        {
            return View();
        }
    }
}