









using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Security.Claims;
using System.Text;

namespace BulkyBookWeb.Areas.Customer.Controllers
{
    [Area("Employee")]
    [Authorize(Roles = "Admin,Employee,Manager")]
    public class CartController : Controller
    {
      
        private readonly IUnitOfWork _unitOfWork;
    

        public OrderVM OrderVM { get; set; }

        

        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            
            
           
        }

        public IActionResult Index()
        {
         return View();
        }



        
      


       

    



        public IActionResult Create()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            
            

            CartVm cartVm = new()
            {
                ShoppingCart = new(),
                OrderHeader = new(),

                ProductList = _unitOfWork.Product.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                CustomerList = _unitOfWork.Customer.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                TransactionList = _unitOfWork.Transaction.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                ListCart = _unitOfWork.ShoppingCart.GetAll(u=>u.ApplicationUserId==claim.Value,includeProperties: "Product")

            };
            cartVm.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(
               u => u.Id == claim.Value);

            cartVm.OrderHeader.Cashier = cartVm.OrderHeader.ApplicationUser.Name;

            cartVm.OrderHeader.CreatedDate = System.DateTime.Now;
            
            foreach (var item in cartVm.ListCart)
            {
                cartVm.OrderHeader.Total += (item.Price * item.Quantity);
                
            }
            
            return View(cartVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CartVm obj)
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            obj.ShoppingCart.ApplicationUserId = claim.Value;

            ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.GetFirstOrDefault(
           u => u.ApplicationUserId == claim.Value && u.ProductId == obj.ShoppingCart.ProductId);

            if (cartFromDb == null)
            {
                _unitOfWork.ShoppingCart.Add(obj.ShoppingCart);
            }
            else 
            {
                _unitOfWork.ShoppingCart.IncrementCount(cartFromDb, obj.ShoppingCart.Quantity);
            }

                _unitOfWork.Save();
                TempData["success"] = "Click SAVE to create bill";
                return RedirectToAction("Create");
            
        }


        [HttpGet]
        public JsonResult getproductprice(int productid)
        {
            double price = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == productid).Price;
            return Json(price);

        }

        public IActionResult Plus(int cartId)
        {
            var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id == cartId);
            _unitOfWork.ShoppingCart.IncrementCount(cart, 1);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Create));
        }

        public IActionResult Minus(int cartId)
        {
            var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id == cartId);
            if (cart.Quantity <= 1)
            {
                _unitOfWork.ShoppingCart.Remove(cart);
            }
            else
            {
                _unitOfWork.ShoppingCart.DecrementCount(cart, 1);
            }
            _unitOfWork.Save();
            return RedirectToAction(nameof(Create));
        }

        public IActionResult Remove(int cartId)
        {
            var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id == cartId);
            _unitOfWork.ShoppingCart.Remove(cart);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Create));
        }


        

        public IActionResult Order()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            CartVm cartVm = new()
            {
                ShoppingCart = new(),
                OrderHeader = new(),

                ProductList = _unitOfWork.Product.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                CustomerList = _unitOfWork.Customer.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                TransactionList = _unitOfWork.Transaction.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                ListCart = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value, includeProperties: "Product")

            };
            cartVm.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(
               u => u.Id == claim.Value);
            cartVm.OrderHeader.Cashier = cartVm.OrderHeader.ApplicationUser.Name;

            cartVm.OrderHeader.CreatedDate = System.DateTime.Now;
            foreach (var item in cartVm.ListCart)
            {
                cartVm.OrderHeader.Total += (item.Price * item.Quantity);
            }

            return View(cartVm);
        }








       

       



    




        [HttpPost]
        [ActionName("Order")]
        [ValidateAntiForgeryToken]
        public IActionResult OrderPOST(CartVm obj)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            


            obj.ListCart = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value,
                includeProperties: "Product");

           
            obj.OrderHeader.CreatedDate = System.DateTime.Now;
            obj.OrderHeader.ApplicationUserId = claim.Value;
            obj.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(
              u => u.Id == claim.Value);
            obj.OrderHeader.Cashier = obj.OrderHeader.ApplicationUser.Name;
          
            foreach (var cart in obj.ListCart)
            {
                
                
               
                obj.OrderHeader.Total += (cart.Price * cart.Quantity);
            }
            if (obj.ListCart.Count() == 0)
            {
                TempData["error"] = "Operation Failed, Add some products";

                
                return RedirectToAction("Create");
            }
            else if (obj.OrderHeader.TransactionId == 2 && obj.OrderHeader.CustomerId == 1)
            {

                TempData["error"] = "Operation Failed, Select valid customer";


                return RedirectToAction("Create");
            }
            else if (obj.OrderHeader.TransactionId == 2 && obj.OrderHeader.CustomerId == 2)
            {
                TempData["error"] = "Operation Failed, Select valid customer";
                return RedirectToAction("Create");

            }
        
        
            else if (obj.OrderHeader.TransactionId == 1 && obj.OrderHeader.CustomerId == 2)
            {
                TempData["error"] = "Operation Failed, Select valid customer";
                return RedirectToAction("Create");

            }
            else if (obj.OrderHeader.TransactionId == 3 && obj.OrderHeader.CustomerId != 2)
            {
                TempData["error"] = "Operation Failed, Select valid customer";
                return RedirectToAction("Create");

            }
            else if (obj.OrderHeader.TransactionId > 3 && obj.OrderHeader.CustomerId == 2)
            {
                TempData["error"] = "Operation Failed, Select valid customer";
                return RedirectToAction("Create");

            }
            else if (obj.OrderHeader.Tendor == null && obj.OrderHeader.TransactionId == 1)
            {
                TempData["error"] = "Operation Failed, Enter Tendor Amount";

                return RedirectToAction("Create");

            }

            else
            {
            

                _unitOfWork.OrderHeader.Add(obj.OrderHeader);
                TempData["success"] = "Successfully added bill";
                _unitOfWork.Save();

            }
            
                
                
            
         
            
            foreach (var cart in obj.ListCart)
            {
                OrderDetail orderDetail = new()
                {
                    ProductId = cart.ProductId,
                    OrderId = obj.OrderHeader.Id,
                    Price = cart.Price,
                    Quantity = cart.Quantity
                };


                if (obj.ListCart.Count() == 0)
                {
                    TempData["error"] = "Operation Failed, Add some products";
                }
                else if (obj.OrderHeader.TransactionId == 2 && obj.OrderHeader.CustomerId == 1)
                {
                    TempData["error"] = "Operation Failed, Add valid customer";
                }
                else
                {
                    _unitOfWork.OrderDetail.Add(orderDetail);
                    _unitOfWork.Save();
                }

               
                
              
            }
            if (obj.ListCart.Count() == 0)
            {
                TempData["error"] = "Operation Failed, Add some products";
            }
            else if (obj.OrderHeader.TransactionId == 2 && obj.OrderHeader.CustomerId == 1)
            {
                TempData["error"] = "Operation Failed, Add valid customer";
            }
            else
            {
                _unitOfWork.ShoppingCart.RemoveRange(obj.ListCart);
                _unitOfWork.Save();
            }


            var orderheaderfromdb = obj.OrderHeader.Id;




            return RedirectToAction("Edit", new { @orderheaderId = orderheaderfromdb });


            
        }




        public IActionResult Edit(int orderheaderId)
        {

            OrderVM = new OrderVM()
            {

                OrderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == orderheaderId, includeProperties: "ApplicationUser,Customer,Transaction"),

                OrderDetailList = _unitOfWork.OrderDetail.GetAll(u => u.OrderId == orderheaderId, includeProperties: "Product,OrderHeader"),
                CustomerList = _unitOfWork.Customer.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                TransactionList = _unitOfWork.Transaction.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                ProductList = _unitOfWork.Product.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),



            };
            //OrderVM.OrderHeader.Total = 0;


            foreach (var item in OrderVM.OrderDetailList)
            {

                OrderVM.OrderHeader.Total += (item.Price * item.Quantity);

            }

            return View(OrderVM);

        }

        [HttpGet]
        public IActionResult GetAll()
        {
            IEnumerable<OrderHeader> orderHeaders;
            orderHeaders = _unitOfWork.OrderHeader.GetAll(includeProperties: "ApplicationUser,Transaction,Customer");
            return Json(new { data = orderHeaders });
        }
    }
}
