using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using examDotNet.Models;
using System.Linq;
using System.Threading.Tasks;
using examDotNet.Data;
using examDotNet.Filters;

namespace examDotNet.Controllers
{
    [TypeFilter(typeof(AdminAuthFilter))]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard()
        {
            return View("Dashboard");
        }

        // ----------- Utilisateurs -----------

        public async Task<IActionResult> Users()
        {
            var users = await _context.Users.ToListAsync();
            return View("Users/Users", users);
        }

        public async Task<IActionResult> UserDetails(int? id)
        {
            if (id == null) return NotFound();

            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
            if (user == null) return NotFound();

            return View("Users/UserDetails", user);
        }

        [HttpPost, ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUserConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Users));
        }

        // ----------- Catégories -----------

        public async Task<IActionResult> Categories()
        {
            return View("Categories/Categories", await _context.Categories.ToListAsync());
        }

        public async Task<IActionResult> CategoryDetails(int? id)
        {
            if (id == null) return NotFound();

            var categorie = await _context.Categories.FirstOrDefaultAsync(m => m.id_categorie == id);
            if (categorie == null) return NotFound();

            return View("Categories/CategoryDetails", categorie);
        }

        public IActionResult CreateCategory()
        {
            return View("Categories/CreateCategory");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory([Bind("id_categorie,nom_categorie")] Categorie categorie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(categorie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Categories));
            }
            return View("Categories/CreateCategory", categorie);
        }

        public async Task<IActionResult> EditCategory(int? id)
        {
            if (id == null) return NotFound();

            var categorie = await _context.Categories.FindAsync(id);
            if (categorie == null) return NotFound();

            return View("Categories/EditCategory", categorie);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategory(int id, [Bind("id_categorie,nom_categorie")] Categorie categorie)
        {
            if (id != categorie.id_categorie) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categorie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategorieExists(categorie.id_categorie)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Categories));
            }
            return View("Categories/EditCategory", categorie);
        }

        public async Task<IActionResult> DeleteCategory(int? id)
        {
            if (id == null) return NotFound();

            var categorie = await _context.Categories.FirstOrDefaultAsync(m => m.id_categorie == id);
            if (categorie == null) return NotFound();

            return View("Categories/DeleteCategory", categorie);
        }

        [HttpPost, ActionName("DeleteCategory")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategoryConfirmed(int id)
        {
            var categorie = await _context.Categories.FindAsync(id);
            if (categorie == null) return NotFound();

            _context.Categories.Remove(categorie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Categories));
        }

        private bool CategorieExists(int id)
        {
            return _context.Categories.Any(e => e.id_categorie == id);
        }

        // ----------- Produits -----------

        public async Task<IActionResult> Products()
        {
            var products = await _context.Produits.Include(p => p.Categorie).ToListAsync();
            return View("Products/Products", products);
        }

        public async Task<IActionResult> ProductDetails(int? id)
        {
            if (id == null) return NotFound();

            var produit = await _context.Produits.Include(p => p.Categorie)
                                                .FirstOrDefaultAsync(m => m.IdProduit == id);
            if (produit == null) return NotFound();

            return View("Products/ProductDetails", produit);
        }

        public async Task<IActionResult> CreateProduct()
        {
            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View("Products/CreateProduct");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct([Bind("IdProduit,NomProduit,Prix,Description,IdCat")] Produit produit)
        {
            if (ModelState.IsValid)
            {
                _context.Add(produit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Products));
            }
            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View("Products/CreateProduct", produit);
        }

        public async Task<IActionResult> EditProduct(int? id)
        {
            if (id == null) return NotFound();

            var produit = await _context.Produits.FindAsync(id);
            if (produit == null) return NotFound();

            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View("Products/EditProduct", produit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(int id, [Bind("IdProduit,NomProduit,Prix,Description,IdCat")] Produit produit)
        {
            if (id != produit.IdProduit) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(produit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProduitExists(produit.IdProduit)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Products));
            }
            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View("Products/EditProduct", produit);
        }

        public async Task<IActionResult> DeleteProduct(int? id)
        {
            if (id == null) return NotFound();

            var produit = await _context.Produits.Include(p => p.Categorie)
                                                .FirstOrDefaultAsync(m => m.IdProduit == id);
            if (produit == null) return NotFound();

            return View("Products/DeleteProduct", produit);
        }

        [HttpPost, ActionName("DeleteProduct")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProductConfirmed(int id)
        {
            var produit = await _context.Produits.FindAsync(id);
            if (produit == null) return NotFound();

            _context.Produits.Remove(produit);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Products));
        }

        private bool ProduitExists(int id)
        {
            return _context.Produits.Any(e => e.IdProduit == id);
        }
    }
}