using System;
using System.Collections.Generic;
using System.Data;
using BE.PN1;
using DAL.PN1;

namespace Mappers.PN1
{
    public sealed class PN1Mapper_33ZS
    {
        private readonly PN1DataAccess_33ZS data = new PN1DataAccess_33ZS();

        public bool DvPendiente() => data.DvPendiente();
        public bool HayUsuarios() => data.HayUsuarios();
        public void ConfirmarDv() => data.ConfirmarDv();
        public bool CargaDemoPendiente() => data.CargaDemoPendiente();
        public void ConfirmarCargaDemo() => data.ConfirmarCargaDemo();

        private static List<T> Mapear<T>(DataTable table, Func<DataRow, T> mapear)
        {
            var result = new List<T>();
            foreach (DataRow row in table.Rows) result.Add(mapear(row));
            return result;
        }

        public List<Cliente_33ZS> BuscarClientes(string termino) =>
            Mapear(data.BuscarClientes(termino), r => new Cliente_33ZS
            {
                Id = Convert.ToInt32(r["ClienteID"]),
                Nombre = r["Nombre"].ToString(),
                Apellido = r["Apellido"].ToString(),
                Telefono = r["Telefono"].ToString(),
                Correo = r["Correo"] == DBNull.Value ? null : r["Correo"].ToString(),
                FechaAlta = Convert.ToDateTime(r["FechaAlta"])
            });

        public int RegistrarCliente(Cliente_33ZS cliente) =>
            data.RegistrarCliente(cliente.Nombre, cliente.Apellido, cliente.Telefono, cliente.Correo);

        public List<Barbero_33ZS> ListarBarberos() =>
            Mapear(data.ListarBarberos(), r => new Barbero_33ZS
            {
                Dni = r["DNI"].ToString(),
                Nombre = r["Nombre"].ToString(),
                Apellido = r["Apellidos"].ToString(),
                UsuarioActivo = Convert.ToBoolean(r["UsuarioActivo"]),
                PerfilActivo = Convert.ToBoolean(r["PerfilActivo"]),
                PorcentajeComision = r["PorcentajeComision"] == DBNull.Value
                    ? (decimal?)null : Convert.ToDecimal(r["PorcentajeComision"])
            });

        public void ConfigurarBarbero(string dni, decimal porcentaje, bool activo) =>
            data.ConfigurarBarbero(dni, porcentaje, activo);

        public List<Servicio_33ZS> ListarServicios() =>
            Mapear(data.ListarServicios(), r => new Servicio_33ZS
            {
                Id = Convert.ToInt32(r["ServicioID"]),
                Nombre = r["Nombre"].ToString(),
                Precio = r["Precio"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(r["Precio"]),
                Activo = Convert.ToBoolean(r["Activo"])
            });

        public List<Consumo_33ZS> ListarConsumos(int servicioId) =>
            Mapear(data.ListarConsumos(servicioId), r => new Consumo_33ZS
            {
                InsumoId = Convert.ToInt32(r["InsumoID"]),
                Insumo = r["Nombre"].ToString(),
                Cantidad = Convert.ToDecimal(r["Cantidad"]),
                Stock = Convert.ToDecimal(r["Stock"]),
                Activo = Convert.ToBoolean(r["Activo"])
            });

        public int GuardarServicio(int? servicioId, string nombre, decimal precio,
            bool activo, List<ConsumoServicio_33ZS> consumos)
        {
            var tabla = new DataTable();
            tabla.Columns.Add("InsumoID", typeof(int));
            tabla.Columns.Add("Cantidad", typeof(decimal));
            foreach (var consumo in consumos)
                tabla.Rows.Add(consumo.InsumoId, consumo.Cantidad);
            return data.GuardarServicio(servicioId, nombre, precio, activo, tabla);
        }

        public List<Insumo_33ZS> ListarInsumos() =>
            Mapear(data.ListarInsumos(), r => new Insumo_33ZS
            {
                Id = Convert.ToInt32(r["InsumoID"]),
                Codigo = r["Codigo"].ToString(),
                Nombre = r["Nombre"].ToString(),
                Stock = Convert.ToDecimal(r["Stock"]),
                StockMinimo = Convert.ToDecimal(r["StockMinimo"]),
                Activo = Convert.ToBoolean(r["Activo"])
            });

        public void AjustarStock(int insumoId, decimal stock, decimal minimo, string dni, string motivo) =>
            data.AjustarStock(insumoId, stock, minimo, dni, motivo);

        public List<MedioPago_33ZS> ListarMedios() =>
            Mapear(data.ListarMedios(), r => new MedioPago_33ZS
            {
                Id = Convert.ToInt32(r["MedioPagoID"]), Nombre = r["Nombre"].ToString()
            });

        public int RegistrarAtencion(Guid operacionId, int clienteId, string barberoDni,
            string recepcionistaDni, int servicioId, int medioPagoId, decimal importe) =>
            data.RegistrarAtencion(operacionId, clienteId, barberoDni, recepcionistaDni,
                servicioId, medioPagoId, importe);

        public List<AtencionPropia_33ZS> AtencionesPropias(string dni, DateTime desde, DateTime hastaExclusivo) =>
            Mapear(data.AtencionesPropias(dni, desde, hastaExclusivo), r => new AtencionPropia_33ZS
            {
                Id = Convert.ToInt32(r["AtencionID"]),
                FechaHora = Convert.ToDateTime(r["FechaHora"]),
                Servicio = r["Servicio"].ToString(),
                PorcentajeComision = Convert.ToDecimal(r["PorcentajeComisionAplicado"]),
                Comision = Convert.ToDecimal(r["ComisionImporte"])
            });

        public ResumenAtenciones_33ZS Reporte(DateTime desde, DateTime hastaExclusivo,
            string barberoDni, int? servicioId, int? medioPagoId) =>
            MapearReporte(data.Reporte(desde, hastaExclusivo, barberoDni, servicioId, medioPagoId));

        private static ResumenAtenciones_33ZS MapearReporte(DataTable tabla)
        {
            var resultado = new ResumenAtenciones_33ZS
            {
                Atenciones = Mapear(tabla, r => new Atencion_33ZS
                {
                    Id = Convert.ToInt32(r["AtencionID"]),
                    FechaHora = Convert.ToDateTime(r["FechaHora"]),
                    Cliente = r["Cliente"].ToString(),
                    Barbero = r["Barbero"].ToString(),
                    Servicio = r["Servicio"].ToString(),
                    MedioPago = r["MedioPago"].ToString(),
                    Importe = Convert.ToDecimal(r["Importe"]),
                    Comision = Convert.ToDecimal(r["ComisionImporte"])
                })
            };
            if (tabla.Rows.Count > 0)
            {
                resultado.Cantidad = Convert.ToInt32(tabla.Rows[0]["CantidadTotal"]);
                resultado.Ingresos = Convert.ToDecimal(tabla.Rows[0]["IngresosTotales"]);
                resultado.Comisiones = Convert.ToDecimal(tabla.Rows[0]["ComisionesTotales"]);
            }
            return resultado;
        }
    }
}
