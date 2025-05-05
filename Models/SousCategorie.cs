using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace examDotNet.Models
{
    public class SousCategorie
    {
        [Key]
        public int IdSousCategorie { get; set; }

        [Required]
        [MaxLength(50)]
        public string NomSousCategorie { get; set; }

        // Chemin de l'image stocké en base de données
        public string? ImagePath { get; set; }

        // Pour l'upload d'image via les formulaires
        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        // Relation avec la grande catégorie
        [ForeignKey("IdGrandCat")]
        public GrandCategorie? GrandCategorie { get; set; }
        public int IdGrandCat { get; set; }

        // Collection de produits associés
        public ICollection<Produit> Produits { get; set; } = new List<Produit>();
    }
}