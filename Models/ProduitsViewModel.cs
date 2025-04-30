using examDotNet.Models;
using System.Collections.Generic;

namespace examDotNet.Models
{
    public class ProduitsViewModel
    {
        public List<Produit> Produits { get; set; } = new List<Produit>();
        public List<string> Categories { get; set; } = new List<string>();
        public string SearchString { get; set; }
        public List<string> CategoriesSelectionnees { get; set; } = new List<string>();
        public bool EnStockSeulement { get; set; }
        public decimal PrixMaximum { get; set; }
        public decimal PrixMaxPossible { get; set; }
    }
}