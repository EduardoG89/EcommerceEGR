using Ecommerce.DTOs.Common;
using Ecommerce.DTOs.Products;
using Ecommerce.Models;
using Ecommerce.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productServices;

        public ProductsController(IProductService productService)
        {
            _productServices = productService;
        }


        [HttpPost("create-product")]
        public async Task<ActionResult<ApiResponse<ProductDto>>> CreateProduct([FromBody] CreateProductDto createProductDto)
        {
            try
            {
                var product = await _productServices.CreateProductAsync(createProductDto);

                return CreatedAtAction(
                nameof(GetProductById),
                new { id = product.Id },
                new ApiResponse<ProductDto>
                {
                    Success = true,
                    Data = product,
                    Message = "Producto creado exitosamente"
                });
            }
            catch (Exception ex)
            {

                return BadRequest(new ApiResponse<ProductDto>
                {
                    Success = false,
                    Message = "Error al crear producto",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpGet("all-products")]
        public async Task<ActionResult<ApiResponse<List<ProductDto>>>> GetAllProducts()
        {

            try
            {
                var product = await _productServices.GetAllProductsAsync();

                return Ok(new ApiResponse<List<ProductDto>>
                {
                    Success = true,
                    Data = product
                });
            }
            catch (Exception ex)
            {

                return BadRequest(new ApiResponse<List<ProductDto>>
                {
                    Success = false,
                    Message = "Error al obtener todos los productos",
                    Errors = new List<string> { ex.Message }
                });
            }

        }

        [HttpGet("product-by-id/{id}")]
        public async Task<ActionResult<ApiResponse<ProductDto>>> GetProductById(int id)
        {
            try
            {
                var product = await _productServices.GetProductByIdAsync(id);

                return Ok(new ApiResponse<ProductDto>
                {

                    Success = true,
                    Data = product
                });
            }
            catch (Exception ex)
            {

                return BadRequest(new ApiResponse<ProductDto>
                {
                    Success = false,
                    Message = "Producto no encontrado",
                    Errors = new List<string> { ex.Message }
                });
            }
        }
    }
}
