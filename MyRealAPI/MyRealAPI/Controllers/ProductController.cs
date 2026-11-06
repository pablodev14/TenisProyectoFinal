using Microsoft.AspNetCore.Mvc;
using MyRealAPI.DTOs;
using MyRealAPI.Services;

namespace MyRealAPI.Controllers;

[ApiController]
[Route("api/products")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ProductResponseDto>), StatusCodes.Status200OK)]
    public ActionResult<IReadOnlyList<ProductResponseDto>> GetAll()
    {
        return Ok(_productService.GetAll());
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ProductResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<ProductResponseDto> Get(int id)
    {
        var product = _productService.Get(id);

        if (product is null)
            return NotFound(new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Producto no encontrado",
                Detail = $"No existe un producto con el identificador {id}."
            });

        return Ok(product);
    }
}
