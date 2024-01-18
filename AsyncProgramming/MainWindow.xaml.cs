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
using System.Threading;

namespace AsyncProgramming
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await ComputeAsync();
            ComputeAsync();
            MessageBox.Show("Fertig");
        }

        private static void Compute()
        {
            Thread.Sleep(2_000);
            
            MessageBox.Show("Dauert aber lang heut, nich?");
        }

        private static Task ComputeAsync()
        {
            
            return Task.Run(() => Compute());
            
        }
    }
}