namespace Industrieanlage;

class Komponententyp
{
    public decimal Preis { get; set; }

    public Komponententyp(decimal preis)
    {
        Preis = preis;
    }

    public override string ToString()
    {
        return Preis.ToString("c");
    }

    
}
