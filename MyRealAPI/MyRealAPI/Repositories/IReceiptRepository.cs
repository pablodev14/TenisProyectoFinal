using MyRealAPI.Models;
namespace MyRealAPI.Repositories
{
    public interface IReceiptRepository
    {
        IEnumerable<Receipt> GetAll();
        Receipt? GetById(int id);
        Receipt Add(Receipt receipt);
        void Update(Receipt receipt);
    }
}