namespace Ecommerce.DTOs.Products
{
    public class CreateProductVariantDto
    {
        public int ProductId { get; set; }
        public string Sku { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public string PhoneNumber { get; set; }
        public string Material { get; set; }
        public int Stock { get; set; }
        public decimal? PriceAdjusment { get; set; }
    }
}
