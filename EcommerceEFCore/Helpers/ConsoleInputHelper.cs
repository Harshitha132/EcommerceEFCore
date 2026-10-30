
namespace ECommerceEFCore.Helpers;

public static class ConsoleInputHelper
{
    // Read non-empty string
    public static string ReadRequiredString(string message)
    {
        while (true)
        {
            Console.Write(message);
            string? input = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(input))
                return input.Trim();

            Console.WriteLine("Input cannot be empty. Try again.");
        }
    }

    // Read integer
    public static int ReadInt(string message)
    {
        while (true)
        {
            Console.Write(message);
            string? input = Console.ReadLine();

            if (int.TryParse(input, out int value))
                return value;

            Console.WriteLine("Enter a valid integer.");
        }
    }

    // Read positive integer
    public static int ReadPositiveInt(string message)
    {
        while (true)
        {
            int value = ReadInt(message);

            if (value > 0)
                return value;

            Console.WriteLine("Value must be greater than zero.");
        }
    }

    // Read non-negative decimal
    public static decimal ReadNonNegativeDecimal(string message)
    {
        while (true)
        {
            Console.Write(message);
            string? input = Console.ReadLine();

            if (decimal.TryParse(input, out decimal value)
                && value >= 0)
            {
                return value;
            }

            Console.WriteLine("Enter a valid non-negative amount.");
        }
    }

    // Read yes/no confirmation
    public static bool ReadConfirmation(string message)
    {
        while (true)
        {
            Console.Write($"{message} (y/n): ");
            string? input = Console.ReadLine();

            if (string.Equals(
                input?.Trim(), "y",
                StringComparison.OrdinalIgnoreCase))
                return true;

            if (string.Equals(
                input?.Trim(), "n",
                StringComparison.OrdinalIgnoreCase))
                return false;

            Console.WriteLine("Please enter y or n.");
        }
    }
}
