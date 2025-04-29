using Microsoft.AspNetCore.Mvc;
using examDotNet.Models;
using BCrypt.Net;
using examDotNet.Data;
using Microsoft.AspNetCore.Http;

namespace examDotNet.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const string UserIdSessionKey = "UserId";
        private const string UserNameSessionKey = "UserName";
        private const string UserEmailSessionKey = "UserEmail";
        private const string UserRoleSessionKey = "UserRole";

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Auth/Register
        public IActionResult Register()
        {
            // Si l'utilisateur est déjà connecté, rediriger vers l'accueil
            if (IsUserLoggedIn())
            {
                return RedirectToAction("Index", "Home");
            }
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
                
                // Connecter automatiquement l'utilisateur après l'inscription
                SetUserSession(user);
                
                return RedirectToAction("Index", "Home");
            }
            return View(user);
        }

        // GET: /Auth/Login
        public IActionResult Login()
        {
            // Si l'utilisateur est déjà connecté, rediriger vers l'accueil
            if (IsUserLoggedIn())
            {
                return RedirectToAction("Index", "Home");
            }
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
                ModelState.AddModelError(string.Empty, "Email ou mot de passe incorrect");
                return View();
            }

            // Créer la session utilisateur
            SetUserSession(user);

            return RedirectToAction("Index", "Home");
        }

        // POST: /Auth/Logout
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        // Méthodes helper pour gérer la session utilisateur
        
        private void SetUserSession(User user)
        {
            HttpContext.Session.SetInt32(UserIdSessionKey, user.Id);
            HttpContext.Session.SetString(UserNameSessionKey, user.Name);
            HttpContext.Session.SetString(UserEmailSessionKey, user.Email);
            HttpContext.Session.SetInt32(UserRoleSessionKey, user.Role);
        }

        public bool IsUserLoggedIn()
        {
            return HttpContext.Session.GetInt32(UserIdSessionKey).HasValue;
        }

        public int? GetCurrentUserId()
        {
            return HttpContext.Session.GetInt32(UserIdSessionKey);
        }

        public string GetCurrentUserName()
        {
            return HttpContext.Session.GetString(UserNameSessionKey);
        }

        public string GetCurrentUserEmail()
        {
            return HttpContext.Session.GetString(UserEmailSessionKey);
        }

        public int? GetCurrentUserRole()
        {
            return HttpContext.Session.GetInt32(UserRoleSessionKey);
        }
    }
}