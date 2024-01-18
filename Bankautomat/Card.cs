
using System.Globalization;
using System.Windows.Markup;

namespace Bankautomat;

internal class Card
{
    public int CardID { get; private set; }
    public string PIN { get; private set; }

    public decimal Balance { get; set; }

    public Card()
    {
        CardID = CreateRandomID();
        PIN = CreateRandomPin();
        Balance = CreateRandomBalance();
    }

    private decimal CreateRandomBalance()
    {
        int min = -50, max = 100000;
        double unitValue = new Random().NextDouble();
        return (decimal) (max * unitValue - min);
    }


    private string CreateRandomPin()
    {
        string pin = "";
        for( int i = 0; i < 4; i++)
        {
            int digit = new Random().Next(0,10);
            pin += digit.ToString();
        }
        return pin;
    }

    private int CreateRandomID()
    {
        string id = "";
        for (int i = 0; i < 3; i++)
        {

            int digitsOfThree = new Random().Next(100, 1000);
            id += digitsOfThree.ToString("000");

        }

        return int.Parse(id);
    }

    public string SubtractFromBalance(decimal value)
    {
        if (Balance - value < 0)
            return "Betrag konnte nicht abgezogen werden.";

        Balance -= value;
        return $"{value.ToString("c")} wurden abgezogen";
    }
}
