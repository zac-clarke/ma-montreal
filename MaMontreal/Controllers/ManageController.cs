using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MaMontreal.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MaMontreal.Controllers
{

    [Route("/Manage")]
    public class ManageController : Controller
    {
        private readonly MamDbContext _context;

        public ManageController(MamDbContext context)
        {
            _context = context;
        }
        [Route("")]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        // [Route("/Manage/Users")]
        // public async Task<IActionResult> Index()
        // {
        //     return _context.Users != null ?
        //                 View(await _context.Users.ToListAsync()) :
        //                 Problem("Entity set 'TestDbContext.Users'  is null.");
        // }

    }
}