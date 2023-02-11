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
        private readonly MamDbContext _context;
        private readonly RequestsService _requestsService;
        private object _logger;

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
        // GET: ManageUserRequests
        public async Task<IActionResult> Index(bool? archived)
        {
            try
            {
                return View(await _requestsService.GetAllAsync(archived));

            }
            catch (NullReferenceException ex)
            {
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage(ex.Message, "danger"));
                return View();
            }
        }


        [Route("Details")]
        // GET: ManageUserRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.UserRequests == null)
            {
                return NotFound();
            }

            var userRequest = await _context.UserRequests
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userRequest == null)
            {
                return NotFound();
            }

            return View(userRequest);
        }


        // GET: ManageUserRequests/Create
        [Route("Create")]
        [Authorize(Roles = "member")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: ManageUserRequests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                    await _requestsService.CreateAsync(User, userRequest, role);
                    return RedirectToAction("Index", "Home");
                }
                catch (NullReferenceException ex)
                {
                    TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage(ex.Message, "warning"));
                    return View(userRequest);
                }
            }
            return View(userRequest);
        }

        // [HttpPost]
        // [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        [Route("Approve")]
        public async Task<IActionResult> Approve(int? id)
        {
            try
            {
                await _requestsService.HandleAsync(id, User, true);
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage("Request Approved and Role updated.", "success"));
            }
            catch (Exception ex)
            {
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage(ex.Message, "warning"));
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }


        // [HttpPost]
        // [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        [Route("Reject")]
        public async Task<IActionResult> Reject(int? id)
        {
            try
            {
                await _requestsService.HandleAsync(id, User, false);
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage("Request Rejected", "success"));
            }
            catch (Exception ex)
            {
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage(ex.Message, "warning"));
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: ManageUserRequests/Edit/5
        [Authorize(Roles = "admin")]
        [Route("Edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.UserRequests == null)
            {
                return NotFound();
            }

            var userRequest = await _context.UserRequests.FindAsync(id);
            if (userRequest == null)
            {
                return NotFound();
            }
            return View(userRequest);
        }

        // POST: ManageUserRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        [Route("Edit")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IsApproved,ProcessedDate,Note,CreatedAt,UpdatedAt,DeletedAt")] UserRequest userRequest)
        {
            if (id != userRequest.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserRequestExists(userRequest.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(userRequest);
        }

        [Authorize(Roles = "admin")]
        // GET: Manage/Userreqests/Archive/5
        [Route("Archive")]
        public async Task<IActionResult> Archive(int? id)
        {
            try
            {
                await _requestsService.ArchiveAsync(id);
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
        // GET: ManageTags/Delete/5
        [Route("Delete")]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                var request = await _requestsService.GetAsync(id);
                return View(request);
            }
            catch (SystemException ex)
            {
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage(ex.Message, "danger"));
                // _logger.LogError(ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: ManageUserRequests/Delete/5
        [HttpPost, ActionName("Delete")]

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.UserRequests == null)
            {
                return Problem("Entity set 'MamDbContext.UserRequests'  is null.");
            }
            var userRequest = await _context.UserRequests.FindAsync(id);
            if (userRequest != null)
            {
                _context.UserRequests.Remove(userRequest);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserRequestExists(int id)
        {
            return (_context.UserRequests?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
