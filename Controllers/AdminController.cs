using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using examDotNet.Models;
using System.Linq;
using System.Threading.Tasks;
using examDotNet.Data;
using examDotNet.Filters;
using Microsoft.AspNetCore.Mvc.Rendering; // Pour SelectList

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
        public IActionResult Categories()
        {
            var categories = _context.Categories.ToList();
            return View("Categories/Categories", categories);
        }

        public IActionResult CreateCategory()
        {
            return View("Categories/CreateCategory");
        }

        [HttpPost]
        public IActionResult CreateCategory(Categorie categorie)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Add(categorie);
                _context.SaveChanges();
                return RedirectToAction(nameof(Categories));
            }
            return View("Categories/CreateCategory", categorie);
        }

        public IActionResult EditCategory(int id)
        {
            var categorie = _context.Categories.Find(id);
            if (categorie == null) return NotFound();
            return View("Categories/EditCategory", categorie);
        }

        [HttpPost]
        public IActionResult EditCategory(Categorie categorie)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Update(categorie);
                _context.SaveChanges();
                return RedirectToAction(nameof(Categories));
            }
            return View("Categories/EditCategory", categorie);
        }

        public IActionResult DeleteCategory(int id)
        {
            var categorie = _context.Categories.Find(id);
            if (categorie == null) return NotFound();
            return View("Categories/DeleteCategory", categorie);
        }

        [HttpPost, ActionName("DeleteCategory")]
        public IActionResult DeleteCategoryConfirmed(int id)
        {
            var categorie = _context.Categories.Find(id);
            if (categorie != null)
            {
                _context.Categories.Remove(categorie);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Categories));
        }

        // ----------- Produits -----------
        public IActionResult Products()
        {
            var produits = _context.Produits.Include(p => p.Categorie).ToList();
            return View("Products/Products", produits);
        }

        public IActionResult CreateProduct()
        {
            ViewBag.Categories = new SelectList(_context.Categories, "id_categorie", "nom_categorie");
            return View("Products/CreateProduct");
        }

        [HttpPost]
        public IActionResult CreateProduct(Produit produit)
        {
            if (ModelState.IsValid)
            {
                _context.Produits.Add(produit);
                _context.SaveChanges();
                return RedirectToAction(nameof(Products));
            }
            ViewBag.Categories = new SelectList(_context.Categories, "id_categorie", "nom_categorie", produit.IdCat);
            return View("Products/CreateProduct", produit);
        }

        public IActionResult EditProduct(int id)
        {
            var produit = _context.Produits.Find(id);
            if (produit == null) return NotFound();

            ViewBag.Categories = new SelectList(_context.Categories, "id_categorie", "nom_categorie", produit.IdCat);
            return View("Products/EditProduct", produit);
        }

        [HttpPost]
        public IActionResult EditProduct(Produit produit)
        {
            if (ModelState.IsValid)
            {
                _context.Produits.Update(produit);
                _context.SaveChanges();
                return RedirectToAction(nameof(Products));
            }
            ViewBag.Categories = new SelectList(_context.Categories, "id_categorie", "nom_categorie", produit.IdCat);
            return View("Products/EditProduct", produit);
        }

        public IActionResult DeleteProduct(int id)
        {
            var produit = _context.Produits
                .Include(p => p.Categorie)
                .FirstOrDefault(p => p.IdProduit == id);
            if (produit == null) return NotFound();
            return View("Products/DeleteProduct", produit);
        }

        [HttpPost, ActionName("DeleteProduct")]
        public IActionResult DeleteProductConfirmed(int id)
        {
            var produit = _context.Produits.Find(id);
            if (produit != null)
            {
                _context.Produits.Remove(produit);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Products));
        }

    }
}