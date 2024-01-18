namespace BitsAndBites;

public class Essen : Posten
{

    private bool extragross;

    public Essen(bool extragross, string name, double preis)
    {
        this.extragross = extragross;
        this.name = name;
        this.preis = preis;
        
    }

    public override double BerechnePreis()
    {
        return extragross ? preis * 1.20 : preis;
    }
}
