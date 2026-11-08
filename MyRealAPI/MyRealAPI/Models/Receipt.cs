namespace MyRealAPI.Models
{
    public enum ReceiptStatus { Abierta, Cerrada }

    public record Receipt
    {
        public int Id { get; init; }
        public DateTime Date { get; init; }
        public string CustomerDocument { get; init; } = string.Empty;
        public string CustomerName { get; init; } = string.Empty;
        public ReceiptStatus Status { get; init; }
        public decimal Total { get; init; }
    }
}
