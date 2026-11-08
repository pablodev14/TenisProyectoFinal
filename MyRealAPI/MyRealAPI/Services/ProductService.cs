using MyRealAPI.DTOs;
using MyRealAPI.Models;
using MyRealAPI.Repositories;

namespace MyRealAPI.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public IReadOnlyList<ProductResponseDto> GetAll()
    {
        return _productRepository.GetAll().Select(ToDto).ToList();
    }

    public ProductResponseDto? Get(int id)
    {
        var product = _productRepository.Get(id);
        return product is null ? null : ToDto(product);
    }

    private static ProductResponseDto ToDto(Product product)
    {
        return new ProductResponseDto
        {
            Id = product.Id,
            Code = product.Code,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock
        };
    }
}
