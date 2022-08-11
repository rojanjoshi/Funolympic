using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, Employee")]
    public class TransactionController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public TransactionController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<Transaction> objtransactionList = _unitOfWork.Transaction.GetAll();
            return View(objtransactionList);
        }


        //GET
        public IActionResult Upsert(int? id)
        {

            Transaction transaction = new Transaction();
            if (id == null || id == 0)
            {
                //create product
                return View(transaction);
            }
            else if (id == 1)
            {
                return BadRequest();
            }
            else if (id == 2)
            {
                return BadRequest();
            }
            else if (id == 3)
            {
                return BadRequest();
            }
            else
            {
                transaction = _unitOfWork.Transaction.GetFirstOrDefault(u => u.Id == id);
                if (transaction == null)
                {
                    return NotFound();
                }
                return View(transaction);
                //update product
            }
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Transaction obj)
        {
            if (ModelState.IsValid)
            {
                var transactionfromdb = _unitOfWork.Transaction.GetFirstOrDefault(u => u.Name == obj.Name);
                if (transactionfromdb == null)
                {
                    if (obj.Id == 0)
                    {
                        _unitOfWork.Transaction.Add(obj);
                        TempData["success"] = "Transaction created successfully";
                    }
                    else
                    {
                        _unitOfWork.Transaction.Update(obj);
                        TempData["success"] = "Transaction updated successfully";
                    }
                    _unitOfWork.Save();
                    
                    return RedirectToAction("Index");
                }
                else
                {
                    //TempData["error"] = "Operation Failed! Duplicate Entry";
                    ModelState.AddModelError("name", "Name already taken");
                }


            }
            return View(obj);
        }




        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var transactionList = _unitOfWork.Transaction.GetAll();
            return Json(new { data = transactionList });
        }

        //POST

        #endregion
      
    }
}
