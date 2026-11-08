using CsvHelper;
using MyRealAPI.Models;
using System.Globalization;

namespace MyRealAPI.Repositories
{
    public class ReceiptRepository : IReceiptRepository
    {
        private readonly object _lock = new();
        private readonly string _path;
        private readonly string _detailsPath;
        private List<Receipt> _receipts;

        public ReceiptRepository(IWebHostEnvironment environment)
        {
            var data = Path.Combine(environment.ContentRootPath, "Data");
            _path = Path.Combine(data, "receipts.csv");
            _detailsPath = Path.Combine(data, "details.csv");
            _receipts = ReadCsv<Receipt>(_path);
            if (_receipts.Any(r => r.Id <= 0) || _receipts.DistinctBy(r => r.Id).Count() != _receipts.Count)
                throw new InvalidDataException("Los números de factura deben ser positivos y únicos.");

            ReadCsv<ReceiptDetailRow>(_detailsPath);
        }

        private static List<T> ReadCsv<T>(string path)
        {
            using var reader = new StreamReader(path);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            return csv.GetRecords<T>().ToList();
        }

        public IEnumerable<Receipt> GetAll()
        {
            lock (_lock) return _receipts.ToList();
        }

        public Receipt? GetById(int id)
        {
            lock (_lock) return _receipts.FirstOrDefault(r => r.Id == id);
        }

        public IReadOnlyList<ReceiptDetailRow> GetDetails(int receiptId)
        {
            return ReadCsv<ReceiptDetailRow>(_detailsPath).Where(d => d.ReceiptId == receiptId).ToList();
        }

        public Receipt Add(Receipt receipt)
        {
            lock (_lock)
            {
                int nextId = _receipts.Count == 0 ? 1 : checked(_receipts.Max(r => r.Id) + 1);
                var added = receipt with { Id = nextId };
                Save([.. _receipts, added]);
                return added;
            }
        }

        public void Update(Receipt receipt)
        {
            lock (_lock)
            {
                var current = _receipts.FirstOrDefault(r => r.Id == receipt.Id)
                    ?? throw new KeyNotFoundException("Factura no encontrada.");
                if (current.Status == ReceiptStatus.Cerrada)
                    throw new InvalidOperationException("La factura ya está cerrada.");

                Save(_receipts.Select(r => r.Id == receipt.Id ? receipt : r).ToList());
            }
        }

        private void Save(List<Receipt> updated)
        {
            // Reemplazar el CSV completo solo después de terminar la escritura.
            var temporaryPath = _path + ".tmp";
            using (var writer = new StreamWriter(temporaryPath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.Context.TypeConverterOptionsCache.GetOptions<DateTime>().Formats = ["O"];
                csv.WriteRecords(updated);
            }

            File.Move(temporaryPath, _path, overwrite: true);
            _receipts = updated;
        }
    }
}
