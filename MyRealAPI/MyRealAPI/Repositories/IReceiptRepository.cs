using MyRealAPI.Models;

namespace MyRealAPI.Repositories
{
    public interface IReceiptRepository
    {
        IEnumerable<Receipt> GetAll();
        Receipt? GetById(int id);
        IReadOnlyList<ReceiptDetailRow> GetDetails(int receiptId);
        Receipt Add(Receipt receipt);
        void Update(Receipt receipt);
    }
}
