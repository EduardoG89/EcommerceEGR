namespace Ecommerce.DTOs.Orders
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public decimal Subtotal { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ShippedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public List<OrderItemDto> Items { get; set; }
        public ShippingAddressDto ShippingAddress { get; set; }
        public PaymentDto Payment { get; set; }
    }
}
