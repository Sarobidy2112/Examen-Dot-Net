using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace examDotNet.Models
{
    public class GrandCategorie
    {
        [Key]
        public int IdGrandCategorie { get; set; }

        [Required]
        [MaxLength(50)]
        public string NomGrandCategorie { get; set; }

        // Chemin de l'image stocké en base de données
        public string? ImagePath { get; set; }

        // Pour l'upload d'image via les formulaires
        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        // Collection de sous-catégories associées
        public ICollection<SousCategorie> SousCategories { get; set; } = new List<SousCategorie>();
    }
}