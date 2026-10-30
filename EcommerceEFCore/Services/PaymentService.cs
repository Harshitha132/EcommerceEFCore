using ECommerceEFCore.Models;
using ECommerceEFCore.Repositories;

namespace ECommerceEFCore.Services;

public class PaymentService
{
    private readonly PaymentRepository _paymentRepository;

    private static readonly string[] AllowedMethods =
    {
        "Cash",
        "Credit Card",
        "Debit Card",
        "UPI",
        "Net Banking"
    };

    private static readonly string[] AllowedStatuses =
    {
        "Pending",
        "Completed",
        "Failed",
        "Refunded"
    };

    public PaymentService()
    {
        _paymentRepository = new PaymentRepository();
    }

    public async Task<int> AddAsync(Payment payment)
    {
        ValidatePayment(payment);

        payment.PaymentMethod = payment.PaymentMethod.Trim();
        payment.PaymentStatus = "Pending";

        return await _paymentRepository.AddAsync(payment);
    }

    public async Task<List<Payment>> GetAllAsync()
    {
        return await _paymentRepository.GetAllAsync();
    }

    public async Task<Payment?> GetByIdAsync(int id)
    {
        ValidateId(id);

        return await _paymentRepository.GetByIdAsync(id);
    }

    public async Task<bool> UpdateAsync(
        int id,
        decimal amount,
        string paymentMethod,
        string paymentStatus,
        string? transactionReference)
    {
        ValidateId(id);

        ValidatePaymentDetails(
            amount,
            paymentMethod,
            paymentStatus,
            transactionReference);

        return await _paymentRepository.UpdateAsync(
            id,
            amount,
            paymentMethod.Trim(),
            paymentStatus.Trim(),
            transactionReference?.Trim());
    }

    public async Task<bool> DeleteAsync(int id)
    {
        ValidateId(id);

        return await _paymentRepository.DeleteAsync(id);
    }

    private static void ValidatePayment(Payment payment)
    {
        ArgumentNullException.ThrowIfNull(payment);

        if (payment.OrderId <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(payment.OrderId),
                "Order ID must be greater than zero.");

        ValidatePaymentDetails(
            payment.Amount,
            payment.PaymentMethod,
            payment.PaymentStatus,
            payment.TransactionReference);
    }

    private static void ValidatePaymentDetails(
        decimal amount,
        string paymentMethod,
        string paymentStatus,
        string? transactionReference)
    {
        if (amount < 0)
            throw new ArgumentOutOfRangeException(
                nameof(amount),
                "Payment amount cannot be negative.");

        if (string.IsNullOrWhiteSpace(paymentMethod))
            throw new ArgumentException(
                "Payment method is required.");

        if (paymentMethod.Trim().Length > 30)
            throw new ArgumentException(
                "Payment method cannot exceed 30 characters.");

        if (!AllowedMethods.Contains(
                paymentMethod.Trim(),
                StringComparer.OrdinalIgnoreCase))
        {
            throw new ArgumentException(
                "Invalid payment method. Allowed: Cash, Credit Card, Debit Card, UPI, Net Banking.");
        }

        if (string.IsNullOrWhiteSpace(paymentStatus))
            throw new ArgumentException(
                "Payment status is required.");

        if (!AllowedStatuses.Contains(
                paymentStatus.Trim(),
                StringComparer.OrdinalIgnoreCase))
        {
            throw new ArgumentException(
                "Invalid payment status. Allowed: Pending, Completed, Failed, Refunded.");
        }

        if (transactionReference?.Trim().Length > 100)
            throw new ArgumentException(
                "Transaction reference cannot exceed 100 characters.");
    }

    private static void ValidateId(int id)
    {
        if (id <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(id),
                "Payment ID must be greater than zero.");
    }
}