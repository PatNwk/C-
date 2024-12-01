// Models/TransferFundsViewModel.cs
using System.Collections.Generic;
using MonWebApp.Models;

namespace MonWebApp.Models
{
    public class TransferFundsViewModel
    {
        public List<Asset> Assets { get; set; } // Liste des actifs disponibles pour le transfert
    }
}
