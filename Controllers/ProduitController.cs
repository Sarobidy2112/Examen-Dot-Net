using examDotNet.Models;
using examDotNet.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

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

        // Action pour afficher la liste des produits avec options de filtrage
        public async Task<IActionResult> Index(string searchString, List<string> categories, bool? inStock, decimal? maxPrice)
        {
            // Récupérer les produits avec leurs sous-catégories
            var produitsQuery = _context.Produits
                .Include(p => p.SousCategorie)
                .AsQueryable();

            // Filtrer par nom si un terme de recherche est fourni
            if (!string.IsNullOrEmpty(searchString))
            {
                produitsQuery = produitsQuery.Where(p => p.NomProduit.Contains(searchString));
            }

            // Filtrer par sous-catégorie si des catégories sont sélectionnées
            if (categories != null && categories.Count > 0)
            {
                produitsQuery = produitsQuery.Where(p => p.SousCategorie != null && categories.Contains(p.SousCategorie.NomSousCategorie));
            }

            // Filtrer par disponibilité si demandé
            if (inStock == true)
            {
                produitsQuery = produitsQuery.Where(p => p.NbStock > 0);
            }

            // Filtrer par prix maximum si spécifié
            if (maxPrice.HasValue)
            {
                produitsQuery = produitsQuery.Where(p => p.Prix <= maxPrice.Value);
            }

            // Récupérer toutes les sous-catégories pour le filtre
            var toutesCategories = await _context.SousCategories
                .Select(c => c.NomSousCategorie)
                .ToListAsync();

            // Récupérer le prix maximum pour le slider
            var prixMaximum = await _context.Produits
                .MaxAsync(p => p.Prix);

            // Arrondir le prix maximum à la centaine supérieure
            prixMaximum = Math.Ceiling(prixMaximum / 100) * 100;

            // Créer le ViewModel pour la vue
            var viewModel = new ProduitsViewModel
            {
                Produits = await produitsQuery.ToListAsync(),
                Categories = toutesCategories,
                SearchString = searchString,
                CategoriesSelectionnees = categories ?? new List<string>(),
                EnStockSeulement = inStock ?? false,
                PrixMaximum = maxPrice ?? prixMaximum,
                PrixMaxPossible = prixMaximum
            };

            return View(viewModel);
        }

        // Action pour afficher les détails d'un produit
        public async Task<IActionResult> Details(string slug)
        {
            if (string.IsNullOrEmpty(slug))
            {
                return NotFound();
            }

            var produit = await _context.Produits
                .Include(p => p.SousCategorie)
                .FirstOrDefaultAsync(p => p.Slug == slug);

            if (produit == null)
            {
                return NotFound();
            }

            // Produits similaires : même sous-catégorie, exclure le produit actuel
            var produitsSimilaires = await _context.Produits
                .Include(p => p.SousCategorie)
                .Where(p => p.IdSousCat == produit.IdSousCat && p.IdProduit != produit.IdProduit)
                .Take(4)
                .ToListAsync();

            var viewModel = new ProductDetailsViewModel
            {
                Produit = produit,
                ProduitsSimilaires = produitsSimilaires
            };

            return View(viewModel);
        }
    }
}
