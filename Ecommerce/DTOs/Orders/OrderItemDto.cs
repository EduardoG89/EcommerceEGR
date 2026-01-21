namespace Ecommerce.DTOs.Orders
{
    public class OrderItemDto
    {
        public int ProductVariantId { get; set; }
        public string ProductName { get; set; }
        public string VariantDetails { get; set; }
        public int Quanty { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
