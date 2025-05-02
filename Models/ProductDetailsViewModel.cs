namespace examDotNet.Models
{
    public class ProductDetailsViewModel
    {
        public Produit Produit { get; set; }
        public List<Produit> ProduitsSimilaires { get; set; } = new List<Produit>();
    }
}