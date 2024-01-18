namespace Industrieanlage;

class Komponente
{
    public Komponententyp Typ { get; set; }
    public string Name { get; set; }

    public Komponente(Komponententyp typ, string name)
    {
        Typ = typ;
        Name = name;
    }

    public override string ToString()
    {
        return $"{Name} ({Typ.Preis.ToString("c")} €)";
    }
}