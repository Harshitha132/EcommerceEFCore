using ECommerceEFCore.Helpers;
using ECommerceEFCore.Models;
using ECommerceEFCore.Services;

namespace ECommerceEFCore.Menus;

public class OrderMenu
{
    private readonly OrderService _orderService;
    private readonly OrderItemMenu _orderItemMenu;

    public OrderMenu(
        OrderService orderService,
        OrderItemMenu orderItemMenu)
    {
        _orderService = orderService;
        _orderItemMenu = orderItemMenu;
    }

    public async Task ShowAsync()
    {
        while (true)
        {
            Console.WriteLine("\n===== ORDER MANAGEMENT =====");
            Console.WriteLine("1. Add Order");
            Console.WriteLine("2. View All Orders");
            Console.WriteLine("3. View Order By ID");
            Console.WriteLine("4. Update Order");
            Console.WriteLine("5. Delete Order");
            Console.WriteLine("6. Manage Order Items");
            Console.WriteLine("0. Back to Main Menu");

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

                case 6:
                    await _orderItemMenu.ShowAsync();
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
        Console.WriteLine("\n===== ADD ORDER =====");

        int customerId = ConsoleInputHelper.ReadPositiveInt(
            "Enter customer ID: ");

        var order = new Order
        {
            CustomerId = customerId,
            OrderDate = DateTime.Now,
            TotalAmount = 0,
            OrderStatus = "Pending"
        };

        try
        {
            int id = await _orderService.AddAsync(order);

            Console.WriteLine($"Order created successfully. Order ID: {id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating order: {ex.Message}");
        }
    }

    private async Task GetAllAsync()
    {
        Console.WriteLine("\n===== ALL ORDERS =====");

        try
        {
            var orders = await _orderService.GetAllAsync();

            if (!orders.Any())
            {
                Console.WriteLine("No orders found.");
                return;
            }

            foreach (var order in orders)
            {
                DisplayOrder(order);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving orders: {ex.Message}");
        }
    }

    private async Task GetByIdAsync()
    {
        Console.WriteLine("\n===== SEARCH ORDER =====");

        int id = ConsoleInputHelper.ReadPositiveInt("Enter order ID: ");

        try
        {
            var order = await _orderService.GetByIdAsync(id);

            if (order is null)
            {
                Console.WriteLine("Order not found.");
                return;
            }

            DisplayOrder(order);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving order: {ex.Message}");
        }
    }

    private async Task UpdateAsync()
    {
        Console.WriteLine("\n===== UPDATE ORDER =====");

        int id = ConsoleInputHelper.ReadPositiveInt("Enter order ID: ");

        try
        {
            var existingOrder = await _orderService.GetByIdAsync(id);

            if (existingOrder is null)
            {
                Console.WriteLine("Order not found.");
                return;
            }

            DisplayOrder(existingOrder);

            Console.WriteLine("\nAvailable statuses:");
            Console.WriteLine("1. Pending");
            Console.WriteLine("2. Processing");
            Console.WriteLine("3. Shipped");
            Console.WriteLine("4. Delivered");
            Console.WriteLine("5. Cancelled");

            int statusChoice = ConsoleInputHelper.ReadInt(
                "Select new order status: ");

            string status = statusChoice switch
            {
                1 => "Pending",
                2 => "Processing",
                3 => "Shipped",
                4 => "Delivered",
                5 => "Cancelled",
                _ => string.Empty
            };

            if (string.IsNullOrEmpty(status))
            {
                Console.WriteLine("Invalid status selection.");
                return;
            }

            decimal totalAmount = ConsoleInputHelper.ReadNonNegativeDecimal(
                "Enter total amount: ");

            bool updated = await _orderService.UpdateAsync(
                id,
                status,
                totalAmount);

            Console.WriteLine(
                updated
                    ? "Order updated successfully."
                    : "Order could not be updated.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating order: {ex.Message}");
        }
    }

    private async Task DeleteAsync()
    {
        Console.WriteLine("\n===== DELETE ORDER =====");

        int id = ConsoleInputHelper.ReadPositiveInt("Enter order ID: ");

        try
        {
            var order = await _orderService.GetByIdAsync(id);

            if (order is null)
            {
                Console.WriteLine("Order not found.");
                return;
            }

            DisplayOrder(order);

            bool confirm = ConsoleInputHelper.ReadConfirmation(
                "Are you sure you want to delete this order? (y/n): ");

            if (!confirm)
            {
                Console.WriteLine("Deletion cancelled.");
                return;
            }

            bool deleted = await _orderService.DeleteAsync(id);

            Console.WriteLine(
                deleted
                    ? "Order deleted successfully."
                    : "Order could not be deleted.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting order: {ex.Message}");
        }
    }

    private static void DisplayOrder(Order order)
    {
        Console.WriteLine("-----------------------------------");
        Console.WriteLine($"Order ID     : {order.OrderId}");
        Console.WriteLine($"Customer ID  : {order.CustomerId}");
        Console.WriteLine($"Customer     : {order.Customer?.FirstName} {order.Customer?.LastName}");
        Console.WriteLine($"Order Date   : {order.OrderDate}");
        Console.WriteLine($"Total Amount : {order.TotalAmount}");
        Console.WriteLine($"Status       : {order.OrderStatus}");
        Console.WriteLine($"Order Items  : {order.OrderItems?.Count ?? 0}");
        Console.WriteLine("-----------------------------------");
    }
}