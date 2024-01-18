using Microsoft.Win32;
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

namespace Industrieanlage
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Anlage currentMachine = new Anlage("Deathstar");
        List<Komponententyp> typen = new();
        List<Komponente> kompos = new();


        public MainWindow()
        {
            InitializeComponent();
            cnvAendern.Visibility = Visibility.Hidden;
            ReadContentFromDatabase_Simulated();

            UpdateWindowTitle();
            UpdateMachineLabel();

            tbxAnlagwert.Text = currentMachine.Gesamtpreis.ToString("c");
            InitKomponenten();



        }

        private void InitKomponenten()
        {
            cmbNeueKomponente.Items.Clear();
            
            foreach (Komponente k in kompos)
            {
                cmbNeueKomponente.Items.Add(k);
            }
        }

        private void UpdateMachineLabel()
        {
            lblKomponente.Content = currentMachine.Name;
        }

        private void UpdateWindowTitle()
        {
            string name = currentMachine.Name;
            string idCode = name.Substring(0, 2).ToUpper();
            string id = currentMachine.ID.ToString();
            wndAnlage.Title = $"{name} : {idCode} - {id}";
        }

        private void ReadContentFromDatabase_Simulated()
        {

            typen.Add(new Komponententyp(99.95m));
            typen.Add(new Komponententyp(12.17m));
            typen.Add(new Komponententyp(123.45m));
            typen.Add(new Komponententyp(10000.01m));
            typen.Add(new Komponententyp(0.01m));
            typen.Add(new Komponententyp(10000.02m));


            kompos.Add(new Komponente(typen[0], "Laser"));
            kompos.Add(new Komponente(typen[1], "Nebelmaschine"));
            kompos.Add(new Komponente(typen[2], "Brücke"));
            kompos.Add(new Komponente(typen[0], "Brigg"));
            kompos.Add(new Komponente(typen[3], "Kajüte"));

            kompos.Add(new Komponente(typen[0], "Küche"));
            kompos.Add(new Komponente(typen[4], "Wurstpresse"));
            kompos.Add(new Komponente(typen[2], "Katapult"));
            kompos.Add(new Komponente(typen[5], "Feuerlöscherraum"));
            kompos.Add(new Komponente(typen[3], "Leiter"));

            kompos.Add(new Komponente(typen[1], "Fahrstuhl"));
            kompos.Add(new Komponente(typen[1], "Laufstuhl"));
            kompos.Add(new Komponente(typen[3], "Disco"));
            kompos.Add(new Komponente(typen[4], "Qualmfabrik"));
            kompos.Add(new Komponente(typen[3], "Quäulfabrik"));
        }

        private void cmbNeueKomponente_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //MessageBox.Show("Hey");
            btnNeueKomponente.IsEnabled = true;
        }

        private void btnNeueKomponente_Click(object sender, RoutedEventArgs e)
        {
            if (cmbNeueKomponente.SelectedIndex == -1)
            {
                return;
            }

            Komponente selectedKomponente = (Komponente)cmbNeueKomponente.Items[cmbNeueKomponente.SelectedIndex];

            if (selectedKomponente != null)
            {
                AddKomponenteToCurrentMachine(selectedKomponente);
                AddKomponenteToListbox(selectedKomponente);
                
                UpdateAnlagewert();
                UpdateTextBoxAnlagewert();
            }

        }

        private void AddKomponenteToListbox(Komponente selectedKomponente)
        {
            lbKomponente.Items.Add(selectedKomponente);
        }

        private void AddKomponenteToCurrentMachine(Komponente selectedKomponente)
        {
            currentMachine.Komponenten.Add(selectedKomponente);
        }

        private void UpdateAnlagewert()
        {
            currentMachine.BerechneAnlagewert();
        }

        private void UpdateTextBoxAnlagewert()
        {
            
            tbxAnlagwert.Text = currentMachine.Gesamtpreis.ToString("c");
        }

        private void btnLoeschen_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = lbKomponente.SelectedIndex;
            if (selectedIndex == -1)
                return;

            currentMachine.Komponenten.RemoveAt(selectedIndex);
            lbKomponente.Items.RemoveAt(selectedIndex);

            lbKomponente.SelectedIndex = -1;
            SetEnableModifyButtons(false);

            UpdateAnlagewert();
            UpdateTextBoxAnlagewert();
            

            if (lbKomponente.Items.Count == 0)
            {

                lbKomponente.Items.Clear();
            }
        }

        private void btnAendern_Click(object sender, RoutedEventArgs e)
        {
            
            WindowAendern window = new WindowAendern();
            window.GetListBox(lbKomponente);
            window.GetKomponente(lbKomponente.SelectedItem);
            window.Show();
        }

        private void lbKomponente_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetEnableModifyButtons(true);
        }

        void SetEnableModifyButtons(bool isEnabled)
        {
            btnAendern.IsEnabled = isEnabled;
            btnLoeschen.IsEnabled = isEnabled;
        }

    }
}