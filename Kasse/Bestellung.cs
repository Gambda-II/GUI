namespace BitsAndBites;

public class Bestellung
{
    public bool bitandbitecard = false;
    public List<Posten> bestellung = new List<Posten>();

    public double BerechneBestellung()
    {
        double total = 0.0;

        foreach (Posten p in bestellung)
        {
            total += p.BerechnePreis();
        }

        return bitandbitecard ? total * 0.95 : total;
    }

    public void AddBestellung(Posten item)
    {
        bestellung.Add(item);
    }

}