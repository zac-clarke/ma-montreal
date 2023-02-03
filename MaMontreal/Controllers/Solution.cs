using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MaMontreal.Models;

namespace MaMontreal.Controllers;

[Route("/Solution")]
public class SolutionController : Controller
{


    [Route("HowItWorks")]
    public IActionResult HowItWorks()
    {
        return View();
    }

    [Route("TwelvePromises")]
    public IActionResult TwelvePromises()
    {
        return View();
    }

    [Route("TwelveQuestions")]
    public IActionResult TwelveQuestions()
    {
        return View();
    }

    [Route("TwelveTraditions")]
    public IActionResult TwelveTraditions()
    {
        return View();
    }

    [Route("Error")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
