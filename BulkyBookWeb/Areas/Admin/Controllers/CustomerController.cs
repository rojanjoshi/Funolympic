using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, Employee")]
    public class CustomerController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        

        public CustomerController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<BulkyBook.Models.Customer> objcustomerList = _unitOfWork.Customer.GetAll();
            return View(objcustomerList);
        }


        //GET
        public IActionResult Upsert(int? id)
        {
            BulkyBook.Models.Customer customer = new BulkyBook.Models.Customer();
            try
            {
                if (id == null || id == 0)
                {
                    //create product
                    return View(customer);
                }
                else if (id == 1)
                {
                    return BadRequest();
                }
                else if (id == 2)
                {
                    return BadRequest();
                }
                else
                {
                    customer = _unitOfWork.Customer.GetFirstOrDefault(u => u.Id == id);
                    if (customer == null)
                    {
                        return NotFound();
                    }
                    return View(customer);
                    //update product
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception Caught.", e);
            }
            return View(customer);

        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(BulkyBook.Models.Customer obj)
        {
                       
                if (ModelState.IsValid)
                {
               
                    var customerfromdb = _unitOfWork.Customer.GetFirstOrDefault(u => u.Name == obj.Name);
                                                           
                    if (obj.Id == 0)
                    {
                        if (customerfromdb == null)
                        {
                            _unitOfWork.Customer.Add(obj);

                            TempData["success"] = "Customer created successfully";
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
                    if (customerfromdb == null)
                    {
                        _unitOfWork.Customer.Update(obj);
                        TempData["success"] = "Customer updated successfully";
                    }

                    else if (customerfromdb.Id == obj.Id)
                    {
                        _unitOfWork.Customer.Update(obj);
                        TempData["success"] = "Customer updated successfully";
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




 
      
    }
}
