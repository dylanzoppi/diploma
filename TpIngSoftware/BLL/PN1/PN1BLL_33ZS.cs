using System;
using System.Collections.Generic;
using System.Linq;
using BE.PN1;
using Mappers.PN1;
using Servicios;

namespace BLL.PN1
{
    public sealed class PN1BLL_33ZS
    {
        private readonly PN1Mapper_33ZS mapper = new PN1Mapper_33ZS();
        private readonly PerfilBLL_33ZS perfiles = new PerfilBLL_33ZS();

        private Usuario_33ZS UsuarioActual()
        {
            Usuario_33ZS usuario = SessionManager_33ZS.GetInstance_33ZS().UsuarioActual_33ZS;
            if (usuario == null || !usuario.Activo_33ZS || usuario.Bloqueo_33ZS)
                throw new UnauthorizedAccessException("Debe iniciar sesión con un usuario activo.");
            return usuario;
        }

        private Usuario_33ZS Exigir(params string[] patentes)
        {
            Usuario_33ZS usuario = UsuarioActual();
            if (!patentes.Any(p => perfiles.RolTienePatente_33ZS(usuario.Rol_33ZS, p)))
                throw new UnauthorizedAccessException("Su rol no tiene permiso para esta operación.");
            return usuario;
        }

        public bool TienePermiso(string patente)
        {
            Usuario_33ZS usuario = SessionManager_33ZS.GetInstance_33ZS().UsuarioActual_33ZS;
            return usuario != null && usuario.Activo_33ZS && !usuario.Bloqueo_33ZS &&
                perfiles.RolTienePatente_33ZS(usuario.Rol_33ZS, patente);
        }

        public List<Cliente_33ZS> BuscarClientes(string termino)
        {
            Exigir("BuscarCliente");
            return mapper.BuscarClientes((termino ?? "").Trim());
        }

        public int RegistrarCliente(Cliente_33ZS cliente)
        {
            Exigir("RegistrarCliente");
            if (cliente == null) throw new ArgumentNullException(nameof(cliente));
            cliente.Nombre = (cliente.Nombre ?? "").Trim();
            cliente.Apellido = (cliente.Apellido ?? "").Trim();
            cliente.Telefono = (cliente.Telefono ?? "").Trim();
            cliente.Correo = string.IsNullOrWhiteSpace(cliente.Correo) ? null : cliente.Correo.Trim();
            if (cliente.Nombre.Length == 0 || cliente.Apellido.Length == 0 || cliente.Telefono.Length == 0)
                throw new ArgumentException("Nombre, apellido y teléfono son obligatorios.");
            if (cliente.Nombre.Length > 100 || cliente.Apellido.Length > 100 ||
                cliente.Telefono.Length > 30 || (cliente.Correo != null && cliente.Correo.Length > 150))
                throw new ArgumentException("Uno de los datos supera la longitud permitida.");
            return mapper.RegistrarCliente(cliente);
        }

        public List<Barbero_33ZS> ListarBarberos()
        {
            Exigir("RegistrarAtencion", "GestionarBarberos", "ConsultarAtencionesGenerales");
            return mapper.ListarBarberos();
        }

        public void ConfigurarBarbero(string dni, decimal porcentaje, bool activo)
        {
            Exigir("GestionarBarberos");
            if (string.IsNullOrWhiteSpace(dni) || porcentaje < 0 || porcentaje > 100)
                throw new ArgumentException("Barbero o porcentaje inválido.");
            mapper.ConfigurarBarbero(dni, porcentaje, activo);
        }

        public List<Servicio_33ZS> ListarServicios()
        {
            Exigir("ConsultarCatalogo", "GestionarCatalogo", "ConsultarAtencionesGenerales");
            return mapper.ListarServicios();
        }

        public List<Consumo_33ZS> ListarConsumos(int servicioId)
        {
            Exigir("ConsultarCatalogo", "GestionarCatalogo");
            return mapper.ListarConsumos(servicioId);
        }

        public DisponibilidadServicio_33ZS EvaluarDisponibilidadServicio(int servicioId)
        {
            Exigir("RegistrarAtencion");
            var consumos = mapper.ListarConsumos(servicioId);
            return new DisponibilidadServicio_33ZS
            {
                Consumos = consumos,
                NoDisponibles = consumos.Where(c => !c.Activo || c.Stock < c.Cantidad)
                    .Select(c => new FaltanteInsumo_33ZS
                    {
                        Insumo = c.Insumo,
                        Faltante = Math.Max(0, c.Cantidad - c.Stock),
                        Activo = c.Activo
                    }).ToList()
            };
        }

        public int GuardarServicio(int? servicioId, string nombre, decimal precio,
            bool activo, List<ConsumoServicio_33ZS> consumos)
        {
            Exigir("GestionarCatalogo");
            nombre = (nombre ?? "").Trim();
            if (servicioId.HasValue && servicioId.Value <= 0)
                throw new ArgumentException("El servicio seleccionado no es válido.");
            if (nombre.Length == 0 || nombre.Length > 100 || precio <= 0)
                throw new ArgumentException("Ingresá un nombre y un precio válidos.");
            if (consumos == null || consumos.Count == 0 ||
                consumos.Any(c => c.InsumoId <= 0 || c.Cantidad <= 0) ||
                consumos.Select(c => c.InsumoId).Distinct().Count() != consumos.Count)
                throw new ArgumentException("Elegí al menos un insumo con una cantidad positiva, sin repetirlo.");
            return mapper.GuardarServicio(servicioId, nombre, precio, activo, consumos);
        }

        public List<Insumo_33ZS> ListarInsumos()
        {
            Exigir("ConsultarStock", "GestionarStock", "GestionarCatalogo");
            return mapper.ListarInsumos();
        }

        public void AjustarStock(int insumoId, decimal stock, decimal minimo, string motivo)
        {
            Usuario_33ZS usuario = Exigir("GestionarStock");
            if (stock < 0 || minimo < 0 || string.IsNullOrWhiteSpace(motivo))
                throw new ArgumentException("Indique stock, mínimo y motivo válidos.");
            mapper.AjustarStock(insumoId, stock, minimo, usuario.DNI_33ZS, motivo.Trim());
        }

        public List<MedioPago_33ZS> ListarMediosPago()
        {
            Exigir("RegistrarAtencion", "ConsultarAtencionesGenerales");
            return mapper.ListarMedios();
        }

        public int RegistrarAtencion(Guid operacionId, int clienteId, string barberoDni,
            int servicioId, int medioPagoId, decimal importe)
        {
            Usuario_33ZS usuario = Exigir("RegistrarAtencion");
            if (operacionId == Guid.Empty || clienteId <= 0 || string.IsNullOrWhiteSpace(barberoDni) ||
                servicioId <= 0 || medioPagoId <= 0 || importe <= 0)
                throw new ArgumentException("Complete cliente, barbero, servicio y cobro.");
            return mapper.RegistrarAtencion(operacionId, clienteId, barberoDni,
                usuario.DNI_33ZS, servicioId, medioPagoId, importe);
        }

        private static DateTime HastaExclusivo(DateTime desde, DateTime hasta)
        {
            if (hasta.Date < desde.Date) throw new ArgumentException("El período es inválido.");
            return hasta.Date.AddDays(1);
        }

        public ResumenAtencionesPropias_33ZS ConsultarPropias(DateTime desde, DateTime hasta)
        {
            Usuario_33ZS usuario = Exigir("ConsultarAtencionesPropias");
            var atenciones = mapper.AtencionesPropias(usuario.DNI_33ZS, desde.Date,
                HastaExclusivo(desde, hasta));
            return new ResumenAtencionesPropias_33ZS
            {
                Atenciones = atenciones,
                Cantidad = atenciones.Count,
                Comisiones = atenciones.Sum(a => a.Comision)
            };
        }

        public ResumenAtenciones_33ZS ConsultarReporte(DateTime desde, DateTime hasta,
            string barberoDni = null, int? servicioId = null, int? medioPagoId = null)
        {
            Exigir("ConsultarAtencionesGenerales");
            return mapper.Reporte(desde.Date, HastaExclusivo(desde, hasta),
                string.IsNullOrWhiteSpace(barberoDni) ? null : barberoDni, servicioId, medioPagoId);
        }
    }
}
