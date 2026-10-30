using ECommerceEFCore.Helpers;
using ECommerceEFCore.Models;
using ECommerceEFCore.Services;

namespace ECommerceEFCore.Menus;

public class PaymentMenu
{
    private readonly PaymentService _service;

    public PaymentMenu(PaymentService service)
    {
        _service = service;
    }

    public async Task ShowAsync()
    {
        while (true)
        {
            Console.WriteLine("\n===== PAYMENT MANAGEMENT =====");
            Console.WriteLine("1. Add Payment");
            Console.WriteLine("2. View All Payments");
            Console.WriteLine("3. View Payment By ID");
            Console.WriteLine("4. Update Payment");
            Console.WriteLine("5. Delete Payment");
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
        Console.WriteLine("\n===== ADD PAYMENT =====");

        int orderId = ConsoleInputHelper.ReadPositiveInt(
            "Enter order ID: ");

        decimal amount = ConsoleInputHelper.ReadNonNegativeDecimal(
            "Enter payment amount: ");

        string paymentMethod = ReadPaymentMethod();

        string? transactionReference = ConsoleInputHelper.ReadRequiredString(
            "Enter transaction reference: ");

        var payment = new Payment
        {
            OrderId = orderId,
            Amount = amount,
            PaymentMethod = paymentMethod,
            PaymentStatus = "Pending",
            TransactionReference = transactionReference,
            PaymentDate = DateTime.Now
        };

        try
        {
            int id = await _service.AddAsync(payment);

            Console.WriteLine($"Payment added successfully. Payment ID: {id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding payment: {ex.Message}");
        }
    }

    private async Task GetAllAsync()
    {
        Console.WriteLine("\n===== ALL PAYMENTS =====");

        try
        {
            var payments = await _service.GetAllAsync();

            if (!payments.Any())
            {
                Console.WriteLine("No payments found.");
                return;
            }

            foreach (var payment in payments)
            {
                DisplayPayment(payment);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving payments: {ex.Message}");
        }
    }

    private async Task GetByIdAsync()
    {
        Console.WriteLine("\n===== SEARCH PAYMENT =====");

        int id = ConsoleInputHelper.ReadPositiveInt(
            "Enter payment ID: ");

        try
        {
            var payment = await _service.GetByIdAsync(id);

            if (payment is null)
            {
                Console.WriteLine("Payment not found.");
                return;
            }

            DisplayPayment(payment);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving payment: {ex.Message}");
        }
    }

    private async Task UpdateAsync()
    {
        Console.WriteLine("\n===== UPDATE PAYMENT =====");

        int id = ConsoleInputHelper.ReadPositiveInt(
            "Enter payment ID: ");

        try
        {
            var existingPayment = await _service.GetByIdAsync(id);

            if (existingPayment is null)
            {
                Console.WriteLine("Payment not found.");
                return;
            }

            DisplayPayment(existingPayment);

            decimal amount = ConsoleInputHelper.ReadNonNegativeDecimal(
                "Enter new payment amount: ");

            string paymentMethod = ReadPaymentMethod();
            string paymentStatus = ReadPaymentStatus();

            Console.Write("Enter new transaction reference (optional): ");
            string? transactionReference = Console.ReadLine()?.Trim();

            bool updated = await _service.UpdateAsync(
                id,
                amount,
                paymentMethod,
                paymentStatus,
                transactionReference);

            Console.WriteLine(
                updated
                    ? "Payment updated successfully."
                    : "Payment could not be updated.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating payment: {ex.Message}");
        }
    }

    private async Task DeleteAsync()
    {
        Console.WriteLine("\n===== DELETE PAYMENT =====");

        int id = ConsoleInputHelper.ReadPositiveInt(
            "Enter payment ID: ");

        try
        {
            var payment = await _service.GetByIdAsync(id);

            if (payment is null)
            {
                Console.WriteLine("Payment not found.");
                return;
            }

            DisplayPayment(payment);

            bool confirm = ConsoleInputHelper.ReadConfirmation(
                "Are you sure you want to delete this payment? (y/n): ");

            if (!confirm)
            {
                Console.WriteLine("Deletion cancelled.");
                return;
            }

            bool deleted = await _service.DeleteAsync(id);

            Console.WriteLine(
                deleted
                    ? "Payment deleted successfully."
                    : "Payment could not be deleted.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting payment: {ex.Message}");
        }
    }

    private static string ReadPaymentMethod()
    {
        Console.WriteLine("\nAvailable Payment Methods:");
        Console.WriteLine("1. Cash");
        Console.WriteLine("2. Credit Card");
        Console.WriteLine("3. Debit Card");
        Console.WriteLine("4. UPI");
        Console.WriteLine("5. Net Banking");

        int choice = ConsoleInputHelper.ReadInt(
            "Select payment method: ");

        return choice switch
        {
            1 => "Cash",
            2 => "Credit Card",
            3 => "Debit Card",
            4 => "UPI",
            5 => "Net Banking",
            _ => string.Empty
        };
    }

    private static string ReadPaymentStatus()
    {
        Console.WriteLine("\nAvailable Payment Statuses:");
        Console.WriteLine("1. Pending");
        Console.WriteLine("2. Completed");
        Console.WriteLine("3. Failed");
        Console.WriteLine("4. Refunded");

        int choice = ConsoleInputHelper.ReadInt(
            "Select payment status: ");

        return choice switch
        {
            1 => "Pending",
            2 => "Completed",
            3 => "Failed",
            4 => "Refunded",
            _ => string.Empty
        };
    }

    private static void DisplayPayment(Payment payment)
    {
        Console.WriteLine("-----------------------------------");
        Console.WriteLine($"Payment ID      : {payment.PaymentId}");
        Console.WriteLine($"Order ID        : {payment.OrderId}");
        Console.WriteLine($"Amount          : {payment.Amount}");
        Console.WriteLine($"Payment Method  : {payment.PaymentMethod}");
        Console.WriteLine($"Payment Status  : {payment.PaymentStatus}");
        Console.WriteLine($"Transaction Ref : {payment.TransactionReference}");
        Console.WriteLine($"Payment Date    : {payment.PaymentDate}");
        Console.WriteLine("-----------------------------------");
    }
}