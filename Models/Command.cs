using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace examDotNet.Models
{
    public class Commande
    {
        [Key]
        public int IdCommande { get; set; }

        [Required]
        public int UserId { get; set; }  // Id de l'utilisateur qui a passé la commande

        [ForeignKey("UserId")]
        public User User { get; set; }  // Propriété de navigation vers l'utilisateur

        [Required]
        public DateTime DateCommande { get; set; }

        // Relation un-à-plusieurs avec la table "CommandeProduit"
        public ICollection<CommandeProduit> CommandeProduits { get; set; }

        // Calculer le prix total de la commande
        public decimal PrixTotal
        {
            get
            {
                decimal total = 0;
                foreach (var commandeProduit in CommandeProduits)
                {
                    total += commandeProduit.PrixTotal;  // Calcul du total pour chaque produit
                }
                return total;
            }
        }
    }
}
