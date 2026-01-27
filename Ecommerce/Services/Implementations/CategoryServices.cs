using Ecommerce.Data;
using Ecommerce.DTOs.Categories;
using Ecommerce.Models;
using Ecommerce.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Services.Implementations
{
    public class CategoryServices : ICategoryService
    {
        private readonly AppDbContext _context;

        public CategoryServices(AppDbContext context)
        {
            _context = context;
        }
        public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto createCategoryDto)
        {
            var caterogy = new Category
            {
                Name = createCategoryDto.Name,
                Description = createCategoryDto.Description,
                ImageUrl = createCategoryDto.ImageUrl,
                IsActive = true
            };

            _context.Categories.Add(caterogy);
            await _context.SaveChangesAsync();

            return new CategoryDto
            {
                Id = caterogy.Id,
                Name = caterogy.Name,
                Description = caterogy.Description,
                ImageUrl = caterogy.ImageUrl,
                IsActive = caterogy.IsActive,
                ProductCount = 0,
            };
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
                return false;

            if(category.Products.Any(p => p.IsActive))
                throw new Exception("No se puede eliminar una categoría con productos activos");

            category.IsActive = false;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _context.Categories
                .Include(c => c.Products)
                .Where(c => c.IsActive)
                .ToListAsync();

            return categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                ImageUrl = c.ImageUrl,
                IsActive = c.IsActive,
                ProductCount = c.Products.Count(p  => p.IsActive),
            }).ToList();
        }

        public async Task<CategoryDto> GetCategoryByIdAsync(int id)
        {
            var category = await _context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
                throw new Exception("Categoria no encontrada");

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                ImageUrl = category.ImageUrl,
                IsActive = category.IsActive,
                ProductCount = category.Products.Count(p => p.IsActive)
            };
        }
    }
}
