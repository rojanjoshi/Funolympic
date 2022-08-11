using BulkyBook.Models;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class UserroleController : Controller
    {

        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<IdentityUser> userManager;
        

        public UserroleController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return View(roleManager.Roles);
        }

        // GET /admin/roles/edit/5
        public async Task<IActionResult> Edit(string id)
        {
            IdentityRole role = await roleManager.FindByIdAsync(id);

            List<ApplicationUser> members = new List<ApplicationUser>();
            List<ApplicationUser> nonMembers = new List<ApplicationUser>();

            foreach (ApplicationUser user in userManager.Users)
            {
                var list = await userManager.IsInRoleAsync(user, role.Name) ? members : nonMembers;
                list.Add(user);
            }

            return View(new UserRole
            {
                Role = role,
                Members = members,
                NonMembers = nonMembers
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserRole userRole)
        {
            IdentityResult result;

            foreach (string userId in userRole.AddIds ?? new string[] { })
            {
                ApplicationUser user = (ApplicationUser)await userManager.FindByIdAsync(userId);
                result = await userManager.AddToRoleAsync(user, userRole.RoleName);
            }

            foreach (string userId in userRole.DeleteIds ?? new string[] { })
            {
                ApplicationUser user = (ApplicationUser)await userManager.FindByIdAsync(userId);
                result = await userManager.RemoveFromRoleAsync(user, userRole.RoleName);
            }

            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
