using ECommerceEFCore.Helpers;
using ECommerceEFCore.Models;
using ECommerceEFCore.Services;

namespace ECommerceEFCore.Menus;

public class CustomerMenu
{
    private readonly CustomerService _service;

    public CustomerMenu(CustomerService service)
    {
        _service = service;
    }

    public async Task ShowAsync()
    {
        while (true)
        {
            Console.WriteLine("\n===== CUSTOMER MANAGEMENT =====");
            Console.WriteLine("1. Add Customer");
            Console.WriteLine("2. View All Customers");
            Console.WriteLine("3. View Customer By ID");
            Console.WriteLine("4. Update Customer");
            Console.WriteLine("5. Delete Customer");
            Console.WriteLine("0. Back to Main Menu");

            int choice = ConsoleInputHelper.ReadInt("\nEnter your choice: ");

            switch (choice)
            {
                case 1:
                    await AddAsync();
                    break;

                case 2:
                    await GetAllAsync();
                    break;

                case 3:
                    await GetByIdAsync();
                    break;

                case 4:
                    await UpdateAsync();
                    break;

                case 5:
                    await DeleteAsync();
                    break;

                case 0:
                    return;

                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    private async Task AddAsync()
    {
        Console.WriteLine("\n===== ADD CUSTOMER =====");

        string firstName =
            ConsoleInputHelper.ReadRequiredString("Enter first name: ");

        string lastName =
            ConsoleInputHelper.ReadRequiredString("Enter last name: ");

        string email =
            ConsoleInputHelper.ReadRequiredString("Enter email: ");

        string phoneNumber =
            ConsoleInputHelper.ReadRequiredString("Enter phone number: ");

        var customer = new Customer
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            PhoneNumber = phoneNumber,
            CreatedAt = DateTime.Now
        };

        try
        {
            int id = await _service.AddAsync(customer);

            Console.WriteLine($"Customer added successfully. Customer ID: {id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding customer: {ex.Message}");
        }
    }

    private async Task GetAllAsync()
    {
        Console.WriteLine("\n===== ALL CUSTOMERS =====");

        try
        {
            var customers = await _service.GetAllAsync();

            if (!customers.Any())
            {
                Console.WriteLine("No customers found.");
                return;
            }

            foreach (var customer in customers)
            {
                DisplayCustomer(customer);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving customers: {ex.Message}");
        }
    }

    private async Task GetByIdAsync()
    {
        Console.WriteLine("\n===== SEARCH CUSTOMER =====");

        int id = ConsoleInputHelper.ReadPositiveInt("Enter customer ID: ");

        try
        {
            var customer = await _service.GetByIdAsync(id);

            if (customer is null)
            {
                Console.WriteLine("Customer not found.");
                return;
            }

            DisplayCustomer(customer);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving customer: {ex.Message}");
        }
    }

    private async Task UpdateAsync()
    {
        Console.WriteLine("\n===== UPDATE CUSTOMER =====");

        int id = ConsoleInputHelper.ReadPositiveInt("Enter customer ID: ");

        try
        {
            var existingCustomer = await _service.GetByIdAsync(id);

            if (existingCustomer is null)
            {
                Console.WriteLine("Customer not found.");
                return;
            }

            Console.WriteLine("\nCurrent customer details:");
            DisplayCustomer(existingCustomer);

            string firstName =
                ConsoleInputHelper.ReadRequiredString("Enter new first name: ");

            string lastName =
                ConsoleInputHelper.ReadRequiredString("Enter new last name: ");

            string email =
                ConsoleInputHelper.ReadRequiredString("Enter new email: ");

            string phoneNumber =
                ConsoleInputHelper.ReadRequiredString("Enter new phone number: ");

            bool updated = await _service.UpdateAsync(
                id,
                firstName,
                lastName,
                email,
                phoneNumber);

            Console.WriteLine(
                updated
                    ? "Customer updated successfully."
                    : "Customer could not be updated.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating customer: {ex.Message}");
        }
    }

    private async Task DeleteAsync()
    {
        Console.WriteLine("\n===== DELETE CUSTOMER =====");

        int id = ConsoleInputHelper.ReadPositiveInt("Enter customer ID: ");

        try
        {
            var customer = await _service.GetByIdAsync(id);

            if (customer is null)
            {
                Console.WriteLine("Customer not found.");
                return;
            }

            DisplayCustomer(customer);

            bool confirm = ConsoleInputHelper.ReadConfirmation(
                "Are you sure you want to delete this customer? (y/n): ");

            if (!confirm)
            {
                Console.WriteLine("Deletion cancelled.");
                return;
            }

            bool deleted = await _service.DeleteAsync(id);

            Console.WriteLine(
                deleted
                    ? "Customer deleted successfully."
                    : "Customer could not be deleted.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting customer: {ex.Message}");
        }
    }

    private static void DisplayCustomer(Customer customer)
    {
        Console.WriteLine("-----------------------------------");
        Console.WriteLine($"Customer ID : {customer.CustomerId}");
        Console.WriteLine($"First Name  : {customer.FirstName}");
        Console.WriteLine($"Last Name   : {customer.LastName}");
        Console.WriteLine($"Email       : {customer.Email}");
        Console.WriteLine($"Phone       : {customer.PhoneNumber}");
        Console.WriteLine($"Created At  : {customer.CreatedAt}");
        Console.WriteLine("-----------------------------------");
    }
}