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
    public partial class EliminarWindow : Window
    {
        public EliminarWindow()
        {
            InitializeComponent();
        }
        private void Confirmar_Click(object sender, RoutedEventArgs e)
        {
            string numeroRegistroEliminar = txtNumeroRegistro.Text;

            if (!string.IsNullOrEmpty(numeroRegistroEliminar))
            {
                if (Application.Current.MainWindow is MainWindow mainWindow)
                {
                    try
                    {
                        mainWindow.EliminarTransaccion(numeroRegistroEliminar);
                    }
                    catch (InvalidOperationException ex)
                    {
                        MessageBox.Show("La transacción no puede eliminarse ya que resultaría en un valor negativo en el inventario.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, ingrese el número de registro de la transacción.");
            }
        }
        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
