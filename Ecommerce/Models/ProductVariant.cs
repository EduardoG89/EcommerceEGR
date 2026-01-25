namespace Ecommerce.Models
{
    public class ProductVariant
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Sku { get; set; }
        public string Color { get; set; }
        public string PhoneModel { get; set; }
        public string Material { get; set; }
        public int Stock { get; set; }
        public string Size { get; set; }
        public decimal? PriceAdjustment { get; set; }
        public bool IsActive { get; set; }
        public Product Product { get; set; }
    }
}
