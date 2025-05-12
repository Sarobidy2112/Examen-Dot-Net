using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using examDotNet.Data;
using examDotNet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Stripe;
using Stripe.Checkout;

namespace examDotNet.Controllers
{
    public class PanierController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;
        private const decimal FRAIS_LIVRAISON = 34.0m;

        public PanierController(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }


        public IActionResult Index()
        {
            var viewModel = new PanierViewModel
                {
                    ProduitsPanier = new List<ProduitPanierViewModel>(),
                    SousTotal = 0,
                    FraisLivraison = FRAIS_LIVRAISON,
                    Total = 0,
                    StripePublicKey = _config["Stripe:PublishableKey"]
                };

                var userId = HttpContext.Session.GetInt32("UserId");
                return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetCartItems()
        {
            try
            {
                var produitsPanier = new List<ProduitPanierViewModel>();
                decimal sousTotal = 0;
 
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

    
        [HttpPost]
        public IActionResult RemoveItem(int productId)
        {   
            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult UpdateQuantity(int productId, int quantity)
        {  
            return Json(new { success = true });
        }

    
        [HttpPost]
        public IActionResult ClearCart()
        {      
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> Checkout([FromBody] List<CartItem> cartItems)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return Unauthorized(new { error = "Utilisateur non connecté" });
            }

            try
            {
                var baseUrl = $"{Request.Scheme}://{Request.Host}";

                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    LineItems = cartItems.Select(item => new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(item.price * 100),
                            Currency = "eur",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.name,
                                Images = new List<string> { 
                                    !string.IsNullOrEmpty(item.image) 
                                        ? item.image.StartsWith("http") 
                                            ? item.image 
                                            : $"{baseUrl}{item.image}"
                                        : $"{baseUrl}/images/no-image.png"
                                }
                            }
                        },
                        Quantity = item.quantity
                    }).ToList(),
                    Mode = "payment",
                    SuccessUrl = $"{baseUrl}/Commande/Confirmation",
                    CancelUrl = $"{baseUrl}/Panier",
                    Metadata = new Dictionary<string, string> { { "userId", userId.ToString() } }
                };

                // Ajout des frais de livraison
                options.LineItems.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(FRAIS_LIVRAISON * 100),
                        Currency = "eur",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Frais de livraison"
                        }
                    },
                    Quantity = 1
                });

                var service = new SessionService();
                var session = await service.CreateAsync(options);

                return Json(new { sessionId = session.Id });
            }
            catch (StripeException e)
            {
                return BadRequest(new { error = e.StripeError.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // Ajoutez cette classe pour désérialiser les items du panier
        public class CartItem
        {
            public string id { get; set; }
            public string name { get; set; }
            public decimal price { get; set; }
            public int quantity { get; set; }
            public string image { get; set; }
            public string category { get; set; }
        }
    }
}