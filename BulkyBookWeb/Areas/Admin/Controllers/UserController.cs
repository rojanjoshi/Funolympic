using BulkyBook.Models;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class UserController : Controller
    {

        private readonly UserManager<ApplicationUser> userManager;

        private readonly RoleManager<IdentityRole> roleManager;

        
        

        public UserController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        { 
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        public IActionResult Index()
        {
           
            var users = userManager.Users;
            return View(users);
        }
    }
}
