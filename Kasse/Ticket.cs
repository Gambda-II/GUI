namespace BitsAndBites;

public class Ticket : Posten
{
    private TimeOnly startzeit;
    private int minuten;

    public Ticket(TimeOnly startzeit, int minuten, string name, double preis)
    {
        this.startzeit = startzeit;
        this.minuten = minuten;
        this.name = name;
        this.preis = preis;
        
    }

    public override double BerechnePreis()
    {
        return minuten * preis;
    }

    public override string ToString()
    {
        return $"{name} ({preis.ToString("0.00")} EUR / min)";
    }
}
