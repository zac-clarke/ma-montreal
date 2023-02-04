using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MaMontreal.Data;
using MaMontreal.Models;
using MaMontreal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MaMontreal.Controllers_Manage
{
    public class ManageMeetingTypesController : Controller
    {
        private readonly MeetingTypesService _service;

        public ManageMeetingTypesController(MamDbContext context)
        {
            try
            {
                _service = new MeetingTypesService(context);
            }
            catch (SystemException ex)
            {
                Problem(ex.Message);
            }
        }

        // GET: ManageMeetingTypes
        public async Task<IActionResult> Index()
        {
            return View(await _service.GetAllMeetingTypes());
        }

        // GET: ManageMeetingTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                return View(await _service.GetMeetingTypeById(id));
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
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
        public async Task<IActionResult> Create([Bind("Title")] MeetingType meetingType)
        {
            if (ModelState.IsValid)
            {
                await _service.CreateMeetingType(meetingType);
                return RedirectToAction(nameof(Index));
            }
            return View(meetingType);
        }

        // GET: ManageMeetingTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                var meetingType = await _service.GetMeetingTypeById(id);
                return View(meetingType);
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // POST: ManageMeetingTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Title")] MeetingType meetingType)
        {
            if (!ModelState.IsValid)
                return View(meetingType);

            try
            {
                await _service.EditMeetingType(id, meetingType);
                return RedirectToAction(nameof(Index));
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
        }

        // GET: ManageMeetingTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                return View(await _service.GetMeetingTypeById(id));
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
        }

        // POST: ManageMeetingTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _service.DeleteMeetingType(id);
                return RedirectToAction(nameof(Index));
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
        }

        private bool MeetingTypeExists(int id)
        {
            return _service.MeetingTypeExists(id);
        }
    }
}
