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

        [HttpGet("{id:int}")]
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

        [HttpPost("{id:int}/close")]
        public IActionResult Close(int id)
        {
            return WithReceipt(() => Ok(_receiptService.CloseReceipt(id)));
        }

        [HttpPost("{id:int}/details")]
        public IActionResult AddDetail(int id)
        {
            return WithReceipt(() =>
            {
                _receiptService.EnsureReceiptIsOpen(id);
                return StatusCode(StatusCodes.Status501NotImplemented, "El registro de detalles corresponde al módulo 3.");
            });
        }

        private IActionResult WithReceipt(Func<IActionResult> action)
        {
            try
            {
                return action();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        // POST /api/receipts/{id}/details
        [HttpPost("{id}/details")]
        public ActionResult<ReceiptDetailDto> AddDetail(int id, [FromBody] CreateDetailReceiptDto dto)
        {
            try
            {
                // El controlador delega toda la responsabilidad al servicio
                var detail = _receiptService.AddDetail(id, dto);

                return Ok(detail); // Retornamos el DTO, nunca la entidad[cite: 7, 9]
            }
            catch (KeyNotFoundException ex)
            {
                // Si no existe la factura o el producto
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                // Si la factura está cerrada o no hay stock (reglas de negocio)
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Cualquier otro error inesperado
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
