using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MaMontreal.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MaMontreal.Controllers.Manage
{

    [Authorize]
    [Route("/Manage")]
    public class ManageController : Controller
    {
        private readonly MamDbContext _context;

        public ManageController(MamDbContext context)
        {
            _context = context;
        }


        [Route("")]
        public IActionResult Index()
        {
            return View();
        }
    }
}