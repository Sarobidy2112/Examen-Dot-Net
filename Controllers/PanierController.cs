using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using examDotNet.Data;
using examDotNet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
// using Newtonsoft.Json;

namespace examDotNet.Controllers
{
    public class PanierController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const decimal FRAIS_LIVRAISON = 34.0m; // Frais de livraison fixes

        public PanierController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Affiche le panier avec les produits
        public IActionResult Index()
        {
            var viewModel = new PanierViewModel
            {
                ProduitsPanier = new List<ProduitPanierViewModel>(),
                SousTotal = 0,
                FraisLivraison = FRAIS_LIVRAISON,
                Total = 0
            };

            // Récupérer l'ID utilisateur s'il est connecté
            var userId = HttpContext.Session.GetInt32("UserId");

            return View(viewModel);
        }

        // Action pour récupérer les produits du panier via AJAX
        [HttpGet]
        public async Task<IActionResult> GetCartItems()
        {
            try
            {
                var produitsPanier = new List<ProduitPanierViewModel>();
                decimal sousTotal = 0;

                // Nous allons récupérer les IDs des produits du localStorage côté client,
                // puis rechercher les informations complètes dans la base de données
                var produits = await _context.Produits
                    .Include(p => p.SousCategorie)
                    .ToListAsync();

                return Json(new { 
                    success = true, 
                    produits = produits.Select(p => new { 
                        id = p.IdProduit,
                        nom = p.NomProduit,
                        prix = p.Prix,
                        image = p.ImagePath ?? "/images/no-image.png",
                        categorie = p.SousCategorie?.NomSousCategorie ?? "Catégorie non définie"
                    })
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // Action pour supprimer un produit du panier
        [HttpPost]
        public IActionResult RemoveItem(int productId)
        {
            // Cette action sera appelée via AJAX
            // La suppression réelle sera gérée côté client dans le localStorage
            return Json(new { success = true });
        }

        // Action pour mettre à jour la quantité d'un produit
        [HttpPost]
        public IActionResult UpdateQuantity(int productId, int quantity)
        {
            // Cette action sera appelée via AJAX
            // La mise à jour réelle sera gérée côté client dans le localStorage
            return Json(new { success = true });
        }

        // Action pour vider le panier
        [HttpPost]
        public IActionResult ClearCart()
        {
            // Cette action sera appelée via AJAX
            // Le vidage réel sera géré côté client dans le localStorage
            return Json(new { success = true });
        }

        // Action pour passer à la caisse (à implémenter plus tard)
        [HttpPost]
        public IActionResult Checkout()
        {
            // Vérifier si l'utilisateur est connecté
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Auth", new { returnUrl = Url.Action("Index", "Panier") });
            }

            // Rediriger vers une page de paiement ou de confirmation (à implémenter)
            return RedirectToAction("Confirmation", "Commande");
        }
    }
}