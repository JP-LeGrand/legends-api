using legend.Entities;
using legend.Entities.Enums;
using legend.Helpers;
using Microsoft.EntityFrameworkCore;

namespace legend.Services
{
    public interface IOrderService
    {

        Task<Guid> PlaceOrder(Guid userId, Order orderItems);

        Task UpdateOrderStatusAsync(Guid orderId, OrderStatus newStatus);

        IEnumerable<Order> GetUserOrders(Guid userId);
    }

    public class OrderService : IOrderService
    {
        private DataContext _context;

        public OrderService(DataContext context)
        {
            _context = context;
        }

        public async Task<Guid> PlaceOrder(Guid userId, Order order)
        {
            // Update the order with additional information
            order.UserId = userId;
            order.OrderDate = DateTime.UtcNow;
            order.Status = OrderStatus.Pending;

            // Add the order to the context
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return order.OrderId;
        }

        public async Task UpdateOrderStatusAsync(Guid orderId, OrderStatus newStatus)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                throw new KeyNotFoundException("Order not found");
            }

            order.Status = newStatus;
            await _context.SaveChangesAsync();
        }

        private async Task<List<OrderItem>> ValidateOrderItemsAsync(List<OrderItem> orderItems)
        {
            var validatedOrderItems = new List<OrderItem>();

            foreach (var item in orderItems)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product == null)
                {
                    throw new KeyNotFoundException($"Product with ID {item.ProductId} not found");
                }

                if (product.StockQuantity < item.Quantity)
                {
                    throw new InvalidOperationException($"Insufficient stock for product {product.Name}");
                }

                validatedOrderItems.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = product.Price // Ensure price is taken from the database
                });
            }

            return validatedOrderItems;
        }

        private async Task UpdateStockQuantityAsync(Guid productId, int quantity)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                throw new KeyNotFoundException($"Product with ID {productId} not found");
            }

            product.StockQuantity -= quantity;
            await _context.SaveChangesAsync();
        }

        public IEnumerable<Order> GetUserOrders(Guid userId)
        {
            return _context.Orders
                .Include(order => order.shippingDetails)
                .Include(order => order.OrderItems)
                .Where(order => order.UserId == userId && order.Status == OrderStatus.Processed);
        }
    }
}
