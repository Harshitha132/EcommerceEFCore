
using ECommerceEFCore.Models;
using ECommerceEFCore.Repositories;

namespace ECommerceEFCore.Services;

public class ProductService
{
    private readonly ProductRepository _productRepository;

    public ProductService()
    {
        _productRepository = new ProductRepository();
    }

    public async Task<int> AddAsync(Product product)
    {
        ValidateProduct(product);

        product.ProductName = product.ProductName.Trim();
        product.Description = product.Description?.Trim();

        return await _productRepository.AddAsync(product);
    }

    public async Task<List<Product>> GetAllAsync()
    {
        return await _productRepository.GetAllAsync();
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        ValidateId(id);

        return await _productRepository.GetByIdAsync(id);
    }

    public async Task<bool> UpdateAsync(
        int id,
        string productName,
        string? description,
        decimal price,
        int stockQuantity,
        int categoryId)
    {
        ValidateId(id);
        ValidateProductDetails(
            productName, description, price, stockQuantity, categoryId);

        return await _productRepository.UpdateAsync(
            id,
            productName.Trim(),
            description?.Trim(),
            price,
            stockQuantity,
            categoryId);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        ValidateId(id);

        return await _productRepository.DeleteAsync(id);
    }

    private static void ValidateProduct(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);

        ValidateProductDetails(
            product.ProductName,
            product.Description,
            product.Price,
            product.StockQuantity,
            product.CategoryId);
    }

    private static void ValidateProductDetails(
        string productName,
        string? description,
        decimal price,
        int stockQuantity,
        int categoryId)
    {
        if (string.IsNullOrWhiteSpace(productName))
            throw new ArgumentException("Product name is required.");

        if (productName.Trim().Length > 150)
            throw new ArgumentException(
                "Product name cannot exceed 150 characters.");

        if (description?.Trim().Length > 1000)
            throw new ArgumentException(
                "Description cannot exceed 1000 characters.");

        if (price < 0)
            throw new ArgumentOutOfRangeException(
                nameof(price), "Price cannot be negative.");

        if (stockQuantity < 0)
            throw new ArgumentOutOfRangeException(
                nameof(stockQuantity), "Stock quantity cannot be negative.");

        if (categoryId <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(categoryId), "Category ID must be greater than zero.");
    }

    private static void ValidateId(int id)
    {
        if (id <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(id), "Product ID must be greater than zero.");
    }
}
