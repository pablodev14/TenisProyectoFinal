using MyRealAPI.DTOs;
using MyRealAPI.Models;
using MyRealAPI.Repositories;

namespace MyRealAPI.Services
{
    public class ReceiptService : IReceiptService
    {
        private readonly IReceiptRepository _repository;

        public ReceiptService(IReceiptRepository repository)
        {
            _repository = repository;
        }

        private ReceiptResponseDto MapToDto(Receipt r)
        {
            return new ReceiptResponseDto
            {
                Id = r.Id,
                Date = r.Date,
                CustomerDocument = r.CustomerDocument,
                CustomerName = r.CustomerName,
                Status = r.Status.ToString(),
                Total = r.Total
            };
        }

        public IEnumerable<ReceiptResponseDto> GetAllReceipts() => _repository.GetAll().Select(MapToDto);
        public ReceiptResponseDto? GetReceiptById(int id)
        {
            var r = _repository.GetById(id);
            return r != null ? MapToDto(r) : null;
        }

        public ReceiptResponseDto CreateReceipt(CreateReceiptDto dto)
        {
            var receipt = new Receipt
            {
                Date = DateTime.Now,
                CustomerDocument = dto.CustomerDocument,
                CustomerName = dto.CustomerName,
                Status = ReceiptStatus.Abierta,
                Total = 0
            };
            return MapToDto(_repository.Add(receipt));
        }

        public ReceiptResponseDto CloseReceipt(int id)
        {
            var receipt = _repository.GetById(id);
            if (receipt == null) throw new Exception("Factura no encontrada");
            if (receipt.Status == ReceiptStatus.Cerrada) throw new Exception("La factura ya está cerrada");

            receipt.Status = ReceiptStatus.Cerrada;
            _repository.Update(receipt);
            return MapToDto(receipt);
        }
    }
}