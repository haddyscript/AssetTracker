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
                var userIdClaim = UserClaimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (int.TryParse(userIdClaim, out int userId))
                {
                    // Get user profile
                    var user = await _context.Users
                        .Include(u => u.UserProfile)
                        .FirstOrDefaultAsync(u => u.id == userId);

                    if (user?.UserProfile != null)
                    {
                        // Get accessible menus
                        var accessibleMenuIds = await _context.UserProfileMenus
                            .Where(upm => upm.user_profile_id == user.UserProfile.id && upm.can_view && upm.status == 1)
                            .Select(upm => upm.menu_id)
                            .ToListAsync();

                        menus = await _context.Menus
                            .Where(m => accessibleMenuIds.Contains(m.id) && m.is_active)
                            .Include(m => m.ChildMenus)
                            .OrderBy(m => m.sort_order)
                            .ToListAsync();
                    }
                }
            }

            return View(menus);
        }
    }
}