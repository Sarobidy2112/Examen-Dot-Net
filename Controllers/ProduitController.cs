using examDotNet.Models;
using examDotNet.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace examDotNet.Controllers
{
    public class ProduitController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProduitController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // Action pour afficher la liste des produits
        public async Task<IActionResult> Index()
        {
            var produits = await _context.Produits
                .Include(p => p.Categorie)
                .ToListAsync();
            
            return View(produits);
        }

        // Action pour afficher les détails d'un produit spécifique
        public async Task<IActionResult> Details(string slug)
        {
            if (string.IsNullOrEmpty(slug))
            {
                return NotFound();
            }

            var produit = await _context.Produits
                .Include(p => p.Categorie)
                .FirstOrDefaultAsync(p => p.Slug == slug);

            if (produit == null)
            {
                return NotFound();
            }

            return View(produit);
        }
    }
}