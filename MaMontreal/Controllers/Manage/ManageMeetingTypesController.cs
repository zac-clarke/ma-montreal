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
    public class ManageMeetingTypesController : Controller
    {
        private readonly MamDbContext _context;

        public ManageMeetingTypesController(MamDbContext context)
        {
            _context = context;
        }

        // GET: ManageMeetingTypes
        public async Task<IActionResult> Index()
        {
              return _context.MeetingTypes != null ? 
                          View(await _context.MeetingTypes.ToListAsync()) :
                          Problem("Entity set 'MamDbContext.MeetingTypes'  is null.");
        }

        // GET: ManageMeetingTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.MeetingTypes == null)
            {
                return NotFound();
            }

            var meetingType = await _context.MeetingTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (meetingType == null)
            {
                return NotFound();
            }

            return View(meetingType);
        }

        // GET: ManageMeetingTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ManageMeetingTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title")] MeetingType meetingType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(meetingType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(meetingType);
        }

        // GET: ManageMeetingTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.MeetingTypes == null)
            {
                return NotFound();
            }

            var meetingType = await _context.MeetingTypes.FindAsync(id);
            if (meetingType == null)
            {
                return NotFound();
            }
            return View(meetingType);
        }

        // POST: ManageMeetingTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title")] MeetingType meetingType)
        {
            if (id != meetingType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(meetingType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MeetingTypeExists(meetingType.Id))
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
            return View(meetingType);
        }

        // GET: ManageMeetingTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.MeetingTypes == null)
            {
                return NotFound();
            }

            var meetingType = await _context.MeetingTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (meetingType == null)
            {
                return NotFound();
            }

            return View(meetingType);
        }

        // POST: ManageMeetingTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.MeetingTypes == null)
            {
                return Problem("Entity set 'MamDbContext.MeetingTypes'  is null.");
            }
            var meetingType = await _context.MeetingTypes.FindAsync(id);
            if (meetingType != null)
            {
                _context.MeetingTypes.Remove(meetingType);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MeetingTypeExists(int id)
        {
          return (_context.MeetingTypes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
