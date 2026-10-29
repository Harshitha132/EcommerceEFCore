
using ECommerceEFCore.Data;
using ECommerceEFCore.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ECommerceEFCore.Repositories;

public class OrderItemRepository
{
    public async Task<int> AddAsync(OrderItem orderItem)
    {
        if (orderItem.Quantity <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(orderItem.Quantity),
                "Quantity must be greater than zero.");
        }

        if (orderItem.UnitPrice < 0 || orderItem.Subtotal < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(orderItem.UnitPrice),
                "Unit price and subtotal cannot be negative.");
        }

        await using var db = new AppDbContext();

        db.OrderItems.Add(orderItem);

        try
        {
            await db.SaveChangesAsync();
            return orderItem.OrderItemId;
        }
        catch (DbUpdateException ex)
            when (ex.InnerException is SqlException sqlEx
                  && sqlEx.Number == 547)
        {
            throw new InvalidOperationException(
                "The specified order or product does not exist, or the order item violates a database constraint.",
                ex);
        }
    }

    public async Task<List<OrderItem>> GetAllAsync()
    {
        await using var db = new AppDbContext();

        return await db.OrderItems
            .AsNoTracking()
            .Include(oi => oi.Order)
            .Include(oi => oi.Product)
            .OrderBy(oi => oi.OrderItemId)
            .ToListAsync();
    }

    public async Task<OrderItem?> GetByIdAsync(int id)
    {
        await using var db = new AppDbContext();

        return await db.OrderItems
            .AsNoTracking()
            .Include(oi => oi.Order)
            .Include(oi => oi.Product)
            .FirstOrDefaultAsync(oi => oi.OrderItemId == id);
    }

    public async Task<bool> UpdateAsync(
        int id,
        int quantity,
        decimal unitPrice)
    {
        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(quantity),
                "Quantity must be greater than zero.");
        }

        if (unitPrice < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(unitPrice),
                "Unit price cannot be negative.");
        }

        await using var db = new AppDbContext();

        var orderItem = await db.OrderItems.FindAsync(id);

        if (orderItem is null)
            return false;

        orderItem.Quantity = quantity;
        orderItem.UnitPrice = unitPrice;
        orderItem.Subtotal = quantity * unitPrice;

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
                "The order item violates a database constraint.", ex);
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        await using var db = new AppDbContext();

        var orderItem = await db.OrderItems.FindAsync(id);

        if (orderItem is null)
            return false;

        db.OrderItems.Remove(orderItem);

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
                "The order item could not be deleted because of a database constraint.",
                ex);
        }
    }
}
