using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private double lastNumber, result;
        private SelectedOperator? currentOperator = null;
        private bool shouldClearOnNextDigit = false;

        public MainWindow()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.InvariantCulture;

            InitializeComponent();

            resultLabel.Content = "0";

            btnClear.Click += ClearButton_Click;

            btnNegate.Click += NegateButton_Click;

            btnPercent.Click += PercentButton_Click;

            btnEquals.Click += EqualsButton_Click;

        }

        private void EqualsButton_Click(object sender, RoutedEventArgs e)
        {
            resultLabel.Content = result.ToString();
        }


        private void PercentButton_Click(object sender, RoutedEventArgs e)
        {
            double tempNumb = 0;

            if (double.TryParse(resultLabel.Content.ToString(), out tempNumb))
            {
                tempNumb = tempNumb / 100;
                if (lastNumber != 0)
                {
                    tempNumb *= lastNumber;
                }
                resultLabel.Content = tempNumb.ToString();

            }
        }

        private void NegateButton_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(resultLabel.Content.ToString(), out lastNumber))
            {
                lastNumber = lastNumber * -1;
                resultLabel.Content = lastNumber.ToString();

            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            resultLabel.Content = "0";
        }

        private void btnDot_Click(object sender, RoutedEventArgs e)
        {
            if (shouldClearOnNextDigit)
            {
                resultLabel.Content = "0.";
                shouldClearOnNextDigit = false;
            }
            else if (!resultLabel.Content.ToString().Contains("."))
            {
                resultLabel.Content = $"{resultLabel.Content}.";
            }

        }

        private void OperationButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                string operation = button.Content.ToString();

                if (double.TryParse(resultLabel.Content.ToString(), out double number))
                {
                    if (operation == "=")
                    {
                        if (currentOperator != null)
                        {
                            double resultCalculated = Calculate(lastNumber, number, currentOperator.Value);
                            resultLabel.Content = resultCalculated.ToString();
                            lastNumber = resultCalculated;
                            result = resultCalculated;
                            currentOperator = null;
                            shouldClearOnNextDigit = true;
                        }
                    }
                    else
                    {
                        lastNumber = number;
                        currentOperator = ParseOperatorStrict(operation);
                        shouldClearOnNextDigit = true;
                    }
                }
            }
        }
        private double Calculate(double a, double b, SelectedOperator op)
        {
            switch (op)
            {
                case SelectedOperator.Addition:
                    return a + b;

                case SelectedOperator.Subtraction:
                    return a - b;

                case SelectedOperator.Multiplication:
                    return a * b;

                case SelectedOperator.Division:
                    {
                        if (b == 0)
                        {
                            MessageBox.Show("Division by 0 is not supporrted", "Wrong operation", MessageBoxButton.OK, MessageBoxImage.Error);
                            return 0;
                        }
                        return a / b;
                    }


                default:
                    return a;

            }
        }

        private void DigitButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                string digit = button.Content.ToString();

                if (resultLabel.Content.ToString() == "0" || shouldClearOnNextDigit)
                {
                    resultLabel.Content = digit;
                    shouldClearOnNextDigit = false;
                }
                else
                {
                    resultLabel.Content = $"{resultLabel.Content}{digit}";
                }

            }
        }

        private enum SelectedOperator
        {
            Addition, Subtraction, Multiplication, Division,


        }
        private SelectedOperator ParseOperatorStrict(string op)
        {
            return op switch
            {
                "+" => SelectedOperator.Addition,
                "-" => SelectedOperator.Subtraction,
                "*" => SelectedOperator.Multiplication,
                "/" => SelectedOperator.Division,
                _ => throw new ArgumentException($"I dkn: {op}")
            };
        }

    }
}
