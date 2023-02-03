using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MaMontreal.Models;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;

namespace MaMontreal.Controllers;

[Route("/")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IConfiguration _configuration;

    public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
    {
        _configuration = configuration;
        _logger = logger;
    }

    // public string connString = _configuration.GetConnectionString("MamBlobStorage") GetConnectionString; //TODO: GetConnectionString from appsettings.json
    // public BlobClient blobClient = new BlobClient("DefaultEndpointsProtocol=https;AccountName=mamontreal;AccountKey=bOT6JfJaJGGRt0dzD0yL8LjOFhiW2SgT71BW1MiwvqoWOpZhZ2IVbDnirgM///Cezdac+8+f+GO6+AStmtW6PA==;EndpointSuffix=core.windows.net", "Literature", "Life_With_Hope.pdf");

    [Route("")]
    public IActionResult Index()
    {
        return View();
    }


    [Route("Privacy")]
    public IActionResult Privacy()
    {
        return View();
    }

    [Route("Literature")]
    public IActionResult Literature()
    {
        return View();
    }

    [Route("SeekHelp")]
    public IActionResult SeekHelp()
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
