using MaMontreal.Data;
using MaMontreal.Models;
using MaMontreal.Models.NotMapped;
using MaMontreal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace MaMontreal.Controllers_Manage
{
    [Authorize(Roles = "admin,gsr")]
    public class ManageMeetingTypesController : Controller
    {
        private readonly MeetingTypesService _service = null!;
        private readonly ILogger<ManageMeetingTypesController> _logger = null!;

        public ManageMeetingTypesController(MamDbContext context, ILogger<ManageMeetingTypesController> logger)
        {
            try
            {
                _service = new MeetingTypesService(context);
                _logger = logger;
            }
            catch (SystemException ex)
            {
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage(ex.Message, "danger"));
                _logger.LogError(ex.Message);
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
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage(ex.Message, "danger"));
                _logger.LogError(ex.Message);
                return RedirectToAction(nameof(Index));
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
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage("Meeting Type created successfully: " + meetingType.Title, "success"));
                _logger.LogInformation("MeetingType created successfully: " + meetingType.Id);
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
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage(ex.Message, "danger"));
                _logger.LogError(ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: ManageMeetingTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title")] MeetingType meetingType)
        {
            if (!ModelState.IsValid)
                return View(meetingType);

            try
            {
                await _service.EditMeetingType(id, meetingType);
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage("Meeting Type edited successfully: " + meetingType.Title, "success"));
                _logger.LogInformation("MeetingType created successfully: " + id);
                return RedirectToAction(nameof(Index));
            }
            catch (NullReferenceException ex)
            {
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage(ex.Message, "danger"));
                _logger.LogError(ex.Message);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage(ex.Message, "danger"));
                _logger.LogError(ex.Message);
                ModelState.AddModelError("Id", "Looks like someone else edited/deleted this Meeting Type!");
                return View(meetingType);
            }
        }

        // GET: ManageMeetingTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                return View(await _service.GetMeetingTypeById(id));
            }
            catch (NullReferenceException ex)
            {
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage(ex.Message, "danger"));
                _logger.LogError(ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: ManageMeetingTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                MeetingType mt = await _service.DeleteMeetingType(id);
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage("Meeting Type deleted successfully: " + mt.Title, "success"));
                _logger.LogInformation("MeetingType deleted successfully: " + id);
                return RedirectToAction(nameof(Index));
            }
            catch (NullReferenceException ex)
            {
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage(ex.Message, "danger"));
                _logger.LogError(ex.Message);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage(ex.Message, "danger"));
                _logger.LogError(ex.Message);
                ViewBag.error = ex.Message;
                return View(await _service.GetMeetingTypeById(id));
            }
        }

        private bool MeetingTypeExists(int id)
        {
            return _service.MeetingTypeExists(id);
        }
    }
}
