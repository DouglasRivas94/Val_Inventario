using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Val_Inventario
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Transaccion> transacciones = new ObservableCollection<Transaccion>();
        private ObservableCollection<Existencia> existencias = new ObservableCollection<Existencia>();
        private LinkedList<ExistenciaData> existenciasDataCollection = new LinkedList<ExistenciaData>();
        private LinkedList<ExistenciaData> entradasLinkedList = new LinkedList<ExistenciaData>();

        public double cantidadDisponible = 0.0;
        private double saldoCantidadActual = 0.0;
        public MainWindow()
        {
            InitializeComponent();
            lblDisponible.Content = "Disponible: " + cantidadDisponible.ToString("");
            listView.ItemsSource = transacciones; // transaccion 
            listViewExistencias.ItemsSource = existencias; // existencias
            ExistenciaData primeraEntrada = existenciasDataCollection.FirstOrDefault(e => e.ConceptoPeps == "Entrada");
            if (primeraEntrada != null)
            {
                saldoCantidadActual = primeraEntrada.SaldoCantidad;
                lblSaldoCantidad.Content = "Saldo Cantidad: " + saldoCantidadActual.ToString("");
            }
        }

        public void AgregarTransaccion(Transaccion transaccion)
        {
            if (transacciones.Any(t => t.NumeroRegistro == transaccion.NumeroRegistro))
            {
                MessageBox.Show("Ya existe una transacción con el mismo número de registro. Por favor, elija otro número.");
                return;
            }
            transacciones.Add(transaccion);
            ActualizarTotalInventario();
            ActualizarListaTransacciones();
        }

        public void ActualizarTotalInventario()
        {
            double cantidadReal = CalcularCantidadRealEnInventario();
            lblDisponible.Content = "Disponible: " + cantidadReal.ToString("");
        }

        public double CalcularCantidadRealEnInventario()
        {
            double cantidadReal = 0.0;

            foreach (var transaccion in transacciones)
            {
                if (transaccion.Movimiento == "Entrada")
                {
                    cantidadReal += transaccion.Cantidad;
                }
                else if (transaccion.Movimiento == "Salida")
                {
                    cantidadReal -= transaccion.Cantidad;
                }
            }
            return cantidadReal;
        }

        public void ActualizarListaTransacciones()
        {
            listView.ItemsSource = null;
            listView.ItemsSource = transacciones;
        }

        private void Nuevo_Click(object sender, RoutedEventArgs e)
        {
            RegistroWindow registroWindow = new RegistroWindow(this, cantidadDisponible);
            registroWindow.ShowDialog();
        }

        private void Modificar_Click(object sender, RoutedEventArgs e)
        {
            ModificarWindow modificarWindow = new ModificarWindow();
            modificarWindow.MainWindowInstance = this;
            modificarWindow.CantidadDisponible = cantidadDisponible;
            modificarWindow.ShowDialog();
            ActualizarListaTransacciones();
        }

        private void Enviar_Click(object sender, RoutedEventArgs e)
        {
            foreach (Transaccion transaccion in transacciones)
            {
                if (transaccion.Movimiento == "Entrada")
                {
                    ExistenciaData nuevaExistencia = new ExistenciaData
                    {
                        FechaPeps = transaccion.Fecha,
                        ConceptoPeps = "Entrada",
                        CantidadEntradaPeps = transaccion.Cantidad,
                        ValorUnitarioEntradaPeps = transaccion.Precio,
                        TotalEntradaPeps = transaccion.Cantidad * transaccion.Precio,
                        SaldoCantidad = transaccion.Cantidad,
                        SaldoPrecio = transaccion.Precio,
                        SaldoTotal = transaccion.Cantidad * transaccion.Precio
                    };

                    ExistenciaData nuevaExistencia2 = new ExistenciaData
                    {
                        CantidadEntradaPeps = transaccion.Cantidad,
                        ValorUnitarioEntradaPeps = transaccion.Precio
                    };
                    existenciasDataCollection.AddLast(nuevaExistencia);
                    entradasLinkedList.AddLast(nuevaExistencia2);
                }
                else if (transaccion.Movimiento == "Salida")
                {
                    if (entradasLinkedList.Count > 0) // 
                    {
                        var current = entradasLinkedList.First;

                        var currentExistencia = current.Value;
                        if (transaccion.Cantidad < currentExistencia.CantidadEntradaPeps)
                        {
                            currentExistencia.CantidadEntradaPeps -= transaccion.Cantidad;
                            ExistenciaData existencia = new ExistenciaData
                            {
                                FechaPeps = transaccion.Fecha,
                                ConceptoPeps = transaccion.Movimiento,
                                CantidadSalidaPeps = transaccion.Cantidad,
                                ValorUnitarioSalidaPeps = currentExistencia.ValorUnitarioEntradaPeps,
                                TotalSalidaPeps = transaccion.Cantidad * currentExistencia.ValorUnitarioEntradaPeps,
                                SaldoCantidad = currentExistencia.CantidadEntradaPeps,
                                SaldoPrecio = currentExistencia.ValorUnitarioEntradaPeps,
                                SaldoTotal = currentExistencia.CantidadEntradaPeps * currentExistencia.ValorUnitarioEntradaPeps
                            };
                            existenciasDataCollection.AddLast(existencia);
                            lblSaldoCantidad.Content = "Saldo Cantidad: " + currentExistencia.SaldoCantidad.ToString("");
                            currentExistencia.CantidadEntradaPeps = currentExistencia.CantidadEntradaPeps;

                        }
                        else if (transaccion.Cantidad > currentExistencia.CantidadEntradaPeps)
                        {
                            var ns = transaccion.Cantidad - currentExistencia.CantidadEntradaPeps;
                            ExistenciaData existencia = new ExistenciaData
                            {
                                FechaPeps = transaccion.Fecha,
                                ConceptoPeps = transaccion.Movimiento,
                                CantidadSalidaPeps = currentExistencia.CantidadEntradaPeps,
                                ValorUnitarioSalidaPeps = currentExistencia.ValorUnitarioEntradaPeps,
                                TotalSalidaPeps = currentExistencia.CantidadEntradaPeps * currentExistencia.ValorUnitarioEntradaPeps
                            };
                            existenciasDataCollection.AddLast(existencia);
                            currentExistencia.CantidadEntradaPeps = 0;

                            if (entradasLinkedList.First != null)
                            {
                                var currentE = entradasLinkedList.First;
                                entradasLinkedList.RemoveFirst();
                                if (currentE.Next != null)
                                {
                                    currentE = currentE.Next;
                                    ExistenciaData existencia1 = new ExistenciaData
                                    {
                                        FechaPeps = transaccion.Fecha,
                                        ConceptoPeps = transaccion.Movimiento,
                                        CantidadSalidaPeps = transaccion.Cantidad,
                                        ValorUnitarioSalidaPeps = currentExistencia.ValorUnitarioEntradaPeps,
                                        TotalSalidaPeps = transaccion.Cantidad * currentExistencia.ValorUnitarioEntradaPeps,
                                        SaldoCantidad = currentExistencia.CantidadEntradaPeps,
                                        SaldoPrecio = currentExistencia.ValorUnitarioEntradaPeps,
                                        SaldoTotal = currentExistencia.CantidadEntradaPeps * currentExistencia.ValorUnitarioEntradaPeps
                                    };
                                    existenciasDataCollection.AddLast(existencia1);
                                    currentExistencia.CantidadEntradaPeps = currentExistencia.CantidadEntradaPeps;
                                    lblSaldoCantidad.Content = "Saldo Cantidad: " + currentExistencia.SaldoCantidad.ToString("");
                                }

                            }
                        }
                    }

                }

            }
            ActualizarListaExistencias();
        }

        private void ActualizarListaExistencias()
        {
            listViewExistencias.ItemsSource = existenciasDataCollection;
        }

        public void EliminarTransaccion(string numeroRegistro)
        {
            Transaccion transaccion = transacciones.FirstOrDefault(t => t.NumeroRegistro == numeroRegistro);

            if (transaccion != null)
            {
                double cantidadRealDespuesEliminacion = CalcularCantidadRealDespuesEliminacion(transaccion);
                if (cantidadRealDespuesEliminacion >= 0)
                {
                    transacciones.Remove(transaccion);
                    ActualizarTotalInventario();
                    ActualizarListaTransacciones();
                }
                else
                {
                    MessageBox.Show("La transacción no puede eliminarse ya que resultaría en un valor negativo en el inventario.");
                }
            }
        }

        private double CalcularCantidadRealDespuesEliminacion(Transaccion transaccion)
        {
            double cantidadReal = CalcularCantidadRealEnInventario();

            if (transaccion.Movimiento == "Entrada")
            {
                cantidadReal -= transaccion.Total;
            }
            else if (transaccion.Movimiento == "Salida")
            {
                cantidadReal += transaccion.Total;
            }

            return cantidadReal;
        }

        public void ActualizarTotalInventario(double total)
        {
            cantidadDisponible = total;
            lblDisponible.Content = "Disponible: " + cantidadDisponible.ToString("0.00");
        }
        private void Eliminar_Click(object sender, RoutedEventArgs e)
        {
            EliminarWindow eliminarWindow = new EliminarWindow();
            eliminarWindow.ShowDialog();
        }
    }
}