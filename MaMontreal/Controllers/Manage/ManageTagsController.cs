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
using Newtonsoft.Json;
using MaMontreal.Models.NotMapped;

namespace MaMontreal.Controllers.Manage
{
    [Route("Manage/Tags/")]
    public class ManageTagsController : Controller
    {
        private readonly TagsService _tagsService = null!;
        private readonly ILogger<ManageTagsController> _logger = null!;

        public ManageTagsController(MamDbContext context, ILogger<ManageTagsController> logger)
        {
            try
            {
                _tagsService = new TagsService(context);
                _logger = logger;
            }
            catch (SystemException ex)
            {
                _logger.LogError(ex.Message);
                Problem(ex.Message);
            }
        }

        // GET: ManageTags
        [Route("")]
        public async Task<IActionResult> Index()
        {
            try
            {
                return View(await _tagsService.GetAllAsync());
            }
            catch (SystemException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }
        }

        // GET: ManageTags/Details/5
        [Route("Details")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                return View(await _tagsService.GetAsync(id));
            }
            catch (SystemException ex)
            {
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage(ex.Message, "danger"));
                _logger.LogError(ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: ManageTags/Create
        [Route("Create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: ManageTags/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create")]
        public async Task<IActionResult> Create([Bind("Id,Title")] Tag tag)
        {
            if (!ModelState.IsValid)
            {
                return View(tag);
            }
            try
            {
                await _tagsService.CreateAsync(tag);
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage($"New tag added: {tag.Title} ", "success"));
                _logger.LogInformation($"New tag added: {tag.Title} ");
                return RedirectToAction(nameof(Index));
            }
            catch (SystemException ex)
            {
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage(ex.Message, "danger"));
                _logger.LogError(ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: ManageTags/Edit/5
        [Route("Edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                var tag = await _tagsService.GetAsync(id);
                return View(tag);
            }
            catch (SystemException ex)
            {
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage(ex.Message, "danger"));
                _logger.LogError(ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: ManageTags/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Edit")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title")] Tag tag)
        {
            if (!ModelState.IsValid)
                return View(tag);
            try
            {
                await _tagsService.UpdateAsync(id, tag);
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage($"New tag Updated: {tag.Title} ", "success"));
                _logger.LogInformation($"New tag Updated: {tag.Title} ");
                return RedirectToAction(nameof(Index));
            }
            catch (SystemException ex)
            {
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage(ex.Message, "danger"));
                _logger.LogError(ex.Message);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage(ex.Message, "danger"));
                _logger.LogError(ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: ManageTags/Delete/5
        [Route("Delete")]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                var tag = await _tagsService.GetAsync(id);
                return View(tag);
            }
            catch (SystemException ex)
            {
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage(ex.Message, "danger"));
                _logger.LogError(ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: ManageTags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _tagsService.DeleteAsync(id);
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage($"Tag Deleted", "success"));
                _logger.LogInformation($"Tag {id} Deleted");
                return RedirectToAction(nameof(Index));
            }
            catch (SystemException ex)
            {
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage(ex.Message, "danger"));
                _logger.LogError(ex.Message);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage("Tag is possibly used for existing meetings", "danger"));
                _logger.LogError(ex.Message);
                _logger.LogError(ex.InnerException?.Message.ToString());

                return View(await _tagsService.GetAsync(id));
            }
        }
    }
}
