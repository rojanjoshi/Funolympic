using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, Employee")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        
        public IActionResult Index()
        {
            return View();
        }

        //GET
        public IActionResult Upsert(int? id)
        {
            Category category = new();

            if (id == null || id == 0)
            {
                return View(category);
            }
            else
            {
                category = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);
                if (category == null)
                {
                    return NotFound();
                }
                return View(category);
            }
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Category obj)
        {

            if (ModelState.IsValid)
            {
                var categoryfromdb = _unitOfWork.Category.GetFirstOrDefault(u => u.Name == obj.Name);
                if (obj.Id == 0)
                {
                    if (categoryfromdb == null)
                    {
                        _unitOfWork.Category.Add(obj);

                        TempData["success"] = "Category created successfully";

                    }
                    else
                    {
                        //TempData["error"] = "Operation Failed! Duplicate Entry";
                        ModelState.AddModelError("name", "Category already exist");
                        return View(obj);
                    }

                }
                else
                {
                    if (categoryfromdb == null)
                    {
                        _unitOfWork.Category.Update(obj);
                        TempData["success"] = "Category updated successfully";
                    }
                    else if (categoryfromdb.Id == obj.Id)
                    {
                        _unitOfWork.Category.Update(obj);
                        TempData["success"] = "Category updated successfully";
                    }
                    else
                    {
                        //TempData["error"] = "Operation Failed! Duplicate Entry";
                        ModelState.AddModelError("name", "Category already exist");
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
            var categorylist = _unitOfWork.Category.GetAll();
            return Json(new { data = categorylist });
        }

        //POST
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

           

            _unitOfWork.Category.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });

        }
        #endregion
    }
}
