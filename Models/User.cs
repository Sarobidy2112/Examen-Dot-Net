using System;
using System.ComponentModel.DataAnnotations;

namespace examDotNet.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "L'email est requis.")]
        [EmailAddress(ErrorMessage = "L'email n'est pas valide.")]
        [StringLength(200, ErrorMessage = "L'email ne peut pas dépasser 200 caractères.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Le mot de passe est requis.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Le mot de passe doit contenir entre 6 et 100 caractères.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "La civilité est requise.")]
        public string Genre { get; set; } // Mr ou Mme

        [Required(ErrorMessage = "Le nom est requis.")]
        [StringLength(100, ErrorMessage = "Le nom ne peut pas dépasser 100 caractères.")]
        public string Nom { get; set; }

        [Required(ErrorMessage = "Le prénom est requis.")]
        [StringLength(100)]
        public string Prenom { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateNaissance { get; set; }

        [Required(ErrorMessage = "Le numéro de téléphone est requis.")]
        [Phone(ErrorMessage = "Le numéro de téléphone est invalide.")]
        public string Telephone { get; set; }

        [Required(ErrorMessage = "L'adresse est requise.")]
        public string Adresse { get; set; }

        [Required(ErrorMessage = "La ville est requise.")]
        public string Ville { get; set; }

        [Required(ErrorMessage = "Le pays est requis.")]
        public string Pays { get; set; }

        public int Role { get; set; } = 0;
    }
}
