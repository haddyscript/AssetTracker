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

            
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Users}/{action=Login}/{id?}");
        }
    }

}

