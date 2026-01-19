using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AssetTracker.Data;
using AssetTracker.Models;
using AssetTracker.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssetTracker.Controllers
{
    public class UserProfileMenusController : Controller
    {
        private readonly AssetDbContext _context;
        private readonly AuthorizationService _authService;

        public UserProfileMenusController(AssetDbContext context, AuthorizationService authService)
        {
            _context = context;
            _authService = authService;
        }

        private async Task<bool> IsAdmin()
        {
            if (!User.Identity.IsAuthenticated) return false;
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdClaim, out int userId))
            {
                return await _authService.IsUserAdminAsync(userId);
            }
            return false;
        }

        // GET: UserProfileMenus
        public async Task<IActionResult> Index()
        {
            if (!await IsAdmin()) return RedirectToAction("AccessDenied", "Home");

            var userProfileMenus = await _context.UserProfileMenus
                .Include(upm => upm.UserProfile)
                .Include(upm => upm.Menu)
                .ToListAsync();
            return View(userProfileMenus);
        }

        // GET: UserProfileMenus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (!await IsAdmin()) return RedirectToAction("AccessDenied", "Home");

            if (id == null)
            {
                return NotFound();
            }

            var userProfileMenu = await _context.UserProfileMenus
                .Include(upm => upm.UserProfile)
                .Include(upm => upm.Menu)
                .FirstOrDefaultAsync(m => m.id == id);
            if (userProfileMenu == null)
            {
                return NotFound();
            }

            return View(userProfileMenu);
        }

        // GET: UserProfileMenus/Create
        public async Task<IActionResult> Create()
        {
            if (!await IsAdmin()) return RedirectToAction("AccessDenied", "Home");

            ViewData["UserProfiles"] = await _context.UserProfiles.ToListAsync();
            ViewData["Menus"] = await _context.Menus.ToListAsync();
            return View();
        }

        // POST: UserProfileMenus/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("user_profile_id,menu_id,can_view,status")] UserProfileMenu userProfileMenu)
        {
            if (!await IsAdmin()) return RedirectToAction("AccessDenied", "Home");

            // Remove navigation properties from model state validation
            ModelState.Remove("Menu");
            ModelState.Remove("UserProfile");

            // Check for duplicate
            var existing = await _context.UserProfileMenus
                .FirstOrDefaultAsync(upm => upm.user_profile_id == userProfileMenu.user_profile_id && upm.menu_id == userProfileMenu.menu_id);
            if (existing != null)
            {
                ModelState.AddModelError("", "This menu is already assigned to the user profile.");
            }

            if (ModelState.IsValid)
            {
                userProfileMenu.created_at = DateTime.Now;
                userProfileMenu.updated_at = DateTime.Now;
                _context.Add(userProfileMenu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserProfiles"] = await _context.UserProfiles.ToListAsync();
            ViewData["Menus"] = await _context.Menus.ToListAsync();
            return View(userProfileMenu);
        }

        // GET: UserProfileMenus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!await IsAdmin()) return RedirectToAction("AccessDenied", "Home");

            if (id == null)
            {
                return NotFound();
            }

            var userProfileMenu = await _context.UserProfileMenus.FindAsync(id);
            if (userProfileMenu == null)
            {
                return NotFound();
            }
            ViewData["UserProfiles"] = await _context.UserProfiles.ToListAsync();
            ViewData["Menus"] = await _context.Menus.ToListAsync();
            return View(userProfileMenu);
        }

        // POST: UserProfileMenus/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,user_profile_id,menu_id,can_view,status")] UserProfileMenu userProfileMenu)
        {
            if (!await IsAdmin()) return RedirectToAction("AccessDenied", "Home");

            if (id != userProfileMenu.id)
            {
                return NotFound();
            }

            // Remove navigation properties from model state validation
            ModelState.Remove("Menu");
            ModelState.Remove("UserProfile");

            // Check for duplicate excluding current
            var existing = await _context.UserProfileMenus
                .FirstOrDefaultAsync(upm => upm.user_profile_id == userProfileMenu.user_profile_id && upm.menu_id == userProfileMenu.menu_id && upm.id != id);
            if (existing != null)
            {
                ModelState.AddModelError("", "This menu is already assigned to the user profile.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    userProfileMenu.updated_at = DateTime.Now;
                    _context.Update(userProfileMenu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserProfileMenuExists(userProfileMenu.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserProfiles"] = await _context.UserProfiles.ToListAsync();
            ViewData["Menus"] = await _context.Menus.ToListAsync();
            return View(userProfileMenu);
        }

        // GET: UserProfileMenus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!await IsAdmin()) return RedirectToAction("AccessDenied", "Home");

            if (id == null)
            {
                return NotFound();
            }

            var userProfileMenu = await _context.UserProfileMenus
                .Include(upm => upm.UserProfile)
                .Include(upm => upm.Menu)
                .FirstOrDefaultAsync(m => m.id == id);
            if (userProfileMenu == null)
            {
                return NotFound();
            }

            return View(userProfileMenu);
        }

        // POST: UserProfileMenus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!await IsAdmin()) return RedirectToAction("AccessDenied", "Home");

            var userProfileMenu = await _context.UserProfileMenus.FindAsync(id);
            if (userProfileMenu != null)
            {
                _context.UserProfileMenus.Remove(userProfileMenu);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool UserProfileMenuExists(int id)
        {
            return _context.UserProfileMenus.Any(e => e.id == id);
        }
    }
}