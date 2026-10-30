using ECommerceEFCore.Helpers;

namespace ECommerceEFCore.Menus;

public class MainMenu
{
    private readonly CategoryMenu _categoryMenu;
    private readonly ProductMenu _productMenu;
    private readonly CustomerMenu _customerMenu;
    private readonly CartMenu _cartMenu;
    private readonly OrderMenu _orderMenu;
    private readonly PaymentMenu _paymentMenu;

    public MainMenu(
        CategoryMenu categoryMenu,
        ProductMenu productMenu,
        CustomerMenu customerMenu,
        CartMenu cartMenu,
        OrderMenu orderMenu,
        PaymentMenu paymentMenu)
    {
        _categoryMenu = categoryMenu;
        _productMenu = productMenu;
        _customerMenu = customerMenu;
        _cartMenu = cartMenu;
        _orderMenu = orderMenu;
        _paymentMenu = paymentMenu;
    }

    public async Task ShowAsync()
    {
        while (true)
        {
            Console.WriteLine("\n======================================");
            Console.WriteLine("       E-COMMERCE MANAGEMENT SYSTEM");
            Console.WriteLine("======================================");

            Console.WriteLine("1. Category Management");
            Console.WriteLine("2. Product Management");
            Console.WriteLine("3. Customer Management");
            Console.WriteLine("4. Cart Management");
            Console.WriteLine("5. Order Management");
            Console.WriteLine("6. Payment Management");
            Console.WriteLine("0. Exit");

            Console.WriteLine("======================================");

            int choice = ConsoleInputHelper.ReadInt(
                "\nEnter your choice: ");

            switch (choice)
            {
                case 1:
                    await _categoryMenu.ShowAsync();
                    break;

                case 2:
                    await _productMenu.ShowAsync();
                    break;

                case 3:
                    await _customerMenu.ShowAsync();
                    break;

                case 4:
                    await _cartMenu.ShowAsync();
                    break;

                case 5:
                    await _orderMenu.ShowAsync();
                    break;

                case 6:
                    await _paymentMenu.ShowAsync();
                    break;

                case 0:
                    Console.WriteLine(
                        "\nThank you for using E-Commerce Management System!");

                    return;

                default:
                    Console.WriteLine(
                        "\nInvalid choice. Please select a valid option.");

                    break;
            }
        }
    }
}