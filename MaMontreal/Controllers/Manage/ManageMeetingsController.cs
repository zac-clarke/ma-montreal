using MaMontreal.Data;
using MaMontreal.Models;
using MaMontreal.Models.NotMapped;
using MaMontreal.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace MaMontreal.Controllers_Manage
{
    public class ManageMeetingsController : Controller
    {
        private readonly MamDbContext _context;
        private readonly MeetingsService _service;
        private readonly ILogger<ManageMeetingsController>? _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public ManageMeetingsController(MamDbContext context, UserManager<ApplicationUser> userManager, ILogger<ManageMeetingsController> logger)
        {
            try
            {
                _context = context;
                _service = new MeetingsService(context);
                _userManager = userManager;
                _logger = logger;

                // _meetingTypes = context.MeetingTypes.ToList<MeetingType>();
                // _languages = context.Languages.ToList<Language>();
            }
            catch (SystemException ex)
            {
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage(ex.Message, "danger"));
                _logger?.LogError(ex.Message);
                Problem(ex.Message);
            }
            if (_context == null)
                throw new NullReferenceException("MamDbContext is null!");
            if (_service == null)
                throw new NullReferenceException("MeetingsService is null!");
            if (_userManager == null)
                throw new NullReferenceException("UserManager is null!");
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
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage(ex.Message, "danger"));
                _logger?.LogError(ex.Message);
                return NotFound(ex.Message);
            }
        }

        // GET: ManageMeetings/Create
        public IActionResult Create()
        {
            ViewBag.MeetingTypes = _context.MeetingTypes.ToList<MeetingType>();
            ViewBag.Languages = _context.Languages.ToList<Language>();
            return View();
        }

        // POST: ManageMeetings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("District,EventName,Description,_MeetingTypeId,_LanguageId,ImageUrl,Address,City,ProvinceCode,PostalCode,DayOfWeek,Date,StartTime,EndTime,Status")] Meeting meeting)
        {
            ViewBag.MeetingTypes = _context.MeetingTypes.ToList<MeetingType>();
            ViewBag.Languages = _context.Languages.ToList<Language>();

            if (!ModelState.IsValid)
                return View(meeting);

            try
            {
                await _service.CreateMeeting(meeting, _userManager, User);
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage("Meeting created successfully: " + meeting.EventName, "success"));
                _logger?.LogInformation("Meeting created successfully: " + meeting.Id);
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage(ex.Message, "danger"));
                _logger?.LogError(ex.Message);
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

                ViewBag.MeetingTypes = _context.MeetingTypes.ToList<MeetingType>();
                ViewBag.Languages = _context.Languages.ToList<Language>();

                meeting._MeetingTypeId = meeting.MeetingType?.Id;
                meeting._LanguageId = meeting.Language?.Id;

                return View(meeting);
            }
            catch (NullReferenceException ex)
            {
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage(ex.Message, "danger"));
                _logger?.LogError(ex.Message);
                return NotFound(ex.Message);
            }
        }

        // POST: ManageMeetings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,District,EventName,Description,_MeetingTypeId,_LanguageId,ImageUrl,Address,City,ProvinceCode,PostalCode,DayOfWeek,Date,StartTime,EndTime,Status")] Meeting meeting)
        {
            ViewBag.MeetingTypes = _context.MeetingTypes.ToList<MeetingType>();
            ViewBag.Languages = _context.Languages.ToList<Language>();

            if (!ModelState.IsValid)
                return View(meeting);

            try
            {
                await _service.EditMeeting(id, meeting, _userManager, User);
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage("Meeting edited successfully: " + meeting.EventName, "success"));
                _logger?.LogInformation("Meeting edited successfully: " + meeting.Id);
                return RedirectToAction(nameof(Index));
            }
            catch (NullReferenceException ex)
            {
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage(ex.Message, "danger"));
                _logger?.LogError(ex.Message);
                return NotFound();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage(ex.Message, "danger"));
                _logger?.LogError(ex.Message);
                ModelState.AddModelError("Id", "Looks like someone else edited/delete this Meeting!");
                return View(meeting);
            }
            catch (ArgumentException ex)
            {
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage(ex.Message, "danger"));
                _logger?.LogError(ex.Message);
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
            catch (NullReferenceException ex)
            {
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage(ex.Message, "danger"));
                _logger?.LogError(ex.Message);
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
                Meeting meeting = await _service.DeleteMeeting(id, _userManager, User);
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage("Meeting deleted successfully: " + meeting.EventName, "success"));
                _logger?.LogInformation("Meeting deleted successfully: " + id);
                return RedirectToAction(nameof(Index));
            }
            catch (NullReferenceException ex)
            {
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage(ex.Message, "danger"));
                _logger?.LogError(ex.Message);
                return NotFound();
            }
        }

        // GET: ManageMeetings/UnDelete/5
        public async Task<IActionResult> Restore(int? id)
        {
            try
            {
                Meeting meeting = await _service.RestoreMeeting(id, _userManager, User);
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage("Meeting restored successfully: " + meeting.EventName, "success"));
                _logger?.LogInformation("Meeting restored successfully: " + id);
                return RedirectToAction(nameof(Index));
            }
            catch (NullReferenceException ex)
            {
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage(ex.Message, "danger"));
                _logger?.LogError(ex.Message);
                return NotFound();
            }
        }

        private bool MeetingExists(int id)
        {
            return _service.MeetingExists(id);
        }
    }
}
