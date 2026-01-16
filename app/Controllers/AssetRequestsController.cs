using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AssetTracker.Models;
using AssetTracker.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace AssetTracker.Controllers
{
    public class AssetRequestsController : Controller
    {
        private readonly AssetDbContext _context;

        public AssetRequestsController(AssetDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var requests = await _context.AssetRequests
                .Include(r => r.user)
                .Include(r => r.asset)
                .Include(r => r.approved_by_admin)
                .OrderByDescending(r => r.requested_at)
                .ToListAsync();

            return View(requests);
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> MyRequests()
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            {
                return RedirectToAction("Login", "Users");
            }

            var requests = await _context.AssetRequests
                .Include(r => r.asset)
                .Include(r => r.approved_by_admin)
                .Where(r => r.user_id == userId)
                .OrderByDescending(r => r.requested_at)
                .ToListAsync();

            return View(requests);
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> CreateBorrowRequest(int? id)
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

            if (asset.status != "Available")
            {
                TempData["Error"] = "This asset is not available for borrowing.";
                return RedirectToAction("Index", "Asset");
            }

            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            {
                return RedirectToAction("Login", "Users");
            }

            var existingRequest = await _context.AssetRequests
                .Where(r => r.user_id == userId && r.asset_id == id && r.status == "Pending")
                .FirstOrDefaultAsync();

            if (existingRequest != null)
            {
                TempData["Error"] = "You already have a pending request for this asset.";
                return RedirectToAction("Index", "Asset");
            }

            ViewBag.Asset = asset;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> CreateBorrowRequest(int assetId, string remarks)
        {
            // Get current user ID
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            {
                return RedirectToAction("Login", "Users");
            }

            var asset = await _context.Assets.FindAsync(assetId);
            if (asset == null || asset.status != "Available")
            {
                TempData["Error"] = "Asset is not available for borrowing.";
                return RedirectToAction("Index", "Asset");
            }

            var request = new AssetRequest
            {
                user_id = userId,
                asset_id = assetId,
                request_type = "Borrow",
                status = "Pending",
                requested_at = DateTime.Now,
                remarks = remarks
            };

            _context.Add(request);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Borrow request submitted successfully!";
            return RedirectToAction("MyRequests");
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> CreateReturnRequest(int? id)
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

            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            {
                return RedirectToAction("Login", "Users");
            }

            if (asset.status != "Assigned" || asset.assigned_to_user_id != userId)
            {
                TempData["Error"] = "You can only return assets that are assigned to you.";
                return RedirectToAction("Index", "Asset");
            }

            ViewBag.Asset = asset;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> CreateReturnRequest(int assetId, string remarks)
        {
            // Get current user ID
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            {
                return RedirectToAction("Login", "Users");
            }

            var asset = await _context.Assets.FindAsync(assetId);
            if (asset == null || asset.status != "Assigned" || asset.assigned_to_user_id != userId)
            {
                TempData["Error"] = "Invalid return request.";
                return RedirectToAction("Index", "Asset");
            }

            var request = new AssetRequest
            {
                user_id = userId,
                asset_id = assetId,
                request_type = "Return",
                status = "Pending",
                requested_at = DateTime.Now,
                remarks = remarks
            };

            _context.Add(request);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Return request submitted successfully!";
            return RedirectToAction("MyRequests");
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Approve(int id, string adminRemarks)
        {
            var request = await _context.AssetRequests
                .Include(r => r.asset)
                .FirstOrDefaultAsync(r => r.id == id);

            if (request == null || request.status != "Pending")
            {
                TempData["Error"] = "Invalid request or request is not pending.";
                return RedirectToAction("Index");
            }

            // Get current admin ID
            var adminIdClaim = User.FindFirst("AdminId")?.Value;
            if (adminIdClaim == null || !int.TryParse(adminIdClaim, out int adminId))
            {
                TempData["Error"] = "Admin authentication required.";
                return RedirectToAction("Index", "Home");
            }

            request.status = "Approved";
            request.approved_at = DateTime.Now;
            request.approved_by_admin_id = adminId;

            if (!string.IsNullOrEmpty(adminRemarks))
            {
                request.remarks = adminRemarks;
            }

            // Update asset status based on request type
            if (request.request_type == "Borrow")
            {
                request.asset.status = "Assigned";
                request.asset.assigned_to_user_id = request.user_id;
                request.asset.assigned_date = DateTime.Now;
            }
            else if (request.request_type == "Return")
            {
                request.asset.status = "Available";
                request.asset.assigned_to_user_id = null;
                request.asset.assigned_date = null;
                request.returned_at = DateTime.Now;
            }

            await _context.SaveChangesAsync();

            TempData["Success"] = $"Request {request.request_type.ToLower()} approved successfully!";
            return RedirectToAction("Index");
        }

        // POST: AssetRequests/Reject/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Reject(int id, string adminRemarks)
        {
            var request = await _context.AssetRequests.FindAsync(id);

            if (request == null || request.status != "Pending")
            {
                TempData["Error"] = "Invalid request or request is not pending.";
                return RedirectToAction("Index");
            }

            // Get current admin ID
            var adminIdClaim = User.FindFirst("AdminId")?.Value;
            if (adminIdClaim == null || !int.TryParse(adminIdClaim, out int adminId))
            {
                TempData["Error"] = "Admin authentication required.";
                return RedirectToAction("Index", "Home");
            }

            request.status = "Rejected";
            request.approved_at = DateTime.Now;
            request.approved_by_admin_id = adminId;

            if (!string.IsNullOrEmpty(adminRemarks))
            {
                request.remarks = adminRemarks;
            }

            await _context.SaveChangesAsync();

            TempData["Success"] = "Request rejected successfully!";
            return RedirectToAction("Index");
        }

        // POST: AssetRequests/ConfirmReturn/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ConfirmReturn(int id, string adminRemarks)
        {
            var request = await _context.AssetRequests
                .Include(r => r.asset)
                .FirstOrDefaultAsync(r => r.id == id);

            if (request == null || request.status != "Approved" || request.request_type != "Return")
            {
                TempData["Error"] = "Invalid return confirmation request.";
                return RedirectToAction("Index");
            }

            // Get current admin ID
            var adminIdClaim = User.FindFirst("AdminId")?.Value;
            if (adminIdClaim == null || !int.TryParse(adminIdClaim, out int adminId))
            {
                TempData["Error"] = "Admin authentication required.";
                return RedirectToAction("Index", "Home");
            }

            request.status = "Returned";
            request.returned_at = DateTime.Now;

            if (!string.IsNullOrEmpty(adminRemarks))
            {
                request.remarks = adminRemarks;
            }

            // Update asset status
            request.asset.status = "Available";
            request.asset.assigned_to_user_id = null;
            request.asset.assigned_date = null;

            await _context.SaveChangesAsync();

            TempData["Success"] = "Return confirmed successfully!";
            return RedirectToAction("Index");
        }

        // GET: AssetRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var request = await _context.AssetRequests
                .Include(r => r.user)
                .Include(r => r.asset)
                .Include(r => r.approved_by_admin)
                .FirstOrDefaultAsync(m => m.id == id);

            if (request == null)
            {
                return NotFound();
            }

            return View(request);
        }
    }
}