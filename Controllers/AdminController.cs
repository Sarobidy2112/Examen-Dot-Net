using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using examDotNet.Models;
using System.Linq;
using System.Threading.Tasks;
using examDotNet.Data;
using examDotNet.Filters;
using Microsoft.AspNetCore.Mvc.Rendering; // Pour SelectList
using System.Text;
using Microsoft.AspNetCore.Hosting;
using System.IO;


namespace examDotNet.Controllers
{
    [TypeFilter(typeof(AdminAuthFilter))]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public AdminController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
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
        public async Task<IActionResult> CreateProduct(Produit produit)
        {
            if (ModelState.IsValid)
            {
                // Générer le slug
                produit.Slug = GenerateSlug(produit.NomProduit);

                // Gestion de l'image
                if (produit.ImageFile != null && produit.ImageFile.Length > 0)
                {
                    try
                    {
                        // Chemin du dossier d'upload
                        var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images", "products");
                        
                        // Créer le dossier s'il n'existe pas
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        // Nom unique pour le fichier
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(produit.ImageFile.FileName);
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        // Sauvegarder le fichier
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await produit.ImageFile.CopyToAsync(fileStream);
                        }

                        // Enregistrer le chemin dans la base
                        produit.ImagePath = "/images/products/" + uniqueFileName;
                    }
                    catch (Exception ex)
                    {
                        // Loguer l'erreur pour le débogage
                        Console.WriteLine($"Erreur lors de l'upload: {ex.Message}");
                        ModelState.AddModelError("", "Erreur lors de l'enregistrement de l'image");
                        ViewBag.Categories = new SelectList(_context.Categories, "id_categorie", "nom_categorie", produit.IdCat);
                        return View("Products/CreateProduct", produit);
                    }
                }

                _context.Produits.Add(produit);
                await _context.SaveChangesAsync();
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
        public async Task<IActionResult> EditProduct(Produit produit)
        {
            if (ModelState.IsValid)
            {
                // Gestion du slug
                produit.Slug = GenerateSlug(produit.NomProduit);

                // Gestion de l'image
                if (produit.ImageFile != null && produit.ImageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images/products");
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + produit.ImageFile.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    Directory.CreateDirectory(uploadsFolder);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await produit.ImageFile.CopyToAsync(fileStream);
                    }

                    // Supprimer l'ancienne image si elle existe
                    if (!string.IsNullOrEmpty(produit.ImagePath))
                    {
                        var oldImagePath = Path.Combine(_hostingEnvironment.WebRootPath, produit.ImagePath.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    produit.ImagePath = "/images/products/" + uniqueFileName;
                }

                _context.Produits.Update(produit);
                await _context.SaveChangesAsync();
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



        // Méthode pour générer un slug
        private string GenerateSlug(string phrase)
        {
            // Convertir en minuscules
            string str = phrase.ToLowerInvariant();
            
            // Remplacer les caractères spéciaux et espaces par des tirets
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if (char.IsLetterOrDigit(c))
                {
                    sb.Append(c);
                }
                else if (c == ' ' || c == '-' || c == '_')
                {
                    sb.Append('-');
                }
                // On ignore les autres caractères
            }
            
            str = sb.ToString();
            
            // Supprimer les doubles tirets
            while (str.Contains("--"))
                str = str.Replace("--", "-");
            
            // Supprimer les tirets en début et fin
            str = str.Trim('-');
            
            // Si le résultat est vide (que des caractères spéciaux), on met une valeur par défaut
            if (string.IsNullOrEmpty(str))
                str = "product";
            
            return str;
        }

    }
}