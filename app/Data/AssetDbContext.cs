using System;
using Microsoft.EntityFrameworkCore;
using AssetTracker.Models; 

namespace AssetTracker.Data
{
	public class AssetDbContext : DbContext
    {
        public AssetDbContext(DbContextOptions<AssetDbContext> options) : base(options)
        {
        }

        public DbSet<Asset> Assets { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<AssetRequest> AssetRequests { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<UserProfilePermission> UserProfilePermissions { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<UserProfileMenu> UserProfileMenus { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ensure username is unique
            modelBuilder.Entity<User>().HasIndex(u => u.username).IsUnique();

            // ensure admin username is unique
            modelBuilder.Entity<Admin>().HasIndex(a => a.username).IsUnique();
        }
    }
}

