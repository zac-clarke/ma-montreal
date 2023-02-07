using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MaMontreal.Data;
using MaMontreal.Models;
using MaMontreal.Models.NotMapped;
using MaMontreal.Services;
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
        private readonly UsersService _usersService;

        public ManageUsersController(
            MamDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            try
            {
                _usersService = new UsersService(context, userManager);
            }
            catch (SystemException ex)
            {
                Problem(ex.Message);
            }
            //tjhese might go
            _context = context;
            _userManager = userManager;
        }


        // GET: All Users
        [Route("")]
        public async Task<IActionResult> Index()
        {
            try
            {
                return View(_usersService.GetAllAsync().Result);
            }
            catch (SystemException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // GET: Manage/Users/Details?id
        [Route("Details")]
        public async Task<IActionResult> Details(string id)
        {
            try
            {
                return View(await _usersService.GetAsync(id));
            }
            catch (NullReferenceException ex)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Manage/Users/EditRoles/?id
        [Route("EditRoles")]
        public async Task<IActionResult> EditRoles(string id)
        {
            try
            {
                UserWithRoles userWithRoles = await _usersService.GetUserWithRolesAsync(id);
                return View(userWithRoles);
            }
            catch (NullReferenceException ex)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Manage/Users/EditRoles/?id
        [Route("EditRoles")]
        [HttpPost]
        public async Task<IActionResult> EditRoles(string id, UserWithRoles userWithRoles)
        {
            try
            {
                UserWithRoles refreshUserWithRoles = await _usersService.GetUserWithRolesAsync(id);
                await _usersService.UpdateRolesForUserAsync(id, userWithRoles);
                TempData["rolesSaved"] = "Changes are saved";
                TempData["rolesSavedAdmin"] = "A user updated their Roles Changes";//TODO: Test
                return View(refreshUserWithRoles);
            }
            catch (NullReferenceException ex)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Manage/Users/Delete/?id
        [Route("Delete")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var applicationUser = await _usersService.GetAsync(id);
                return View(applicationUser);
            }
            catch (NullReferenceException ex)
            {
                return RedirectToAction(nameof(Index));
            }
        }


        // POST: Manage/Users/Delete/?id
        [Route("Delete")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                await _usersService.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (NullReferenceException ex)
            {
                return RedirectToAction(nameof(Index));
            }
        }
    }
}