using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace examDotNet.Filters
{
    public class AdminAuthFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Vérifier si l'utilisateur est connecté et a le rôle d'administrateur
            var userId = context.HttpContext.Session.GetInt32("UserId");
            var userRole = context.HttpContext.Session.GetInt32("UserRole");
            
            if (userId == null || userRole != 1)
            {
                // Rediriger vers la page de connexion si non autorisé
                context.Result = new RedirectToActionResult("Login", "Auth", null);
            }
        }
    }
}