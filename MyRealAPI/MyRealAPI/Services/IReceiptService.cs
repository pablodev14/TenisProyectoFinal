using MyRealAPI.DTOs;
namespace MyRealAPI.Services
{
    public interface IReceiptService
    {
        IEnumerable<ReceiptResponseDto> GetAllReceipts();
        ReceiptResponseDto? GetReceiptById(int id);
        ReceiptResponseDto CreateReceipt(CreateReceiptDto dto);
        ReceiptResponseDto CloseReceipt(int id);
    }
}