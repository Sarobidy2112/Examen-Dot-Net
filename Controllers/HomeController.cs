using Microsoft.AspNetCore.Mvc;
using examDotNet.Data;
using examDotNet.Models;
using Microsoft.EntityFrameworkCore;

namespace examDotNet.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.UserRole = HttpContext.Session.GetInt32("UserRole");

            var grandCategories = _context.GrandCategories.ToList();
            var sousCategories = _context.SousCategories
                                        .Include(sc => sc.Produits)
                                        .ToList();

            ViewBag.GrandCategories = grandCategories;
            return View(sousCategories); // modèle principal = sous-catégories
        }
    }
}