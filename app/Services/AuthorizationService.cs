using AssetTracker.Data;
using AssetTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace AssetTracker.Services
{
    public class AuthorizationService
    {
        private readonly AssetDbContext _context;

        public AuthorizationService(AssetDbContext context)
        {
            _context = context;
        }

        /// Checks if a user has admin privileges (Admin or Super Admin profile)
        /// This checks both the Users table and the Admins table
        /// </summary>
        /// <param name="userId">The user ID to check</param>
        /// <returns>True if user is admin, false otherwise</returns>
        public async Task<bool> IsUserAdminAsync(int userId)
        {
            // First check if the user exists in the Users table with admin profile
            var user = await _context.Users
                .Include(u => u.UserProfile)
                .FirstOrDefaultAsync(u => u.id == userId);

            if (user != null && user.UserProfile != null)
            {
                var userProfileName = user.UserProfile.profile_name;
                if (userProfileName == "Admin" || userProfileName == "Super Admin")
                {
                    return true;
                }
            }

            // check Admin table with admin profile
            var admin = await _context.Admins
                .Include(a => a.UserProfile)
                .FirstOrDefaultAsync(a => a.id == userId);

            if (admin != null && admin.UserProfile != null)
            {
                var adminProfileName = admin.UserProfile.profile_name;
                return adminProfileName == "Admin" || adminProfileName == "Super Admin";
            }

            return false;
        }

        /// Gets the profile name for a user
        /// </summary>
        /// <param name="userId">The user ID</param>
        /// <returns>The profile name or null if user not found</returns>
        public async Task<string> GetUserProfileNameAsync(int userId)
        {
            var user = await _context.Users
                .Include(u => u.UserProfile)
                .FirstOrDefaultAsync(u => u.id == userId);

            return user?.UserProfile?.profile_name;
        }
    }
}