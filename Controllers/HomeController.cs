using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MonWebApp.Data;
using MonWebApp.Models;
using MonWebApp.Services;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using MonWebApp.Utils;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MonWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly WalletContext _context;
        private readonly StockService _stockService;

        public HomeController(WalletContext context, StockService stockService)
        {
            _context = context;
            _stockService = stockService;
        }

        // Page d'accueil avec vérification de l'état de connexion et cours boursiers
        public async Task<IActionResult> Index()
        {
            var username = HttpContext.Session.GetString("Username");

            if (!string.IsNullOrEmpty(username))
            {
                ViewData["WelcomeMessage"] = $"Bienvenue, {username} !";
                ViewData["IsLoggedIn"] = true;
            }
            else
            {
                ViewData["WelcomeMessage"] = "Bienvenue sur MonWebApp ! Connectez-vous pour accéder à vos informations.";
                ViewData["IsLoggedIn"] = false;
            }

            // Ajouter les cours de la bourse
            var symbols = new[] { "AAPL", "MSFT", "GOOG" }; // Symboles à récupérer
            var stockPrices = new Dictionary<string, string>();

            foreach (var symbol in symbols)
            {
                var price = await _stockService.GetStockPrice(symbol);
                stockPrices[symbol] = price;
            }

            ViewData["StockPrices"] = stockPrices;

            return View();
        }

        // Page de connexion (GET)
        public IActionResult Login()
        {
            return View();
        }

        // Action pour se connecter et traiter la connexion (POST)
        [HttpPost]
        public IActionResult LoginPost(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user != null)
            {
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("Username", user.Username);
                return RedirectToAction("Index");
            }

            ViewData["Error"] = "Nom d'utilisateur ou mot de passe incorrect.";
            return View("Login");
        }

        // Action pour se déconnecter
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        // Page d'inscription (GET)
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegisterPost(string username, string password, string email)
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.Username == username || u.Email == email);
            if (existingUser != null)
            {
                ViewData["Error"] = "Nom d'utilisateur ou email déjà utilisé.";
                return View("Register");
            }

            var randomWalletAddress = WalletUtils.GenerateRandomWalletAddress();

            var newUser = new User
            {
                Username = username,
                Password = password,
                Email = email,
                WalletAddress = randomWalletAddress
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            return RedirectToAction("Login");
        }

        // Méthode d'action exécutée avant chaque action (vérification de l'utilisateur connecté)
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId != null)
            {
                var user = _context.Users.Find(userId);
                ViewData["Balance"] = user?.Balance ?? 0;
            }
            base.OnActionExecuting(context);
        }

        // Liste des actifs disponibles (GET)
        [HttpGet]
        public IActionResult AssetList()
        {
            var assets = _context.Assets.ToList();
            if (assets == null || !assets.Any())
            {
                TempData["ErrorMessage"] = "Aucun actif disponible.";
            }

            return View(assets);
        }

        
    }
}
