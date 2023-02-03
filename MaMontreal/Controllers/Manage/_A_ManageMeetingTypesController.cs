using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MaMontreal.Data;
using MaMontreal.Models;
using MaMontreal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MaMontreal.Controllers.Manage
{
    [Route("_A_Manage/MeetingTypes/")]
    public class _A_ManageMeetingTypesController : Controller
    {
        private readonly MamDbContext _context;
        private readonly ILogger<_A_ManageMeetingTypesController> _logger;
        private MeetingTypesService _meetingTypesService;

        [BindProperty]
        public MeetingType MeetingType { get; set; } = new MeetingType();

        public _A_ManageMeetingTypesController(MamDbContext context, ILogger<_A_ManageMeetingTypesController> logger)
        {
            _context = context;
            _logger = logger;
            _meetingTypesService = new MeetingTypesService(_context);
        }

        [Route("Create")]
        public async Task<IActionResult> Create()
        {
            Console.WriteLine(value: "GetCreate MeetingType");
            try
            {
                return View(MeetingType);
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetCreate MeetingType Problem");
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create")]
        public async Task<IActionResult> CreatePost()
        {
            Console.WriteLine("PostCreate MeetingType");
            try
            {
                ModelState.Remove("Meetings");
                if (ModelState.IsValid)
                {
                    MeetingType = _meetingTypesService.CreateMeetingType(MeetingType).Result;

                    Console.WriteLine("PostCreate MeetingType Done");
                    return RedirectToPage("Manage/MeetingTypes");
                }
                else
                {
                    return View(model: MeetingType, viewName: "Create");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Problem(ex.Message);
            }
        }

        public IActionResult Index()
        {
            return View(_meetingTypesService.GetAllMeetingTypes());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}