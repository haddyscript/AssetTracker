using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AssetTracker.Models;
using AssetTracker.Data;
using Microsoft.EntityFrameworkCore;

namespace AssetTracker.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AssetDbContext _dbContext;

    public HomeController(ILogger<HomeController> logger, AssetDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<IActionResult> Index()
    {
        var userRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
        var userName = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;

        if (userRole == "Admin")
        {
            // Admin Dashboard Data

            ViewBag.UserRole = "Admin";
            ViewBag.UserName = userName;
        }
        else
        {
            // User Dashboard Data

            ViewBag.UserRole = "User";
            ViewBag.UserName = userName;
        }

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

