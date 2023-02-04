using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MaMontreal.Data;
using MaMontreal.Models;
using MaMontreal.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MaMontreal.Controllers_Manage
{
    public class ManageMeetingsController : Controller
    {
        private readonly MeetingsService _service;
        private readonly UserManager<ApplicationUser> _userManager;

        public ManageMeetingsController(MamDbContext context, UserManager<ApplicationUser> userManager)
        {
            try
            {
                _service = new MeetingsService(context);
                _userManager = userManager;
            }
            catch (SystemException ex)
            {
                Problem(ex.Message);
            }
        }

        // GET: ManageMeetings
        public async Task<IActionResult> Index()
        {
            return View(await _service.GetAllMeetings());
        }

        // GET: ManageMeetings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                return View(await _service.GetMeetingById(id));
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // GET: ManageMeetings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ManageMeetings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("District,EventName,Description,ImageUrl,Address,City,ProvinceCode,PostalCode,DayOfWeek,Date,StartTime,EndTime,Status")] Meeting meeting)
        {


            if (!ModelState.IsValid)
                return View(meeting);

            try
            {
                await _service.CreateMeeting(meeting, _userManager, User);
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                string[] exception = ex.Message.Split(",");
                string key = exception[0];
                string message = exception[1];
                ModelState.AddModelError(key, message);
                return View(meeting);
            }
        }

        // GET: ManageMeetings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                var meeting = await _service.GetMeetingById(id);
                return View(meeting);
            }
            catch (NullReferenceException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // POST: ManageMeetings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("District,EventName,Description,ImageUrl,Address,City,ProvinceCode,PostalCode,DayOfWeek,Date,StartTime,EndTime,Status")] Meeting meeting)
        {
            if (!ModelState.IsValid)
                return View(meeting);

            try
            {
                await _service.EditMeeting(id, meeting, _userManager, User);
                return RedirectToAction(nameof(Index));
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
            catch (ArgumentException ex)
            {
                string[] exception = ex.Message.Split(",");
                string key = exception[0];
                string message = exception[1];
                ModelState.AddModelError(key, message);
                return View(meeting);
            }
        }

        // GET: ManageMeetings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                return View(await _service.GetMeetingById(id));
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
        }

        // POST: ManageMeetings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _service.DeleteMeeting(id, _userManager, User);
                return RedirectToAction(nameof(Index));
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
        }

        // GET: ManageMeetings/UnDelete/5
        public async Task<IActionResult> UnDelete(int? id)
        {
            try
            {
                await _service.UnDeleteMeeting(id, _userManager, User);
                return RedirectToAction(nameof(Index));
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
        }

        private bool MeetingExists(int id)
        {
            return _service.MeetingExists(id);
        }
    }
}
