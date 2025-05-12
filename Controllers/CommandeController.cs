using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using examDotNet.Data;
using examDotNet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace examDotNet.Controllers
{
    public class CommandeController : Controller
    {
        public IActionResult Confirmation()
        {
            // Pour l'instant, afficher une simple confirmation
            // Plus tard, vous pourrez récupérer les détails de la session Stripe
            return View();
        }
    }
}