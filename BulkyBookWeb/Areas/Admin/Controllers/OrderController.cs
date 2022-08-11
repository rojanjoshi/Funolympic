using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, Employee")]
    
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
        public OrderVM OrderVM { get; set; }

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
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
            OrderVM.OrderHeader.Total = 0;
            

            foreach (var item in OrderVM.OrderDetailList)
            {

                OrderVM.OrderHeader.Total += (item.Price * item.Quantity);               

            }

            return View(OrderVM);

        }



        public IActionResult EditView(int orderheaderId)
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
            OrderVM.OrderHeader.Total = 0;


            foreach (var item in OrderVM.OrderDetailList)
            {

                OrderVM.OrderHeader.Total += (item.Price * item.Quantity);

            }

            return View(OrderVM);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(OrderVM obj)
        {
            OrderHeader orderheaderfromdb = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == obj.OrderHeader.Id);
            OrderDetail orderdetailfromdb = _unitOfWork.OrderDetail.GetFirstOrDefault(u => u.OrderId == obj.OrderHeader.Id);
            var orderheaderidfromdb = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == obj.OrderHeader.Id).Id;

            if (obj.OrderHeader.TransactionId == 2 && obj.OrderHeader.CustomerId == 1)
            {

                TempData["error"] = "Operation Failed, Select valid customer";
                return RedirectToAction("Edit", new { @orderheaderId = orderheaderidfromdb });
            }
            else if (obj.OrderHeader.TransactionId == 2 && obj.OrderHeader.CustomerId == 2)
            {
                TempData["error"] = "Operation Failed, Select valid customer";
                return RedirectToAction("Edit", new { @orderheaderId = orderheaderidfromdb });
            }
        
            else if (obj.OrderHeader.TransactionId == 1 && obj.OrderHeader.CustomerId == 2)
            {
                TempData["error"] = "Operation Failed, Select valid customer";
                return RedirectToAction("Edit", new { @orderheaderId = orderheaderidfromdb });
            }
            else if (obj.OrderHeader.TransactionId == 3 && obj.OrderHeader.CustomerId != 2)
            {
                TempData["error"] = "Operation Failed, Select valid customer";
                return RedirectToAction("Edit", new { @orderheaderId = orderheaderidfromdb });
            }
            else if (obj.OrderHeader.TransactionId > 3 && obj.OrderHeader.CustomerId == 2)
            {
                TempData["error"] = "Operation Failed, Select valid customer";
                return RedirectToAction("Edit", new { @orderheaderId = orderheaderidfromdb });

            }
            else if (obj.OrderHeader.Tendor == null && obj.OrderHeader.TransactionId == 1)
            {
                TempData["error"] = "Operation Failed, Enter Tendor Amount";
                return RedirectToAction("Edit", new { @orderheaderId = orderheaderidfromdb });

            }
            else
            {
                _unitOfWork.OrderHeader.Update(obj.OrderHeader);
                TempData["success"] = "Successfully updated bill";
                _unitOfWork.Save();
            }
           
            

            return RedirectToAction("Index", "Order");
        }









        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(OrderVM obj)
        {



       

            OrderDetail orderdetailfromdb = _unitOfWork.OrderDetail.GetFirstOrDefault(
                u=>u.OrderId==obj.OrderDetail.OrderId && u.ProductId==obj.OrderDetail.ProductId
                );
           

            if (orderdetailfromdb == null)
            {
                _unitOfWork.OrderDetail.Add(obj.OrderDetail);
            }
            else
            {
                _unitOfWork.OrderDetail.IncrementCount(orderdetailfromdb, obj.OrderDetail.Quantity);
            }

            _unitOfWork.Save();
            TempData["success"] = "Product added successfully";
            var orderheaderfromdb = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == obj.OrderDetail.OrderId).Id;
            



            return RedirectToAction("Edit", new { @orderheaderId = orderheaderfromdb });

        }





        [HttpGet]
        public JsonResult getproductprice(int productid)
        {
            double price = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == productid).Price;
            return Json(price);

        }



        public IActionResult Plus(int orderdetailId)
        {
            var orderdetailfromdb = _unitOfWork.OrderDetail.GetFirstOrDefault(u => u.Id == orderdetailId);
            _unitOfWork.OrderDetail.IncrementCount(orderdetailfromdb, 1);
            var orderheaderfromdb = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == orderdetailfromdb.OrderId).Id;
            _unitOfWork.Save();
       

            
            return RedirectToAction("Edit", new { @orderheaderId = orderheaderfromdb });
        }

        public IActionResult Minus(int orderdetailId)
        {
            var orderdetailfromdb = _unitOfWork.OrderDetail.GetFirstOrDefault(u => u.Id == orderdetailId);
            var orderheaderfromdb = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == orderdetailfromdb.OrderId).Id;
            if (orderdetailfromdb.Quantity <= 1)
            {
                _unitOfWork.OrderDetail.Remove(orderdetailfromdb);
               
           

            }
            else
            {
                
                _unitOfWork.OrderDetail.DecrementCount(orderdetailfromdb, 1);
         
            }
            _unitOfWork.Save();
       
            return RedirectToAction("Edit", new { @orderheaderId = orderheaderfromdb } );
        }




        public IActionResult Remove(int orderdetailId)
        {
            var orderdetailfromdb = _unitOfWork.OrderDetail.GetFirstOrDefault(u => u.Id == orderdetailId);
            var orderheaderfromdb = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == orderdetailfromdb.OrderId).Id;
            _unitOfWork.OrderDetail.Remove(orderdetailfromdb);
        
            _unitOfWork.Save();
          

            return RedirectToAction("Edit", new { @orderheaderId = orderheaderfromdb});
        }



        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            IEnumerable<OrderHeader> orderHeaders;
            orderHeaders = _unitOfWork.OrderHeader.GetAll(includeProperties: "ApplicationUser,Transaction,Customer");
            return Json(new { data = orderHeaders });
        }
    
        #endregion
    }
}
