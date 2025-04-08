using Microsoft.AspNetCore.Mvc;

namespace examDotNet.Controllers
{
    public class HomeController : Controller
    {
        // GET: /Home/Index
        public IActionResult Index()
        {
            // Vérifier si l'utilisateur est connecté
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            // Récupérer les informations de l'utilisateur depuis la session
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.UserRole = HttpContext.Session.GetInt32("UserRole");

            return View();
        }
    }
}