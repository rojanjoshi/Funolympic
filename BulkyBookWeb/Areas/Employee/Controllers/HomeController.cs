using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;

namespace BulkyBookWeb.Controllers;
[Area("Employee")]
[Authorize(Roles = "Admin,Employee,Individual")]

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUnitOfWork _unitOfWork;
    

    

    public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }


    [Authorize(Roles = "Admin,Employee,Individual")]
    public IActionResult Index()
    {
        HomeVM homeVM = new()
        {

            Videolist = _unitOfWork.Video.GetAll(includeProperties: "Category"),
            Categorylist = _unitOfWork.Category.GetAll(),
            Message = new(),
            Gallerylist = _unitOfWork.Gallery.GetAll().Take(8),
            IndexGallerylist = _unitOfWork.Gallery.GetAll(),
            Upcomminglist = _unitOfWork.Upcomming.GetAll(),
        };
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

        if (claim == null)
        {
            return View(homeVM);
        }
        else
        {
            homeVM.Message.ApplicationUserId = claim.Value;
        }
        

        return View(homeVM);
    }

    [Authorize(Roles = "Admin,Employee,Individual")]
    public IActionResult Indexgallery()
    {
        HomeVM homeVM = new()
        {

           
            IndexGallerylist = _unitOfWork.Gallery.GetAll(),
        };
        return View(homeVM);
    }

    [Authorize(Roles = "Admin,Employee,Individual")]
    public IActionResult Details(int videoId)
    {
        Comment commentObj = new()
        {
            
            VideoId = videoId,
            Video = _unitOfWork.Video.GetFirstOrDefault(u => u.Id == videoId, includeProperties: "Category"),
            Commentlist = _unitOfWork.Comment.GetAll(u=>u.VideoId==videoId, includeProperties: "Video,ApplicationUser"),
            
        };
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
        commentObj.ApplicationUserId = claim.Value;
        return View(commentObj);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,Employee,Individual")]
    public IActionResult Details(Comment comment)
    {
        


        //Comment CommentFromDb = _unitOfWork.Comment.GetFirstOrDefault(
        //    u => u.ApplicationUserId == claim.Value && u.VideoId == comment.VideoId);


        if (ModelState.IsValid)
        {

            _unitOfWork.Comment.Add(comment);
            TempData["success"] = "Comment created successfully";
            _unitOfWork.Save();

        }
        else 
        {
            TempData["error"] = "Operation Failed";
        }
      


        return RedirectToAction("Details", new { @videoId = comment.VideoId });
    }

    [Authorize(Roles = "Admin,Employee,Individual")]
    public IActionResult Edit(int commentId)
    {
        if (commentId == null || commentId == 0)
        {
            return NotFound();
        }
        var commentFromDbFirst = _unitOfWork.Comment.GetFirstOrDefault(u => u.Id == commentId, includeProperties:"Video,ApplicationUser");

        if (commentFromDbFirst == null)
        {
            return NotFound();
        }

        return View(commentFromDbFirst);

    }


    //POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,Employee,Individual")]
    public IActionResult Edit(Comment commentObj)
    {
       

        if (ModelState.IsValid)
        {
            _unitOfWork.Comment.Update(commentObj);

            _unitOfWork.Save();
            TempData["success"] = "Comment edited successfully";
            return RedirectToAction("Details", new { @videoId = commentObj.VideoId });
        }
        else { TempData["error"] = "Operation failed"; }
        return RedirectToAction("Details", new { @videoId = commentObj.VideoId });
    }



    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [Authorize(Roles = "Admin,Employee,Individual")]
    public IActionResult Delete(int? commentId)
    {
        if (commentId == null || commentId == 0)
        {
            return NotFound();
        }
        //var categoryFromDb = _db.Categories.Find(id);
        var commentFromDbFirst = _unitOfWork.Comment.GetFirstOrDefault(u => u.Id == commentId, includeProperties:"Video,ApplicationUser");
        //var categoryFromDbSingle = _db.Categories.SingleOrDefault(u => u.Id == id);

        if (commentFromDbFirst == null)
        {
            return NotFound();
        }

        return View(commentFromDbFirst);
    }

    //POST
    [Authorize(Roles = "Admin,Employee,Individual")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(Comment commentobj)
    {
        var obj = _unitOfWork.Comment.GetFirstOrDefault(u => u.Id == commentobj.Id, includeProperties:"Video,ApplicationUser");
        if (obj == null)
        {
            return NotFound();
        }

        _unitOfWork.Comment.Remove(obj);
        _unitOfWork.Save();
        TempData["success"] = "Comment deleted successfully";
        return RedirectToAction("Details", new { @videoId = obj.VideoId });

    }

    //POST
    [HttpPost]
    [ActionName("Message")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,Employee,Individual")]
    public IActionResult Message(HomeVM obj)
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
        obj.Message.ApplicationUserId = claim.Value;


        if (ModelState.IsValid)
        {
            _unitOfWork.Message.Add(obj.Message);
            _unitOfWork.Save();
            TempData["success"] = "Message sent successfully";
            return RedirectToAction("Index");
        }
        else
        {
            TempData["error"] = "Message sending failed";
        }


        return RedirectToAction("Index");
    }
}
