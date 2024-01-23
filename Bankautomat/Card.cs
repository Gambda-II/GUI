
using System.Globalization;
using System.Numerics;
using System.Text;
using System.Windows.Markup;

namespace Bankautomat;

internal class Card
{
    public int CardID { get; private set; }
    public string PIN { get; private set; }

    public decimal Balance { get; set; }

    public Card()
    {
        CardID = SimulateCreatingRandomID();
        PIN = SimulateCreatingRandomPin();
        Balance = CreateRandomBalance();
    }

    private decimal CreateRandomBalance()
    {
        int min = -50, max = 100000;
        double unitValue = new Random().NextDouble();
        return (decimal) (max * unitValue - min);
    }


    private string SimulateCreatingRandomPin()
    {
        string pin = "";
        for( int i = 0; i < 4; i++)
        {
            int digit = new Random().Next(0,10);
            pin += digit.ToString();
        }
        return pin;
    }

    private int SimulateCreatingRandomID()
    {
        
        StringBuilder id = new StringBuilder("");
        int minimumValue = 100;
        int maximumValue = 1000;

        Random random = new Random(123);

        for (int i = 0; i < 3; i++)
        {
            int digitsOfThree = random.Next(minimumValue, maximumValue + 1);  
            id.Append(digitsOfThree);
        }

        int returnInteger;
        bool parseWorked = int.TryParse(id.ToString(), out returnInteger);

        return parseWorked ? returnInteger : 123456789;
    }

    public string SubtractFromBalance(decimal value)
    {
        if (Balance - value < 0)
            return "Betrag konnte nicht abgezogen werden.";

        Balance -= value;
        return $"{value.ToString("c")} wurden abgezogen";
    }
}
