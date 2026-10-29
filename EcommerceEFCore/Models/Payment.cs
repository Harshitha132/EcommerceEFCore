
namespace ECommerceEFCore.Models;

public class Payment
{
    public int PaymentId { get; set; }

    public int OrderId { get; set; }

    public decimal Amount { get; set; }

    public string PaymentMethod { get; set; } = string.Empty;

    public string PaymentStatus { get; set; } = "Pending";

    public string? TransactionReference { get; set; }

    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

    public Order? Order { get; set; }
}
