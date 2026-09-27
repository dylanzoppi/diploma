using DAL.Security;
using Servicios;
using System.Collections.Generic;
using System.Data;

namespace Mappers.Security
{
    public class BitacoraEventoMapper_33ZS
    {
        private readonly EventoDataAccess_33ZS data = new EventoDataAccess_33ZS();

        public void RegistrarEvento_33ZS(BitacoraEvento_33ZS evento)
        {
            data.Registrar_33ZS(evento.Login_33ZS, evento.Fecha_33ZS, evento.Hora_33ZS,
                evento.Modulo_33ZS, evento.NombreEvento_33ZS, evento.Criticidad_33ZS);
        }

        public List<BitacoraEvento_33ZS> ObtenerEventos_33ZS()
        {
            List<BitacoraEvento_33ZS> result = new List<BitacoraEvento_33ZS>();
            DataTable table = data.ObtenerTodos_33ZS();
            foreach (DataRow row in table.Rows)
                result.Add(new BitacoraEvento_33ZS(row));
            return result;
        }
    }
}
