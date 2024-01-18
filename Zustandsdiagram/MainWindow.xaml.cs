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

namespace Zustandsdiagram
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string codewordString = "555-Nase";

        public MainWindow()
        {
            InitializeComponent();
            lblSwitch.Background = Brushes.DarkRed;
        }

        private void btnClick_Click(object sender, RoutedEventArgs e)
        {
            if (tbCode.Text.Length == 0 | tbCode.Text != codewordString) 
            {
                MessageBox.Show("Codewort falsch");
                tbCode.Text = "";
                return;
            }

            if ((string)lblSwitch.Content == "EIN")
            {
                Ausschalten();
            }
            else
            {
                Einschalten();
            }
            tbCode.Text = "";
            
        }

        private void Ausschalten()
        {
            lblSwitch.Background = Brushes.DarkRed;
            lblSwitch.Content = "AUS";
        }

        private void Einschalten()
        {
            lblSwitch.Background = Brushes.DarkGreen;
            lblSwitch.Content = "EIN";
        }
    }
}