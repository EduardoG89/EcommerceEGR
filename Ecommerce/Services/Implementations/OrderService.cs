using Ecommerce.Data;
using Ecommerce.DTOs.Orders;
using Ecommerce.Models;
using Ecommerce.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Services.Implementations
{
    public class OrderService : IOrderService
    {

        private readonly AppDbContext _context;

        public OrderService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CancelOrderAsync(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.ProductVariant)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                return false;
            }

            if (order.Status != "Pending")
            {
                throw new Exception("Solo se pueden cancelar órdenes pendientes");
            }

            foreach (var item in order.OrderItems)
            {
                item.ProductVariant.Stock += item.Quantity;
            }

            order.Status = "Cancelled";
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<OrderDto> CreateOrderAsync(CreateOrderDto createOrderDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                foreach (var item in createOrderDto.Items)
                {
                    var variant = await _context.ProductVariants.FindAsync(item.ProductVariantId);

                    if (variant == null || !variant.IsActive)
                    {
                        throw new Exception($"Product variant {item.ProductVariantId} no disponible");
                    }

                    if (variant.Stock < item.Quanty)
                    {
                        throw new Exception($"Stock insuficiente para {item.ProductName}");
                    }

                }

                decimal subtotal = 0;
                var orderItems = new List<OrderItem>();

                foreach (var item in createOrderDto.Items)
                {
                    var variant = await _context.ProductVariants
                        .Include(v => v.Product)
                        .FirstOrDefaultAsync(v => v.Id == item.ProductVariantId);

                    var unitPrice = variant.Product.Price + (variant.PriceAdjustment ?? 0);
                    var totalPrice = unitPrice * item.Quanty;
                    subtotal += totalPrice;

                    orderItems.Add(new OrderItem
                    {
                        ProductVariantId = item.ProductVariantId,
                        ProductName = variant.Product.Name,
                        VariantDetails = $"{variant.Color} - {variant.Size} - {variant.PhoneModel}".Trim(' ', '-'),
                        Quantity = item.Quanty,
                        UnitPrice = unitPrice,
                        TotalPrice = totalPrice
                    });

                    variant.Stock -= item.Quanty;
                }

                decimal shippingCost = CalculateShippingCost(createOrderDto.ShippingAddress);

                var order = new Order
                {
                    UserId = createOrderDto.UserId ?? 0, 
                    OrderNumber = GenerateOrderNumber(),
                    Subtotal = subtotal,
                    ShippingCost = shippingCost,
                    Total = subtotal + shippingCost,
                    Status = "Pending",
                    CreatedAt = DateTime.Now,
                    OrderItems = orderItems
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();


                var shippingAddress = new ShippingAddress
                {
                    OrderId = order.Id,
                    UserId = createOrderDto.UserId,
                    FullName = createOrderDto.ShippingAddress.FullName,
                    PhoneNumber = createOrderDto.ShippingAddress.PhoneNumber,
                    Street = createOrderDto.ShippingAddress.Street,
                    Number = createOrderDto.ShippingAddress.Number,
                    AdditionalInfo = createOrderDto.ShippingAddress.AdditionalInfo,
                    City = createOrderDto.ShippingAddress.City,
                    State = createOrderDto.ShippingAddress.State,
                    PostalCode = createOrderDto.ShippingAddress.PostalCode,
                    Country = createOrderDto.ShippingAddress.Country,
                    IsDefault = false
                };

                _context.ShippingAddresses.Add(shippingAddress);

                var payment = new Payment
                {
                    OrderId = order.Id,
                    PaymentMethod = createOrderDto.Payment.PaymentMethod,
                    TransactionId = createOrderDto.Payment.TransactionId,
                    Amount = order.Total,
                    Status = "Pending",
                    CreatedAt = DateTime.Now
                };

                _context.Payments.Add(payment);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return await GetOrderByIdAsync(order.Id);

            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<List<OrderDto>> GetAllOrdersAsync()
        {
            var orders = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .Include(o => o.ShippingAddress)
                .Include(o => o.Payment)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            return orders.Select(o => MapToOrderDto(o)).ToList();
        }

        public async Task<OrderDto> GetOrderByIdAsync(int orderId)
        {
            var order = await _context.Orders
               .Include(o => o.User)
               .Include(o => o.OrderItems)
                   .ThenInclude(oi => oi.ProductVariant)
               .Include(o => o.ShippingAddress)
               .Include(o => o.Payment)
               .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                throw new Exception("Orden no encontrada");
            }

            return MapToOrderDto(order);
        }

        public async Task<List<OrderDto>> GetOrdersByUserIdAsync(int userId)
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .Include(o => o.ShippingAddress)
                .Include(o => o.Payment)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            return orders.Select(o => MapToOrderDto(o)).ToList();
        }

        public async Task<bool> UpdateOrderStatusAsync(UpdateOrderStatusDto updateOrderStatusDto)
        {
            var order = await _context.Orders.FindAsync(updateOrderStatusDto.OrderId);

            if (order == null)
            {
                return false;
            }

            order.Status = updateOrderStatusDto.Status;

            if (updateOrderStatusDto.Status == "Shipped")
            {
                order.ShippedAt = DateTime.Now;
            }
            else if (updateOrderStatusDto.Status == "Delivered")
            {
                order.DeliveredAt = DateTime.Now;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        private string GenerateOrderNumber()
        {
            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            var random = new Random().Next(1000, 9999);
            return $"ORD-{timestamp}-{random}";
        }

        private decimal CalculateShippingCost(ShippingAddressDto address)
        {
            return 50m;
        }

        private OrderDto MapToOrderDto(Order order)
        {
            return new OrderDto
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                Subtotal = order.Subtotal,
                ShippingCost = order.ShippingCost,
                Total = order.Total,
                Status = order.Status,
                CreatedAt = order.CreatedAt,
                ShippedAt = order.ShippedAt,
                DeliveredAt = order.DeliveredAt,
                Items = order.OrderItems?.Select(oi => new OrderItemDto
                {
                    ProductVariantId = oi.ProductVariantId,
                    ProductName = oi.ProductName,
                    VariantDetails = oi.VariantDetails,
                    Quanty = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                    TotalPrice = oi.TotalPrice
                }).ToList() ?? new List<OrderItemDto>(),
                ShippingAddress = order.ShippingAddress != null ? new ShippingAddressDto
                {
                    FullName = order.ShippingAddress.FullName,
                    PhoneNumber = order.ShippingAddress.PhoneNumber,
                    Street = order.ShippingAddress.Street,
                    Number = order.ShippingAddress.Number,
                    AdditionalInfo = order.ShippingAddress.AdditionalInfo,
                    City = order.ShippingAddress.City,
                    State = order.ShippingAddress.State,
                    PostalCode = order.ShippingAddress.PostalCode,
                    Country = order.ShippingAddress.Country
                } : null,
                Payment = order.Payment != null ? new PaymentDto
                {
                    PaymentMethod = order.Payment.PaymentMethod,
                    TransactionId = order.Payment.TransactionId,
                    Amount = order.Payment.Amount,
                    Status = order.Payment.Status
                } : null
            };
        }
    }
}
