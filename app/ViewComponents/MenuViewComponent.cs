using System.Security.Claims;
using AssetTracker.Data;
using AssetTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssetTracker.ViewComponents
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly AssetDbContext _context;

        public MenuViewComponent(AssetDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var menus = new List<Menu>();

            if (User.Identity?.IsAuthenticated == true)
            {
                var userIdClaim = ((ClaimsPrincipal)User).Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                if (int.TryParse(userIdClaim, out int userId))
                {
                    // Get user profile
                    var user = await _context.Users
                        .Include(u => u.UserProfile)
                        .FirstOrDefaultAsync(u => u.id == userId);

                    UserProfile userProfile = user?.UserProfile;
                    bool isAdmin = false;

                    if (userProfile == null)
                    {
                        // Try to find as admin
                        var admin = await _context.Admins
                            .Include(a => a.UserProfile)
                            .FirstOrDefaultAsync(a => a.id == userId);

                        userProfile = admin?.UserProfile;
                        isAdmin = admin != null;
                    }

                    if (userProfile != null)
                    {
                        // Get accessible menus from user_profile_menus
                        var accessibleMenuIds = await _context.UserProfileMenus
                            .Where(upm => upm.user_profile_id == userProfile.id && upm.can_view && upm.status == 1)
                            .Select(upm => upm.menu_id)
                            .ToListAsync();

                        // If admin and no accessible menus, give access to all
                        if (isAdmin && !accessibleMenuIds.Any())
                        {
                            accessibleMenuIds = await _context.Menus.Select(m => m.id).ToListAsync();
                        }

                        // Include children of accessible menus
                        var childIds = await _context.Menus
                            .Where(m => m.parent_id.HasValue && accessibleMenuIds.Contains(m.parent_id.Value) && m.is_active)
                            .Select(m => m.id)
                            .ToListAsync();
                        accessibleMenuIds = accessibleMenuIds.Union(childIds).ToList();

                        // Optionally respect user_profile_permissions for can_view
                        var permittedModuleNames = await _context.UserProfilePermissions
                            .Where(upp => upp.user_profile_id == userProfile.id && upp.can_view && upp.status == 1)
                            .Select(upp => upp.module_name)
                            .ToListAsync();

                        // Get menus that are accessible
                        var menuQuery = _context.Menus
                            .Where(m => accessibleMenuIds.Contains(m.id) && m.is_active);

                        menus = await menuQuery
                            .Include(m => m.ChildMenus)
                            .OrderBy(m => m.sort_order)
                            .ToListAsync();

                        // Filter ChildMenus to only include accessible children
                        foreach (var menu in menus)
                        {
                            menu.ChildMenus = menu.ChildMenus
                                .Where(cm => accessibleMenuIds.Contains(cm.id) && cm.is_active)
                                .ToList();
                        }

                        // Only include menus that have a route or have children
                        menus = menus.Where(m => !string.IsNullOrEmpty(m.route) || m.ChildMenus.Any()).ToList();
                    }
                }
            }

            return View(menus);
        }
    }
}