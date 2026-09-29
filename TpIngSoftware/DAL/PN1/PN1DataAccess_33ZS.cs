using System;
using System.Data;
using System.Data.SqlClient;

namespace DAL.PN1
{
    public sealed class PN1DataAccess_33ZS
    {
        private readonly Acceso_33ZS acceso = new Acceso_33ZS();
        private static SqlParameter P(string name, object value) =>
            new SqlParameter(name, value ?? DBNull.Value);

        private DataTable Tabla(string sp, params SqlParameter[] parametros) =>
            acceso.ExecuteDataSet_33ZS(sp, parametros).Tables[0];

        public bool DvPendiente() =>
            !Convert.ToBoolean(acceso.ExecuteScalar_33ZS("dbo.PN1_DVPendiente_33ZS"));

        public bool HayUsuarios() => Tabla("dbo.Usuario_ObtenerTodos_33ZS").Rows.Count > 0;

        public void ConfirmarDv() =>
            acceso.ExecuteNonQuery_33ZS("dbo.PN1_ConfirmarDV_33ZS");

        public bool CargaDemoPendiente() =>
            Convert.ToBoolean(acceso.ExecuteScalar_33ZS("dbo.CargaDemo_Pendiente_33ZS"));

        public void ConfirmarCargaDemo() =>
            acceso.ExecuteNonQuery_33ZS("dbo.CargaDemo_Confirmar_33ZS");

        public DataTable BuscarClientes(string termino) =>
            Tabla("dbo.Cliente_Buscar_33ZS", P("@Termino", termino));

        public int RegistrarCliente(string nombre, string apellido, string telefono, string correo) =>
            Convert.ToInt32(acceso.ExecuteScalar_33ZS("dbo.Cliente_Registrar_33ZS",
                new[] { P("@Nombre", nombre), P("@Apellido", apellido),
                        P("@Telefono", telefono), P("@Correo", correo) }));

        public DataTable ListarBarberos() => Tabla("dbo.Barbero_Listar_33ZS");

        public void ConfigurarBarbero(string dni, decimal porcentaje, bool activo) =>
            acceso.ExecuteNonQuery_33ZS("dbo.Barbero_Configurar_33ZS",
                new[] { P("@UsuarioDNI", dni), P("@PorcentajeComision", porcentaje), P("@Activo", activo) });

        public DataTable ListarServicios() => Tabla("dbo.Servicio_Listar_33ZS");
        public DataTable ListarConsumos(int servicioId) =>
            Tabla("dbo.Servicio_Consumos_33ZS", P("@ServicioID", servicioId));

        public int GuardarServicio(int? servicioId, string nombre, decimal precio,
            bool activo, DataTable consumos)
        {
            var parametroConsumos = new SqlParameter("@Consumos", SqlDbType.Structured)
            {
                TypeName = "dbo.ConsumosServicio_33ZS",
                Value = consumos
            };
            return Convert.ToInt32(acceso.ExecuteScalar_33ZS("dbo.Servicio_Guardar_33ZS",
                new[] { P("@ServicioID", servicioId), P("@Nombre", nombre),
                    P("@Precio", precio), P("@Activo", activo), parametroConsumos }));
        }

        public DataTable ListarInsumos() => Tabla("dbo.Insumo_Listar_33ZS");

        public void AjustarStock(int insumoId, decimal stock, decimal minimo, string dni, string motivo) =>
            acceso.ExecuteNonQuery_33ZS("dbo.Insumo_AjustarStock_33ZS",
                new[] { P("@InsumoID", insumoId), P("@StockNuevo", stock),
                        P("@StockMinimo", minimo), P("@UsuarioDNI", dni), P("@Motivo", motivo) });

        public DataTable ListarMedios() => Tabla("dbo.MedioPago_Listar_33ZS");

        public int RegistrarAtencion(Guid operacionId, int clienteId, string barberoDni,
            string recepcionistaDni, int servicioId, int medioPagoId, decimal importe) =>
            Convert.ToInt32(acceso.ExecuteScalar_33ZS("dbo.Atencion_Registrar_33ZS",
                new[] { P("@OperacionID", operacionId), P("@ClienteID", clienteId),
                        P("@BarberoDNI", barberoDni), P("@RecepcionistaDNI", recepcionistaDni),
                        P("@ServicioID", servicioId), P("@MedioPagoID", medioPagoId),
                        P("@Importe", importe) }));

        public DataTable AtencionesPropias(string dni, DateTime desde, DateTime hastaExclusivo) =>
            Tabla("dbo.Atencion_Propias_33ZS", P("@BarberoDNI", dni),
                P("@Desde", desde), P("@HastaExclusivo", hastaExclusivo));

        public DataTable Reporte(DateTime desde, DateTime hastaExclusivo,
            string barberoDni, int? servicioId, int? medioPagoId) =>
            Tabla("dbo.Atencion_Reporte_33ZS", P("@Desde", desde),
                P("@HastaExclusivo", hastaExclusivo), P("@BarberoDNI", barberoDni),
                P("@ServicioID", servicioId), P("@MedioPagoID", medioPagoId));
    }
}
