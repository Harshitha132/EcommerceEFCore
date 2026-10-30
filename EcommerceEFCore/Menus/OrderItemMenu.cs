using ECommerceEFCore.Helpers;
using ECommerceEFCore.Models;
using ECommerceEFCore.Services;

namespace ECommerceEFCore.Menus;

public class OrderItemMenu
{
    private readonly OrderItemService _service;

    public OrderItemMenu(OrderItemService service)
    {
        _service = service;
    }

    public async Task ShowAsync()
    {
        while (true)
        {
            Console.WriteLine("\n===== ORDER ITEM MANAGEMENT =====");
            Console.WriteLine("1. Add Order Item");
            Console.WriteLine("2. View All Order Items");
            Console.WriteLine("3. View Order Item By ID");
            Console.WriteLine("4. Update Order Item");
            Console.WriteLine("5. Delete Order Item");
            Console.WriteLine("0. Back to Order Menu");

            int choice = ConsoleInputHelper.ReadInt("\nEnter your choice: ");

            switch (choice)
            {
                case 1:
                    await AddAsync();
                    break;

                case 2:
                    await GetAllAsync();
                    break;

                case 3:
                    await GetByIdAsync();
                    break;

                case 4:
                    await UpdateAsync();
                    break;

                case 5:
                    await DeleteAsync();
                    break;

                case 0:
                    return;

                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    private async Task AddAsync()
    {
        Console.WriteLine("\n===== ADD ORDER ITEM =====");

        int orderId = ConsoleInputHelper.ReadPositiveInt(
            "Enter order ID: ");

        int productId = ConsoleInputHelper.ReadPositiveInt(
            "Enter product ID: ");

        int quantity = ConsoleInputHelper.ReadPositiveInt(
            "Enter quantity: ");

        decimal unitPrice = ConsoleInputHelper.ReadNonNegativeDecimal(
            "Enter unit price: ");

        var orderItem = new OrderItem
        {
            OrderId = orderId,
            ProductId = productId,
            Quantity = quantity,
            UnitPrice = unitPrice,
            Subtotal = quantity * unitPrice
        };

        try
        {
            int id = await _service.AddAsync(orderItem);

            Console.WriteLine($"Order item added successfully. Order Item ID: {id}");
            Console.WriteLine($"Subtotal: {orderItem.Subtotal}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding order item: {ex.Message}");
        }
    }

    private async Task GetAllAsync()
    {
        Console.WriteLine("\n===== ALL ORDER ITEMS =====");

        try
        {
            var orderItems = await _service.GetAllAsync();

            if (!orderItems.Any())
            {
                Console.WriteLine("No order items found.");
                return;
            }

            foreach (var item in orderItems)
            {
                DisplayOrderItem(item);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving order items: {ex.Message}");
        }
    }

    private async Task GetByIdAsync()
    {
        Console.WriteLine("\n===== SEARCH ORDER ITEM =====");

        int id = ConsoleInputHelper.ReadPositiveInt(
            "Enter order item ID: ");

        try
        {
            var item = await _service.GetByIdAsync(id);

            if (item is null)
            {
                Console.WriteLine("Order item not found.");
                return;
            }

            DisplayOrderItem(item);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving order item: {ex.Message}");
        }
    }

    private async Task UpdateAsync()
    {
        Console.WriteLine("\n===== UPDATE ORDER ITEM =====");

        int id = ConsoleInputHelper.ReadPositiveInt(
            "Enter order item ID: ");

        try
        {
            var existingItem = await _service.GetByIdAsync(id);

            if (existingItem is null)
            {
                Console.WriteLine("Order item not found.");
                return;
            }

            DisplayOrderItem(existingItem);

            int quantity = ConsoleInputHelper.ReadPositiveInt(
                "Enter new quantity: ");

            decimal unitPrice = ConsoleInputHelper.ReadNonNegativeDecimal(
                "Enter new unit price: ");

            bool updated = await _service.UpdateAsync(
                id,
                quantity,
                unitPrice);

            Console.WriteLine(
                updated
                    ? "Order item updated successfully."
                    : "Order item could not be updated.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating order item: {ex.Message}");
        }
    }

    private async Task DeleteAsync()
    {
        Console.WriteLine("\n===== DELETE ORDER ITEM =====");

        int id = ConsoleInputHelper.ReadPositiveInt(
            "Enter order item ID: ");

        try
        {
            var item = await _service.GetByIdAsync(id);

            if (item is null)
            {
                Console.WriteLine("Order item not found.");
                return;
            }

            DisplayOrderItem(item);

            bool confirm = ConsoleInputHelper.ReadConfirmation(
                "Are you sure you want to delete this order item? (y/n): ");

            if (!confirm)
            {
                Console.WriteLine("Deletion cancelled.");
                return;
            }

            bool deleted = await _service.DeleteAsync(id);

            Console.WriteLine(
                deleted
                    ? "Order item deleted successfully."
                    : "Order item could not be deleted.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting order item: {ex.Message}");
        }
    }

    private static void DisplayOrderItem(OrderItem item)
    {
        Console.WriteLine("-----------------------------------");
        Console.WriteLine($"Order Item ID : {item.OrderItemId}");
        Console.WriteLine($"Order ID      : {item.OrderId}");
        Console.WriteLine($"Product ID    : {item.ProductId}");
        Console.WriteLine($"Product Name  : {item.Product?.ProductName}");
        Console.WriteLine($"Quantity      : {item.Quantity}");
        Console.WriteLine($"Unit Price    : {item.UnitPrice}");
        Console.WriteLine($"Subtotal      : {item.Subtotal}");
        Console.WriteLine("-----------------------------------");
    }
}