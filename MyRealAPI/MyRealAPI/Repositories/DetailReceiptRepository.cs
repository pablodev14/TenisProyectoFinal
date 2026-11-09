using CsvHelper;
using CsvHelper.Configuration;
using MyRealAPI.Models;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace MyRealAPI.Repositories
{
    public class DetailReceiptRepository
    {
        private readonly string _filePath = "DetailReceipts.csv";
        private List<DetailReceipt> _details;

        public DetailReceiptRepository()
        {
            _details = new List<DetailReceipt>();
            LoadData();
        }

        private void LoadData()
        {
            if (!File.Exists(_filePath)) return;

            using var reader = new StreamReader(_filePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            _details = csv.GetRecords<DetailReceipt>().ToList(); // Se carga todo al levantar la API
        }

        private void SaveData()
        {
            using var writer = new StreamWriter(_filePath);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteRecords(_details); // Cada cambio se guarda inmediatamente
        }

        public void Add(DetailReceipt detail)
        {
            // Autoincremental manual simple en memoria
            detail.Id = _details.Any() ? _details.Max(d => d.Id) + 1 : 1;
            _details.Add(detail);
            SaveData(); // Nada de leer/escribir a mano, lo hace CsvHelper
        }

        public List<DetailReceipt> GetByReceiptId(int receiptId)
        {
            return _details.Where(d => d.ReceiptId == receiptId).ToList();
        }
    }
}
