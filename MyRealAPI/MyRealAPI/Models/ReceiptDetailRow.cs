namespace MyRealAPI.Models
{
    // Datos históricos que Receipt lee del CSV de detalles.
    public class ReceiptDetailRow
    {
        public int Id { get; set; }
        public int ReceiptId { get; set; }
        public int ProductId { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}
