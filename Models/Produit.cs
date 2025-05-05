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

        public string? Slug { get; set; }

        public string? ImagePath { get; set; }

        // Relation avec la sous-catégorie au lieu de la catégorie
        [ForeignKey("IdSousCat")]
        public SousCategorie? SousCategorie { get; set; }
        public int IdSousCat { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        public ICollection<CommandeProduit> CommandeProduits { get; set; } = new List<CommandeProduit>();
    }
}