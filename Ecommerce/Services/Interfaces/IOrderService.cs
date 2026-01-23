using Ecommerce.DTOs.Orders;

namespace Ecommerce.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDto> CreateAsync(CreateOrderDto createOrderDto);
        Task<OrderDto> GetOrderByIdAsync(int orderId);
        Task<List<OrderDto>> GetOrdersByUserIdAsync(int userId);
        Task<List<OrderDto>> GetAllOrdersAsync();
        Task<bool> UpdateOrderStatusAsync(UpdateOrderStatusDto updateOrderStatusDto);
        Task<bool> CancelOrderAsync(int orderId);
    }
}
