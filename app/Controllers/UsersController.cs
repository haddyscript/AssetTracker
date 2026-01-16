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
        private readonly PasswordHasher<Admin> _adminPasswordHasher;

        public UsersController(AssetDbContext context)
		{
            _dbData = context;
			_passwordHasher = new PasswordHasher<User>();
            _adminPasswordHasher = new PasswordHasher<Admin>();
		}

        [HttpGet]
        public IActionResult Login()
		{
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
            ModelState.Remove("full_name");
            ModelState.Remove("email");

            if (string.IsNullOrEmpty(model.username) || string.IsNullOrEmpty(model.password))
            {
                ViewBag.Error = "Username and password are required.";
                return View(model);
            }

            var adminLoginResult = await TryAdminLogin(model);
            if (adminLoginResult != null)
            {
                return adminLoginResult;
            }

            var userLoginResult = await TryUserLogin(model);
            return userLoginResult;
        }

        private async Task<IActionResult> TryAdminLogin(User model)
        {
            var admin = await _dbData.Admins.FirstOrDefaultAsync(a => a.username == model.username && a.is_active);

            if (admin == null)
            {
                return null; 
            }

            var adminResult = _adminPasswordHasher.VerifyHashedPassword(admin, admin.password_hash, model.password);
            if (adminResult == PasswordVerificationResult.Failed)
            {
                return null;
            }

            var claimsPrincipal = CreateClaimsPrincipal(admin, "Admin");
            var authProperties = CreateAuthenticationProperties();

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                claimsPrincipal,
                authProperties);

            return RedirectToAction("Index", "Home");
        }

        private async Task<IActionResult> TryUserLogin(User model)
        {
            var user = await _dbData.Users.FirstOrDefaultAsync(u => u.username == model.username);

            if (user == null)
            {
                ViewBag.Error = "Invalid username.";
                return View(model);
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.password, model.password);
            if (result == PasswordVerificationResult.Failed)
            {
                ViewBag.Error = "Invalid password.";
                return View(model);
            }

            var claimsPrincipal = CreateClaimsPrincipal(user, "User");
            var authProperties = CreateAuthenticationProperties();

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                claimsPrincipal,
                authProperties);

            return RedirectToAction("Index", "Home");
        }

        private ClaimsPrincipal CreateClaimsPrincipal(object user, string role)
        {
            var claims = new List<Claim>();

            if (user is Admin admin)
            {
                claims.AddRange(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, admin.id.ToString()),
                    new Claim(ClaimTypes.Name, admin.username),
                    new Claim(ClaimTypes.Email, admin.email),
                    new Claim("FullName", admin.full_name),
                    new Claim(ClaimTypes.Role, role)
                });
            }
            else if (user is User regularUser)
            {
                claims.AddRange(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, regularUser.id.ToString()),
                    new Claim(ClaimTypes.Name, regularUser.username),
                    new Claim(ClaimTypes.Email, regularUser.email),
                    new Claim("FullName", regularUser.full_name),
                    new Claim(ClaimTypes.Role, role)
                });
            }

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            return new ClaimsPrincipal(claimsIdentity);
        }

        private AuthenticationProperties CreateAuthenticationProperties()
        {
            return new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
            };
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

            var registrationResult = await RegisterUserAsync(user);
            if (registrationResult.Success)
            {
                return RedirectToAction("Login");
            }

            ModelState.AddModelError("", registrationResult.ErrorMessage);
            return View("Register", user);
        }

        private async Task<RegistrationResult> RegisterUserAsync(User user)
        {
            // Check if username or email already exists
            var existingUser = await _dbData.Users
                .FirstOrDefaultAsync(u => u.username == user.username || u.email == user.email);

            if (existingUser != null)
            {
                return new RegistrationResult { Success = false, ErrorMessage = "Username or email already exists." };
            }

            // Hash the password and set timestamps
            user.password = _passwordHasher.HashPassword(user, user.password);
            user.created_at = DateTime.Now;
            user.updated_at = DateTime.Now;

            // Save to database
            _dbData.Users.Add(user);
            int rowsAffected = await _dbData.SaveChangesAsync();

            if (rowsAffected > 0)
            {
                return new RegistrationResult { Success = true };
            }

            return new RegistrationResult { Success = false, ErrorMessage = "Failed to save user to database." };
        }

        private class RegistrationResult
        {
            public bool Success { get; set; }
            public string ErrorMessage { get; set; }
        }
    }
}

