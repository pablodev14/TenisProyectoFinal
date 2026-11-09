namespace MyRealAPI.Models
{
    public class DetailReceipt
    {
        public int Id { get; set; }
        public int ReceiptId { get; set; } // La llave que conecta este detalle con su factura
        public int ProductId { get; set; }

        // Copia exacta del momento de la venta
        public string Description { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Subtotal { get; set; }
    }
}
