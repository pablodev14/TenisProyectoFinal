using MyRealAPI.Models;

namespace MyRealAPI.Repositories;

public interface IProductRepository
{
    IReadOnlyList<Product> GetAll();
    Product? Get(int id);
    void UpdateStock(int id, int stock);
}
