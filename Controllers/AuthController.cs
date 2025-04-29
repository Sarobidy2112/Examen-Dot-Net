using Microsoft.AspNetCore.Mvc;
using examDotNet.Models;
using BCrypt.Net;
using examDotNet.Data;


namespace examDotNet.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Auth/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Auth/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user, string confirmPassword)
        {
            if (user.Password != confirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Les mots de passe ne correspondent pas");
            }

            if (ModelState.IsValid)
            {
                // Vérifier si l'email existe déjà
                if (_context.Users.Any(u => u.Email == user.Email))
                {
                    ModelState.AddModelError("Email", "Cet email est déjà utilisé");
                    return View(user);
                }

                // Hacher le mot de passe
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login");
            }
            return View(user);
        }

        // GET: /Auth/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Auth/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                ModelState.AddModelError("Password", "Email ou mot de passe incorrect");
                return View();
            }

            // Créer la session utilisateur
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserName", user.Name);
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetInt32("UserRole", user.Role);

            return RedirectToAction("Index", "Home");
        }

        // POST: /Auth/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}