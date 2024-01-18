namespace Bankautomat;

internal class Logic
{
    public static bool isValidID;
    public static bool isCorrectPIN;
    //private Card cardToCheck;


    // I'm returning bool, but never using them
    // REMOVE bool return type and corresponding lines later
    public static void CheckID(Card card)
    {
        isValidID = card.CardID % 10 != 5;


    }

    public static void CheckPIN(Card card, List<int>? pin)
    {
        string inputPin = "";
        foreach(int i in pin)
        {
            inputPin += i.ToString();
        }

        isCorrectPIN = card.PIN.Equals(inputPin);

    }

}
