using Ecommerce.DTOs.Products;

namespace Ecommerce.Services.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllProductsAsync();
        Task<List<ProductDto>> GetProductsByCategoryAsync(int categoryId);
        Task<ProductDto> GetProductByIdAsync(int id);
        Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto);
        Task<ProductDto> UpdateProductAsync(UpdateProductDto updateProductDto);
        Task<bool> DeleteProductAsync(int id);
        Task<ProductVariantDto> AddVariantAsync(CreateProductVariantDto variantDto);
        Task<bool> UpdateStockAsync(int variantId, int quanty);
    }
}
