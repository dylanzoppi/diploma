using System;
using System.Collections.Generic;

namespace BE.PN1
{
    public sealed class Cliente_33ZS
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public DateTime FechaAlta { get; set; }
        public string Descripcion => Apellido + ", " + Nombre + " · " + Telefono;
    }

    public sealed class Barbero_33ZS
    {
        public string Dni { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public decimal? PorcentajeComision { get; set; }
        public bool UsuarioActivo { get; set; }
        public bool PerfilActivo { get; set; }
        public string Descripcion => Apellido + ", " + Nombre;
    }

    public sealed class Servicio_33ZS
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal? Precio { get; set; }
        public bool Activo { get; set; }
        public string Descripcion => Nombre + (Precio.HasValue ? " · $" + Precio.Value.ToString("N2") : " · sin precio");
    }

    public sealed class Insumo_33ZS
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public decimal Stock { get; set; }
        public decimal StockMinimo { get; set; }
        public bool Activo { get; set; }
    }

    public sealed class Consumo_33ZS
    {
        public int InsumoId { get; set; }
        public string Insumo { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Stock { get; set; }
        public bool Activo { get; set; }
    }

    public sealed class ConsumoServicio_33ZS
    {
        public int InsumoId { get; set; }
        public decimal Cantidad { get; set; }
    }

    public sealed class DisponibilidadServicio_33ZS
    {
        public List<Consumo_33ZS> Consumos { get; set; } = new List<Consumo_33ZS>();
        public List<FaltanteInsumo_33ZS> NoDisponibles { get; set; } = new List<FaltanteInsumo_33ZS>();
        public bool Disponible => Consumos.Count > 0 && NoDisponibles.Count == 0;
    }

    public sealed class FaltanteInsumo_33ZS
    {
        public string Insumo { get; set; }
        public decimal Faltante { get; set; }
        public bool Activo { get; set; }
    }

    public sealed class MedioPago_33ZS
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public override string ToString() => Nombre;
    }

    public sealed class AtencionPropia_33ZS
    {
        public int Id { get; set; }
        public DateTime FechaHora { get; set; }
        public string Servicio { get; set; }
        public decimal PorcentajeComision { get; set; }
        public decimal Comision { get; set; }
    }

    public sealed class ResumenAtencionesPropias_33ZS
    {
        public List<AtencionPropia_33ZS> Atenciones { get; set; } = new List<AtencionPropia_33ZS>();
        public int Cantidad { get; set; }
        public decimal Comisiones { get; set; }
    }

    public sealed class Atencion_33ZS
    {
        public int Id { get; set; }
        public DateTime FechaHora { get; set; }
        public string Cliente { get; set; }
        public string Barbero { get; set; }
        public string Servicio { get; set; }
        public string MedioPago { get; set; }
        public decimal Importe { get; set; }
        public decimal PorcentajeComision { get; set; }
        public decimal Comision { get; set; }
    }

    public sealed class ResumenAtenciones_33ZS
    {
        public List<Atencion_33ZS> Atenciones { get; set; } = new List<Atencion_33ZS>();
        public int Cantidad { get; set; }
        public decimal Ingresos { get; set; }
        public decimal Comisiones { get; set; }
    }
}
