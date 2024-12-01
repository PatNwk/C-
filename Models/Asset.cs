using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonWebApp.Models
{
    // Models/Asset.cs
    public class Asset
    {
        public int AssetId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; } // Quantité disponible de l'actif

        // La relation inverse (vers UserAssets) doit être définie ici
        public ICollection<UserAsset> UserAssets { get; set; } // Relation vers UserAsset
    }


}
