using Ecommerce.DTOs.Categories;
using Ecommerce.DTOs.Common;
using Ecommerce.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<CategoryDto>>>> GetAllCategories()
        {
            try
            {
                var categories = await _categoryService.GetAllCategoriesAsync();

                return Ok(new ApiResponse<List<CategoryDto>>
                {
                    Success = true,
                    Data = categories
                });
            }
            catch (Exception ex)
            {

                return BadRequest(new ApiResponse<List<CategoryDto>>
                {
                    Success = false,
                    Message = "Error al obtener las categorias",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<CategoryDto>>> GetCategoryById(int id)
        {
            try
            {
                var category = await _categoryService.GetCategoryByIdAsync(id);

                return Ok(new ApiResponse<CategoryDto>
                {
                    Success = true,
                    Data = category
                });
            }
            catch (Exception ex)
            {

                return BadRequest(new ApiResponse<CategoryDto>
                {
                    Success = false,
                    Message = "Error al obtener la categoria",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<CategoryDto>>> CreateCategory([FromBody] CreateCategoryDto createCategoryDto)
        {
            try
            {
                var category = await _categoryService.CreateCategoryAsync(createCategoryDto);

                return CreatedAtAction(
                    nameof(GetCategoryById),
                    new { id = category.Id },
                    new ApiResponse<CategoryDto>
                    {
                        Success = true,
                        Data = category,
                        Message = "Categoria creada exitosamente"
                    });
            }
            catch (Exception ex)
            {

                return BadRequest(new ApiResponse<CategoryDto>
                {
                    Success = false,
                    Message = "Error al crear categoria",
                    Errors = new List<string> { ex.Message }
                });
            }
        }
    }
}
