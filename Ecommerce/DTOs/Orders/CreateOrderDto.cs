
namespace Ecommerce.DTOs.Orders
{
    public class CreateOrderDto
    {
        public int? UserId { get; set; }
        public List<OrderItemDto> Items { get; set; }
        public ShippingAddressDto ShippingAddress { get; set; }
        public PaymentDto Payment { get; set; }
    }
}
