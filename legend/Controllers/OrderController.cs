namespace legend.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using legend.Authorization;
    using legend.Services;
    using legend.Entities;
    using legend.Entities.Enums;

    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("place-order")]
        public async Task<IActionResult> PlaceOrder([FromBody] Order order)
        {
            // Get the current user from the JWT token
            var user = HttpContext.GetUserIdFromContext();

            // Create an order
            var orderId = await _orderService.PlaceOrder(user.Id, order);

            return Ok(new { OrderId = orderId });
        }

        [HttpPost("update-order-status")]
        public async Task<IActionResult> UpdateOrderStatus(Guid orderId)
        {

            await _orderService.UpdateOrderStatusAsync(orderId, OrderStatus.Processed);

            return Ok(new { Message = "Order status updated successfully" });
        }

    }
}
