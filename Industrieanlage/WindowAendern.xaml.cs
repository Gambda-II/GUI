using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Industrieanlage
{
    /// <summary>
    /// Interaction logic for WindowAendern.xaml
    /// </summary>
    public partial class WindowAendern : Window
    {
        private int state = 0;
        private const int maxState = 2;
        private Komponente Selected;
        private ListBox listbox;

        string name;
        Komponententyp typ;

        public WindowAendern()
        {
            InitializeComponent();

            //InitState();
        }

        public void GetListBox(ListBox listbox)
        {
            this.listbox = listbox;
        }

        public void GetKomponente(Object k)
        {
            if (k != null & k is Komponente)
            {

                Selected = k as Komponente;
                InitState();
            }

        }

        private void InitState()
        {
            tbxAlterWert.Text = Selected.Name;
        }

        private void btnWeiter_Click(object sender, RoutedEventArgs e)
        {
            if (state < maxState)
            {
                ChangeState(++state);
                pbAendern.Value += 50;
            }


        }

        private void ChangeState(int newState)
        {
            if (newState > 0)
            {
                btnZurueck.IsEnabled = true;
            }
            else
            {
                btnZurueck.IsEnabled = false;
            }

            if (newState >= maxState)
            {
                btnSpeichern.Visibility = Visibility.Hidden;
                btnSpeichern.Visibility = Visibility.Visible;
                btnSpeichern.IsEnabled = true;
            }
            else
            {
                btnSpeichern.Visibility = Visibility.Visible;
                btnSpeichern.Visibility = Visibility.Hidden;
                btnSpeichern.IsEnabled = false;
            }

            tbxNeuerWert.Focusable = true;
            switch (newState)
            {
                case 0:
                    InitState();
                    break;
                case 1:
                    if (name == null)
                    {

                        name = tbxNeuerWert.Text;
                    }
                    tbxAlterWert.Text = Selected.Typ.ToString();

                    break;
                case 2:
                    decimal price = Selected.Typ.Preis;
                    decimal.TryParse(tbxNeuerWert.Text, out price);
                    typ = new Komponententyp(decimal.Floor(price * 100) / 100);
                    tbxNeuerWert.Focusable = false;


                    break;
                default:

                    break;
            }
        }

        private void btnSpeichern_Click(object sender, RoutedEventArgs e)
        {


            Selected = new Komponente(typ, name);
            //MessageBox.Show($"{Selected.Name} und {Selected.Typ}");
            int index = listbox.SelectedIndex;
            listbox.Items[index] = Selected;


            this.Close();
        }

        private void btnZurueck_Click(object sender, RoutedEventArgs e)
        {
            if (state > 0)
            {
                ChangeState(--state);
                pbAendern.Value -= 50;
            }
        }
    }
}
