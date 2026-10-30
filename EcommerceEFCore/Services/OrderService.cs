using ECommerceEFCore.Models;
using ECommerceEFCore.Repositories;

namespace ECommerceEFCore.Services;

public class OrderService
{
    private readonly OrderRepository _orderRepository;

    public OrderService()
    {
        _orderRepository = new OrderRepository();
    }

    private static readonly string[] AllowedStatuses =
    {
        "Pending",
        "Processing",
        "Shipped",
        "Delivered",
        "Cancelled"
    };

    public async Task<int> AddAsync(Order order)
    {
        ValidateOrder(order);

        order.OrderStatus = "Pending";

        return await _orderRepository.AddAsync(order);
    }

    public async Task<List<Order>> GetAllAsync()
    {
        return await _orderRepository.GetAllAsync();
    }

    public async Task<Order?> GetByIdAsync(int id)
    {
        ValidateId(id);

        return await _orderRepository.GetByIdAsync(id);
    }

    public async Task<bool> UpdateAsync(
        int id,
        string orderStatus,
        decimal totalAmount)
    {
        ValidateId(id);
        ValidateOrderDetails(orderStatus, totalAmount);

        return await _orderRepository.UpdateAsync(
            id,
            orderStatus.Trim(),
            totalAmount);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        ValidateId(id);

        return await _orderRepository.DeleteAsync(id);
    }

    private static void ValidateOrder(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);

        if (order.CustomerId <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(order.CustomerId),
                "Customer ID must be greater than zero.");

        if (order.TotalAmount < 0)
            throw new ArgumentOutOfRangeException(
                nameof(order.TotalAmount),
                "Total amount cannot be negative.");

        ValidateOrderDetails(order.OrderStatus, order.TotalAmount);
    }

    private static void ValidateOrderDetails(
        string orderStatus,
        decimal totalAmount)
    {
        if (string.IsNullOrWhiteSpace(orderStatus))
            throw new ArgumentException("Order status is required.");

        if (!AllowedStatuses.Contains(
                orderStatus.Trim(),
                StringComparer.OrdinalIgnoreCase))
        {
            throw new ArgumentException(
                "Invalid order status. Allowed values: Pending, Processing, Shipped, Delivered, Cancelled.");
        }

        if (totalAmount < 0)
            throw new ArgumentOutOfRangeException(
                nameof(totalAmount),
                "Total amount cannot be negative.");
    }

    private static void ValidateId(int id)
    {
        if (id <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(id),
                "Order ID must be greater than zero.");
    }
}