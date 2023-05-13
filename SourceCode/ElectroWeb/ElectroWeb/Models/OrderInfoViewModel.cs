using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ElectroWeb.Models
{
    public class OrderInfoViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tên.")]
        public string CustomerName { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại.")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Địa chỉ giao hàng không được để trống.")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập email.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn phương thức thanh toán.")]
        public int Payment { get; set; }
        public int PaymentVN { get; set; }
    }
}