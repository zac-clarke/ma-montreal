using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MaMontreal.Data;
using MaMontreal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MaMontreal.Controllers.Manage
{

    [Route("Manage/Users/")]
    public class ManageUsersController : Controller
    {
        private readonly MamDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ManageUsersController(MamDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        // GET: All Users
        [Route("")]
        public async Task<IActionResult> Index()
        {
            return _context.Users != null ?
                        View(await _context.Users.ToListAsync()) :
                        Problem("Entity set 'TestDbContext.Users'  is null.");
        }


        // GET: User/Details/ID
        [Route("Details")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var applicationUser = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            return View(applicationUser);
        }
        ///////////////////////////////////////////////////////////////////////////////////// Edit Roles ///////////////////////////////////////////////////////////////////////////////
        //Custom class to bind to
        public class UserWithRoles
        {
            public string _userId { get; set; }
            public List<ManagedRole> _selectedRoles { get; set; }
        }
        public class ManagedRole
        {
            public string _roleName { get; set; }
            public bool _roleSelected { get; set; } = false;
        }


        [Route("EditRoles")]
        public async Task<IActionResult> EditRoles(string id)
        {

            UserWithRoles userWithRoles = PrepareView(id).Result;
            return View(userWithRoles);
        }

        [Route("EditRoles")]
        [HttpPost]
        public async Task<IActionResult> EditRoles(string id, UserWithRoles userWithRoles)
        {
            UserWithRoles refreshUserWithRoles = PrepareView(id).Result;
            userWithRoles._selectedRoles.ForEach(x => Console.WriteLine(x._roleName + " " + x._roleSelected));
            var user = await _context.Users.FindAsync(id);
            foreach (var role in userWithRoles._selectedRoles)
            {
                if (role._roleSelected)
                    await _userManager.AddToRoleAsync(user, role._roleName);
                else
                    await _userManager.RemoveFromRoleAsync(user, role._roleName);
            }
            TempData["rolesSaved"] = "Changed roles saved";
            return View(refreshUserWithRoles);
        }

        private async Task<UserWithRoles> PrepareView(string id)
        {
            var user = await _context.Users.FindAsync(id);
            var userRoles = await _userManager.GetRolesAsync(user);
            var appRoles = _context.Roles.Select(x => x.Name).ToListAsync().Result;
            List<ManagedRole> currentUserRoles = new List<ManagedRole>();
            foreach (var role in appRoles)
            {
                ManagedRole newRole = new ManagedRole() { _roleName = role, _roleSelected = false };
                if (userRoles.Contains(role))
                    newRole._roleSelected = true;

                currentUserRoles.Add(newRole);
            }
            UserWithRoles userWithRoles = new UserWithRoles()
            {
                _userId = user.Id,
                _selectedRoles = currentUserRoles
            };

            return userWithRoles;
        }

        ///////////////////////////////////////////////////////////////////////////////////// End Edit Roles ///////////////////////////////////////////////////////////////////////////////

        // GET: User/Delete/5
        [Route("Delete")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var applicationUser = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            return View(applicationUser);
        }

        // POST: User/Delete/5
        [Route("Delete")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'TestDbContext.Users'  is null.");
            }
            var applicationUser = await _context.Users.FindAsync(id);
            if (applicationUser != null)
            {
                _context.Users.Remove(applicationUser);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicationUserExists(string id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }

    }
}