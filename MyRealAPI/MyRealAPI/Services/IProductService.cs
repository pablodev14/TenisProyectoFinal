using MyRealAPI.DTOs;

namespace MyRealAPI.Services;

public interface IProductService
{
    IReadOnlyList<ProductResponseDto> GetAll();
    ProductResponseDto? Get(int id);
}
