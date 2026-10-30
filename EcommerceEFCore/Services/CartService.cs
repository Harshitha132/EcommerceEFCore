using ECommerceEFCore.Models;
using ECommerceEFCore.Repositories;

namespace ECommerceEFCore.Services;

public class CartService
{
    private readonly CartRepository _cartRepository;

    public CartService()
    {
        _cartRepository = new CartRepository();
    }

    public async Task<int> AddAsync(Cart cart)
    {
        ValidateCart(cart);

        return await _cartRepository.AddAsync(cart);
    }

    public async Task<List<Cart>> GetAllAsync()
    {
        return await _cartRepository.GetAllAsync();
    }

    public async Task<Cart?> GetByIdAsync(int id)
    {
        ValidateId(id);

        return await _cartRepository.GetByIdAsync(id);
    }

    public async Task<bool> UpdateAsync(
        int id,
        int customerId)
    {
        ValidateId(id);
        ValidateCustomerId(customerId);

        return await _cartRepository.UpdateAsync(
            id,
            customerId);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        ValidateId(id);

        return await _cartRepository.DeleteAsync(id);
    }

    private static void ValidateCart(Cart cart)
    {
        ArgumentNullException.ThrowIfNull(cart);

        ValidateCustomerId(cart.CustomerId);
    }

    private static void ValidateId(int id)
    {
        if (id <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(id), "Cart ID must be greater than zero.");
    }

    private static void ValidateCustomerId(int customerId)
    {
        if (customerId <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(customerId),
                "Customer ID must be greater than zero.");
    }
}