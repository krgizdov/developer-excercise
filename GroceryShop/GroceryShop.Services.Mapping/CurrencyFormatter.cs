namespace GroceryShop.Services.Mapping
{
    public static class CurrencyFormatter
    {
        public static string Convert(decimal number)
        {
            if (number < 100)
            {
                return $"{(int)number} clouds";
            }
            else
            {
                return $"{(int)number / 100} aws{(number % 100 == 0 ? string.Empty : $" {(int)number % 100} clouds")}";
            }
        }
    }
}
