using MyRealAPI.DTOs;
namespace MyRealAPI.Services
{
    public interface IProductService
    {
        IEnumerable<ProductResponseDto> GetAll();
        ProductResponseDto? Get(int id);
    }
}