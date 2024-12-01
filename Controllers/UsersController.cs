using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MonWebApp.Data;
using MonWebApp.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using MonWebApp.ViewModels;

namespace MonWebApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly WalletContext _context;

        public UsersController(WalletContext context)
        {
            _context = context;
        }

        // Action pour afficher la page d'ajout/retrait de fonds
        public IActionResult AddFunds()
        {
            return View();
        }

        public async Task<IActionResult> Details(int? id)
        {
            // Utiliser l'utilisateur connecté si aucun ID n'est fourni
            id ??= HttpContext.Session.GetInt32("UserId");

            if (id == null)
            {
                return RedirectToAction("Login", "Home");
            }

            // Charger les détails de l'utilisateur avec les actifs associés
            var user = await _context.Users
                                     .Include(u => u.UserAssets)  // Inclure les UserAssets de l'utilisateur
                                     .ThenInclude(ua => ua.Asset) // Inclure les informations des actifs
                                     .AsNoTracking() // Utilisation d'AsNoTracking pour améliorer les performances si on ne fait pas de mise à jour
                                     .FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }


        // Action AJAX pour gérer les transactions (ajout/retrait)
        public async Task<IActionResult> ManageFundsAjax(string transactionType, decimal amount)
        {
            // Vérifier si l'utilisateur est connecté
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return JsonError("Vous devez être connecté pour gérer vos fonds.");
            }

            // Rechercher l'utilisateur
            var user = await _context.Users.FindAsync(userId.Value);
            if (user == null)
            {
                return JsonError("Utilisateur introuvable.");
            }

            // Vérifier le montant
            if (amount <= 0)
            {
                return JsonError("Le montant doit être supérieur à zéro.");
            }

            // Exécuter la transaction
            try
            {
                if (transactionType == "add")
                {
                    user.Balance += amount;
                    await _context.SaveChangesAsync();
                    return JsonSuccess($"Fonds ajoutés avec succès! Nouveau solde : {user.Balance:C}", user.Balance);
                }
                else if (transactionType == "withdraw")
                {
                    if (user.Balance < amount)
                    {
                        return JsonError("Fonds insuffisants pour effectuer ce retrait.");
                    }

                    user.Balance -= amount;
                    await _context.SaveChangesAsync();
                    return JsonSuccess($"Fonds retirés avec succès! Nouveau solde : {user.Balance:C}", user.Balance);
                }
                else
                {
                    return JsonError("Type de transaction invalide.");
                }
            }
            catch
            {
                return JsonError("Une erreur est survenue lors de la gestion des fonds.");
            }
        }

        // Méthode privée pour retourner une réponse JSON réussie
        private IActionResult JsonSuccess(string message, decimal balance)
        {
            return Json(new { success = true, message, balance });
        }

        // Méthode privée pour retourner une réponse JSON en cas d'erreur
        private IActionResult JsonError(string message)
        {
            return Json(new { success = false, message });
        }

        // Affichage de la page de vente d'actifs
        public IActionResult SellAsset()
        {
            // Récupérer tous les actifs disponibles dans la base de données
            var assets = _context.Assets.ToList();

            // Créer un ViewModel avec la liste des actifs
            var model = new SellAssetViewModel
            {
                Assets = assets
            };

            return View(model); // Retourner la vue avec la liste des actifs
        }

        public async Task<IActionResult> SellAssetAjax(int assetId, decimal quantity)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                return Json(new { success = false, message = "Vous devez être connecté pour vendre des actifs." });
            }

            var user = await _context.Users.FindAsync(userId.Value);
            if (user == null)
            {
                return Json(new { success = false, message = "Utilisateur introuvable." });
            }

            var userAsset = await _context.UserAssets
                                           .Where(ua => ua.UserId == userId.Value && ua.AssetId == assetId)
                                           .FirstOrDefaultAsync();

            if (userAsset == null || userAsset.Quantity < quantity)
            {
                return Json(new { success = false, message = "Quantité insuffisante pour vendre cet actif." });
            }

            userAsset.Quantity -= quantity; // Réduire la quantité de l'actif

            var asset = await _context.Assets.FindAsync(assetId);
            if (asset == null)
            {
                return Json(new { success = false, message = "Actif non trouvé." });
            }

            var totalSaleValue = asset.Price * quantity; // totalSaleValue est de type decimal

            // Ajouter le montant total à la balance de l'utilisateur
            user.Balance += totalSaleValue;  // Pas besoin de conversion si Balance est decimal

            await _context.SaveChangesAsync();

            return Json(new { success = true, message = $"Actif vendu avec succès! Nouveau solde : {user.Balance:C}", balance = user.Balance });
        }

    }
}
