
namespace ECommerceEFCore.Models;

public class Cart
{
    public int CartId { get; set; }

    public int CustomerId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public Customer? Customer { get; set; }

    public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
}
