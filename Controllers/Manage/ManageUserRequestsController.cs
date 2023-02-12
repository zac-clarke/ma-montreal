using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MaMontreal.Data;
using MaMontreal.Models;
using MaMontreal.Services;
using Microsoft.AspNetCore.Identity;
using MaMontreal.Models.NotMapped;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace MaMontreal.Controllers_Manage
{
    [Authorize(Roles = "admin,member")]
    [Route("Manage/Requests/")]
    public class ManageUserRequestsController : Controller
    {
        private readonly MamDbContext _context = null!;
        private readonly RequestsService _requestsService = null!;
        private object _logger = null!;

        public ManageUserRequestsController(
            MamDbContext context,
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ILogger<ManageUserRequestsController> logger)
        {
            try
            {
                _context = context;
                _requestsService = new RequestsService(context, roleManager, userManager);
                _logger = logger;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }


        [Route("")]
        public async Task<IActionResult> Index(bool? archived)
        {
            try
            {
                if (await _requestsService.GetAnyPendingAsync())
                {
                    TempData["dashFlashMessage"] = JsonConvert.SerializeObject(new FlashMessage("You have pending GSR requests!", "warning"));
                }
                return View(await _requestsService.GetAllAsync(archived, User));

            }
            catch (SystemException ex)
            {
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage(ex.Message, "warning"));
                return RedirectToAction(nameof(Index));
            }
        }


        [Route("Details")]
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                var userRequest = await _requestsService.GetAsync(id, User);
                return View(userRequest);
            }
            catch (SystemException ex)
            {
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage(ex.Message, "warning"));
                return RedirectToAction(nameof(Index));
            }
        }



        [Route("Create")]
        [Authorize(Roles = "member")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "member")]
        [Route("Create")]
        public async Task<IActionResult> Create(string? role, [Bind("Note")] UserRequest userRequest)
        {
            if (ModelState.IsValid && User != null)
            {
                try
                {
                    int id = await _requestsService.CreateAsync(User, userRequest, role);
                    return RedirectToAction("Details", new { id = id });
                }
                catch (SystemException ex)
                {
                    TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage(ex.Message, "warning"));
                    return View(userRequest);
                }
            }
            return View(userRequest);
        }


        [Authorize(Roles = "admin")]
        [Route("Approve")]
        public async Task<IActionResult> Approve(int? id)
        {
            try
            {
                await _requestsService.HandleAsync(id, User, true);
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage("Request Approved and Role updated.", "success"));
                return RedirectToAction("Details", new { id = id });
            }
            catch (SystemException ex)
            {
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage(ex.Message, "warning"));
                return RedirectToAction("Details", new { id = id });
            }

        }



        [Authorize(Roles = "admin")]
        [Route("Reject")]
        public async Task<IActionResult> Reject(int? id)
        {
            try
            {
                await _requestsService.HandleAsync(id, User, false);
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage("Request Rejected", "success"));
                return RedirectToAction("Details", new { id = id });
            }
            catch (SystemException ex)
            {
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage(ex.Message, "warning"));
                return RedirectToAction("Details", new { id = id });
            }
        }


        [Authorize(Roles = "member")]
        [Route("Edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                var userRequest = await _requestsService.GetAsync(id, User);
                return View(userRequest);
            }
            catch (SystemException ex)
            {
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage(ex.Message, "warning"));
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "member")]
        [Route("Edit")]
        public async Task<IActionResult> Edit(int id, [Bind("Note")] UserRequest userRequest)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _requestsService.UpdateAsync(id, userRequest, User);
                    TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage("Note Updated", "success"));
                    // return View(await _requestsService.GetAsync(id, User));
                    return RedirectToAction("Details", new { id = id });
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage(ex.Message, "warning"));
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(userRequest);
        }

        [Authorize(Roles = "admin")]
        [Route("Archive")]
        public async Task<IActionResult> Archive(int? id)
        {
            try
            {
                await _requestsService.ToggleArchiveAsync(id, true, User);
                return RedirectToAction(nameof(Index));
            }
            catch (SystemException ex)
            {
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage(ex.Message, "danger"));
                // _logger.LogError(ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }


        [Authorize(Roles = "admin")]
        [Route("Unarchive")]
        public async Task<IActionResult> Unarchive(int? id)
        {
            try
            {
                await _requestsService.ToggleArchiveAsync(id, false, User);
                return RedirectToAction(nameof(Index));
            }
            catch (SystemException ex)
            {
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage(ex.Message, "danger"));
                // _logger.LogError(ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }

        [Authorize(Roles = "admin")]
        [Route("Delete")]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                var request = await _requestsService.GetAsync(id, User);
                return View(request);
            }
            catch (SystemException ex)
            {
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage(ex.Message, "danger"));
                // _logger.LogError(ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }

        [Route("Delete")]
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _requestsService.DeleteAsync(id, User);
                return RedirectToAction("Index", new { archived = true });
            }
            catch (SystemException ex)
            {
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage(ex.Message, "danger"));
                return RedirectToAction("Details", new { id = id });
            }
        }

        private bool UserRequestExists(int id)
        {
            return (_context.UserRequests?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
