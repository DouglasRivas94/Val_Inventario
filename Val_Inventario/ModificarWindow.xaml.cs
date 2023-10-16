using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

    public partial class ModificarWindow : Window
    {
        public MainWindow MainWindowInstance { get; set; }
        public double CantidadDisponible { get; set; }
        public Transaccion Transaccion { get; set; }
        public ObservableCollection<Transaccion> Transacciones { get; set; }

        public ModificarWindow()
        {
            InitializeComponent();
        }

        private void Modificar_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;

            if (mainWindow != null)
            {
                string numeroRegistroModificar = txtNumeroRegistroModificar.Text;

                Transaccion transaccionModificar = mainWindow.transacciones.FirstOrDefault(t => t.NumeroRegistro == numeroRegistroModificar);

                if (transaccionModificar != null)
                {
                    double nuevoPrecio;
                    if (double.TryParse(txtNuevoPrecio.Text, out nuevoPrecio))
                    {
                        transaccionModificar.Precio = nuevoPrecio;
                    }
                    else
                    {
                        MessageBox.Show("Ingrese un precio válido.");
                        return;
                    }

                    int nuevaCantidad;
                    if (int.TryParse(txtNuevaCantidad.Text, out nuevaCantidad))
                    {
                        double cantidadOriginal = transaccionModificar.Cantidad;
                        double diferenciaCantidad = nuevaCantidad - transaccionModificar.Cantidad;

                        if (transaccionModificar.Movimiento == "Salida" && diferenciaCantidad > 0 && diferenciaCantidad > cantidadOriginal)
                        {
                            MessageBox.Show("No puede sacar más de la cantidad original disponible.");
                            return;
                        }
                        transaccionModificar.Cantidad = nuevaCantidad;
                        transaccionModificar.Total = transaccionModificar.Precio * nuevaCantidad;

                        double diferenciaValorMonetario = diferenciaCantidad * transaccionModificar.Precio;

                        if (transaccionModificar.Movimiento == "Entrada")
                        {
                            mainWindow.cantidadDisponible += diferenciaValorMonetario;
                        }
                        else if (transaccionModificar.Movimiento == "Salida")
                        {
                            mainWindow.cantidadDisponible -= diferenciaValorMonetario;
                        }

                        mainWindow.ActualizarTotalInventario();
                        mainWindow.ActualizarListaTransacciones();
                        MessageBox.Show("Transacción modificada correctamente.");
                    }
                    else
                    {
                        MessageBox.Show("Ingrese una cantidad válida.");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Transacción no encontrada. Verifique el número de registro.");
                }
            }
        }
        private void Salir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
