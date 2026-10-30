using ECommerceEFCore.Helpers;
using ECommerceEFCore.Models;
using ECommerceEFCore.Services;

namespace ECommerceEFCore.Menus;

public class CartItemMenu
{
    private readonly CartItemService _service;

    public CartItemMenu(CartItemService service)
    {
        _service = service;
    }

    public async Task ShowAsync()
    {
        while (true)
        {
            Console.WriteLine("\n===== CART ITEM MANAGEMENT =====");
            Console.WriteLine("1. Add Cart Item");
            Console.WriteLine("2. View All Cart Items");
            Console.WriteLine("3. View Cart Item By ID");
            Console.WriteLine("4. Update Cart Item");
            Console.WriteLine("5. Delete Cart Item");
            Console.WriteLine("0. Back to Cart Menu");

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
        Console.WriteLine("\n===== ADD CART ITEM =====");

        int cartId =
            ConsoleInputHelper.ReadPositiveInt("Enter cart ID: ");

        int productId =
            ConsoleInputHelper.ReadPositiveInt("Enter product ID: ");

        int quantity =
            ConsoleInputHelper.ReadPositiveInt("Enter quantity: ");

        var cartItem = new CartItem
        {
            CartId = cartId,
            ProductId = productId,
            Quantity = quantity,
            AddedAt = DateTime.Now
        };

        try
        {
            int id = await _service.AddAsync(cartItem);

            Console.WriteLine($"Cart item added successfully. Cart Item ID: {id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding cart item: {ex.Message}");
        }
    }

    private async Task GetAllAsync()
    {
        Console.WriteLine("\n===== ALL CART ITEMS =====");

        try
        {
            var cartItems = await _service.GetAllAsync();

            if (!cartItems.Any())
            {
                Console.WriteLine("No cart items found.");
                return;
            }

            foreach (var item in cartItems)
            {
                DisplayCartItem(item);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving cart items: {ex.Message}");
        }
    }

    private async Task GetByIdAsync()
    {
        Console.WriteLine("\n===== SEARCH CART ITEM =====");

        int id = ConsoleInputHelper.ReadPositiveInt("Enter cart item ID: ");

        try
        {
            var item = await _service.GetByIdAsync(id);

            if (item is null)
            {
                Console.WriteLine("Cart item not found.");
                return;
            }

            DisplayCartItem(item);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving cart item: {ex.Message}");
        }
    }

    private async Task UpdateAsync()
    {
        Console.WriteLine("\n===== UPDATE CART ITEM =====");

        int id = ConsoleInputHelper.ReadPositiveInt(
            "Enter cart item ID: ");

        try
        {
            var existingItem = await _service.GetByIdAsync(id);

            if (existingItem is null)
            {
                Console.WriteLine("Cart item not found.");
                return;
            }

            DisplayCartItem(existingItem);

            int quantity = ConsoleInputHelper.ReadPositiveInt(
                "Enter new quantity: ");

            bool updated = await _service.UpdateAsync(
                id,
                quantity);

            Console.WriteLine(
                updated
                    ? "Cart item updated successfully."
                    : "Cart item could not be updated.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating cart item: {ex.Message}");
        }
    }

    private async Task DeleteAsync()
    {
        Console.WriteLine("\n===== DELETE CART ITEM =====");

        int id = ConsoleInputHelper.ReadPositiveInt("Enter cart item ID: ");

        try
        {
            var item = await _service.GetByIdAsync(id);

            if (item is null)
            {
                Console.WriteLine("Cart item not found.");
                return;
            }

            DisplayCartItem(item);

            bool confirm = ConsoleInputHelper.ReadConfirmation(
                "Are you sure you want to delete this cart item? (y/n): ");

            if (!confirm)
            {
                Console.WriteLine("Deletion cancelled.");
                return;
            }

            bool deleted = await _service.DeleteAsync(id);

            Console.WriteLine(
                deleted
                    ? "Cart item deleted successfully."
                    : "Cart item could not be deleted.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting cart item: {ex.Message}");
        }
    }

    private static void DisplayCartItem(CartItem item)
    {
        Console.WriteLine("-----------------------------------");
        Console.WriteLine($"Cart Item ID : {item.CartItemId}");
        Console.WriteLine($"Cart ID      : {item.CartId}");
        Console.WriteLine($"Product ID   : {item.ProductId}");
        Console.WriteLine($"Product Name : {item.Product?.ProductName}");
        Console.WriteLine($"Quantity     : {item.Quantity}");
        Console.WriteLine($"Added At     : {item.AddedAt}");
        Console.WriteLine("-----------------------------------");
    }
}