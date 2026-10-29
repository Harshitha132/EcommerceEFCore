
using ECommerceEFCore.Data;
using ECommerceEFCore.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ECommerceEFCore.Repositories;

public class OrderRepository
{
    public async Task<int> AddAsync(Order order)
    {
        await using var db = new AppDbContext();

        db.Orders.Add(order);

        try
        {
            await db.SaveChangesAsync();
            return order.OrderId;
        }
        catch (DbUpdateException ex)
            when (ex.InnerException is SqlException sqlEx
                  && sqlEx.Number == 547)
        {
            throw new InvalidOperationException(
                "The specified customer does not exist, or the order violates a database constraint.",
                ex);
        }
    }

    public async Task<List<Order>> GetAllAsync()
    {
        await using var db = new AppDbContext();

        return await db.Orders
            .AsNoTracking()
            .Include(o => o.Customer)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
    }

    public async Task<Order?> GetByIdAsync(int id)
    {
        await using var db = new AppDbContext();

        return await db.Orders
            .AsNoTracking()
            .Include(o => o.Customer)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .Include(o => o.Payment)
            .FirstOrDefaultAsync(o => o.OrderId == id);
    }

    public async Task<bool> UpdateAsync(
        int id,
        string orderStatus,
        decimal totalAmount)
    {
        if (totalAmount < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(totalAmount),
                "Total amount cannot be negative.");
        }

        await using var db = new AppDbContext();

        var order = await db.Orders.FindAsync(id);

        if (order is null)
            return false;

        order.OrderStatus = orderStatus;
        order.TotalAmount = totalAmount;

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
                "The order violates a database constraint.", ex);
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        await using var db = new AppDbContext();

        var order = await db.Orders.FindAsync(id);

        if (order is null)
            return false;

        db.Orders.Remove(order);

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
                "This order cannot be deleted because a related payment or database constraint prevents deletion.",
                ex);
        }
    }
}
