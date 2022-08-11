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
    public class VideoController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public VideoController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
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
            VideoVM videoVM = new()
            {
                Video = new(),
                CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
         
            };

            if (id == null || id == 0)
            {
                return View(videoVM);
            }
            else
            {
                videoVM.Video = _unitOfWork.Video.GetFirstOrDefault(u => u.Id == id);
                if (videoVM.Video == null)
                {
                    return NotFound();
                }
                return View(videoVM);
            }

        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(VideoVM obj, IFormFile? file)
        {

            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\videos");
                    var extension = Path.GetExtension(file.FileName);

                    if (obj.Video.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, obj.Video.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    obj.Video.ImageUrl = @"\images\videos\" + fileName + extension;
                }
                var videofromdb = _unitOfWork.Video.GetFirstOrDefault(u => u.Videolink == obj.Video.Videolink);
                if (obj.Video.Id == 0)
                {
                    if (videofromdb == null)
                    {
                        _unitOfWork.Video.Add(obj.Video);

                        TempData["success"] = "Video created successfully";

                    }
                    else
                    {
                        //TempData["error"] = "Operation Failed! Duplicate Entry";
                        ModelState.AddModelError("name", "Video already exist");
                        return View(obj);
                    }

                }
                else
                {
                    if (videofromdb == null)
                    {
                        _unitOfWork.Video.Update(obj.Video);
                        TempData["success"] = "Video updated successfully";
                    }
                    else if (videofromdb.Id == obj.Video.Id)
                    {
                        _unitOfWork.Video.Update(obj.Video);
                        TempData["success"] = "Video updated successfully";
                    }
                    else
                    {
                        //TempData["error"] = "Operation Failed! Duplicate Entry";
                        ModelState.AddModelError("name", "Video already exist");
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
            var videoList = _unitOfWork.Video.GetAll(includeProperties: "Category");
            return Json(new { data = videoList });
        }

        //POST
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = _unitOfWork.Video.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.Video.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });

        }
        #endregion

    }
}
