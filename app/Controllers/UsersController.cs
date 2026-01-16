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
	public class UsersController : Controller
	{
        private readonly AssetDbContext _dbData;
        private readonly PasswordHasher<User> _passwordHasher;

        public UsersController(AssetDbContext context)
		{
            _dbData = context;
			_passwordHasher = new PasswordHasher<User>();
		}

        [HttpGet]
        public IActionResult Login()
		{
            // If user is already authenticated, redirect to home
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }
			return View();
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User model)
		{
            // Only validate username and password for login
            ModelState.Remove("full_name");
            ModelState.Remove("email");

            if (string.IsNullOrEmpty(model.username) || string.IsNullOrEmpty(model.password))
            {
                ViewBag.Error = "Username and password are required.";
                return View(model);
            }

            // Find user by username
            var user = await _dbData.Users
                .FirstOrDefaultAsync(u => u.username == model.username);

            if (user == null)
            {
                ViewBag.Error = "Invalid username or password.";
                return View(model);
            }

            // Verify password
            var result = _passwordHasher.VerifyHashedPassword(user, user.password, model.password);
            if (result == PasswordVerificationResult.Failed)
            {
                ViewBag.Error = "Invalid username or password.";
                return View(model);
            }

            // Create claims for the authenticated user
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
                new Claim(ClaimTypes.Name, user.username),
                new Claim(ClaimTypes.Email, user.email),
                new Claim("FullName", user.full_name)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
            };

            // Sign in the user
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
        [ValidateAntiForgeryToken]
		public async Task<IActionResult> RegisterUser(User user)
		{
            if (!ModelState.IsValid)
            {
                return View("Register", user);
            }

            // Check if username already exists
            var existingUser = await _dbData.Users
                .FirstOrDefaultAsync(u => u.username == user.username || u.email == user.email);

            if (existingUser != null)
            {
                ModelState.AddModelError("", "Username or email already exists.");
                return View("Register", user);
            }

            // Hash the password before saving
            user.password = _passwordHasher.HashPassword(user, user.password);
            user.created_at = DateTime.Now;
            user.updated_at = DateTime.Now;

			_dbData.Users.Add(user);
			int rowsAffected = await _dbData.SaveChangesAsync();
			if(rowsAffected > 0)
			{
                return RedirectToAction("Login");
            }
            ModelState.AddModelError("", "Failed to save user to database.");
            return View("Register", user);
        }
    }
}

