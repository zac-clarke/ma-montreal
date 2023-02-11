using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MaMontreal.Data;
using MaMontreal.Models.NotMapped;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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
            if (User.IsInRole("admin"))
                return RedirectToAction("AdminDash");
            if (User.IsInRole("gsr"))
                return RedirectToAction("GsrDash");
            return RedirectToAction("MemberDash");
        }

        [Route("Member")]
        public IActionResult MemberDash()
        {
            return View();
        }

        [Route("Admin")]
        public IActionResult AdminDash()
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