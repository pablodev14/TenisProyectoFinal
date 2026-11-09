namespace MyRealAPI.DTOs
{
    // Solo entra el ID del producto y la cantidad
    public record CreateDetailReceiptDto(int ProductId, int Quantity);

    // Sale la información calculada y congelada en el tiempo[cite: 7]
    public record DetailReceiptDto(
        int Id,
        string Description,
        decimal UnitPrice,
        int Quantity,
        decimal Subtotal
    );
}