using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MaMontreal.Data;
using MaMontreal.Models;

namespace MaMontreal.Controllers_Manage
{
    public class ManageUserRequestsController : Controller
    {
        private readonly MamDbContext _context;

        public ManageUserRequestsController(MamDbContext context)
        {
            _context = context;
        }

        // GET: ManageUserRequests
        public async Task<IActionResult> Index()
        {
            return _context.UserRequests != null ?
                        View(await _context.UserRequests.ToListAsync()) :
                        Problem("Entity set 'MamDbContext.UserRequests'  is null.");
        }

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
        public IActionResult Create()
        {
            return View();
        }

        // POST: ManageUserRequests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IsApproved,ProcessedDate,Note,UpdatedAt,DeletedAt")] UserRequest userRequest)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userRequest);
        }

        // GET: ManageUserRequests/Edit/5
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
