using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace MonWebApp.Services
{
    public class StockService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = " EAEXRN7DC24REINT"; // Remplacez par votre clé API Alpha Vantage

        public StockService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetStockPrice(string symbol)
        {
            // URL de l'API Alpha Vantage
            var url = $"https://www.alphavantage.co/query?function=GLOBAL_QUOTE&symbol={symbol}&apikey={_apiKey}";

            // Appeler l'API
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(jsonString);

                // Extraire le prix de l'action
                var price = json["Global Quote"]?["05. price"]?.ToString();
                return price ?? "Non disponible";
            }

            return "Erreur lors de la récupération";
        }
    }
}
