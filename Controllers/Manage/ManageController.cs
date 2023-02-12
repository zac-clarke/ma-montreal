using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MaMontreal.Data;
using MaMontreal.Models;
using MaMontreal.Models.NotMapped;
using MaMontreal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using static MaMontreal.Services.AdminDashService;

namespace MaMontreal.Controllers.Manage
{

    [Authorize]
    [Route("/Manage")]
    public class ManageController : Controller
    {
        private readonly AdminDashService _adminDashService;

        private readonly MeetingsService _meetingsService;


        public ManageController(MamDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _adminDashService = new AdminDashService(context, userManager, roleManager);
            _meetingsService = new MeetingsService(context);

        }


        [Route("")]
        public IActionResult Index()
        {
            if (User.IsInRole("admin"))
                return RedirectToAction("AdminDash");
            if (User.IsInRole("gsr"))
                return RedirectToAction("GsrDash");
            return RedirectToAction("MemberDash");
        }

        [Route("Admin")]
        public async Task<IActionResult> AdminDash()
        {
            AdminDashData data = await _adminDashService.GetAdminDashDataAsync();
            if (await _adminDashService.GetAnyPendingAsync())
            {
                TempData["dashFlashMessage"] = JsonConvert.SerializeObject(new FlashMessage("You have pending GSR requests!", "warning"));
            }
            if (await _meetingsService.GetAnyPendingAsync())
            {
                TempData["meetingFlashMessage"] = JsonConvert.SerializeObject(new FlashMessage("You have an unapproved meetings!", "warning"));
            }
            return View(data);
        }

        [Route("Member")]
        public IActionResult MemberDash()
        {
            return View();
        }

        [Route("Gsr")]
        public IActionResult GsrDash()
        {
            return View();
        }
    }
}