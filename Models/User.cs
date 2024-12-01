using System.ComponentModel.DataAnnotations;

namespace MonWebApp.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        // Propriété WalletAddress ajoutée
        public string WalletAddress { get; set; }
        public decimal Balance { get; set; }

        // Si tu veux une propriété Id calculée, fais-le explicitement
        public int Id { get { return UserId; } }

        public ICollection<UserAsset> UserAssets { get; set; }
    }

}
