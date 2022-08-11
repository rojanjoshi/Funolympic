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

namespace BulkyBookWeb.Areas.Identity.Pages.Account
{
    public class IndexModel1 : PageModel
    {
        private readonly ApplicationDbContext _db;

        public IndexModel1(ApplicationDbContext db)
        {
            _db = db;
        }

        public IEnumerable<ApplicationUser> ApplicationUsers { get; set; }

        public async Task OnGet()
        {
            ApplicationUsers = await _db.ApplicationUsers.ToListAsync();
        }

    
    }
}