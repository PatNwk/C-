using System.Diagnostics.Metrics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonWebApp.Models;

public class TransactionController : Controller
{
    private readonly WalletContext _context;
    private readonly TransactionService _transactionService;

    // Injection du service de transaction
    public TransactionController(WalletContext context, TransactionService transactionService)
    {
        _context = context;
        _transactionService = transactionService;
    }

    // Page de transfert des fonds (GET)
    [HttpGet("Transaction/TransferFunds")]
    public IActionResult TransferFunds()
    {
        var assets = _context.Assets.ToList();
        var model = new TransferFundsViewModel
        {
            Assets = assets
        };
        return View(model);
    }

    [HttpPost("Transaction/TransferFunds")]
    public async Task<IActionResult> TransferFunds(string recipientWalletAddress, int assetId, int assetQuantity)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (!userId.HasValue)
        {
            TempData["ErrorMessage"] = "Vous devez être connecté pour effectuer un transfert.";
            return RedirectToAction("Login", "Home");
        }

        var sender = await _context.Users.FindAsync(userId.Value);
        if (sender == null)
        {
            TempData["ErrorMessage"] = "Utilisateur introuvable.";
            return RedirectToAction("Login", "Home");
        }

        var recipient = await _context.Users.FirstOrDefaultAsync(u => u.WalletAddress == recipientWalletAddress);
        if (recipient == null)
        {
            TempData["ErrorMessage"] = "L'adresse du wallet du destinataire est invalide.";
            return RedirectToAction("TransferFunds");
        }

        var senderAsset = await _context.UserAssets
            .FirstOrDefaultAsync(ua => ua.UserId == sender.UserId && ua.AssetId == assetId);

        if (senderAsset == null || senderAsset.Quantity < assetQuantity)
        {
            TempData["ErrorMessage"] = "Vous n'avez pas suffisamment d'actifs pour effectuer ce transfert.";
            return RedirectToAction("TransferFunds");
        }

        var recipientAsset = await _context.UserAssets
            .FirstOrDefaultAsync(ua => ua.UserId == recipient.UserId && ua.AssetId == assetId);

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // Réduction de la quantité d'actifs du sender
            senderAsset.Quantity -= assetQuantity;
            _context.Entry(senderAsset).State = EntityState.Modified;

            if (recipientAsset == null)
            {
                // Si l'actif n'existe pas pour le destinataire, en créer un nouveau
                recipientAsset = new UserAsset
                {
                    UserId = recipient.UserId,
                    AssetId = assetId,
                    Quantity = assetQuantity,
                    TotalPrice = senderAsset.TotalPrice // Ajustez selon votre logique
                };
                _context.UserAssets.Add(recipientAsset);  // Ajout de l'actif pour le destinataire
            }
            else
            {
                // Si l'actif existe déjà pour le destinataire, mettre à jour sa quantité
                recipientAsset.Quantity += assetQuantity;
                _context.Entry(recipientAsset).State = EntityState.Modified; // Mise à jour de l'entité
            }

            // Sauvegarder les modifications dans la base de données
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            TempData["SuccessMessage"] = "Transfert réussi !";
            return RedirectToAction("TransferFunds");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            Console.WriteLine($"Erreur lors du transfert : {ex.Message}");
            TempData["ErrorMessage"] = "Une erreur s'est produite lors du transfert.";
            return RedirectToAction("TransferFunds");
        }
    }




    // Action d'achat d'actifs
    [HttpPost]
    public IActionResult BuyAsset(int assetId, int quantity)
    {
        // Vérifier si l'utilisateur est connecté
        var userId = HttpContext.Session.GetInt32("UserId");
        if (!userId.HasValue)
        {
            TempData["ErrorMessage"] = "Vous devez être connecté pour acheter un actif.";
            return RedirectToAction("Login");
        }

        try
        {
            // Appel au service pour acheter l'actif
            _transactionService.BuyAsset(userId.Value, assetId, quantity);
            TempData["SuccessMessage"] = "Achat réussi !";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction("AssetList");
    }



    [HttpGet]
    public IActionResult AssetList()
    {
        var assets = _context.Assets.ToList();  // Récupérer tous les actifs depuis la base de données
        return View(assets);  // Passer les données à la vue
    }
}



