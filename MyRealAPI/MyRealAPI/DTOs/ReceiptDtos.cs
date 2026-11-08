namespace MyRealAPI.DTOs
{
    public class CreateReceiptDto
    {
        public string CustomerDocument { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
    }
    public class ReceiptResponseDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string CustomerDocument { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public decimal Total { get; set; }
    }
}