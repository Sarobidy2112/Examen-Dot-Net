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
        public decimal Prix { get; set; }

        [Required]
        [MaxLength(225)]
        public string Description { get; set; }

        [Required]
        public int NbStock { get; set; }

        public string? Slug { get; set; }  // Nouveau champ pour le slug

        public string? ImagePath { get; set; }  // Chemin de l'image

        [ForeignKey("IdCat")]
        public Categorie? Categorie { get; set; }
        public int IdCat { get; set; }

        [NotMapped]  // Ne sera pas enregistré en base
        public IFormFile? ImageFile { get; set; }  // Pour l'upload

        public ICollection<CommandeProduit> CommandeProduits { get; set; } = new List<CommandeProduit>();
    }
}