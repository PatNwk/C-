using MonWebApp.Models;
using System.Linq;

namespace MonWebApp.Data
{
    public class DbInitializer
    {
        public static void Initialize(WalletContext context)
        {
            // Vérifie si la base de données a déjà des actifs
            if (context.Assets.Any())
            {
                return;   // La base de données est déjà initialisée
            }

            // Ajoute des actifs de test
            var assets = new Asset[]
            {
                new Asset { Name = "Bitcoin", Price = 50000m },
                new Asset { Name = "Ethereum", Price = 4000m },
                new Asset { Name = "Litecoin", Price = 200m }
            };

            foreach (var asset in assets)
            {
                context.Assets.Add(asset);
            }

            context.SaveChanges();  // Sauvegarde les actifs dans la base de données
        }
    }
}
