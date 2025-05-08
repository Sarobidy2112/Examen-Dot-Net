using examDotNet.Models;
using System.Collections.Generic;

namespace examDotNet.Models {
    public class ProduitsViewModel
    {
        public List<Produit> Produits { get; set; } = new List<Produit>();
        
        // Liste des sous-catégories pour le filtre
        public List<string> Categories { get; set; } = new List<string>();
        
        // Ajout des grandes catégories
        public List<GrandCategorie> GrandesCategories { get; set; } = new List<GrandCategorie>();
        
        // Mapping des sous-catégories par grande catégorie
        public Dictionary<int, List<SousCategorie>> SousCategoriesParGrandCat { get; set; } = new Dictionary<int, List<SousCategorie>>();
        
        public string SearchString { get; set; }
        public List<string> CategoriesSelectionnees { get; set; } = new List<string>();
        
        // Ajout pour stocker la grande catégorie sélectionnée
        public int? GrandeCategorieSelectionnee { get; set; }
        
        public bool EnStockSeulement { get; set; }
        public decimal PrixMaximum { get; set; }
        public decimal PrixMaxPossible { get; set; }
    }
}