using Microsoft.AspNetCore.Mvc;
using MonWebApp.Models;

public class AssetController : Controller
{
    private readonly WalletContext _context;

    public AssetController(WalletContext context)
    {
        _context = context;
    }

    // Méthode pour ajouter des actifs à la base de données
    public async Task<IActionResult> AddAssets()
    {
        if (!_context.Assets.Any())  // Si la table Assets est vide
        {
            var assets = new List<Asset>
            {
                new Asset { Name = "Action ABC", Price = 100, Quantity = 50 },
                new Asset { Name = "Crypto XYZ", Price = 50, Quantity = 100 },
                new Asset { Name = "Action DEF", Price = 200, Quantity = 30 }
            };

            await _context.Assets.AddRangeAsync(assets);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Actifs ajoutés avec succès!";
        }
        else
        {
            TempData["ErrorMessage"] = "Les actifs existent déjà dans la base de données.";
        }

        return RedirectToAction("Index");  // Rediriger vers la page d'accueil ou la page des actifs
    }
}
