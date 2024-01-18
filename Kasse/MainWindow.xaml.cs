using BitsAndBites;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kasse;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    Bestellung currentOrder = new Bestellung();

    public MainWindow()
    {
        InitializeComponent();
        cnvOptions.Visibility = Visibility.Hidden;
        cnvOrderAddedItem.Visibility = Visibility.Hidden;
        cnvOrderComplete.Visibility = Visibility.Hidden;
        btnRemoveItem.Visibility = Visibility.Hidden;
    }

    private void btnMenueNew_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button b)
        {
            currentOrder.bitandbitecard = b.Name.Contains("Plus") ? true : false;
        }

        if (btnMenueNew.IsEnabled)
        {
            btnMenueNew.Visibility = Visibility.Hidden;
            btnMenueNewPlus.Visibility = Visibility.Hidden;
            btnMenueNew.IsEnabled = false;
            btnMenueNewPlus.IsEnabled = false;

            cnvOptions.Visibility = Visibility.Visible;

        }
    }

    private void btnMenueNew_MouseEnter(object sender, MouseEventArgs e)
    {
        BrushConverter bc = new BrushConverter();
        Brush brushBG = (Brush)bc.ConvertFrom("#FF00B6A9");
        brushBG.Freeze();
        (sender as Button).Foreground = brushBG;
    }

    private void btnMenueNew_MouseLeave(object sender, MouseEventArgs e)
    {
        BrushConverter bc = new BrushConverter();

        Brush brushBG = (Brush)bc.ConvertFrom("#FF00B6A9");
        brushBG.Freeze();
        (sender as Button).Background = brushBG;

        Brush brushFG = (Brush)bc.ConvertFrom("#FFFF3400");
        brushFG.Freeze();
        (sender as Button).Foreground = brushFG;
    }

    private void btnOrder_Click(object sender, RoutedEventArgs e)
    {
        string tag = (sender as Button).Tag.ToString();
        int currentHour = DateTime.Now.Hour;
        bool isHappyHourNow = (currentHour > 22 && currentHour < 23);

        Posten tempOrder = GetOrder(tag, isHappyHourNow);

        if (tempOrder != null)
        {
            currentOrder.AddBestellung(tempOrder);
            //MessageBox.Show(tempOrder.ToString());
            cnvOrderAddedItem.Visibility = Visibility.Visible;
            tbAddedItem.Text = $"+ {tempOrder.ToString()}";
        }


    }

    private Posten GetOrder(string tag, bool isHappyHourNow)
    {
        Posten tempOrder;
        switch (tag)
        {
            case "PizzaXL":
            case "Pizza":
                tempOrder = new Essen(tag.EndsWith("XL"), "IT'S " + tag + "time", 4.59);
                break;

            case "PommesXL":
            case "Pommes":
                tempOrder = new Essen(tag.EndsWith("XL"), tag, 3.99);
                break;

            case "PastaXL":
            case "Pasta":
                tempOrder = new Essen(tag.EndsWith("XL"), tag, 7.23);
                break;

            case "WedgesXL":
            case "Wedges":
                tempOrder = new Essen(tag.EndsWith("XL"), tag, 3.99 + 0.50);
                break;

            case "WürgerXL":
            case "Würger":
                tempOrder = new Essen(tag.EndsWith("XL"), tag, 5.50);
                break;

            case "Wasser":
                tempOrder = new Getraenk(false, isHappyHourNow, tag, 2.00);
                break;
            case "Bier":
                tempOrder = new Getraenk(true, isHappyHourNow, tag, 3.78);
                break;
            case "Cola":
                tempOrder = new Getraenk(false, isHappyHourNow, tag, 2.90);
                break;
            case "Pespi":
                tempOrder = new Getraenk(false, isHappyHourNow, tag, 3.10);
                break;
            case "Cocktail":
                tempOrder = new Getraenk(true, isHappyHourNow, tag, 6.66);
                break;
            case "Krabbensaft":
                tempOrder = new Getraenk(false, isHappyHourNow, tag, 1.23);
                break;

            case "Hour":
                tempOrder = new Ticket(TimeOnly.FromDateTime(DateTime.Now), 60, "Stundensurfer", 0.09);
                break;
            case "Day":
                tempOrder = new Ticket((TimeOnly.FromDateTime(DateTime.Now)), 60 * 24, "Tagessurfer", .02);
                break;
            case "Group":
                tempOrder = new Ticket((TimeOnly.FromDateTime(DateTime.Now)), 60 * 2, "Gruppensurfer", 0.4);
                break;
            case "Party":

                tempOrder = tempOrder = new Ticket((TimeOnly.FromDateTime(DateTime.Now)), 60 * 4, "PAAAAAARTEEEEEEEY", 0.69);
                break;
            default:
                tempOrder = null;
                break;
        }
        return tempOrder;
    }

    private void tbAddedItem_MouseEnter(object sender, MouseEventArgs e)
    {

    }

    private void btnFinishOrder_Click(object sender, RoutedEventArgs e)
    {
        if (currentOrder.bestellung.Count < 1)
        {
            return;
        }

        cnvOptions.Visibility = Visibility.Hidden;
        cnvOrderAddedItem.Visibility = Visibility.Hidden;
        cnvOrderComplete.Visibility = Visibility.Visible;

        DisplayOrderComplete();
    }

    private void DisplayOrderComplete()
    {
        foreach (Posten p in currentOrder.bestellung)
        {
            if (p is Ticket t)
            {
                lbOrderComplete.Items.Add($"+ {t.GetName()}");
                lbOrderCompletePrice.Items.Add($"{t.BerechnePreis().ToString("0.00")} EUR");

            }
            else
            {
                lbOrderComplete.Items.Add($"+ {p.GetName()}");
                lbOrderCompletePrice.Items.Add($"{p.BerechnePreis().ToString("0.00")} EUR");

            }
        }

        DisplayTotalPrice();
    }

    private void lbOrderComplete_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ListBox lb = sender as ListBox;

        if (lb.SelectedIndex != -1 && lb.SelectedIndex < currentOrder.bestellung.Count)
        {
            btnRemoveItem.Visibility = Visibility.Visible;
            
            return;
        }

        btnRemoveItem.Visibility = Visibility.Hidden;
    }

    private void btnRemoveItem_Click(object sender, RoutedEventArgs e)
    {
        
        int selectedIndex = lbOrderComplete.SelectedIndex;
        currentOrder.bestellung.RemoveAt(selectedIndex);
        lbOrderComplete.Items.RemoveAt(selectedIndex);
        lbOrderCompletePrice.Items.RemoveAt(selectedIndex);

        if (currentOrder.bitandbitecard)
        {
            int lastIndex = lbOrderComplete.Items.Count - 1;

            lbOrderComplete.Items.RemoveAt(lastIndex);
            lbOrderCompletePrice.Items.RemoveAt(lastIndex);
        }

        
        DisplayTotalPrice();
        btnRemoveItem.Visibility = Visibility.Hidden;

        

    }

    private void DisplayTotalPrice()
    {
        double total = currentOrder.BerechneBestellung();
        double rabatt = total * 0.05 / 0.95;

        if (currentOrder.bitandbitecard)
        {

            lbOrderComplete.Items.Add($"- BitAndBiteCardRabatt");
            lbOrderCompletePrice.Items.Add($"{rabatt.ToString("0.00")} EUR");
        }

        tbOrderCompleteTotal.Text = $"Summe \t {total.ToString("0.00")} EUR";

    }
}