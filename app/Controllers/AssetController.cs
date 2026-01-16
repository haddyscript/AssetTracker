using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AssetTracker.Models;
using AssetTracker.Data;
using Microsoft.EntityFrameworkCore;
using AssetTracker.Attributes;

namespace AssetTracker.Controllers
{
	public class AssetController : Controller
    {
        private readonly AssetDbContext _context;

        public AssetController(AssetDbContext context)
        {
            _context = context;
        }

        // GET: Asset
        public async Task<IActionResult> Index()
        {
            var assets = await _context.Assets.Include(a => a.assigned_user).ToListAsync();
            return View(assets);
        }

        // GET: Asset/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asset = await _context.Assets
                .Include(a => a.assigned_user)
                .FirstOrDefaultAsync(m => m.id == id);

            if (asset == null)
            {
                return NotFound();
            }

            return View(asset);
        }

        // GET: Asset/Create
        [AdminOnly]
        public IActionResult Create()
        {
            ViewBag.Users = _context.Users.ToList();
            return View(new Asset());
        }

        // POST: Asset/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminOnly]
        public async Task<IActionResult> Create([Bind("asset_tag,asset_name,description,category,brand,model,serial_number,purchase_date,purchase_price,status,condition,assigned_to_user_id,assigned_date")] Asset asset)
        {
            if (ModelState.IsValid)
            {
                asset.created_at = DateTime.Now;
                asset.updated_at = DateTime.Now;
                _context.Add(asset);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    System.Diagnostics.Debug.WriteLine($"Asset Create Validation Error: {error.ErrorMessage}");
                }

                ModelState.AddModelError("", "Please correct the errors below and try again.");
            }

            ViewBag.Users = _context.Users.ToList();
            return View(asset);
        }

        // GET: Asset/Edit/5
        [AdminOnly]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asset = await _context.Assets.FindAsync(id);
            if (asset == null)
            {
                return NotFound();
            }
            ViewBag.Users = _context.Users.ToList();
            return View(asset);
        }

        // POST: Asset/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminOnly]
        public async Task<IActionResult> Edit(int id, [Bind("id,asset_tag,asset_name,description,category,brand,model,serial_number,purchase_date,purchase_price,status,condition,assigned_to_user_id,assigned_date,created_at")] Asset asset)
        {
            if (id != asset.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    asset.updated_at = DateTime.Now;
                    _context.Update(asset);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssetExists(asset.id))
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

            // Debug: Log validation errors
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    System.Diagnostics.Debug.WriteLine($"Asset Edit Validation Error: {error.ErrorMessage}");
                }

                // Add a general error message for users
                ModelState.AddModelError("", "Please correct the errors below and try again.");
            }

            ViewBag.Users = _context.Users.ToList();
            return View(asset);
        }

        // GET: Asset/Delete/5
        [AdminOnly]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asset = await _context.Assets
                .Include(a => a.assigned_user)
                .FirstOrDefaultAsync(m => m.id == id);
            if (asset == null)
            {
                return NotFound();
            }

            return View(asset);
        }

        // POST: Asset/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AdminOnly]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var asset = await _context.Assets.FindAsync(id);
            if (asset != null)
            {
                _context.Assets.Remove(asset);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool AssetExists(int id)
        {
            return _context.Assets.Any(e => e.id == id);
        }
    }
}

