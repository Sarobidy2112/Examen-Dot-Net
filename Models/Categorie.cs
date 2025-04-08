using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace examDotNet.Models{
    public class Categorie{
        [Key]
        public int id_categorie {get; set;}
        [Required]
        [MaxLength(50)]
        public string nom_categorie {get; set;}
        public ICollection <Produit> Produits {get; set;} = new List <Produit>();
    }
}