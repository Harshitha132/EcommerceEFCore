
using ECommerceEFCore.Data;
using ECommerceEFCore.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ECommerceEFCore.Repositories;

public class ProductRepository
{
    public async Task<int> AddAsync(Product product)
    {
        await using var db = new AppDbContext();

        db.Products.Add(product);

        try
        {
            await db.SaveChangesAsync();
            return product.ProductId;
        }
        catch (DbUpdateException ex)
            when (ex.InnerException is SqlException sqlEx
                  && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
        {
            throw new InvalidOperationException(
                "A product violates a unique database constraint.", ex);
        }
        catch (DbUpdateException ex)
            when (ex.InnerException is SqlException sqlEx
                  && sqlEx.Number == 547)
        {
            throw new InvalidOperationException(
                "The specified category does not exist, or the product violates a database constraint.",
                ex);
        }
    }

    public async Task<List<Product>> GetAllAsync()
    {
        await using var db = new AppDbContext();

        return await db.Products
            .AsNoTracking()
            .Include(p => p.Category)
            .OrderBy(p => p.ProductId)
            .ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        await using var db = new AppDbContext();

        return await db.Products
            .AsNoTracking()
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.ProductId == id);
    }

    public async Task<bool> UpdateAsync(
        int id,
        string productName,
        string? description,
        decimal price,
        int stockQuantity,
        int categoryId)
    {
        await using var db = new AppDbContext();

        var product = await db.Products.FindAsync(id);

        if (product is null)
            return false;

        product.ProductName = productName;
        product.Description = description;
        product.Price = price;
        product.StockQuantity = stockQuantity;
        product.CategoryId = categoryId;

        try
        {
            await db.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException ex)
            when (ex.InnerException is SqlException sqlEx
                  && sqlEx.Number == 547)
        {
            throw new InvalidOperationException(
                "The specified category does not exist, or the product violates a database constraint.",
                ex);
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        await using var db = new AppDbContext();

        var product = await db.Products.FindAsync(id);

        if (product is null)
            return false;

        db.Products.Remove(product);

        try
        {
            await db.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException ex)
            when (ex.InnerException is SqlException sqlEx
                  && sqlEx.Number == 547)
        {
            throw new InvalidOperationException(
                "This product cannot be deleted because it is referenced by a cart item or order item.",
                ex);
        }
    }
}
