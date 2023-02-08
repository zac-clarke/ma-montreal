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
using Newtonsoft.Json;

namespace MaMontreal.Controllers.Manage
{

    [Route("Manage/Users/")]
    public class ManageUsersController : Controller
    {

        private readonly UsersService _usersService;
        private readonly ILogger<ManageUsersController> _logger;

        public ManageUsersController(
            MamDbContext context,
            UserManager<ApplicationUser> userManager,
            ILogger<ManageUsersController> logger)
        {
            try
            {
                _usersService = new UsersService(context, userManager);
                _logger = logger;
            }
            catch (SystemException ex)
            {
                _logger.LogError(ex.Message);
                Problem(ex.Message);
            }
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
                _logger.LogError(ex.Message);
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
                // TempData["flashMessageList"] = JsonConvert.SerializeObject(new List<FlashMessage>() {
                //     new FlashMessage("ex.Message", "success"),
                //     new FlashMessage("ex.Message", "danger")
                // });

                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage(ex.Message, "danger"));
                _logger.LogError(ex.Message);
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
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage(ex.Message, "danger"));
                _logger.LogError(ex.Message);
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
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage("Roles Updated", "success"));
                _logger.LogInformation($"Roles Updated for User {id}");
                return View(refreshUserWithRoles);
            }
            catch (NullReferenceException ex)
            {
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage(ex.Message, "danger"));
                _logger.LogError(ex.Message);
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
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage(ex.Message, "danger"));
                _logger.LogError(ex.Message);
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
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage("User Deleted", "sucess"));
                _logger.LogInformation($"User {id} Deleted");
                return RedirectToAction(nameof(Index));
            }
            catch (NullReferenceException ex)
            {
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage(ex.Message, "danger"));
                _logger.LogError(ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }
    }
}