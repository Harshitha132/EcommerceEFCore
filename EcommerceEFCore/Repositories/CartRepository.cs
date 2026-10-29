
using ECommerceEFCore.Data;
using ECommerceEFCore.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ECommerceEFCore.Repositories;

public class CartRepository
{
    public async Task<int> AddAsync(Cart cart)
    {
        await using var db = new AppDbContext();

        db.Carts.Add(cart);

        try
        {
            await db.SaveChangesAsync();
            return cart.CartId;
        }
        catch (DbUpdateException ex)
            when (ex.InnerException is SqlException sqlEx
                  && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
        {
            throw new InvalidOperationException(
                "This customer already has a shopping cart.", ex);
        }
        catch (DbUpdateException ex)
            when (ex.InnerException is SqlException sqlEx
                  && sqlEx.Number == 547)
        {
            throw new InvalidOperationException(
                "The specified customer does not exist or the cart violates a database constraint.",
                ex);
        }
    }

    public async Task<List<Cart>> GetAllAsync()
    {
        await using var db = new AppDbContext();

        return await db.Carts
            .AsNoTracking()
            .Include(c => c.Customer)
            .OrderBy(c => c.CartId)
            .ToListAsync();
    }

    public async Task<Cart?> GetByIdAsync(int id)
    {
        await using var db = new AppDbContext();

        return await db.Carts
            .AsNoTracking()
            .Include(c => c.Customer)
            .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
            .FirstOrDefaultAsync(c => c.CartId == id);
    }

    public async Task<bool> UpdateAsync(int id, int customerId)
    {
        await using var db = new AppDbContext();

        var cart = await db.Carts.FindAsync(id);

        if (cart is null)
            return false;

        cart.CustomerId = customerId;
        cart.UpdatedAt = DateTime.UtcNow;

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
                "This customer already has another shopping cart.", ex);
        }
        catch (DbUpdateException ex)
            when (ex.InnerException is SqlException sqlEx
                  && sqlEx.Number == 547)
        {
            throw new InvalidOperationException(
                "The specified customer does not exist or the cart violates a database constraint.",
                ex);
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        await using var db = new AppDbContext();

        var cart = await db.Carts.FindAsync(id);

        if (cart is null)
            return false;

        db.Carts.Remove(cart);

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
                "The cart cannot be deleted because of a related database record.",
                ex);
        }
    }
}
