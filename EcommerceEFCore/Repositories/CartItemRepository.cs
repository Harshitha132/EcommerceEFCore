
using ECommerceEFCore.Data;
using ECommerceEFCore.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ECommerceEFCore.Repositories;

public class CartItemRepository
{
    public async Task<int> AddAsync(CartItem cartItem)
    {
        await using var db = new AppDbContext();

        db.CartItems.Add(cartItem);

        try
        {
            await db.SaveChangesAsync();
            return cartItem.CartItemId;
        }
        catch (DbUpdateException ex)
            when (ex.InnerException is SqlException sqlEx
                  && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
        {
            throw new InvalidOperationException(
                "This product already exists in the cart. Update its quantity instead.",
                ex);
        }
        catch (DbUpdateException ex)
            when (ex.InnerException is SqlException sqlEx
                  && sqlEx.Number == 547)
        {
            throw new InvalidOperationException(
                "The specified cart or product does not exist, or the item violates a database constraint.",
                ex);
        }
    }

    public async Task<List<CartItem>> GetAllAsync()
    {
        await using var db = new AppDbContext();

        return await db.CartItems
            .AsNoTracking()
            .Include(ci => ci.Cart)
            .Include(ci => ci.Product)
            .OrderBy(ci => ci.CartItemId)
            .ToListAsync();
    }

    public async Task<CartItem?> GetByIdAsync(int id)
    {
        await using var db = new AppDbContext();

        return await db.CartItems
            .AsNoTracking()
            .Include(ci => ci.Cart)
            .Include(ci => ci.Product)
            .FirstOrDefaultAsync(ci => ci.CartItemId == id);
    }

    public async Task<bool> UpdateAsync(
        int id,
        int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(quantity),
                "Quantity must be greater than zero.");
        }

        await using var db = new AppDbContext();

        var cartItem = await db.CartItems.FindAsync(id);

        if (cartItem is null)
            return false;

        cartItem.Quantity = quantity;
        cartItem.AddedAt = DateTime.UtcNow;

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
                "The cart item violates a database constraint.", ex);
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        await using var db = new AppDbContext();

        var cartItem = await db.CartItems.FindAsync(id);

        if (cartItem is null)
            return false;

        db.CartItems.Remove(cartItem);
        await db.SaveChangesAsync();

        return true;
    }
}
