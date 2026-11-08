using Microsoft.AspNetCore.Mvc;
using MyRealAPI.Services;

namespace MyRealAPI.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_productService.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var product = _productService.Get(id);
            if (product == null) return NotFound("Producto no encontrado");
            return Ok(product);
        }
    }
}