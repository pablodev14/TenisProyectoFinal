using Microsoft.AspNetCore.Mvc;
using MyRealAPI.DTOs;
using MyRealAPI.Services;

namespace MyRealAPI.Controllers
{
    [ApiController]
    [Route("api/receipts")]
    public class ReceiptsController : ControllerBase
    {
        private readonly IReceiptService _receiptService;

        public ReceiptsController(IReceiptService receiptService)
        {
            _receiptService = receiptService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_receiptService.GetAllReceipts());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var receipt = _receiptService.GetReceiptById(id);
            if (receipt == null) return NotFound("Factura no encontrada");
            return Ok(receipt);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateReceiptDto dto)
        {
            var newReceipt = _receiptService.CreateReceipt(dto);
            return CreatedAtAction(nameof(GetById), new { id = newReceipt.Id }, newReceipt);
        }

        [HttpPost("{id}/close")]
        public IActionResult Close(int id)
        {
            try
            {
                var closedReceipt = _receiptService.CloseReceipt(id);
                return Ok(closedReceipt);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPost("{id}/details")]
        public IActionResult AddDetail(int id)
        {
            return Ok("Endpoint para agregar detalles a la factura " + id + ". Pendiente de Módulo 3.");
        }
    }
}