using Ecommerce.Data;
using Ecommerce.DTOs.Categories;
using Ecommerce.Services.Interfaces;

namespace Ecommerce.Services.Implementations
{
    public class CategoryServices : ICategoryService
    {
        private readonly AppDbContext _context;

        public CategoryServices(AppDbContext context)
        {
            _context = context;
        }
        public Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto createCategoryDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteCategoryAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<CategoryDto>> GetAllCategoriesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<CategoryDto> GetCategoryByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
