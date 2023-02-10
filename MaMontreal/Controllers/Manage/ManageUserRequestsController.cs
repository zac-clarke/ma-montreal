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

        public ManageUserRequestsController(
            MamDbContext context,
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager)
        {
            try
            {
                _context = context;
                _requestsService = new RequestsService(context, roleManager, userManager);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }


        [Route("")]
        // GET: ManageUserRequests
        public async Task<IActionResult> Index()
        {
            return _context.UserRequests != null ?
                        View(await _context.UserRequests.Include("Requestee").Include("RequestHandler").Include("RoleRequested").OrderByDescending(r => r.CreatedAt).ToListAsync()) :
                        Problem("Entity set 'MamDbContext.UserRequests'  is null.");
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
        [Authorize(Roles = "member,admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: ManageUserRequests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "member,admin")]
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

        // GET: ManageUserRequests/Delete/5
        [Route("Delete")]

        public async Task<IActionResult> Delete(int? id)
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
