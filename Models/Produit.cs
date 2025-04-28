using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace examDotNet.Models
{
    public class Produit
    {
        [Key]
        public int IdProduit { get; set; }

        [Required]
        [MaxLength(50)]
        public string NomProduit { get; set; }

        [Required]
        public decimal Prix { get; set; }  // Le prix unitaire du produit

        [Required]
        [MaxLength(225)]
        public string Description { get; set; }

        [ForeignKey("IdCat")]
        public Categorie? Categorie { get; set; }
        public int IdCat { get; set; }

        // Ajoutez cette propriété de navigation pour la relation avec CommandeProduit
        public ICollection<CommandeProduit> CommandeProduits { get; set; }  // Relation avec CommandeProduit
    }
}
