namespace BitsAndBites;

public abstract class Posten
{
    protected string name;
    protected double preis;

    public abstract double BerechnePreis();

    public string GetName()
    {
        return name;
    }

    public override string ToString()
    {
        return $"{name} ({BerechnePreis().ToString("0.00")} EUR)";
    }
}
