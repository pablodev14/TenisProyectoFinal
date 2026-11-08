using CsvHelper;
using CsvHelper.Configuration;
using MyRealAPI.Models;
using System.Globalization;

namespace MyRealAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly string _csvPath;

        public ProductRepository()
        {
            _csvPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "products.csv");
        }

        public IReadOnlyList<Product> GetAll()
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = true };
            using var reader = new StreamReader(_csvPath);
            using var csv = new CsvReader(reader, config);
            return csv.GetRecords<Product>().ToList().AsReadOnly();
        }

        public Product? Get(int id)
        {
            return GetAll().FirstOrDefault(p => p.Id == id);
        }

        public void UpdateStock(int id, int amount)
        {
            var products = GetAll().ToList();
            var product = products.FirstOrDefault(p => p.Id == id);
            
            if (product != null)
            {
                product.Stock += amount; 
                using var writer = new StreamWriter(_csvPath);
                using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
                csv.WriteRecords(products);
            }
        }
    }
}