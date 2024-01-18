using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;


namespace Bankautomat
{


    public partial class MainWindow : Window
    {

        // button dragging fields
        private const double _dragThreshold = 1.0;
        private bool _dragging;
        private Point startpos;
        private Button _draggedButton;
        CancellationTokenSource cancellation;
        //

        // hitbox flag
        private bool hitOccured = false;

        // animation flag
        private bool animationInProgress = false;

        // global Timer counter
        private int timerCounter = 0;

        // state "machine"
        private int currentState = 0;

        // input from numpad
        List<int> inputsPIN = new();
        bool inputsFinished = false;

        Card? currentCard = null;

        public MainWindow()
        {
            InitializeComponent();
            InitNumpad();
            grdNumpad.Visibility = Visibility.Hidden;
            cnvMainDisplay.Visibility = Visibility.Hidden;
            _draggedButton = btnCard;
            currentCard = new Card();
            MessageBox.Show($"ID: {currentCard.CardID} \nPIN:{currentCard.PIN} \nBalance: {currentCard.Balance.ToString("c")}");
        }

        private void InitNumpad()
        {
            /*
            int numRows = 4; 

            for (int i = 0; i < numRows; i++)
            {
                RowDefinition rowDefinition = new RowDefinition();
                grdNumpad.RowDefinitions.Add(rowDefinition);
            }

            
            int numColumns = 4; 

            for (int i = 0; i < numColumns; i++)
            {
                ColumnDefinition columnDefinition = new ColumnDefinition();
                grdNumpad.ColumnDefinitions.Add(columnDefinition);
            }
            
            // grdNumpad.Children.Add();

            */
        }

        private void btnCard_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left)
                return;

            _dragging = false;
            _draggedButton = btnCard;
            startpos = e.GetPosition(_draggedButton);

            cancellation?.Cancel();
            cancellation = new CancellationTokenSource();
        }

        private async void btnCard_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (_draggedButton == null)
                return;

            var currentpos = e.GetPosition(_draggedButton);

            var delta = currentpos - startpos;
            if ((delta.Length > _dragThreshold || _dragging) && e.LeftButton == MouseButtonState.Pressed)
            {
                _dragging = true;

                // Update the position of the button instead of the window
                Canvas.SetLeft(_draggedButton, Canvas.GetLeft(_draggedButton) + delta.X);
                Canvas.SetTop(_draggedButton, Canvas.GetTop(_draggedButton) + delta.Y);

                // Check if the button is within the borders of the rectangle
                Point buttonPosition = _draggedButton.TranslatePoint(new Point(0, 0), hitboxCardReader);

                if (buttonPosition.X >= 0 && buttonPosition.Y >= 0 &&
                    buttonPosition.X + _draggedButton.ActualWidth <= hitboxCardReader.ActualWidth &&
                    buttonPosition.Y + _draggedButton.ActualHeight <= hitboxCardReader.ActualHeight && !hitOccured)
                {
                    // Button is within the borders of the rectangle
                    // You can handle this case as needed
                    if (!hitOccured)
                    {

                        hitboxCardReader.IsEnabled = false;
                        hitOccured = true;

                        progCardReader.Visibility = Visibility.Visible;
                        progCardReader.IsEnabled = true;

                        btnCard.Visibility = Visibility.Hidden;

                        //Logic.ValidationProcess(currentCard);
                        Logic.CheckID(currentCard);
                        DisplayValidationProcess(currentState);

                        // DOES NOT WORK = CRAAAAAAAAAAAASH

                        //AnimateRotation(_draggedButton, 90.0, TimeSpan.FromSeconds(0.5));

                        // Wait for a short duration before scaling down
                        //await Task.Delay(TimeSpan.FromSeconds(0.5));

                        // Scale the button down to a small line
                        //AnimateScale(_draggedButton, 1.0, 0.2, TimeSpan.FromSeconds(0.5));
                    }
                }
            }
        }

        private void DisplayValidationProcess(int state)
        {

            switch (state)
            {
                case 0:
                    ProcessID();
                    break;
                case 1:
                    InputPIN();
                    break;
                case 2:
                    ProcessPIN();
                    break;
                case 3:
                    MainButtonPressed();
                    break;
                default:
                    InvalidCard();
                    break;
            }
        }

        private void MainButtonPressed()
        {
            cnvMainDisplay.Visibility = Visibility.Visible;
            tblockShowBalance.Visibility = Visibility.Hidden;
        }

        private void InvalidCard()
        {
            tblockInfo.Text = "Karte einbehalten";
        }

        private void InputPIN()
        {
            grdNumpad.Visibility = Visibility.Visible;
            tblockInfo.Text = "Bitte PIN eingeben";
            tblockStatus.Text = "";


        }

        private void ProcessPIN()
        {
            Logic.CheckPIN(currentCard, inputsPIN);

            if (Logic.isCorrectPIN)
            {
                currentState++;
            }
            else
            {
                currentState += 100;
            }
            DisplayValidationProcess(currentState);
        }

        private void ProcessID()
        {
            tblockInfo.Text = "Bitte Warten";

            DispatcherTimer simulateValidationID = new DispatcherTimer();
            simulateValidationID.Tick += SimulateValidationID_Tick;
            simulateValidationID.Interval = new TimeSpan(0, 0, 0, 0, 200);
            simulateValidationID.Start();
            tblockStatus.Text = "";
        }

        private void SimulateValidationID_Tick(object? sender, EventArgs e)
        {
            timerCounter++;

            if (timerCounter <= 10)
            {
                tblockStatus.Text += "I";
                return;
            }

            if (timerCounter == 11)
            {
                tblockStatus.Text = DisplayValidationOutcomeID(Logic.isValidID);
                timerCounter = 0;
                (sender as DispatcherTimer).Stop();
                progCardReader.Visibility = Visibility.Hidden;
                currentState = currentState + (Logic.isValidID ? 1 : 100);
                DisplayValidationProcess(currentState);
                return;
            }


        }

        private string DisplayValidationOutcomeID(bool isValidID)
        {
            return isValidID ? "Karte akzeptiert" : "FEHLER!";
        }

        private void btnCard_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_dragging)
            {
                _dragging = false;
                e.Handled = true;
                _draggedButton.ReleaseMouseCapture();
                _draggedButton = null;
            }

            cancellation?.Cancel();
        }


        private async void AnimateScale(UIElement element, double startScale, double endScale, TimeSpan duration)
        {
            DateTime startTime = DateTime.Now;
            while ((DateTime.Now - startTime) < duration)
            {
                double progress = (DateTime.Now - startTime).TotalMilliseconds / duration.TotalMilliseconds;

                double scale = MathUtils.Lerp(startScale, endScale, progress);

                ScaleTransform scaleTransform = new ScaleTransform(scale, scale);
                element.RenderTransform = scaleTransform;

                await Task.Delay(16);
            }
        }

        private async void AnimateRotation(UIElement element, double endRotation, TimeSpan duration)
        {
            DateTime startTime = DateTime.Now;
            while ((DateTime.Now - startTime) < duration)
            {
                double progress = (DateTime.Now - startTime).TotalMilliseconds / duration.TotalMilliseconds;

                double rotation = MathUtils.Lerp(0, endRotation, progress);

                RotateTransform rotateTransform = new RotateTransform(rotation);
                element.RenderTransform = rotateTransform;

                await Task.Delay(160);
            }

            
            RotateTransform finalRotateTransform = new RotateTransform(endRotation);
            element.RenderTransform = finalRotateTransform;
        }

        public static class MathUtils
        {
            public static double Lerp(double start, double end, double t)
            {
                // Ensure t is in the [0, 1] range
                t = Math.Max(0, Math.Min(1, t));
                return start + (end - start) * t;
            }
        }

        private void ButtonMainDisplay_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            string tag = button.Tag.ToString();
            switch (tag.ToLower())
            {
                case "show":
                    cnvBalance.Visibility = Visibility.Hidden;
                    cnvValues.Visibility = Visibility.Hidden;
                    cnvWithdraw.Visibility = Visibility.Hidden;
                    tblockShowBalance.Visibility = Visibility.Visible;
                    tblockShowBalance.Text += $"({DateTime.Now}) \n {currentCard.Balance.ToString("c")}";
                    break;
                case "withdraw":
                    cnvValues.Visibility = Visibility.Visible;
                    
                    break;
                case "return":
                    btnCard.Visibility = Visibility.Visible;
                    cnvBalance.Visibility = Visibility.Hidden;
                    cnvWithdraw.Visibility = Visibility.Hidden;
                    cnvReturnCard.Visibility = Visibility.Hidden;
                    cnvMainDisplay.Visibility = Visibility.Hidden;
                    currentState = 0;
                    hitOccured = false;
                    break;
                default:
                    break;
            }
        }

        private void ButtonAddValue_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            string tag = button.Tag.ToString();
            decimal value = Convert.ToDecimal(tag);

            tblockStatus.Text = $"{value.ToString("c")}";
            tblockInfo.Text = currentCard.SubtractFromBalance(value) + $"\n{currentCard.Balance.ToString("c")}";
        }

        private void ButtonNumpad_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button)
            {
                return;
            }
            Button button = sender as Button;
            string tag = button.Tag as string;
            int value = -1;

            bool isNumberButton = int.TryParse(tag, out value);



            if (isNumberButton)
            {
                inputsPIN.Add(value);
                tblockStatus.Text += "*";
            }
            else
            {
                switch (tag.ToLower())
                {
                    case "clear":
                        inputsPIN.Clear();
                        tblockStatus.Text = "";
                        break;
                    case "undo":
                        int lastIndex = inputsPIN.Count - 1;
                        if (lastIndex >= 0)
                        {
                            string status = tblockStatus.Text;
                            tblockStatus.Text = status.Substring(0, status.Length - 1);
                            inputsPIN.RemoveAt(lastIndex);
                        }
                        break;
                    case "okay":
                        currentState++;
                        DisplayValidationProcess(currentState);
                        grdNumpad.Visibility = Visibility.Hidden;
                        tblockInfo.Text = "";
                        tblockStatus.Text = "";
                        break;
                    default:
                        break;
                }
            }

        }
    }
}