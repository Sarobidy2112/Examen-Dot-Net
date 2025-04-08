using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace examDotNet.Models
{
    public class CommandeProduit
    {
        [Key]
        public int IdCommandeProduit { get; set; }

        [Required]
        public int IdCommande { get; set; }  // Clé étrangère vers Commande

        [ForeignKey("IdCommande")]
        public Commande Commande { get; set; }  // Propriété de navigation vers Commande

        [Required]
        public int IdProduit { get; set; }  // Clé étrangère vers Produit

        [ForeignKey("IdProduit")]
        public Produit Produit { get; set; }  // Propriété de navigation vers Produit

        [Required]
        public int Quantite { get; set; }  // Quantité de produit dans la commande

        // Calculer le prix total pour ce produit dans la commande
        public decimal PrixTotal
        {
            get
            {
                return Produit.Prix * Quantite;  // Prix * Quantité
            }
        }
    }
}
