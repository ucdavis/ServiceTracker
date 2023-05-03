using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ServiceTracker.Models;
using Microsoft.AspNetCore.Authorization;

namespace ServiceTracker.Controllers;


public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

   
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    
    public IActionResult Claims ()
    {
        var test = "";
        if(User.IsInRole("Admin"))
        {
            test = "blah";
        }
        ViewBag.Test = test;
        return View();
    }

    [Authorize(Roles = "Admin")]
    public IActionResult AdminClaims ()
    {       
        return View("Claims");
    }

    public string Version()
    {
        return $"Version of MVC: {typeof(Controller).Assembly.GetName().Version.ToString()}";
    }
}
