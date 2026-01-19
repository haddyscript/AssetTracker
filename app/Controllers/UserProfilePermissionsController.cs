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
    public class UserProfilePermissionsController : Controller
    {
        private readonly AssetDbContext _context;
        private readonly AuthorizationService _authService;

        public UserProfilePermissionsController(AssetDbContext context, AuthorizationService authService)
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

        // GET: UserProfilePermissions
        public async Task<IActionResult> Index()
        {
            if (!await IsAdmin()) return RedirectToAction("AccessDenied", "Home");

            var permissions = await _context.UserProfilePermissions
                .Include(p => p.UserProfile)
                .ToListAsync();
            return View(permissions);
        }

        // GET: UserProfilePermissions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (!await IsAdmin()) return RedirectToAction("AccessDenied", "Home");

            if (id == null)
            {
                return NotFound();
            }

            var permission = await _context.UserProfilePermissions
                .Include(p => p.UserProfile)
                .FirstOrDefaultAsync(m => m.id == id);
            if (permission == null)
            {
                return NotFound();
            }

            return View(permission);
        }

        // GET: UserProfilePermissions/Create
        public async Task<IActionResult> Create()
        {
            if (!await IsAdmin()) return RedirectToAction("AccessDenied", "Home");

            ViewData["UserProfiles"] = await _context.UserProfiles.ToListAsync();
            ViewData["MenuAssignments"] = await GetMenuAssignments();
            return View();
        }

        // POST: UserProfilePermissions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("user_profile_id,module_name,can_view,can_create,can_edit,can_delete")] UserProfilePermission permission)
        {
            if (!await IsAdmin()) return RedirectToAction("AccessDenied", "Home");

            // Check for duplicate
            var existing = await _context.UserProfilePermissions
                .FirstOrDefaultAsync(p => p.user_profile_id == permission.user_profile_id && p.module_name == permission.module_name);
            if (existing != null)
            {
                ModelState.AddModelError("", "A permission for this user profile and module already exists.");
            }

            // Validate that the module is accessible for the user profile
            var accessibleMenus = await GetAccessibleMenus(permission.user_profile_id);
            if (!accessibleMenus.Contains(permission.module_name))
            {
                ModelState.AddModelError("module_name", "Selected module is not accessible for this user profile.");
            }

            if (ModelState.IsValid)
            {
                permission.created_at = DateTime.Now;
                permission.updated_at = DateTime.Now;
                _context.Add(permission);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserProfiles"] = await _context.UserProfiles.ToListAsync();
            ViewData["MenuAssignments"] = await GetMenuAssignments();
            return View(permission);
        }

        // GET: UserProfilePermissions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!await IsAdmin()) return RedirectToAction("AccessDenied", "Home");

            if (id == null)
            {
                return NotFound();
            }

            var permission = await _context.UserProfilePermissions.FindAsync(id);
            if (permission == null)
            {
                return NotFound();
            }
            ViewData["UserProfiles"] = await _context.UserProfiles.ToListAsync();
            ViewData["AvailableModules"] = await GetAccessibleMenus(permission.user_profile_id);
            ViewData["MenuAssignments"] = await GetMenuAssignments();
            return View(permission);
        }

        // POST: UserProfilePermissions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,user_profile_id,module_name,can_view,can_create,can_edit,can_delete,status")] UserProfilePermission permission)
        {
            if (!await IsAdmin()) return RedirectToAction("AccessDenied", "Home");

            if (id != permission.id)
            {
                return NotFound();
            }

            // Check for duplicate excluding current
            var existing = await _context.UserProfilePermissions
                .FirstOrDefaultAsync(p => p.user_profile_id == permission.user_profile_id && p.module_name == permission.module_name && p.id != id);
            if (existing != null)
            {
                ModelState.AddModelError("", "A permission for this user profile and module already exists.");
            }

            // Validate that the module is accessible for the user profile
            var accessibleMenus = await GetAccessibleMenus(permission.user_profile_id);
            if (!accessibleMenus.Contains(permission.module_name))
            {
                ModelState.AddModelError("module_name", "Selected module is not accessible for this user profile.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    permission.updated_at = DateTime.Now;
                    _context.Update(permission);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserProfilePermissionExists(permission.id))
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
            ViewData["AvailableModules"] = await GetAccessibleMenus(permission.user_profile_id);
            ViewData["MenuAssignments"] = await GetMenuAssignments();
            return View(permission);
        }

        // GET: UserProfilePermissions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!await IsAdmin()) return RedirectToAction("AccessDenied", "Home");

            if (id == null)
            {
                return NotFound();
            }

            var permission = await _context.UserProfilePermissions
                .Include(p => p.UserProfile)
                .FirstOrDefaultAsync(m => m.id == id);
            if (permission == null)
            {
                return NotFound();
            }

            return View(permission);
        }

        // POST: UserProfilePermissions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!await IsAdmin()) return RedirectToAction("AccessDenied", "Home");

            var permission = await _context.UserProfilePermissions.FindAsync(id);
            if (permission != null)
            {
                _context.UserProfilePermissions.Remove(permission);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool UserProfilePermissionExists(int id)
        {
            return _context.UserProfilePermissions.Any(e => e.id == id);
        }

        private async Task<Dictionary<int, List<string>>> GetMenuAssignments()
        {
            var menuAssignments = await _context.UserProfileMenus
                .Include(upm => upm.Menu)
                .Where(upm => upm.can_view && upm.status == 1 && upm.Menu.is_active)
                .Select(upm => new { upm.user_profile_id, upm.Menu.menu_name })
                .ToListAsync();

            var dict = menuAssignments.GroupBy(x => x.user_profile_id)
                .ToDictionary(g => g.Key, g => g.Select(x => x.menu_name).ToList());

            return dict;
        }

        private async Task<List<string>> GetAccessibleMenus(int userProfileId)
        {
            var menus = await _context.UserProfileMenus
                .Include(upm => upm.Menu)
                .Where(upm => upm.user_profile_id == userProfileId && upm.can_view && upm.status == 1 && upm.Menu.is_active)
                .Select(upm => upm.Menu.menu_name)
                .ToListAsync();
            return menus;
        }
    }
}