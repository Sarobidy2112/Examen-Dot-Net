using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace examDotNet.Models
{
    // Modèle pour la vue du panier
    public class PanierViewModel
    {
        public List<ProduitPanierViewModel> ProduitsPanier { get; set; }
        public decimal SousTotal { get; set; }
        public decimal FraisLivraison { get; set; }
        public decimal Total { get; set; }
        public string StripePublicKey { get; set; }
    }

    // Modèle pour les produits dans le panier
    public class ProduitPanierViewModel
    {
        public int IdProduit { get; set; }
        public string NomProduit { get; set; }
        public string ImagePath { get; set; }
        public string Categorie { get; set; }
        public decimal Prix { get; set; }
        public int Quantite { get; set; }
        public decimal SousTotal => Prix * Quantite;
    }

    // Modèle pour les données de panier côté client (correspondant à la structure localStorage)
    public class CartItemModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public int Quantity { get; set; }
    }
}