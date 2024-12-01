using System.Security.Cryptography;
using System.Text;

namespace MonWebApp.Services
{
    public class WalletService
    {
        public string GenerateRandomWalletAddress()
        {
            const int length = 42;
            const string prefix = "0x";
            var randomBytes = RandomNumberGenerator.GetBytes((length - prefix.Length) / 2);
            var walletAddress = new StringBuilder(prefix);

            foreach (var b in randomBytes)
            {
                walletAddress.Append(b.ToString("x2"));
            }

            return walletAddress.ToString();
        }
    }
}
