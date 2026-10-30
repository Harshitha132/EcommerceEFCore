using ECommerceEFCore.Helpers;
using ECommerceEFCore.Models;
using ECommerceEFCore.Services;

namespace ECommerceEFCore.Menus;

public class CartMenu
{
    private readonly CartService _cartService;
    private readonly CartItemMenu _cartItemMenu;

    public CartMenu(
        CartService cartService,
        CartItemMenu cartItemMenu)
    {
        _cartService = cartService;
        _cartItemMenu = cartItemMenu;
    }

    public async Task ShowAsync()
    {
        while (true)
        {
            Console.WriteLine("\n===== CART MANAGEMENT =====");
            Console.WriteLine("1. Add Cart");
            Console.WriteLine("2. View All Carts");
            Console.WriteLine("3. View Cart By ID");
            Console.WriteLine("4. Update Cart");
            Console.WriteLine("5. Delete Cart");
            Console.WriteLine("6. Manage Cart Items");
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
                    await _cartItemMenu.ShowAsync();
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
        Console.WriteLine("\n===== ADD CART =====");

        int customerId =
            ConsoleInputHelper.ReadPositiveInt("Enter customer ID: ");

        var cart = new Cart
        {
            CustomerId = customerId,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        try
        {
            int id = await _cartService.AddAsync(cart);
            Console.WriteLine($"Cart created successfully. Cart ID: {id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating cart: {ex.Message}");
        }
    }

    private async Task GetAllAsync()
    {
        Console.WriteLine("\n===== ALL CARTS =====");

        try
        {
            var carts = await _cartService.GetAllAsync();

            if (!carts.Any())
            {
                Console.WriteLine("No carts found.");
                return;
            }

            foreach (var cart in carts)
            {
                DisplayCart(cart);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving carts: {ex.Message}");
        }
    }

    private async Task GetByIdAsync()
    {
        Console.WriteLine("\n===== SEARCH CART =====");

        int id = ConsoleInputHelper.ReadPositiveInt("Enter cart ID: ");

        try
        {
            var cart = await _cartService.GetByIdAsync(id);

            if (cart is null)
            {
                Console.WriteLine("Cart not found.");
                return;
            }

            DisplayCart(cart);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving cart: {ex.Message}");
        }
    }

    private async Task UpdateAsync()
    {
        Console.WriteLine("\n===== UPDATE CART =====");

        int id = ConsoleInputHelper.ReadPositiveInt("Enter cart ID: ");

        try
        {
            var existingCart = await _cartService.GetByIdAsync(id);

            if (existingCart is null)
            {
                Console.WriteLine("Cart not found.");
                return;
            }

            DisplayCart(existingCart);

            int customerId =
                ConsoleInputHelper.ReadPositiveInt("Enter new customer ID: ");

            bool updated = await _cartService.UpdateAsync(id, customerId);

            Console.WriteLine(
                updated
                    ? "Cart updated successfully."
                    : "Cart could not be updated.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating cart: {ex.Message}");
        }
    }

    private async Task DeleteAsync()
    {
        Console.WriteLine("\n===== DELETE CART =====");

        int id = ConsoleInputHelper.ReadPositiveInt("Enter cart ID: ");

        try
        {
            var cart = await _cartService.GetByIdAsync(id);

            if (cart is null)
            {
                Console.WriteLine("Cart not found.");
                return;
            }

            DisplayCart(cart);

            bool confirm = ConsoleInputHelper.ReadConfirmation(
                "Are you sure you want to delete this cart? (y/n): ");

            if (!confirm)
            {
                Console.WriteLine("Deletion cancelled.");
                return;
            }

            bool deleted = await _cartService.DeleteAsync(id);

            Console.WriteLine(
                deleted
                    ? "Cart deleted successfully."
                    : "Cart could not be deleted.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting cart: {ex.Message}");
        }
    }

    private static void DisplayCart(Cart cart)
    {
        Console.WriteLine("-----------------------------------");
        Console.WriteLine($"Cart ID     : {cart.CartId}");
        Console.WriteLine($"Customer ID : {cart.CustomerId}");
        Console.WriteLine($"Customer    : {cart.Customer?.FirstName} {cart.Customer?.LastName}");
        Console.WriteLine($"Created At  : {cart.CreatedAt}");
        Console.WriteLine($"Updated At  : {cart.UpdatedAt}");
        Console.WriteLine($"Items       : {cart.CartItems?.Count ?? 0}");
        Console.WriteLine("-----------------------------------");
    }
}