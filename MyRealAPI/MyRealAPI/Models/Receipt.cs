namespace MyRealAPI.Models
{
    public enum ReceiptStatus { Abierta, Cerrada }
    public class Receipt
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string CustomerDocument { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public ReceiptStatus Status { get; set; }
        public decimal Total { get; set; }
    }
}