using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.IO;
using System.Linq;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, Employee")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }


        //GET
        public IActionResult Upsert(int? id)
        {

            ProductVM productVM = new()
            {
                Product = new(),
                CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
           
            };


            if (id == null || id == 0)
            {
                //create product
                return View(productVM);
            }
            else
            {
                productVM.Product = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);
               
                return View(productVM);
                //update product
            }
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM obj, IFormFile? file)
        {
            if (ModelState.IsValid)
            {

                string wwwRootPath = _hostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\products");
                    var extension = Path.GetExtension(file.FileName);

                    if (obj.Product.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, obj.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    obj.Product.ImageUrl = @"\images\products\" + fileName + extension;

                }


                var productfromdb = _unitOfWork.Product.GetFirstOrDefault(u => u.Name == obj.Product.Name);
                if (obj.Product.Id == 0)
                {
                    if (productfromdb == null)
                    {
                        _unitOfWork.Product.Add(obj.Product);

                        TempData["success"] = "Product created successfully";

                    }
                    else
                    {
                        //TempData["error"] = "Operation Failed! Duplicate Entry";
                        ModelState.AddModelError("name", "Name already taken");
                        return View(obj);
                    }

                }
                else
                {
                    if (productfromdb == null)
                    {
                        _unitOfWork.Product.Update(obj.Product);
                        TempData["success"] = "Product updated successfully";
                    }
                    else if (productfromdb.Id == obj.Product.Id)
                    {
                        _unitOfWork.Product.Update(obj.Product);
                        TempData["success"] = "Product updated successfully";
                    }
                    else
                    {
                        //TempData["error"] = "Operation Failed! Duplicate Entry";
                        ModelState.AddModelError("name", "Name already taken");
                        return View(obj);
                    }
                }

                _unitOfWork.Save();

            }
            return RedirectToAction("Index");
        }




        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var productList = _unitOfWork.Product.GetAll(includeProperties: "Category");
            return Json(new { data = productList });
        }

        //POST
   
        #endregion
      
    }
}
