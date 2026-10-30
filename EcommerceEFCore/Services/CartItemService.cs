using ECommerceEFCore.Models;
using ECommerceEFCore.Repositories;

namespace ECommerceEFCore.Services;

public class CartItemService
{
    private readonly CartItemRepository _cartItemRepository;

    public CartItemService()
    {
        _cartItemRepository = new CartItemRepository();
    }

    public async Task<int> AddAsync(CartItem cartItem)
    {
        ValidateCartItem(cartItem);

        return await _cartItemRepository.AddAsync(cartItem);
    }

    public async Task<List<CartItem>> GetAllAsync()
    {
        return await _cartItemRepository.GetAllAsync();
    }

    public async Task<CartItem?> GetByIdAsync(int id)
    {
        ValidateId(id);

        return await _cartItemRepository.GetByIdAsync(id);
    }

    public async Task<bool> UpdateAsync(
        int id,
        int quantity)
    {
        ValidateId(id);
        ValidateQuantity(quantity);

        return await _cartItemRepository.UpdateAsync(
            id,
            quantity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        ValidateId(id);

        return await _cartItemRepository.DeleteAsync(id);
    }

    private static void ValidateCartItem(CartItem cartItem)
    {
        ArgumentNullException.ThrowIfNull(cartItem);

        if (cartItem.CartId <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(cartItem.CartId),
                "Cart ID must be greater than zero.");

        if (cartItem.ProductId <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(cartItem.ProductId),
                "Product ID must be greater than zero.");

        ValidateQuantity(cartItem.Quantity);
    }

    private static void ValidateQuantity(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(quantity),
                "Quantity must be greater than zero.");
    }

    private static void ValidateId(int id)
    {
        if (id <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(id),
                "Cart item ID must be greater than zero.");
    }
}