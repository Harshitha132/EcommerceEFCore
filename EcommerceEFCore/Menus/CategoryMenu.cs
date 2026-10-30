
using ECommerceEFCore.Helpers;
using ECommerceEFCore.Models;
using ECommerceEFCore.Services;

namespace ECommerceEFCore.Menus;

public class CategoryMenu
{
    private readonly CategoryService _service;

    public CategoryMenu(CategoryService service)
    {
        _service = service;
    }

    public async Task ShowAsync()
    {
        while (true)
        {
            Console.WriteLine("\n===== CATEGORY MANAGEMENT =====");
            Console.WriteLine("1. Add Category");
            Console.WriteLine("2. View All Categories");
            Console.WriteLine("3. View Category By ID");
            Console.WriteLine("4. Update Category");
            Console.WriteLine("5. Delete Category");
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
            "Enter category name: ");

        Console.Write("Enter description (optional): ");
        string? description = Console.ReadLine();

        var category = new Category
        {
            CategoryName = name,
            Description = string.IsNullOrWhiteSpace(description)
                ? null
                : description.Trim()
        };

        try
        {
            int id = await _service.AddAsync(category);

            Console.WriteLine(
                $"Category added successfully. ID: {id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not add category: {ex.Message}");
        }
    }

    private async Task GetAllAsync()
    {
        try
        {
            var categories = await _service.GetAllAsync();

            if (categories.Count == 0)
            {
                Console.WriteLine("No categories found.");
                return;
            }

            Console.WriteLine("\nID | Category Name | Description");

            foreach (var category in categories)
            {
                Console.WriteLine(
                    $"{category.CategoryId} | " +
                    $"{category.CategoryName} | " +
                    $"{category.Description ?? "N/A"}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not retrieve categories: {ex.Message}");
        }
    }

    private async Task GetByIdAsync()
    {
        int id = ConsoleInputHelper.ReadPositiveInt(
            "Enter category ID: ");

        try
        {
            var category = await _service.GetByIdAsync(id);

            if (category is null)
            {
                Console.WriteLine("Category not found.");
                return;
            }

            Console.WriteLine("\nCategory Details");
            Console.WriteLine($"ID: {category.CategoryId}");
            Console.WriteLine($"Name: {category.CategoryName}");
            Console.WriteLine($"Description: {category.Description ?? "N/A"}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not retrieve category: {ex.Message}");
        }
    }

    private async Task UpdateAsync()
    {
        int id = ConsoleInputHelper.ReadPositiveInt(
            "Enter category ID to update: ");

        try
        {
            var category = await _service.GetByIdAsync(id);

            if (category is null)
            {
                Console.WriteLine("Category not found.");
                return;
            }

            string name = ConsoleInputHelper.ReadRequiredString(
                "Enter new category name: ");

            Console.Write("Enter new description (optional): ");
            string? description = Console.ReadLine();

            bool updated = await _service.UpdateAsync(
                id,
                name,
                string.IsNullOrWhiteSpace(description)
                    ? null
                    : description.Trim());

            Console.WriteLine(
                updated
                    ? "Category updated successfully."
                    : "Category not found.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not update category: {ex.Message}");
        }
    }

    private async Task DeleteAsync()
    {
        int id = ConsoleInputHelper.ReadPositiveInt(
            "Enter category ID to delete: ");

        try
        {
            var category = await _service.GetByIdAsync(id);

            if (category is null)
            {
                Console.WriteLine("Category not found.");
                return;
            }

            if (!ConsoleInputHelper.ReadConfirmation(
                    $"Delete category '{category.CategoryName}'?"))
            {
                Console.WriteLine("Deletion cancelled.");
                return;
            }

            bool deleted = await _service.DeleteAsync(id);

            Console.WriteLine(
                deleted
                    ? "Category deleted successfully."
                    : "Category not found.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not delete category: {ex.Message}");
        }
    }
}
