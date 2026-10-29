
using ECommerceEFCore.Data;
using ECommerceEFCore.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ECommerceEFCore.Repositories;

public class CategoryRepository
{
    public async Task<int> AddAsync(Category category)
    {
        await using var db = new AppDbContext();

        db.Categories.Add(category);

        try
        {
            await db.SaveChangesAsync();
            return category.CategoryId;
        }
        catch (DbUpdateException ex)
            when (ex.InnerException is SqlException sqlEx
                  && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
        {
            throw new InvalidOperationException(
                "A category with this name already exists.", ex);
        }
    }

    public async Task<List<Category>> GetAllAsync()
    {
        await using var db = new AppDbContext();

        return await db.Categories
            .AsNoTracking()
            .OrderBy(c => c.CategoryId)
            .ToListAsync();
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        await using var db = new AppDbContext();

        return await db.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.CategoryId == id);
    }

    public async Task<bool> UpdateAsync(
        int id,
        string categoryName,
        string? description)
    {
        await using var db = new AppDbContext();

        var category = await db.Categories.FindAsync(id);

        if (category is null)
            return false;

        category.CategoryName = categoryName;
        category.Description = description;

        try
        {
            await db.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException ex)
            when (ex.InnerException is SqlException sqlEx
                  && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
        {
            throw new InvalidOperationException(
                "A category with this name already exists.", ex);
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        await using var db = new AppDbContext();

        var category = await db.Categories.FindAsync(id);

        if (category is null)
            return false;

        db.Categories.Remove(category);

        try
        {
            await db.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException ex)
            when (ex.InnerException is SqlException sqlEx
                  && (sqlEx.Number == 547))
        {
            throw new InvalidOperationException(
                "This category cannot be deleted because products are associated with it.",
                ex);
        }
    }
}
