
using ECommerceEFCore.Data;
using ECommerceEFCore.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ECommerceEFCore.Repositories;

public class PaymentRepository
{
    public async Task<int> AddAsync(Payment payment)
    {
        if (payment.Amount < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(payment.Amount),
                "Payment amount cannot be negative.");
        }

        await using var db = new AppDbContext();

        db.Payments.Add(payment);

        try
        {
            await db.SaveChangesAsync();
            return payment.PaymentId;
        }
        catch (DbUpdateException ex)
            when (ex.InnerException is SqlException sqlEx
                  && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
        {
            throw new InvalidOperationException(
                "A payment record already exists for this order.", ex);
        }
        catch (DbUpdateException ex)
            when (ex.InnerException is SqlException sqlEx
                  && sqlEx.Number == 547)
        {
            throw new InvalidOperationException(
                "The specified order does not exist, or the payment violates a database constraint.",
                ex);
        }
    }

    public async Task<List<Payment>> GetAllAsync()
    {
        await using var db = new AppDbContext();

        return await db.Payments
            .AsNoTracking()
            .Include(p => p.Order)
            .OrderByDescending(p => p.PaymentDate)
            .ToListAsync();
    }

    public async Task<Payment?> GetByIdAsync(int id)
    {
        await using var db = new AppDbContext();

        return await db.Payments
            .AsNoTracking()
            .Include(p => p.Order)
            .FirstOrDefaultAsync(p => p.PaymentId == id);
    }

    public async Task<bool> UpdateAsync(
        int id,
        decimal amount,
        string paymentMethod,
        string paymentStatus,
        string? transactionReference)
    {
        if (amount < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(amount),
                "Payment amount cannot be negative.");
        }

        await using var db = new AppDbContext();

        var payment = await db.Payments.FindAsync(id);

        if (payment is null)
            return false;

        payment.Amount = amount;
        payment.PaymentMethod = paymentMethod;
        payment.PaymentStatus = paymentStatus;
        payment.TransactionReference = transactionReference;

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
                "The payment violates a database constraint.", ex);
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        await using var db = new AppDbContext();

        var payment = await db.Payments.FindAsync(id);

        if (payment is null)
            return false;

        db.Payments.Remove(payment);

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
                "The payment could not be deleted because of a database constraint.",
                ex);
        }
    }
}
