using MyRealAPI.DTOs;
namespace MyRealAPI.Services
{
    public interface IReceiptService
    {
        IEnumerable<ReceiptResponseDto> GetAllReceipts();
        ReceiptResponseDto? GetReceiptById(int id);
        ReceiptResponseDto CreateReceipt(CreateReceiptDto dto);
        void EnsureReceiptIsOpen(int id);
        ReceiptResponseDto CloseReceipt(int id);
        ReceiptDetailDto AddDetail(int receiptId, CreateDetailReceiptDto dto);
    }
}