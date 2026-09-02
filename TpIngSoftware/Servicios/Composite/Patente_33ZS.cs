using System.Collections.Generic;

namespace Servicios.Composite
{
    public class Patente_33ZS : Componente_33ZS
    {
        public Patente_33ZS(string nombre)
        {
            Nombre = nombre;
        }

        public Patente_33ZS(int id, string nombre)
        {
            Id = id;
            Nombre = nombre;
        }

        public override void Agregar_33ZS(Componente_33ZS componente)
        {
            throw new System.NotImplementedException();
        }

        public override void Eliminar_33ZS(Componente_33ZS componente)
        {
            throw new System.NotImplementedException();
        }

        public override List<Componente_33ZS> ObtenerSubComponentes_33ZS()
        {
            return new List<Componente_33ZS>();
        }
    }
}
