using System.Security.Claims;
using MaMontreal.Data;
using MaMontreal.Models;
using MaMontreal.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MaMontreal.Controllers
{
    [Route("Manage/Meetings/")]
    public class ManageMeetingsController : Controller
    {
        private readonly MamDbContext _context;
        private readonly ILogger<ManageMeetingsController> _logger;

        [BindProperty]
        public Meeting Meeting { get; set; } = new Meeting();

        public ManageMeetingsController(MamDbContext context, ILogger<ManageMeetingsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [Route("")]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [Route("Create")]
        public async Task<IActionResult> Create()
        {
            Console.WriteLine(value: "GetCreate Meeting");
            try
            {
                Meeting.EventName = "test";
                Meeting.Description = "test";
                Meeting.Address = "test";
                Meeting.City = "test";
                Meeting.ProvinceCode = "QC";
                Meeting.PostalCode = "test";
                Meeting.DayOfWeek = 0;
                Meeting.Date = DateTime.Now;
                Meeting.StartTime = DateTime.Now;
                Meeting.EndTime = DateTime.Now;
                return View(Meeting);
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetCreate Meeting Problem");
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> CreatePost()
        {
            Console.WriteLine("PostCreate Meeting");
            try
            {
                ModelState.Remove("Meeting.Language");
                ModelState.Remove("Meeting.Gsr");
                ModelState.Remove("Meeting.UpdatedBy");
                if (ModelState.IsValid)
                {
                    UsersService usersService = new UsersService(_context);
                    ApplicationUser user = usersService.GetCurrentUser(User);

                    MeetingsService MeetingsService = new MeetingsService(_context);
                    Meeting = MeetingsService.CreateMeeting(Meeting, user).Result;


                    Console.WriteLine("PostCreate Meeting Done");
                    return RedirectToPage("Manage");
                }
                else
                {
                    return View(model: Meeting, viewName: "Create");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("PostCreate Meeting Problem");
                return Problem(ex.Message);
            }
        }

        // public IActionResult Details(int id)
        // {
        //     return View();
        // }

        // public IActionResult Create()
        // {
        //     return View();
        // }

        // public IActionResult Edit()
        // {
        //     return View();
        // }

        // public IActionResult Delete()
        // {
        //     return View();
        // }

        // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        // public IActionResult Error()
        // {
        //     return View("Error!");
        // }
    }
}