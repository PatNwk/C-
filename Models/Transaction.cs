namespace MonWebApp.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int AssetId { get; set; }
        public int Quantity { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }

        // Navigation properties
        public virtual User User { get; set; }
        public virtual Asset Asset { get; set; }
    }
}
