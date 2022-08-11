using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BulkyBook.Models;
using BulkyBook.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BulkyBook.DataAccess;
using Microsoft.AspNetCore.Identity;

namespace BulkyBookWeb.Areas.Identity.Pages.Account
{
    public class IndexModel2 : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> userManager;

        public IndexModel2(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            this.userManager = userManager;
        }

   
        [BindProperty]
        public ApplicationUser ApplicationUser { get; set; }
        public async Task OnGet(string id)
        {
         
            ApplicationUser = await _db.ApplicationUsers.FindAsync(id);

         
            
        }
        

        public async Task<IActionResult> OnPost()
        {
             
            if (ModelState.IsValid)
            {
                var userfromdb = await _db.ApplicationUsers.FindAsync(ApplicationUser.Id);
                userfromdb.Name = ApplicationUser.Name;
                userfromdb.City = ApplicationUser.City;
                userfromdb.Email = ApplicationUser.Email;
                userfromdb.State = ApplicationUser.State;
                userfromdb.StreetAddress = ApplicationUser.StreetAddress;
                userfromdb.PhoneNumber = ApplicationUser.PhoneNumber;
                
                await userManager.UpdateAsync(userfromdb);
                return RedirectToPage("Index1");
            }
            return RedirectToPage("Index1");
            
            
         
            
            
            
        }
    }
}