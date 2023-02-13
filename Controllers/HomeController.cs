using System.Diagnostics;
using Azure.Storage.Blobs;
using MaMontreal;
using MaMontreal.Models;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;


namespace MaMontreal.Controllers;

[Route("/")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IConfiguration _configuration;
    private readonly IStringLocalizer<SharedResource> _sharedLocalizer;

    public HomeController(ILogger<HomeController> logger, IConfiguration configuration, IStringLocalizer<SharedResource> sharedLocalizer)
    {
        _configuration = configuration;
        _logger = logger;
        _sharedLocalizer = sharedLocalizer;
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

    [HttpPost]
    public IActionResult SetLanguage(string culture, string returnUrl)
    {
        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
        );

        return LocalRedirect(returnUrl);
    }

    [Route("Error")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
