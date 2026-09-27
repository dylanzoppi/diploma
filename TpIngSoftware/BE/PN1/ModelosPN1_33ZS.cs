using System;

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
        public string Insumo { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Stock { get; set; }
    }

    public sealed class MedioPago_33ZS
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public override string ToString() => Nombre;
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
}
