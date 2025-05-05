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

        // ----------- Grandes Catégories -----------
        public async Task<IActionResult> GrandCategories()
        {
            var grandCategories = await _context.GrandCategories.ToListAsync();
            return View("GrandCategories/GrandCategories", grandCategories);
        }

        public IActionResult CreateGrandCategory()
        {
            return View("GrandCategories/CreateGrandCategory");
        }

        [HttpPost]
        public async Task<IActionResult> CreateGrandCategory(GrandCategorie grandCategorie)
        {
            if (ModelState.IsValid)
            {
                // Gestion de l'image
                if (grandCategorie.ImageFile != null && grandCategorie.ImageFile.Length > 0)
                {
                    try
                    {
                        // Chemin du dossier d'upload
                        var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images", "categories");
                        
                        // Créer le dossier s'il n'existe pas
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        // Nom unique pour le fichier
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(grandCategorie.ImageFile.FileName);
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        // Sauvegarder le fichier
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await grandCategorie.ImageFile.CopyToAsync(fileStream);
                        }

                        // Enregistrer le chemin dans la base
                        grandCategorie.ImagePath = "/images/categories/" + uniqueFileName;
                    }
                    catch (Exception ex)
                    {
                        // Loguer l'erreur pour le débogage
                        Console.WriteLine($"Erreur lors de l'upload: {ex.Message}");
                        ModelState.AddModelError("", "Erreur lors de l'enregistrement de l'image");
                        return View("GrandCategories/CreateGrandCategory", grandCategorie);
                    }
                }

                _context.GrandCategories.Add(grandCategorie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(GrandCategories));
            }
            return View("GrandCategories/CreateGrandCategory", grandCategorie);
        }

        public async Task<IActionResult> EditGrandCategory(int id)
        {
            var grandCategorie = await _context.GrandCategories.FindAsync(id);
            if (grandCategorie == null) return NotFound();
            return View("GrandCategories/EditGrandCategory", grandCategorie);
        }

        [HttpPost]
        public async Task<IActionResult> EditGrandCategory(GrandCategorie grandCategorie)
        {
            if (ModelState.IsValid)
            {
                // Gestion de l'image
                if (grandCategorie.ImageFile != null && grandCategorie.ImageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images/categories");
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + grandCategorie.ImageFile.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    Directory.CreateDirectory(uploadsFolder);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await grandCategorie.ImageFile.CopyToAsync(fileStream);
                    }

                    // Supprimer l'ancienne image si elle existe
                    if (!string.IsNullOrEmpty(grandCategorie.ImagePath))
                    {
                        var oldImagePath = Path.Combine(_hostingEnvironment.WebRootPath, grandCategorie.ImagePath.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    grandCategorie.ImagePath = "/images/categories/" + uniqueFileName;
                }

                _context.GrandCategories.Update(grandCategorie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(GrandCategories));
            }
            return View("GrandCategories/EditGrandCategory", grandCategorie);
        }

        public async Task<IActionResult> DeleteGrandCategory(int id)
        {
            var grandCategorie = await _context.GrandCategories
                .Include(gc => gc.SousCategories)
                .FirstOrDefaultAsync(gc => gc.IdGrandCategorie == id);
            
            if (grandCategorie == null) return NotFound();
            return View("GrandCategories/DeleteGrandCategory", grandCategorie);
        }

        [HttpPost, ActionName("DeleteGrandCategory")]
        public async Task<IActionResult> DeleteGrandCategoryConfirmed(int id)
        {
            var grandCategorie = await _context.GrandCategories.FindAsync(id);
            if (grandCategorie != null)
            {
                // Supprimer l'image associée
                if (!string.IsNullOrEmpty(grandCategorie.ImagePath))
                {
                    var imagePath = Path.Combine(_hostingEnvironment.WebRootPath, grandCategorie.ImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                _context.GrandCategories.Remove(grandCategorie);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(GrandCategories));
        }

        // ----------- Sous-Catégories -----------
        public async Task<IActionResult> SousCategories()
        {
            var sousCategories = await _context.SousCategories
                .Include(sc => sc.GrandCategorie)
                .ToListAsync();
            return View("SousCategories/SousCategories", sousCategories);
        }

        public IActionResult CreateSousCategory()
        {
            ViewBag.GrandCategories = new SelectList(_context.GrandCategories, "IdGrandCategorie", "NomGrandCategorie");
            return View("SousCategories/CreateSousCategory");
        }

        [HttpPost]
        public async Task<IActionResult> CreateSousCategory(SousCategorie sousCategorie)
        {
            if (ModelState.IsValid)
            {
                // Gestion de l'image
                if (sousCategorie.ImageFile != null && sousCategorie.ImageFile.Length > 0)
                {
                    try
                    {
                        // Chemin du dossier d'upload
                        var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images", "souscategories");
                        
                        // Créer le dossier s'il n'existe pas
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        // Nom unique pour le fichier
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(sousCategorie.ImageFile.FileName);
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        // Sauvegarder le fichier
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await sousCategorie.ImageFile.CopyToAsync(fileStream);
                        }

                        // Enregistrer le chemin dans la base
                        sousCategorie.ImagePath = "/images/souscategories/" + uniqueFileName;
                    }
                    catch (Exception ex)
                    {
                        // Loguer l'erreur pour le débogage
                        Console.WriteLine($"Erreur lors de l'upload: {ex.Message}");
                        ModelState.AddModelError("", "Erreur lors de l'enregistrement de l'image");
                        ViewBag.GrandCategories = new SelectList(_context.GrandCategories, "IdGrandCategorie", "NomGrandCategorie", sousCategorie.IdGrandCat);
                        return View("SousCategories/CreateSousCategory", sousCategorie);
                    }
                }

                _context.SousCategories.Add(sousCategorie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(SousCategories));
            }
            ViewBag.GrandCategories = new SelectList(_context.GrandCategories, "IdGrandCategorie", "NomGrandCategorie", sousCategorie.IdGrandCat);
            return View("SousCategories/CreateSousCategory", sousCategorie);
        }

        public async Task<IActionResult> EditSousCategory(int id)
        {
            var sousCategorie = await _context.SousCategories.FindAsync(id);
            if (sousCategorie == null) return NotFound();

            ViewBag.GrandCategories = new SelectList(_context.GrandCategories, "IdGrandCategorie", "NomGrandCategorie", sousCategorie.IdGrandCat);
            return View("SousCategories/EditSousCategory", sousCategorie);
        }

        [HttpPost]
        public async Task<IActionResult> EditSousCategory(SousCategorie sousCategorie)
        {
            if (ModelState.IsValid)
            {
                // Gestion de l'image
                if (sousCategorie.ImageFile != null && sousCategorie.ImageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images/souscategories");
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + sousCategorie.ImageFile.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    Directory.CreateDirectory(uploadsFolder);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await sousCategorie.ImageFile.CopyToAsync(fileStream);
                    }

                    // Supprimer l'ancienne image si elle existe
                    if (!string.IsNullOrEmpty(sousCategorie.ImagePath))
                    {
                        var oldImagePath = Path.Combine(_hostingEnvironment.WebRootPath, sousCategorie.ImagePath.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    sousCategorie.ImagePath = "/images/souscategories/" + uniqueFileName;
                }

                _context.SousCategories.Update(sousCategorie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(SousCategories));
            }
            ViewBag.GrandCategories = new SelectList(_context.GrandCategories, "IdGrandCategorie", "NomGrandCategorie", sousCategorie.IdGrandCat);
            return View("SousCategories/EditSousCategory", sousCategorie);
        }

        public async Task<IActionResult> DeleteSousCategory(int id)
        {
            var sousCategorie = await _context.SousCategories
                .Include(sc => sc.GrandCategorie)
                .FirstOrDefaultAsync(sc => sc.IdSousCategorie == id);
            
            if (sousCategorie == null) return NotFound();
            return View("SousCategories/DeleteSousCategory", sousCategorie);
        }

        [HttpPost, ActionName("DeleteSousCategory")]
        public async Task<IActionResult> DeleteSousCategoryConfirmed(int id)
        {
            var sousCategorie = await _context.SousCategories.FindAsync(id);
            if (sousCategorie != null)
            {
                // Supprimer l'image associée
                if (!string.IsNullOrEmpty(sousCategorie.ImagePath))
                {
                    var imagePath = Path.Combine(_hostingEnvironment.WebRootPath, sousCategorie.ImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                _context.SousCategories.Remove(sousCategorie);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(SousCategories));
        }

        // ----------- Produits -----------
        public async Task<IActionResult> Products()
        {
            var produits = await _context.Produits
                .Include(p => p.SousCategorie)
                .ThenInclude(sc => sc.GrandCategorie)
                .ToListAsync();
            return View("Products/Products", produits);
        }

        public IActionResult CreateProduct()
        {
            ViewBag.SousCategories = new SelectList(_context.SousCategories, "IdSousCategorie", "NomSousCategorie");
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
                        ViewBag.SousCategories = new SelectList(_context.SousCategories, "IdSousCategorie", "NomSousCategorie", produit.IdSousCat);
                        return View("Products/CreateProduct", produit);
                    }
                }

                _context.Produits.Add(produit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Products));
            }

            ViewBag.SousCategories = new SelectList(_context.SousCategories, "IdSousCategorie", "NomSousCategorie", produit.IdSousCat);
            return View("Products/CreateProduct", produit);
        }

        public async Task<IActionResult> EditProduct(int id)
        {
            var produit = await _context.Produits.FindAsync(id);
            if (produit == null) return NotFound();

            ViewBag.SousCategories = new SelectList(_context.SousCategories, "IdSousCategorie", "NomSousCategorie", produit.IdSousCat);
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
            ViewBag.SousCategories = new SelectList(_context.SousCategories, "IdSousCategorie", "NomSousCategorie", produit.IdSousCat);
            return View("Products/EditProduct", produit);
        }

        public async Task<IActionResult> DeleteProduct(int id)
        {
            var produit = await _context.Produits
                .Include(p => p.SousCategorie)
                .FirstOrDefaultAsync(p => p.IdProduit == id);
            if (produit == null) return NotFound();
            return View("Products/DeleteProduct", produit);
        }

        [HttpPost, ActionName("DeleteProduct")]
        public async Task<IActionResult> DeleteProductConfirmed(int id)
        {
            var produit = await _context.Produits.FindAsync(id);
            if (produit != null)
            {
                // Supprimer l'image associée
                if (!string.IsNullOrEmpty(produit.ImagePath))
                {
                    var imagePath = Path.Combine(_hostingEnvironment.WebRootPath, produit.ImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                _context.Produits.Remove(produit);
                await _context.SaveChangesAsync();
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