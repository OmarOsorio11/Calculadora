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

        // Controlador de eventos para hacer clic en botones de números y operadores
        private void ButtonNumberOperator_Click(object sender, RoutedEventArgs e)
        {
            // Agregar el contenido del botón clicado (número u operador) al cuadro de texto de entrada
            tbInput.Text += (sender as Button).Content.ToString();

            // Enfocar en el cuadro de texto de entrada para recibir más entradas
            tbInput.Focus();

            // Evaluar la expresión actual y mostrar el resultado en el cuadro de texto de salida
            tbOutput.Text = Calcula.EvaluarExpresion(tbInput.Text);
        }

        // Controlador de eventos para la vista previa de entrada de texto en el cuadro de texto de entrada
        private void tbInput_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Usar una expresión regular para evitar que se ingresen caracteres no válidos
            Regex regex = new Regex("[^0-9.*/+-]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        // Controlador de eventos para el clic en el botón "Eliminar"
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            // Eliminar el último carácter del cuadro de texto de entrada
            if (tbInput.Text.Length > 0)
            {
                tbInput.Text = (tbInput.Text).Substring(0, tbInput.Text.Length - 1);
            }

            // Evaluar la expresión actualizada y mostrar el resultado en el cuadro de texto de salida
            tbOutput.Text = Calcula.EvaluarExpresion(tbInput.Text);
        }

        // Controlador de eventos para el clic en el botón "Borrar todo"
        private void btnDeleteAll_Click(object sender, RoutedEventArgs e)
        {
            // Limpiar el cuadro de texto de entrada
            tbInput.Text = "";

            // Establecer el cuadro de texto de salida para mostrar "0"
            tbOutput.Text = "0";
        }

        // Controlador de eventos para el clic en el botón "Igual"
        private void btnIgual_Click(object sender, RoutedEventArgs e)
        {
            // Establecer el contenido del cuadro de texto de entrada para que sea igual al contenido actual del cuadro de texto de salida
            // Esto permite al usuario usar el resultado de la expresión anterior en un nuevo cálculo.
            tbInput.Text = tbOutput.Text;
        }

        // Controlador de eventos para el cambio de texto en el cuadro de texto de entrada
        private void tbInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Evaluar la expresión actual y mostrar el resultado en el cuadro de texto de salida
            if (tbInput.Text.Length > 0 && tbOutput != null)
            {
                tbOutput.Text = Calcula.EvaluarExpresion(tbInput.Text);
            }
        }
    }
}
