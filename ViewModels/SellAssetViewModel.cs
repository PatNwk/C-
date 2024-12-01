using MonWebApp.Models;

namespace MonWebApp.ViewModels
{
    public class SellAssetViewModel
    {
        public List<Asset> Assets { get; set; }  // Liste des actifs disponibles
        public int AssetId { get; set; }          // Identifiant de l'actif sélectionné
        public decimal Quantity { get; set; }     // Quantité d'actifs à vendre
    }
}
