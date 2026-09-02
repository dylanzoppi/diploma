using DAL;
using Servicios;
using System;
using System.Collections.Generic;

namespace BLL
{
    public class BitacoraEventoBLL_33ZS
    {
        BitacoraEventoDAL_33ZS bitacoraDAL = new BitacoraEventoDAL_33ZS();

        public void RegistrarEvento_33ZS(BitacoraEvento_33ZS evento)
        {
            bitacoraDAL.RegistrarEvento_33ZS(evento);
        }

        public List<BitacoraEvento_33ZS> ObtenerEventos_33ZS()
        {
            return bitacoraDAL.ObtenerEventos_33ZS();
        }
    }
}