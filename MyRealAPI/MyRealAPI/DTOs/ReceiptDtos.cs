using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MyRealAPI.DTOs
{
    [JsonUnmappedMemberHandling(JsonUnmappedMemberHandling.Disallow)]
    public class CreateReceiptDto
    {
        [Required(ErrorMessage = "El documento del cliente es obligatorio.")]
        public string CustomerDocument { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre del cliente es obligatorio.")]
        public string CustomerName { get; set; } = string.Empty;
    }

    public record ReceiptDetailDto(int Id, int ProductId, string Description, decimal UnitPrice, int Quantity)
    {
        public decimal Subtotal => UnitPrice * Quantity;
    }

    public class ReceiptResponseDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string CustomerDocument { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public IReadOnlyList<ReceiptDetailDto> Details { get; set; } = [];
    }
}
