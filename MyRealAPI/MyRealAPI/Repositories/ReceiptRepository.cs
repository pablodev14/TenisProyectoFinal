using MyRealAPI.Models;
namespace MyRealAPI.Repositories
{
    public class ReceiptRepository : IReceiptRepository
    {
        private static readonly List<Receipt> _receipts = new();
        private static int _nextId = 1;

        public IEnumerable<Receipt> GetAll() => _receipts;
        public Receipt? GetById(int id) => _receipts.FirstOrDefault(r => r.Id == id);
        public Receipt Add(Receipt receipt)
        {
            receipt.Id = _nextId++;
            _receipts.Add(receipt);
            return receipt;
        }
        public void Update(Receipt receipt)
        {
            var index = _receipts.FindIndex(r => r.Id == receipt.Id);
            if (index != -1) _receipts[index] = receipt;
        }
    }
}