using Servicios;
using Mappers.Security;
using System;
using System.Collections.Generic;

namespace BLL
{
    public class BitacoraEventoBLL_33ZS
    {
        BitacoraEventoMapper_33ZS bitacoraDAL = new BitacoraEventoMapper_33ZS();

        public void RegistrarEvento_33ZS(BitacoraEvento_33ZS evento)
        {
            bitacoraDAL.RegistrarEvento_33ZS(evento);
        }

        public List<BitacoraEvento_33ZS> ObtenerEventos_33ZS()
        {
            Usuario_33ZS actual = SessionManager_33ZS.HaySesionActiva_33ZS()
                ? SessionManager_33ZS.GetInstance_33ZS().UsuarioActual_33ZS : null;
            if (actual == null || !actual.Activo_33ZS || actual.Bloqueo_33ZS ||
                !new PerfilBLL_33ZS().RolTienePatente_33ZS(actual.Rol_33ZS, "ConsultarBitacora"))
                throw new UnauthorizedAccessException("Su rol no tiene permiso para esta operación.");
            return bitacoraDAL.ObtenerEventos_33ZS();
        }
    }
}
