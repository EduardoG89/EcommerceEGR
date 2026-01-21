namespace Ecommerce.DTOs.Products
{
    public class ProductImageDto
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public int Order { get; set; }
        public bool IsPrimary { get; set; }
    }
}
