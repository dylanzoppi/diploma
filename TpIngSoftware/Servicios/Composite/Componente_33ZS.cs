using System.Collections.Generic;

namespace Servicios.Composite
{
    public abstract class Componente_33ZS
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public abstract void Agregar_33ZS(Componente_33ZS componente);
        public abstract void Eliminar_33ZS(Componente_33ZS componente);
        public abstract List<Componente_33ZS> ObtenerSubComponentes_33ZS();
    }
}
