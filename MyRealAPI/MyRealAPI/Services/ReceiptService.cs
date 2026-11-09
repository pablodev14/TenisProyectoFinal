using System;
using System.Collections.Generic;
using System.Linq;
using MyRealAPI.DTOs;
using MyRealAPI.Models;
using MyRealAPI.Repositories;

namespace MyRealAPI.Services
{
    public class ReceiptService : IReceiptService
    {
        private readonly IReceiptRepository _repository;

        
        private readonly IProductRepository _productRepository;

        
        private readonly DetailReceiptRepository _detailRepository;

        
        public ReceiptService(
            IReceiptRepository repository,
            IProductRepository productRepository,
            DetailReceiptRepository detailRepository)
        {
            _repository = repository;
            _productRepository = productRepository;
            _detailRepository = detailRepository;
        }

        private ReceiptResponseDto MapToDto(Receipt receipt)
        {
            var details = _repository.GetDetails(receipt.Id)
                .Select(d => new ReceiptDetailDto(d.Id, d.ProductId, d.Description, d.UnitPrice, d.Quantity))
                .ToList();

            return new ReceiptResponseDto
            {
                Id = receipt.Id,
                Date = receipt.Date,
                CustomerDocument = receipt.CustomerDocument,
                CustomerName = receipt.CustomerName,
                Status = receipt.Status.ToString(),
                Total = receipt.Status == ReceiptStatus.Cerrada ? receipt.Total : details.Sum(d => d.Subtotal),
                Details = details
            };
        }

        public IEnumerable<ReceiptResponseDto> GetAllReceipts() => _repository.GetAll().Select(MapToDto).ToList();

        public ReceiptResponseDto? GetReceiptById(int id)
        {
            var receipt = _repository.GetById(id);
            return receipt == null ? null : MapToDto(receipt);
        }

        public ReceiptResponseDto CreateReceipt(CreateReceiptDto dto)
        {
            var receipt = new Receipt
            {
                Date = DateTime.Now,
                CustomerDocument = dto.CustomerDocument.Trim(),
                CustomerName = dto.CustomerName.Trim(),
                Status = ReceiptStatus.Abierta,
                Total = 0
            };
            return MapToDto(_repository.Add(receipt));
        }

        public void EnsureReceiptIsOpen(int id) => GetOpenReceipt(id);

        private Receipt GetOpenReceipt(int id)
        {
            var receipt = _repository.GetById(id)
                ?? throw new KeyNotFoundException("Factura no encontrada.");

            if (receipt.Status == ReceiptStatus.Cerrada)
                throw new InvalidOperationException("La factura ya está cerrada.");

            return receipt;
        }

        public ReceiptResponseDto CloseReceipt(int id)
        {
            var receipt = GetOpenReceipt(id);
            var response = MapToDto(receipt);

            if (response.Details.Count == 0)
                throw new InvalidOperationException("Una factura sin detalles no se puede cerrar.");

            var closed = receipt with { Status = ReceiptStatus.Cerrada, Total = response.Total };
            _repository.Update(closed);
            response.Status = closed.Status.ToString();
            return response;
        }

        // --- FIN DE TU CÓDIGO ORIGINAL ---

        // --- NUEVO MÉTODO MÓDULO 3 ---
        public ReceiptDetailDto AddDetail(int receiptId, CreateDetailReceiptDto dto)
        {
            // Validamos que la factura exista y esté abierta
            var receipt = GetOpenReceipt(receiptId);

            // 1. CAMBIO AQUÍ: Usamos .Get() en lugar de .GetById()
            var product = _productRepository.Get(dto.ProductId)
                ?? throw new KeyNotFoundException("Producto no encontrado.");

            // Regla: Si no alcanza el stock, el detalle no se agrega
            if (product.Stock < dto.Quantity)
                throw new InvalidOperationException("No hay stock suficiente. El detalle no se agrega.");

            // 2. CAMBIO AQUÍ: Usamos UpdateStock calculando el nuevo valor
            var nuevoStock = product.Stock - dto.Quantity;
            _productRepository.UpdateStock(product.Id, nuevoStock);

            // Regla: El detalle copia lo que se vendió tal como estaba ese día
            var detail = new DetailReceipt
            {
                ReceiptId = receiptId,
                ProductId = product.Id,
                Description = product.Description,
                UnitPrice = product.Price,
                Quantity = dto.Quantity,
                Subtotal = product.Price * dto.Quantity
            };

            // Guardamos el detalle
            _detailRepository.Add(detail);

            // Devolvemos el DTO
            return new ReceiptDetailDto(
                detail.Id,
                detail.ProductId,
                detail.Description,
                detail.UnitPrice,
                detail.Quantity
            );
        }
    }
}