using AssetTracker.Data;
using AssetTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssetTracker.Controllers
{
    public class UserProfilesController : Controller
    {
        private readonly AssetDbContext _context;

        public UserProfilesController(AssetDbContext context)
        {
            _context = context;
        }

        // GET: UserProfiles
        public async Task<IActionResult> Index()
        {
            return View(await _context.UserProfiles.ToListAsync());
        }

        // GET: UserProfiles/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var profile = await _context.UserProfiles.FindAsync(id);
            if (profile == null) return NotFound();
            return View(profile);
        }

        // GET: UserProfiles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserProfiles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserProfile userProfile)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userProfile);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userProfile);
        }

        // GET: UserProfiles/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var profile = await _context.UserProfiles.FindAsync(id);
            if (profile == null) return NotFound();
            return View(profile);
        }

        // POST: UserProfiles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UserProfile userProfile)
        {
            if (id != userProfile.id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(userProfile);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userProfile);
        }

        // GET: UserProfiles/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var profile = await _context.UserProfiles.FindAsync(id);
            if (profile == null) return NotFound();
            return View(profile);
        }

        // POST: UserProfiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var profile = await _context.UserProfiles.FindAsync(id);
            _context.UserProfiles.Remove(profile);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
