using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Val_Inventario
{
    public class Transaccion
    {
        public string NumeroRegistro { get; set; }
        public string Movimiento { get; set; }
        public double Precio { get; set; }
        public double Cantidad { get; set; }
        public double Total { get; set; }
        public string Fecha { get; set; }

    }
}
