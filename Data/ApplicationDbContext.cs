using examDotNet.Models;
using Microsoft.EntityFrameworkCore;

namespace examDotNet.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        // Nouvelles tables pour la hiérarchie de catégories
        public DbSet<GrandCategorie> GrandCategories { get; set; }
        public DbSet<SousCategorie> SousCategories { get; set; }
        public DbSet<Produit> Produits { get; set; }
        public DbSet<Commande> Commandes { get; set; }
        public DbSet<CommandeProduit> CommandeProduits { get; set; }
        
        // Vous pouvez conserver cette DbSet pour la transition, 
        // mais elle ne sera plus utilisée à terme
        // public DbSet<Categorie> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuration de la relation GrandCategorie -> SousCategorie
            modelBuilder.Entity<SousCategorie>()
                .HasOne(sc => sc.GrandCategorie)
                .WithMany(gc => gc.SousCategories)
                .HasForeignKey(sc => sc.IdGrandCat)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuration de la relation SousCategorie -> Produit
            modelBuilder.Entity<Produit>()
                .HasOne(p => p.SousCategorie)
                .WithMany(sc => sc.Produits)
                .HasForeignKey(p => p.IdSousCat)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuration pour CommandeProduit (table de liaison)
            modelBuilder.Entity<CommandeProduit>()
                .HasKey(cp => new { cp.IdCommande, cp.IdProduit });

            modelBuilder.Entity<CommandeProduit>()
                .HasOne(cp => cp.Commande)
                .WithMany(c => c.CommandeProduits)
                .HasForeignKey(cp => cp.IdCommande);

            modelBuilder.Entity<CommandeProduit>()
                .HasOne(cp => cp.Produit)
                .WithMany(p => p.CommandeProduits)
                .HasForeignKey(cp => cp.IdProduit);
        }
    }
}