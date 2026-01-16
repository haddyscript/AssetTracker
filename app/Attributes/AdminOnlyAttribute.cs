using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using AssetTracker.Services;

namespace AssetTracker.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AdminOnlyAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Check if user is authenticated
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new RedirectToActionResult("Login", "Users", null);
                return;
            }

            // Get user ID from claims (assuming it's stored during authentication)
            var userIdClaim = context.HttpContext.User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                context.Result = new RedirectToActionResult("AccessDenied", "Home", null);
                return;
            }

            // Get authorization service from request services
            var authService = context.HttpContext.RequestServices.GetService(typeof(AuthorizationService)) as AuthorizationService;
            if (authService == null)
            {
                context.Result = new RedirectToActionResult("AccessDenied", "Home", null);
                return;
            }

            // Check if user has admin privileges using the service
            var isAdminTask = authService.IsUserAdminAsync(userId);
            isAdminTask.Wait(); // Since we're in a synchronous context, we need to wait

            if (!isAdminTask.Result)
            {
                context.Result = new RedirectToActionResult("AccessDenied", "Home", null);
                return;
            }

            // User is authorized - continue with request
        }
    }
}