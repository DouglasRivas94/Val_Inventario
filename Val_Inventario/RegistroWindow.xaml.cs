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


namespace Val_Inventario
{
    public partial class RegistroWindow : Window
    {
        private MainWindow mainWindow;
        private double cantidadDisponible;

        public RegistroWindow(MainWindow mainWindow, double cantidadDisponible)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            this.cantidadDisponible = cantidadDisponible;
            txtPrecio.TextChanged += TxtPrecio_TextChanged;
            txtCantidad.TextChanged += TxtCantidad_TextChanged;
        }

        private void TxtPrecio_TextChanged(object sender, TextChangedEventArgs e)
        {
            ActualizarTotal();
        }

        private void TxtCantidad_TextChanged(object sender, TextChangedEventArgs e)
        {
            ActualizarTotal();
        }

        private void ActualizarTotal()
        {
            if (double.TryParse(txtPrecio.Text, out double precio) && int.TryParse(txtCantidad.Text, out int cantidad))
            {
                double total = precio * cantidad;
                lblTotal.Content = total.ToString("0.00");
            }
            else
            {
                lblTotal.Content = "0";
            }
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNumeroRegistro.Text) || cmbMovimiento.SelectedIndex == -1 ||
                string.IsNullOrWhiteSpace(txtPrecio.Text) || string.IsNullOrWhiteSpace(txtCantidad.Text) ||
                dpFecha.SelectedDate == null)
            {
                MessageBox.Show("Por favor, complete todos los campos.");
                return;
            }

            string _numeroRegistro = txtNumeroRegistro.Text;
            string _movimiento = ((ComboBoxItem)cmbMovimiento.SelectedItem).Content.ToString();
            double _precio = double.Parse(txtPrecio.Text);
            int _cantidad = int.Parse(txtCantidad.Text);
            DateTime _fecha = dpFecha.SelectedDate ?? DateTime.Now;
            string fechaFormateada = _fecha.ToString("dd/MM/yyyy");
            double total = CalcularTotal(_precio, _cantidad);

            if (_movimiento == "Salida" && _cantidad > cantidadDisponible)
            {
                MessageBox.Show("No hay suficiente cantidad disponible para realizar la salida.");
                return;
            }

            Transaccion nuevaTransaccion = new Transaccion
            {
                NumeroRegistro = _numeroRegistro,
                Movimiento = _movimiento,
                Precio = _precio,
                Cantidad = _cantidad,
                Total = total,
                Fecha = fechaFormateada
            };
            if (mainWindow != null)
            {
                mainWindow.AgregarTransaccion(nuevaTransaccion);
            }

            cantidadDisponible = mainWindow.CalcularCantidadRealEnInventario();
            mainWindow.ActualizarTotalInventario(cantidadDisponible);
            mainWindow.ActualizarTotalInventario(cantidadDisponible);
            this.Close();
        }

        public double CalcularTotal(double precio, int cantidad)
        {
            return precio * cantidad;
        }

        private void Deshacer_Click(object sender, RoutedEventArgs e)
        {
            txtNumeroRegistro.Clear();
            cmbMovimiento.SelectedIndex = -1;
            txtPrecio.Clear();
            txtCantidad.Clear();
            dpFecha.SelectedDate = null;
            lblTotal.Content = "0";
        }

        private void Salir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}