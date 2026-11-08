using MyRealAPI.DTOs;
using MyRealAPI.Repositories;

namespace MyRealAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<ProductResponseDto> GetAll()
        {
            return _repository.GetAll().Select(p => new ProductResponseDto
            {
                Codigo = p.Id.ToString(),
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                Precio = p.Precio,
                Stock = p.Stock
            });
        }

        public ProductResponseDto? Get(int id)
        {
            var p = _repository.Get(id);
            if (p == null) return null;

            return new ProductResponseDto
            {
                Codigo = p.Id.ToString(),
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                Precio = p.Precio,
                Stock = p.Stock
            };
        }
    }
}