using DAL.Integrity;
using System.Data;

namespace Mappers.Integrity
{
    public class DigitoVerificadorMapper_33ZS
    {
        private readonly DigitoVerificadorDataAccess_33ZS data = new DigitoVerificadorDataAccess_33ZS();

        public DataTable ObtenerTabla_33ZS(string tabla)
        {
            // El procedimiento contiene una lista cerrada de tablas y su orden estable.
            return data.ObtenerTabla_33ZS(tabla);
        }

        public void GuardarDV_33ZS(string tabla, string dvh, string dvv)
        {
            data.Guardar_33ZS(tabla, dvh, dvv);
        }

        public DataTable ObtenerDVGuardado_33ZS(string tabla)
        {
            return data.ObtenerGuardado_33ZS(tabla);
        }
    }
}
