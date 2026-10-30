
using ECommerceEFCore.Models;
using ECommerceEFCore.Repositories;

namespace ECommerceEFCore.Services;

public class CategoryService
{
    private readonly CategoryRepository _categoryRepository;

    public CategoryService()
    {
        _categoryRepository = new CategoryRepository();
    }

    public async Task<int> AddAsync(Category category)
    {
        ValidateCategory(category);

        category.CategoryName = category.CategoryName.Trim();

        if (category.Description is not null)
            category.Description = category.Description.Trim();

        return await _categoryRepository.AddAsync(category);
    }

    public async Task<List<Category>> GetAllAsync()
    {
        return await _categoryRepository.GetAllAsync();
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(id), "Category ID must be greater than zero.");

        return await _categoryRepository.GetByIdAsync(id);
    }

    public async Task<bool> UpdateAsync(
        int id,
        string categoryName,
        string? description)
    {
        if (id <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(id), "Category ID must be greater than zero.");

        if (string.IsNullOrWhiteSpace(categoryName))
            throw new ArgumentException(
                "Category name is required.", nameof(categoryName));

        categoryName = categoryName.Trim();

        if (categoryName.Length > 100)
            throw new ArgumentException(
                "Category name cannot exceed 100 characters.",
                nameof(categoryName));

        if (description?.Trim().Length > 500)
            throw new ArgumentException(
                "Description cannot exceed 500 characters.",
                nameof(description));

        return await _categoryRepository.UpdateAsync(
            id,
            categoryName,
            description?.Trim());
    }

    public async Task<bool> DeleteAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(id), "Category ID must be greater than zero.");

        return await _categoryRepository.DeleteAsync(id);
    }

    private static void ValidateCategory(Category category)
    {
        ArgumentNullException.ThrowIfNull(category);

        if (string.IsNullOrWhiteSpace(category.CategoryName))
            throw new ArgumentException(
                "Category name is required.", nameof(category));

        if (category.CategoryName.Trim().Length > 100)
            throw new ArgumentException(
                "Category name cannot exceed 100 characters.",
                nameof(category));

        if (category.Description?.Trim().Length > 500)
            throw new ArgumentException(
                "Description cannot exceed 500 characters.",
                nameof(category));
    }
}
