using Ecommerce.DTOs.Common;
using Ecommerce.DTOs.Products;
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

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<ProductDto>>>> GetAllProducts()
        {

            try
            {
                var result = await _productServices.GetAllProductsAsync();

                return Ok(new ApiResponse<List<ProductDto>>
                {
                    Success = true,
                    Data = result
                });
            }
            catch (Exception ex)
            {

                return BadRequest(new ApiResponse<List<ProductDto>>
                {
                    Success = false,
                    Data = [],
                    Message = ex.Message
                });
            }

        }
    }
}
