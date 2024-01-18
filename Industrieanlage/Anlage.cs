
namespace Industrieanlage;

class Anlage
{
    private static int nextID = 5461;
    private int id;

    public int ID { get { return id; } private set { id = ++nextID; } }
    public string Name { get; set; }
    public List<Komponente> Komponenten { get; set; }
    public decimal Gesamtpreis { get; private set; }


    public Anlage(string name)
    {
        ID = nextID;
        Name = name;
        Komponenten = new();
        InitAnlagewert();
    }

    public void BerechneAnlagewert()
    {
        InitAnlagewert();

        foreach (Komponente komponente in Komponenten)
        {
            Komponententyp typ = komponente.Typ;
            decimal preis = typ.Preis;
            AddiereKomponentenPreis(preis);
        }
    }

    private void AddiereKomponentenPreis(decimal preis)
    {
        Gesamtpreis += preis;
    }

    private void InitAnlagewert()
    {

        Gesamtpreis = 0;

    }
}
