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
    public class MenusController : Controller
    {
        private readonly AssetDbContext _context;
        private readonly AuthorizationService _authService;

        public MenusController(AssetDbContext context, AuthorizationService authService)
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

        // GET: Menus
        public async Task<IActionResult> Index()
        {
            if (!await IsAdmin()) return RedirectToAction("AccessDenied", "Home");

            var menus = await _context.Menus
                .Include(m => m.ParentMenu)
                .OrderBy(m => m.sort_order)
                .ToListAsync();
            return View(menus);
        }

        // GET: Menus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (!await IsAdmin()) return RedirectToAction("AccessDenied", "Home");

            if (id == null)
            {
                return NotFound();
            }

            var menu = await _context.Menus
                .Include(m => m.ParentMenu)
                .FirstOrDefaultAsync(m => m.id == id);
            if (menu == null)
            {
                return NotFound();
            }

            return View(menu);
        }

        // GET: Menus/Create
        public async Task<IActionResult> Create()
        {
            if (!await IsAdmin()) return RedirectToAction("AccessDenied", "Home");

            ViewData["ParentMenus"] = await _context.Menus.Where(m => m.parent_id == null).ToListAsync();
            return View();
        }

        // POST: Menus/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("menu_name,route,icon,parent_id,sort_order,is_active")] Menu menu)
        {
            if (!await IsAdmin()) return RedirectToAction("AccessDenied", "Home");

            if (ModelState.IsValid)
            {
                menu.created_at = DateTime.Now;
                menu.updated_at = DateTime.Now;
                _context.Add(menu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ParentMenus"] = await _context.Menus.Where(m => m.parent_id == null).ToListAsync();
            return View(menu);
        }

        // GET: Menus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!await IsAdmin()) return RedirectToAction("AccessDenied", "Home");

            if (id == null)
            {
                return NotFound();
            }

            var menu = await _context.Menus.FindAsync(id);
            if (menu == null)
            {
                return NotFound();
            }
            ViewData["ParentMenus"] = await _context.Menus.Where(m => m.parent_id == null && m.id != id).ToListAsync();
            return View(menu);
        }

        // POST: Menus/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,menu_name,route,icon,parent_id,sort_order,is_active")] Menu menu)
        {
            if (!await IsAdmin()) return RedirectToAction("AccessDenied", "Home");

            if (id != menu.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    menu.updated_at = DateTime.Now;
                    _context.Update(menu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MenuExists(menu.id))
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
            ViewData["ParentMenus"] = await _context.Menus.Where(m => m.parent_id == null && m.id != id).ToListAsync();
            return View(menu);
        }

        // GET: Menus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!await IsAdmin()) return RedirectToAction("AccessDenied", "Home");

            if (id == null)
            {
                return NotFound();
            }

            var menu = await _context.Menus
                .Include(m => m.ParentMenu)
                .FirstOrDefaultAsync(m => m.id == id);
            if (menu == null)
            {
                return NotFound();
            }

            return View(menu);
        }

        // POST: Menus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!await IsAdmin()) return RedirectToAction("AccessDenied", "Home");

            var menu = await _context.Menus.FindAsync(id);
            if (menu != null)
            {
                _context.Menus.Remove(menu);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool MenuExists(int id)
        {
            return _context.Menus.Any(e => e.id == id);
        }
    }
}