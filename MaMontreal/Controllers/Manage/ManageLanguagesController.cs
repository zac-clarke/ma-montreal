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
using MaMontreal.Models.NotMapped;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace MaMontreal.Controllers.Manage
{

    [Authorize(Roles = "admin,gsr")]
    [Route("Manage/Languages/")]
    public class ManageLanguagesController : Controller
    {
        private readonly LanguagesService _languagesService = null!;
        private readonly ILogger<ManageLanguagesController>? _logger;

        public ManageLanguagesController(MamDbContext context, ILogger<ManageLanguagesController> logger)
        {
            try
            {
                _languagesService = new LanguagesService(context);
                _logger = logger;
            }
            catch (SystemException ex)
            {
                _logger?.LogError(ex.Message);
                Problem(ex.Message);
            }

        }

        // GET: ManageLanguages
        [Route("")]
        public async Task<IActionResult> Index()
        {
            try
            {
                return View(await _languagesService.GetAllAsync());
            }
            catch (SystemException ex)
            {
                _logger?.LogError(ex.Message);
                return NotFound(ex.Message);
            }
        }

        // GET: ManageLanguages/Details/5
        [Route("Details")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                return View(await _languagesService.GetAsync(id));
            }
            catch (SystemException ex)
            {
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage(ex.Message, "danger"));
                _logger?.LogError(ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: ManageLanguages/Create
        [Authorize(Roles = "admin")]
        [Route("Create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: ManageLanguages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create")]
        public async Task<IActionResult> Create([Bind("Id,Title")] Language language)
        {
            if (!ModelState.IsValid)
            {
                return View(language);
            }
            try
            {
                await _languagesService.CreateAsync(language);
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage($"New language added: {language.Title} ", "success"));
                _logger?.LogInformation($"New language added: {language.Title} ");
                return RedirectToAction(nameof(Index));
            }
            catch (SystemException ex)
            {
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage(ex.Message, "danger"));
                _logger?.LogError(ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }


        // GET: ManageLanguages/Edit/5
        [Route("Edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                var language = await _languagesService.GetAsync(id);
                return View(language);
            }
            catch (SystemException ex)
            {
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage(ex.Message, "danger"));
                _logger?.LogError(ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }

        // // POST: ManageLanguages/Edit/5
        // // To protect from overposting attacks, enable the specific properties you want to bind to.
        // // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title")] Language language)
        {
            if (!ModelState.IsValid)
                return View(language);
            try
            {
                await _languagesService.UpdateAsync(id, language);
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage($"New language Updated: {language.Title} ", "success"));
                _logger?.LogInformation($"New language Updated: {language.Title} ");
                return RedirectToAction("Edit", new { id = language.Id });
            }
            catch (SystemException ex)
            {
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage(ex.Message, "danger"));
                _logger?.LogError(ex.Message);
                return RedirectToAction("Edit", new { id = language.Id });

            }
            catch (DbUpdateConcurrencyException ex)
            {
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage(ex.Message, "danger"));
                _logger?.LogError(ex.Message);
                return RedirectToAction("Edit", new { id = language.Id });

            }
        }

        // // GET: ManageLanguages/Delete/5
        [Route("Delete")]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                var language = await _languagesService.GetAsync(id);
                return View(language);
            }
            catch (SystemException ex)
            {
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage(ex.Message, "danger"));
                _logger?.LogError(ex.Message);
                return RedirectToAction(nameof(Index));
            }

        }

        // POST: ManageLanguages/Delete/5
        [Route("Delete")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            try
            {
                await _languagesService.DeleteAsync(id);
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage($"Language Deleted", "success"));
                _logger?.LogInformation($"Language {id} Deleted");
                return RedirectToAction(nameof(Index));
            }
            catch (SystemException ex)
            {
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage(ex.Message, "danger"));
                _logger?.LogError(ex.Message);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                TempData["flashMessage"] = JsonConvert.SerializeObject(new FlashMessage("Language is possibly used for existing meetings", "danger"));
                _logger?.LogError(ex.Message);
                _logger?.LogError(ex.InnerException?.Message.ToString());

                return View(await _languagesService.GetAsync(id));
            }
        }
    }
}
