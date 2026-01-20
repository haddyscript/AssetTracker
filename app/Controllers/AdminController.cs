using System;
using System.Security.Claims;
using AssetTracker.Data;
using AssetTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace AssetTracker.Controllers
{
    public class AdminController : Controller
    {
        private readonly AssetDbContext _dbData;
        private readonly PasswordHasher<Admin> _adminPasswordHasher;

        public AdminController(AssetDbContext context)
        {
            _dbData = context;
            _adminPasswordHasher = new PasswordHasher<Admin>();
        }
        public async Task<IActionResult> Index()
        {
            var admins = await _dbData.Admins
                .Include(a => a.UserProfile)
                .OrderBy(a => a.username)
                .ToListAsync();

            return View(admins);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Admin admin)
        {
            if (!ModelState.IsValid)
            {
                return View(admin);
            }

            var registrationResult = await RegisterAdminAsync(admin);
            if (registrationResult.Success)
            {
                return RedirectToAction("Login", "Users");
            }

            ModelState.AddModelError("", registrationResult.ErrorMessage);
            return View(admin);
        }

        private async Task<RegistrationResult> RegisterAdminAsync(Admin admin)
        {
            // Check if username or email already exists
            var existingAdmin = await _dbData.Admins
                .FirstOrDefaultAsync(a => a.username == admin.username || a.email == admin.email);

            if (existingAdmin != null)
            {
                return new RegistrationResult { Success = false, ErrorMessage = "Username or email already exists." };
            }

            // Hash the password and set admin properties
            admin.password_hash = _adminPasswordHasher.HashPassword(admin, admin.password_hash);
            admin.created_at = DateTime.Now;
            admin.updated_at = DateTime.Now;
            admin.is_active = true;

            // Save to database
            _dbData.Admins.Add(admin);
            int rowsAffected = await _dbData.SaveChangesAsync();

            if (rowsAffected > 0)
            {
                return new RegistrationResult { Success = true };
            }

            return new RegistrationResult { Success = false, ErrorMessage = "Failed to save admin to database." };
        }

        private class RegistrationResult
        {
            public bool Success { get; set; }
            public string ErrorMessage { get; set; }
        }
    }
}