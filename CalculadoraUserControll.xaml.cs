using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Calculadora
{
    /// <summary>
    /// Lógica de interacción para CalculadoraUserControll.xaml
    /// </summary>
    public partial class CalculadoraUserControll : UserControl
    {
        public CalculadoraUserControll()
        {
            InitializeComponent();
        }

        // Event handler for number and operator buttons click
        private void ButtonNumberOperator_Click(object sender, RoutedEventArgs e)
        {
            // Append the clicked button's content (number or operator) to the input textbox
            tbInput.Text += (sender as Button).Content.ToString();

            // Focus on the input textbox to receive further inputs
            tbInput.Focus();

            // Evaluate the current expression and display the result in the output textbox
            tbOutput.Text = Calcula.EvaluarExpresion(tbInput.Text);
        }

        // Event handler for text input preview in the input textbox
        private void tbInput_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Use a regular expression to prevent invalid characters from being entered
            Regex regex = new Regex("[^0-9.*/+-]+");
            e.Handled = regex.IsMatch(e.Text);


        }

        // Event handler for the "Delete" button click
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            // Remove the last character from the input textbox
            if (tbInput.Text.Length > 0)
            {
                tbInput.Text = (tbInput.Text).Substring(0, tbInput.Text.Length - 1);
            }

            // Evaluate the updated expression and display the result in the output textbox
            tbOutput.Text = Calcula.EvaluarExpresion(tbInput.Text);
        }

        // Event handler for the "Delete All" button click
        private void btnDeleteAll_Click(object sender, RoutedEventArgs e)
        {
            // Clear the input textbox
            tbInput.Text = "";

            // Set the output textbox to display "0"
            tbOutput.Text = "0";
        }

        // Event handler for the "Igual" (Equal) button click
        private void btnIgual_Click(object sender, RoutedEventArgs e)
        {
            // Set the content of the input textbox to be equal to the current content of the output textbox
            // This allows the user to use the result of the previous expression in a new calculation.
            tbInput.Text = tbOutput.Text;
        }

        private void tbInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Evaluate the current expression and display the result in the output textbox
            if (tbInput.Text.Length > 0 && tbOutput!=null)
            {
                tbOutput.Text = Calcula.EvaluarExpresion(tbInput.Text);
            }
        }
    }
}
