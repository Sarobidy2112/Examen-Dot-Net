using Microsoft.EntityFrameworkCore;
using examDotNet.Models;

namespace examDotNet.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options) { }

        // Définition des DbSets pour chaque entité
        public DbSet<User> Users { get; set; }
        public DbSet<Categorie> Categories { get; set; }
        public DbSet<Produit> Produits { get; set; }
        public DbSet<Commande> Commandes { get; set; }  // Ajout du DbSet pour Commande
        public DbSet<CommandeProduit> CommandeProduits { get; set; }  // Ajout du DbSet pour CommandeProduit

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuration de la relation entre Produit et Categorie
            modelBuilder.Entity<Categorie>()
                .HasMany(c => c.Produits)  // Une catégorie a plusieurs produits
                .WithOne(p => p.Categorie)  // Chaque produit appartient à une catégorie
                .HasForeignKey(p => p.IdCat)  // La clé étrangère sur Produit
                .OnDelete(DeleteBehavior.Cascade);  // Définir le comportement de suppression en cascade, si nécessaire

            // Configuration de la relation entre Commande et CommandeProduit
            modelBuilder.Entity<Commande>()
                .HasMany(c => c.CommandeProduits)  // Une commande peut avoir plusieurs produits
                .WithOne(cp => cp.Commande)  // Chaque CommandeProduit appartient à une commande
                .HasForeignKey(cp => cp.IdCommande);  // La clé étrangère vers Commande

            // Configuration de la relation entre Produit et CommandeProduit
            modelBuilder.Entity<Produit>()
                .HasMany(p => p.CommandeProduits)  // Un produit peut être dans plusieurs CommandeProduit
                .WithOne(cp => cp.Produit)  // Chaque CommandeProduit est lié à un produit
                .HasForeignKey(cp => cp.IdProduit);  // La clé étrangère vers Produit
        }
    }
}
