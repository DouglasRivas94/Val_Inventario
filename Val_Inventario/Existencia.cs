using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Val_Inventario
{
    public class Existencia
    {
        public DateTime Fecha { get; set; }
        public string Concepto { get; set; }
        public double Cantidad { get; set; }
        public double PrecioUnitario { get; set; }
        public double Total { get; set; }
    }
}