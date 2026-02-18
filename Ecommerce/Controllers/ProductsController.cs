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


        [HttpPost]
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

        [HttpGet]
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

        [HttpGet("{id}")]
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

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<ProductDto>>> UpdateProduct(int id, [FromBody] UpdateProductDto updateProductDto)
        {
            try
            {
                if (id != updateProductDto.Id)
                {
                    return BadRequest(new ApiResponse<ProductDto>
                    {
                        Success = false,
                        Message = "Id no coincide"
                    });
                }

                var product = await _productServices.UpdateProductAsync(updateProductDto);

                return Ok(new ApiResponse<ProductDto>
                {
                    Success = true,
                    Data = product,
                    Message = "Producto actualizado correctamente"
                });
            }
            catch (Exception ex)
            {

                return BadRequest(new ApiResponse<ProductDto>
                {
                    Success = false,
                    Message = "Error al actualizar el producto",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<ProductDto>>> DeleteProduct(int id)
        {
            try
            {
                var product = await _productServices.DeleteProductAsync(id);

                return Ok(new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Producto eliminado exitosamente"
                });
            }
            catch (Exception ex)
            {

                return BadRequest(new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Error al eliminar el producto",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpGet("category/{id}")]
        public async Task<ActionResult<ApiResponse<List<ProductDto>>>> GetProductByCategory(int id)
        {
            try
            {
                var products = await _productServices.GetProductsByCategoryAsync(id);

                return Ok(new ApiResponse<List<ProductDto>>
                {
                    Success = true,
                    Data = products
                });
            }
            catch (Exception ex)
            {

                return BadRequest(new ApiResponse<List<Product>>
                {
                    Success = false,
                    Message = "Error al obtener productos por categoria",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpPost("variants")]
        public async Task<ActionResult<ApiResponse<ProductVariantDto>>> AddVariant([FromBody] CreateProductVariantDto createProductVariantDto)
        {
            try
            {
                var variant = await _productServices.AddVariantAsync(createProductVariantDto);

                return Ok(new ApiResponse<ProductVariantDto>
                {
                    Success = true,
                    Message = "Variante creado exitosamente",
                    Data = variant
                });
            }
            catch (Exception ex)
            {

                return BadRequest(new ApiResponse<ProductVariantDto>
                {
                    Success = false,
                    Message = "Error al crear la variante",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpPatch("variant/{variantId}/stock")]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateStock(int variantId, [FromBody] int quantity)
        {
            try
            {
                var result = await _productServices.UpdateStockAsync(variantId, quantity);

                return Ok(new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Stock actualizado exitosamente",
                    Data = result
                });
            }
            catch (Exception ex)
            {

                return BadRequest(new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Error al actualizar stock",
                    Errors = new List<string> { ex.Message }
                });
            }
        }
    }
}
