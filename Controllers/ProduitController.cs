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
            // Récupérer les produits avec leurs catégories
            var produitsQuery = _context.Produits
                .Include(p => p.Categorie)
                .AsQueryable();

            // Filtrer par nom si un terme de recherche est fourni
            if (!string.IsNullOrEmpty(searchString))
            {
                produitsQuery = produitsQuery.Where(p => p.NomProduit.Contains(searchString));
            }

            // Filtrer par catégorie si des catégories sont sélectionnées
            if (categories != null && categories.Count > 0)
            {
                produitsQuery = produitsQuery.Where(p => p.Categorie != null && categories.Contains(p.Categorie.nom_categorie));
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

            // Récupérer toutes les catégories pour le filtre
            var toutesCategories = await _context.Categories
                .Select(c => c.nom_categorie)
                .ToListAsync();

            // Récupérer le prix maximum pour le slider
            var prixMaximum = await _context.Produits
                .MaxAsync(p => p.Prix);

            // Arrondir le prix maximum à la centaine supérieure pour le slider
            prixMaximum = Math.Ceiling(prixMaximum / 100) * 100;

            // Créer un modèle pour la vue qui contient les produits et les filtres
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

            // Récupérer les produits similaires (même catégorie, mais pas le produit actuel)
            var produitsSimilaires = await _context.Produits
                .Include(p => p.Categorie)
                .Where(p => p.IdCat == produit.IdCat && p.IdProduit != produit.IdProduit)
                .Take(4) // Limiter à 4 produits similaires pour ne pas surcharger la page
                .ToListAsync();

            // Créer le ViewModel qui contient à la fois le produit et les produits similaires
            var viewModel = new ProductDetailsViewModel
            {
                Produit = produit,
                ProduitsSimilaires = produitsSimilaires
            };

            return View(viewModel);
        }
    }
}