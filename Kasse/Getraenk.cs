namespace BitsAndBites;

public class Getraenk : Posten
{
    private bool alkoholisch;
    private bool happyhour;

    public Getraenk(bool alkoholisch, bool happyhour, string name, double preis)
    {
        this.alkoholisch = alkoholisch;
        this.happyhour = happyhour;
        this.name = name;
        this.preis = preis;
        this.preis = BerechnePreis();
    }
    public override double BerechnePreis()
    {
        return happyhour && alkoholisch ? preis * 0.75 : preis;
    }
}
