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
    public class UpcommingController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public UpcommingController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
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
            UpcommingVM upcommingVM = new()
            {
                Upcomming = new(),
                CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
         
            };

            if (id == null || id == 0)
            {
                return View(upcommingVM);
            }
            else
            {
                upcommingVM.Upcomming = _unitOfWork.Upcomming.GetFirstOrDefault(u => u.Id == id);
                if (upcommingVM.Upcomming == null)
                {
                    return NotFound();
                }
                return View(upcommingVM);
            }

        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(UpcommingVM obj, IFormFile? file)
        {

            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\upcomming");
                    var extension = Path.GetExtension(file.FileName);

                    if (obj.Upcomming.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, obj.Upcomming.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    obj.Upcomming.ImageUrl = @"\images\upcomming\" + fileName + extension;
                }
                var upcommingfromdb = _unitOfWork.Upcomming.GetFirstOrDefault(u => u.Name == obj.Upcomming.Name);
                if (obj.Upcomming.Id == 0)
                {
                    if (upcommingfromdb == null)
                    {
                        _unitOfWork.Upcomming.Add(obj.Upcomming);

                        TempData["success"] = "Upcomming games created successfully";

                    }
                    else
                    {
                        //TempData["error"] = "Operation Failed! Duplicate Entry";
                        ModelState.AddModelError("name", "Game already exist");
                        return View(obj);
                    }

                }
                else
                {
                    if (upcommingfromdb == null)
                    {
                        _unitOfWork.Upcomming.Update(obj.Upcomming);
                        TempData["success"] = "Game updated successfully";
                    }
                    else if (upcommingfromdb.Id == obj.Upcomming.Id)
                    {
                        _unitOfWork.Upcomming.Update(obj.Upcomming);
                        TempData["success"] = "Game updated successfully";
                    }
                    else
                    {
                        //TempData["error"] = "Operation Failed! Duplicate Entry";
                        ModelState.AddModelError("name", "Game already exist");
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
            var upcommingList = _unitOfWork.Upcomming.GetAll(includeProperties: "Category");
            return Json(new { data = upcommingList });
        }

        //POST
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = _unitOfWork.Upcomming.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.Upcomming.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });

        }
        #endregion

    }
}
