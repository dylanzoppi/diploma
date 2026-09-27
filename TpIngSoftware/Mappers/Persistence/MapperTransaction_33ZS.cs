using DAL;
using System;

namespace Mappers.Persistence
{
    public static class MapperTransaction_33ZS
    {
        public static void Ejecutar_33ZS(Action operation)
        {
            Acceso_33ZS.EjecutarEnTransaccion_33ZS(operation);
        }
    }
}
