using System;
using AssetTracker.Data;
using AssetTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
namespace AssetTracker.Controllers

{
	public class UsersController : Controller
	{
        private readonly AssetDbContext _dbData;
        private readonly PasswordHasher<User> _passwordHasher;

        public UsersController(AssetDbContext context)
		{
            _dbData			= context;
			_passwordHasher = new PasswordHasher<User>();
		}
		
        public IActionResult Login()
		{
			return View();
		}
		public IActionResult Register()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> RegisterUser(User user)
		{
            if (!ModelState.IsValid)
            {
                return View("Register", user);
            }

            user.password = _passwordHasher.HashPassword(user, user.password);
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

