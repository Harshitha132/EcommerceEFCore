
using ECommerceEFCore.Helpers;
using ECommerceEFCore.Models;
using ECommerceEFCore.Services;

namespace ECommerceEFCore.Menus;

public class ProductMenu
{
    private readonly ProductService _service;

    public ProductMenu(ProductService service)
    {
        _service = service;
    }

    public async Task ShowAsync()
    {
        while (true)
        {
            Console.WriteLine("\n===== PRODUCT MANAGEMENT =====");
            Console.WriteLine("1. Add Product");
            Console.WriteLine("2. View All Products");
            Console.WriteLine("3. View Product By ID");
            Console.WriteLine("4. Update Product");
            Console.WriteLine("5. Delete Product");
            Console.WriteLine("0. Back to Main Menu");

            int choice = ConsoleInputHelper.ReadInt(
                "\nEnter your choice: ");

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
                    Console.WriteLine("Invalid choice. Select 0 to 5.");
                    break;
            }
        }
    }

    private async Task AddAsync()
    {
        string name = ConsoleInputHelper.ReadRequiredString(
            "Enter product name: ");

        Console.Write("Enter description (optional): ");
        string? description = Console.ReadLine();

        decimal price = ConsoleInputHelper.ReadNonNegativeDecimal(
            "Enter product price: ");

        int stockQuantity = ConsoleInputHelper.ReadInt(
            "Enter stock quantity: ");

        if (stockQuantity < 0)
        {
            Console.WriteLine("Stock quantity cannot be negative.");
            return;
        }

        int categoryId = ConsoleInputHelper.ReadPositiveInt(
            "Enter category ID: ");

        var product = new Product
        {
            ProductName = name,
            Description = string.IsNullOrWhiteSpace(description)
                ? null
                : description.Trim(),
            Price = price,
            StockQuantity = stockQuantity,
            CategoryId = categoryId
        };

        try
        {
            int id = await _service.AddAsync(product);

            Console.WriteLine(
                $"Product added successfully. ID: {id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not add product: {ex.Message}");
        }
    }

    private async Task GetAllAsync()
    {
        try
        {
            var products = await _service.GetAllAsync();

            if (products.Count == 0)
            {
                Console.WriteLine("No products found.");
                return;
            }

            Console.WriteLine(
                "\nID | Product Name | Price | Stock | Category");

            foreach (var product in products)
            {
                Console.WriteLine(
                    $"{product.ProductId} | " +
                    $"{product.ProductName} | " +
                    $"{product.Price:F2} | " +
                    $"{product.StockQuantity} | " +
                    $"{product.Category?.CategoryName ?? "N/A"}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not retrieve products: {ex.Message}");
        }
    }

    private async Task GetByIdAsync()
    {
        int id = ConsoleInputHelper.ReadPositiveInt(
            "Enter product ID: ");

        try
        {
            var product = await _service.GetByIdAsync(id);

            if (product is null)
            {
                Console.WriteLine("Product not found.");
                return;
            }

            DisplayProduct(product);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not retrieve product: {ex.Message}");
        }
    }

    private async Task UpdateAsync()
    {
        int id = ConsoleInputHelper.ReadPositiveInt(
            "Enter product ID to update: ");

        try
        {
            var product = await _service.GetByIdAsync(id);

            if (product is null)
            {
                Console.WriteLine("Product not found.");
                return;
            }

            string name = ConsoleInputHelper.ReadRequiredString(
                "Enter new product name: ");

            Console.Write("Enter new description (optional): ");
            string? description = Console.ReadLine();

            decimal price = ConsoleInputHelper.ReadNonNegativeDecimal(
                "Enter new price: ");

            int stockQuantity = ConsoleInputHelper.ReadInt(
                "Enter new stock quantity: ");

            if (stockQuantity < 0)
            {
                Console.WriteLine(
                    "Stock quantity cannot be negative.");
                return;
            }

            int categoryId = ConsoleInputHelper.ReadPositiveInt(
                "Enter new category ID: ");

            bool updated = await _service.UpdateAsync(
                id,
                name,
                string.IsNullOrWhiteSpace(description)
                    ? null
                    : description.Trim(),
                price,
                stockQuantity,
                categoryId);

            Console.WriteLine(
                updated
                    ? "Product updated successfully."
                    : "Product not found.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not update product: {ex.Message}");
        }
    }

    private async Task DeleteAsync()
    {
        int id = ConsoleInputHelper.ReadPositiveInt(
            "Enter product ID to delete: ");

        try
        {
            var product = await _service.GetByIdAsync(id);

            if (product is null)
            {
                Console.WriteLine("Product not found.");
                return;
            }

            if (!ConsoleInputHelper.ReadConfirmation(
                    $"Delete product '{product.ProductName}'?"))
            {
                Console.WriteLine("Deletion cancelled.");
                return;
            }

            bool deleted = await _service.DeleteAsync(id);

            Console.WriteLine(
                deleted
                    ? "Product deleted successfully."
                    : "Product not found.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not delete product: {ex.Message}");
        }
    }

    private static void DisplayProduct(Product product)
    {
        Console.WriteLine("\n===== PRODUCT DETAILS =====");
        Console.WriteLine($"ID: {product.ProductId}");
        Console.WriteLine($"Name: {product.ProductName}");
        Console.WriteLine($"Description: {product.Description ?? "N/A"}");
        Console.WriteLine($"Price: {product.Price:F2}");
        Console.WriteLine($"Stock: {product.StockQuantity}");
        Console.WriteLine(
            $"Category: {product.Category?.CategoryName ?? "N/A"}");
    }
}
