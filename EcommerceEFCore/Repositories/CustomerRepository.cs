
using ECommerceEFCore.Data;
using ECommerceEFCore.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ECommerceEFCore.Repositories;

public class CustomerRepository
{
    public async Task<int> AddAsync(Customer customer)
    {
        await using var db = new AppDbContext();

        db.Customers.Add(customer);

        try
        {
            await db.SaveChangesAsync();
            return customer.CustomerId;
        }
        catch (DbUpdateException ex)
            when (ex.InnerException is SqlException sqlEx
                  && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
        {
            throw new InvalidOperationException(
                "A customer with this email address already exists.", ex);
        }
    }

    public async Task<List<Customer>> GetAllAsync()
    {
        await using var db = new AppDbContext();

        return await db.Customers
            .AsNoTracking()
            .OrderBy(c => c.CustomerId)
            .ToListAsync();
    }

    public async Task<Customer?> GetByIdAsync(int id)
    {
        await using var db = new AppDbContext();

        return await db.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.CustomerId == id);
    }

    public async Task<bool> UpdateAsync(
        int id,
        string firstName,
        string lastName,
        string email,
        string phoneNumber)
    {
        await using var db = new AppDbContext();

        var customer = await db.Customers.FindAsync(id);

        if (customer is null)
            return false;

        customer.FirstName = firstName;
        customer.LastName = lastName;
        customer.Email = email;
        customer.PhoneNumber = phoneNumber;

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
                "Another customer already uses this email address.", ex);
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        await using var db = new AppDbContext();

        var customer = await db.Customers.FindAsync(id);

        if (customer is null)
            return false;

        db.Customers.Remove(customer);

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
                "This customer cannot be deleted because related orders or a cart exist.",
                ex);
        }
    }
}
