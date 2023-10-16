using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Val_Inventario
{
    public class ExistenciaData
    {
        public string FechaPeps { get; set; }
        public string ConceptoPeps { get; set; }
        public double CantidadEntradaPeps { get; set; }
        public double ValorUnitarioEntradaPeps { get; set; }
        public double TotalEntradaPeps { get; set; }
        public double CantidadSalidaPeps { get; set; }
        public double ValorUnitarioSalidaPeps { get; set; }
        public double TotalSalidaPeps { get; set; }
        public double SaldoCantidad { get; set; }
        public double SaldoPrecio { get; set; }
        public double SaldoTotal { get; set; }
    }
}
