using Ecommerce.DTOs.Common;
using Ecommerce.DTOs.Orders;
using Ecommerce.Models;
using Ecommerce.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }


        [HttpPost]
        public async Task<ActionResult<ApiResponse<OrderDto>>> CreateOrder([FromBody] CreateOrderDto createOrderDto)
        {
            try
            {
                var order = await _orderService.CreateOrderAsync(createOrderDto);

                return CreatedAtAction(
                    nameof(GetOrderById),
                    new { id = order.Id },
                    new ApiResponse<OrderDto>
                    {
                        Success = true,
                        Data = order,
                        Message = "Orden creado exitosamente"
                    });
            }
            catch (Exception ex)
            {

                return BadRequest(new ApiResponse<OrderDto>
                {
                    Success = false,
                    Message = "Error al crear la orden",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<OrderDto>>> GetOrderById(int id)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(id);

                return Ok(new ApiResponse<OrderDto>
                {
                    Success = true,
                    Data = order,
                });

            }
            catch (Exception ex)
            {

                return BadRequest(new ApiResponse<OrderDto>
                {
                    Success = false,
                    Message = "Error al obtener el orden",
                    Errors = new List<string> { ex.Message }
                });
            }

        }

        [HttpGet("order-by-user/{id}")]
        public async Task<ActionResult<ApiResponse<List<OrderDto>>>> GetOrderByUserId(int id)
        {
            try
            {
                var order = await _orderService.GetOrdersByUserIdAsync(id);

                return Ok(new ApiResponse<List<OrderDto>>
                {
                    Success = true,
                    Data = order
                });
            }
            catch (Exception ex)
            {

                return BadRequest(new ApiResponse<List<OrderDto>>
                {
                    Success = false,
                    Message = "Error al obtener la orden",
                    Errors = new List<string> { ex.Message }
                });
            }

        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<OrderDto>>>> GetAllOrders()
        {
            try
            {
                var order = await _orderService.GetAllOrdersAsync();

                return Ok(new ApiResponse<List<OrderDto>>
                {
                    Success = true,
                    Data = order
                });
            }
            catch (Exception ex)
            {

                return BadRequest(new ApiResponse<List<OrderDto>>
                {
                    Success = false,
                    Message = "Error al obtener las ordenes",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpPut]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateOrderStatus(int id, [FromBody] UpdateOrderStatusDto updateOrderStatusDto)
        {
            try
            {
                if (id != updateOrderStatusDto.OrderId)
                {
                    return BadRequest(new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Id de la orden no coincide"
                    });
                }

                var order = await _orderService.UpdateOrderStatusAsync(updateOrderStatusDto);

                return Ok(new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Estatus actualizado"
                });
            }
            catch (Exception ex)
            {

                return BadRequest(new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Error al actualizar el estatus",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> CancelOrder(int id)
        {
            try
            {
                var result = await _orderService.CancelOrderAsync(id);

                return Ok(new ApiResponse<bool>
                {
                    Success = false,
                    Data = result,
                    Message = "Orden cancalada exitosamente"
                });
            }
            catch (Exception ex)
            {

                return BadRequest(new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Error al cancelar la orden",
                    Errors = new List<string> { ex.Message }
                });
            }
        }
    }
}
