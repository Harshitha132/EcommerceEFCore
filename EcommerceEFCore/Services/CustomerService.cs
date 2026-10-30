using ECommerceEFCore.Models;
using ECommerceEFCore.Repositories;

namespace ECommerceEFCore.Services;

public class CustomerService
{
    private readonly CustomerRepository _customerRepository;

    public CustomerService()
    {
        _customerRepository = new CustomerRepository();
    }

    public async Task<int> AddAsync(Customer customer)
    {
        ValidateCustomer(customer);

        customer.FirstName = customer.FirstName.Trim();
        customer.LastName = customer.LastName.Trim();
        customer.Email = customer.Email.Trim().ToLowerInvariant();
        customer.PhoneNumber = customer.PhoneNumber.Trim();

        return await _customerRepository.AddAsync(customer);
    }

    public async Task<List<Customer>> GetAllAsync()
    {
        return await _customerRepository.GetAllAsync();
    }

    public async Task<Customer?> GetByIdAsync(int id)
    {
        ValidateId(id);

        return await _customerRepository.GetByIdAsync(id);
    }

    public async Task<bool> UpdateAsync(
        int id,
        string firstName,
        string lastName,
        string email,
        string phoneNumber)
    {
        ValidateId(id);

        ValidateCustomerDetails(
            firstName, lastName, email, phoneNumber);

        return await _customerRepository.UpdateAsync(
            id,
            firstName.Trim(),
            lastName.Trim(),
            email.Trim().ToLowerInvariant(),
            phoneNumber.Trim());
    }

    public async Task<bool> DeleteAsync(int id)
    {
        ValidateId(id);

        return await _customerRepository.DeleteAsync(id);
    }

    private static void ValidateCustomer(Customer customer)
    {
        ArgumentNullException.ThrowIfNull(customer);

        ValidateCustomerDetails(
            customer.FirstName,
            customer.LastName,
            customer.Email,
            customer.PhoneNumber);
    }

    private static void ValidateCustomerDetails(
        string firstName,
        string lastName,
        string email,
        string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name is required.");

        if (firstName.Trim().Length > 100)
            throw new ArgumentException(
                "First name cannot exceed 100 characters.");

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name is required.");

        if (lastName.Trim().Length > 100)
            throw new ArgumentException(
                "Last name cannot exceed 100 characters.");

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required.");

        if (email.Trim().Length > 256 ||
            !email.Contains('@') ||
            email.StartsWith('@') ||
            email.EndsWith('@'))
            throw new ArgumentException("Enter a valid email address.");

        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new ArgumentException("Phone number is required.");

        if (phoneNumber.Trim().Length > 20)
            throw new ArgumentException(
                "Phone number cannot exceed 20 characters.");
    }

    private static void ValidateId(int id)
    {
        if (id <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(id), "Customer ID must be greater than zero.");
    }
}