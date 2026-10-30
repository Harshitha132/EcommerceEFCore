using ECommerceEFCore.Models;
using ECommerceEFCore.Repositories;

namespace ECommerceEFCore.Services;

public class OrderItemService
{
    private readonly OrderItemRepository _orderItemRepository;

    public OrderItemService()
    {
        _orderItemRepository = new OrderItemRepository();
    }

    public async Task<int> AddAsync(OrderItem orderItem)
    {
        ValidateOrderItem(orderItem);

        orderItem.Subtotal =
            orderItem.Quantity * orderItem.UnitPrice;

        return await _orderItemRepository.AddAsync(orderItem);
    }

    public async Task<List<OrderItem>> GetAllAsync()
    {
        return await _orderItemRepository.GetAllAsync();
    }

    public async Task<OrderItem?> GetByIdAsync(int id)
    {
        ValidateId(id);

        return await _orderItemRepository.GetByIdAsync(id);
    }

    public async Task<bool> UpdateAsync(
        int id,
        int quantity,
        decimal unitPrice)
    {
        ValidateId(id);
        ValidateDetails(quantity, unitPrice);

        return await _orderItemRepository.UpdateAsync(
    id,
    quantity,
    unitPrice);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        ValidateId(id);

        return await _orderItemRepository.DeleteAsync(id);
    }

    private static void ValidateOrderItem(OrderItem orderItem)
    {
        ArgumentNullException.ThrowIfNull(orderItem);

        if (orderItem.OrderId <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(orderItem.OrderId),
                "Order ID must be greater than zero.");

        if (orderItem.ProductId <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(orderItem.ProductId),
                "Product ID must be greater than zero.");

        ValidateDetails(
            orderItem.Quantity,
            orderItem.UnitPrice);
    }

    private static void ValidateDetails(
        int quantity,
        decimal unitPrice)
    {
        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(quantity),
                "Quantity must be greater than zero.");

        if (unitPrice < 0)
            throw new ArgumentOutOfRangeException(
                nameof(unitPrice),
                "Unit price cannot be negative.");
    }

    private static void ValidateId(int id)
    {
        if (id <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(id),
                "Order item ID must be greater than zero.");
    }
}