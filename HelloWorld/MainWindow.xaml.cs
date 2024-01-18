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

namespace HelloWorld
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        double firstNumber = 0, secondNumber = 0;

        // Code behind -> Der Code nach der Schnittstelle
        public MainWindow()
        {
            InitializeComponent();
            ResetTextFields();


        }

        private void ResetTextFields()
        {
            txtOutputSolution.Text = " Waiting for inputs...";
            tbFirstInputNumber.Text = "0";
            tbSecondInputNumber.Text = "0";
        }



        private void btnAddition_Click(object sender, RoutedEventArgs e)
        {
            txtOutputSolution.Text.Replace("?", "+");
            txtOutputSolution.Text = $"\n{firstNumber + secondNumber}";
        }
        private void btnSubtraction_Click(object sender, RoutedEventArgs e)
        {
            txtOutputSolution.Text.Replace("?", "─");
            txtOutputSolution.Text = $"\n{firstNumber - secondNumber}";
        }

        private void btnMultiply_Click(object sender, RoutedEventArgs e)
        {
            txtOutputSolution.Text.Replace("?", "•");
            txtOutputSolution.Text = $"\n{firstNumber * secondNumber}";
        }

        private void btnDivision_Click(object sender, RoutedEventArgs e)
        {
            txtOutputSolution.Text.Replace("?", "∕");
            txtOutputSolution.Text = $"\n{firstNumber / secondNumber}";
        }



        private void tbInputNumber_TextChanged(object sender, TextChangedEventArgs e)
        {

            txtOutputSolution.Text = "";
            string content = (sender as TextBox).Text;
            if (tbFirstInputNumber != null && tbSecondInputNumber != null)
                txtOutputSolution.Text += tbFirstInputNumber.Text + " ? " + tbSecondInputNumber.Text + " = ";
        }

        private void tbInputNumber_LostFocus(object sender, RoutedEventArgs e)
        {
            double number;


            if (sender is TextBox t)
            {
                string content = t.Text;

                if (!double.TryParse(content, out number))
                {
                    // to check
                    number = 0;
                }

                if (t.Name.StartsWith("tbFirst"))
                {
                    firstNumber = number;
                    return;
                }
                if (t.Name.StartsWith("tbSecond"))
                {
                    secondNumber = number;
                    return;
                }
            }

        }


        private void tbInputNumber_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox t)
            {
                t.Text = "";
                string content = t.Text;
                t.Text = content;
            }



        }
    }
}