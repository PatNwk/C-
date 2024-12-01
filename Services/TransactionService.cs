using MonWebApp.Models;

public class TransactionService
{
    private readonly WalletContext _context;

    public TransactionService(WalletContext context)
    {
        _context = context;
    }

    public void BuyAsset(int userId, int assetId, int quantity)
    {
        // Vérifier si l'actif existe
        var asset = _context.Assets.SingleOrDefault(a => a.AssetId == assetId);
        if (asset == null)
        {
            throw new Exception("L'actif demandé est introuvable.");
        }

        // Vérifier si l'utilisateur existe
        var user = _context.Users.SingleOrDefault(u => u.UserId == userId);
        if (user == null)
        {
            throw new Exception("L'utilisateur est introuvable.");
        }

        // Calculer le prix total
        var totalPrice = asset.Price * quantity;

        // Vérifier si l'utilisateur a suffisamment de fonds
        if (user.Balance < totalPrice)
        {
            throw new Exception("Solde insuffisant.");
        }

        // Réduire le solde de l'utilisateur
        user.Balance -= totalPrice;

        // Vérifier si l'utilisateur possède déjà cet actif
        var userAsset = _context.UserAssets.SingleOrDefault(ua => ua.UserId == userId && ua.AssetId == assetId);
        if (userAsset == null)
        {
            // Si l'utilisateur n'a pas cet actif, ajouter un nouvel enregistrement
            _context.UserAssets.Add(new UserAsset
            {
                UserId = userId,
                AssetId = assetId,
                Quantity = quantity,
                TotalPrice = totalPrice
            });
        }
        else
        {
            // Si l'utilisateur possède déjà cet actif, mettre à jour la quantité et le prix total
            userAsset.Quantity += quantity;
            userAsset.TotalPrice += totalPrice;
        }

        // Sauvegarder les changements dans la base de données
        _context.SaveChanges();
    }


}
