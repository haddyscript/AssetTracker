using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace AssetTracker.Infrastructure
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(this IEndpointRouteBuilder endpoints)
        {
            
            endpoints.MapControllerRoute(
                name: "register",
                pattern: "/register/user",
                defaults: new { controller = "Users", action = "Register" });

            endpoints.MapControllerRoute(
                name: "register",
                pattern: "/register/admin",
                defaults: new { controller = "Admin", action = "Register" });

            endpoints.MapControllerRoute(
               name: "view all asset",
               pattern: "/view/all/asset",
               defaults: new { controller = "Asset", action = "Index" });

            endpoints.MapControllerRoute(
               name: "create asset",
               pattern: "/create/asset",
               defaults: new { controller = "Asset", action = "Create" });

            endpoints.MapControllerRoute(
                name: "update asset detail",
                pattern: "update/asset/{id?}",
                defaults: new { controller = "Asset", action = "Edit" });

            endpoints.MapControllerRoute(
                name: "view asset detail",
                pattern: "view/asset/detail/{id?}",
                defaults: new { controller = "Asset", action = "Details" });


            endpoints.MapControllerRoute(
                name: "login",
                pattern: "login",
                defaults: new { controller = "Users", action = "Login" });

            endpoints.MapControllerRoute(
                name: "logout",
                pattern: "logout",
                defaults: new { controller = "Users", action = "Logout" });
            
            endpoints.MapControllerRoute(
                name: "home",
                pattern: "home",
                defaults: new { controller = "Home", action = "Index" });

            // Asset Requests Routes
            endpoints.MapControllerRoute(
                name: "asset requests",
                pattern: "/asset-requests",
                defaults: new { controller = "AssetRequests", action = "Index" });

            endpoints.MapControllerRoute(
                name: "create asset request",
                pattern: "/asset-requests/create",
                defaults: new { controller = "AssetRequests", action = "Create" });

            endpoints.MapControllerRoute(
                name: "my asset requests",
                pattern: "/my-asset-requests",
                defaults: new { controller = "AssetRequests", action = "MyRequests" });

            // User Profile Permissions Routes
            endpoints.MapControllerRoute(
                name: "user profile permissions",
                pattern: "/user-profile-permissions",
                defaults: new { controller = "UserProfilePermissions", action = "Index" });

            endpoints.MapControllerRoute(
                name: "create user profile permission",
                pattern: "/user-profile-permissions/create",
                defaults: new { controller = "UserProfilePermissions", action = "Create" });

            // Menus Routes
            endpoints.MapControllerRoute(
                name: "menus",
                pattern: "/menus",
                defaults: new { controller = "Menus", action = "Index" });

            endpoints.MapControllerRoute(
                name: "create menu",
                pattern: "/menus/create",
                defaults: new { controller = "Menus", action = "Create" });

            // User Profile Menus Routes
            endpoints.MapControllerRoute(
                name: "user profile menus",
                pattern: "/user-profile-menus",
                defaults: new { controller = "UserProfileMenus", action = "Index" });

            endpoints.MapControllerRoute(
                name: "assign menu to profile",
                pattern: "/user-profile-menus/create",
                defaults: new { controller = "UserProfileMenus", action = "Create" });

            
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Users}/{action=Login}/{id?}");
        }
    }

}

