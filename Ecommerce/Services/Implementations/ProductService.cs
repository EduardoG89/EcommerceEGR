using Ecommerce.Data;
using Ecommerce.DTOs.Products;
using Ecommerce.Models;
using Ecommerce.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ProductVariantDto> AddVariantAsync(CreateProductVariantDto variantDto)
        {
            var product = await _context.Products.FindAsync(variantDto.ProductId);

            if (product == null)
                throw new Exception("Producto no encontrado");

            var variant = new ProductVariant()
            {
                ProductId = product.Id,
                Sku = variantDto.Sku,
                Color = variantDto.Color,
                PhoneModel = variantDto.PhoneModel,
                Material = variantDto.Material,
                Stock = variantDto.Stock,
                PriceAdjustment = variantDto.PriceAdjusment,
                IsActive = true
            };

            _context.ProductVariants.Add(variant);
            await _context.SaveChangesAsync();

            return MapProductVariantDto(variant, product.Price);
        }

        public async Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto)
        {
            var product = new Product
            {
                Name = createProductDto.Name,
                Description = createProductDto.Description,
                Price = createProductDto.Price,
                CategoryId = createProductDto.CategoryId,
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return MapProductDto(product);
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return false;

            product.IsActive = false;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<ProductDto>> GetAllProductsAsync()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Include(p => p.Variants)
                .Where(p => p.IsActive)
                .ToListAsync();

            return products.Select(p => MapProductDto(p)).ToList();
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Include(p => p.Variants)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                throw new Exception("El producto no existe");

            return MapProductDto(product);

        }

        public async Task<List<ProductDto>> GetProductsByCategoryAsync(int categoryId)
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Include(p => p.Variants)
                .Where(p => p.CategoryId == categoryId && p.IsActive)
                .ToListAsync();

            return products.Select(p => MapProductDto(p)).ToList();
        }

        public async Task<ProductDto> UpdateProductAsync(UpdateProductDto updateProductDto)
        {
            var product = await _context.Products.FindAsync(updateProductDto.Id);

            if (product == null)
                throw new Exception("El producto no existe");

            product.Name = updateProductDto.Name;
            product.Description = updateProductDto.Description;
            product.Price = updateProductDto.Price;
            product.CategoryId = updateProductDto.CategoryId;
            product.IsActive = updateProductDto.IsActive;
            product.UpdateAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return await GetProductByIdAsync(product.Id);
        }

        public async Task<bool> UpdateStockAsync(int variantId, int quanty)
        {
            var variant = await _context.ProductVariants.FindAsync(variantId);

            if (variant == null)
                return false;

            variant.Stock = quanty;

            await _context.SaveChangesAsync();

            return true;
        }

        private ProductDto MapProductDto(Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId,
                CategoryName = product.Category.Name,
                IsActive = product.IsActive,
                Images = product.Images?.Select(i => new ProductImageDto
                {
                    Id = i.Id,
                    ImageUrl = i.ImageUrl,
                    Order = i.Order,
                    IsPrimary = i.IsPrimary
                }).OrderBy(i => i.Order).ToList() ?? new List<ProductImageDto>(),
                Variants = product.Variants?.Select(v => MapProductVariantDto(v, product.Price)).ToList() ?? new List<ProductVariantDto>(),
            };
        }

        private ProductVariantDto MapProductVariantDto(ProductVariant variant, decimal basePrice)
        {
            return new ProductVariantDto
            {
                Id = variant.Id,
                ProductId = variant.ProductId,
                Sku = variant.Sku,
                Color = variant.Color,
                Size = variant.Size,
                PhoneModel = variant.PhoneModel,
                Material = variant.Material,
                Stock = variant.Stock,
                PriceAdjustment = variant.PriceAdjustment,
                IsActive = variant.IsActive,
                FinalPrice = basePrice + (variant.PriceAdjustment ?? 0m),
            };
        }
    }
}
