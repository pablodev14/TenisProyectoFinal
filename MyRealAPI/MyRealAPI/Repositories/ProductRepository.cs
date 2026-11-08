using CsvHelper;
using MyRealAPI.Models;
using System.Globalization;

namespace MyRealAPI.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly string _path;
    private readonly List<Product> _products;
    private readonly object _lock = new();

    public ProductRepository(IWebHostEnvironment environment)
    {
        _path = Path.Combine(environment.ContentRootPath, "Data", "products.csv");

        if (!File.Exists(_path))
            throw new FileNotFoundException("No se encontró el catálogo Data/products.csv.", _path);

        using var reader = new StreamReader(_path);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        _products = csv.GetRecords<Product>().ToList();

        if (_products.Any(p => p.Id <= 0 || string.IsNullOrWhiteSpace(p.Code)
            || string.IsNullOrWhiteSpace(p.Name) || string.IsNullOrWhiteSpace(p.Description)
            || p.Price < 0 || p.Stock < 0))
            throw new InvalidDataException("El catálogo contiene productos con datos inválidos.");

        if (_products.Select(p => p.Id).Distinct().Count() != _products.Count
            || _products.Select(p => p.Code).Distinct(StringComparer.OrdinalIgnoreCase).Count() != _products.Count)
            throw new InvalidDataException("El catálogo contiene identificadores o códigos repetidos.");
    }

    public IReadOnlyList<Product> GetAll()
    {
        lock (_lock)
        {
            return _products.Select(Copy).ToList();
        }
    }

    public Product? Get(int id)
    {
        lock (_lock)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            return product is null ? null : Copy(product);
        }
    }

    // El servicio de ventas usará este método; no existe un endpoint para editar stock.
    public void UpdateStock(int id, int stock)
    {
        if (stock < 0)
            throw new ArgumentOutOfRangeException(nameof(stock), "El stock no puede ser negativo.");

        lock (_lock)
        {
            var product = _products.FirstOrDefault(p => p.Id == id)
                ?? throw new KeyNotFoundException("El producto no existe.");
            var updatedProducts = _products.Select(Copy).ToList();
            updatedProducts.Single(p => p.Id == id).Stock = stock;

            var temporaryPath = _path + ".tmp";
            using (var writer = new StreamWriter(temporaryPath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(updatedProducts);
            }

            // Primero se guarda el archivo; después se actualiza la memoria.
            File.Move(temporaryPath, _path, overwrite: true);
            product.Stock = stock;
        }
    }

    private static Product Copy(Product product)
    {
        return new Product
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
