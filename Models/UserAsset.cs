using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonWebApp.Models
{
    public class UserAsset
    {
        [Key] // Indique qu'il s'agit de la clé primaire
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserAssetId { get; set; }  // ID unique de la transaction d'achat
        public int UserId { get; set; }       // Clé étrangère vers l'utilisateur
        public int AssetId { get; set; }      // Clé étrangère vers l'actif acheté
        public decimal Quantity { get; set; }     // Quantité achetée
        public decimal TotalPrice { get; set; } // Prix total de l'achat (Prix * Quantité)

        public User User { get; set; }        // Relation vers l'utilisateur
        public Asset Asset { get; set; }      // Relation vers l'actif
    }
}
