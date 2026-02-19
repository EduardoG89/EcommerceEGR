using Ecommerce.DTOs.Common;
using Ecommerce.DTOs.Orders;
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
    }
}
