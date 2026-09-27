using Mappers.PN1;
using Mappers.Persistence;
using System;
using System.Linq;

namespace BLL.PN1
{
    public static class PN1Inicializacion_33ZS
    {
        public static void CompletarMigracion_33ZS()
        {
            var mapper = new PN1Mapper_33ZS();
            if (!mapper.DvPendiente()) return;
            if (mapper.HayUsuarios())
            {
                var otrosErrores = new DigitoVerificadorBLL_33ZS().Verificar_33ZS()
                    .Where(tabla => tabla != "Rol" && tabla != "Patente" && tabla != "Rol_Patente")
                    .ToList();
                if (otrosErrores.Count > 0)
                    throw new InvalidOperationException("Hay inconsistencias de integridad ajenas a PN1: " +
                        string.Join(", ", otrosErrores) + ".");
            }
            MapperTransaction_33ZS.Ejecutar_33ZS(() =>
            {
                new DigitoVerificadorBLL_33ZS().GenerarTodo_33ZS();
                mapper.ConfirmarDv();
            });
        }
    }
}
