using System.Collections.Generic;

namespace Servicios.Composite
{
    public class Familia_33ZS : Componente_33ZS
    {
        private List<Componente_33ZS> _subComponentes = new List<Componente_33ZS>();

        public Familia_33ZS() { }

        public Familia_33ZS(int id, string nombre)
        {
            Id = id;
            Nombre = nombre;
        }

        public override void Agregar_33ZS(Componente_33ZS componente)
        {
            _subComponentes.Add(componente);
        }

        public override void Eliminar_33ZS(Componente_33ZS componente)
        {
            _subComponentes.Remove(componente);
        }

        public bool ContienePatente_33ZS(Componente_33ZS patente)
        {
            return _subComponentes.Contains(patente);
        }

        public override List<Componente_33ZS> ObtenerSubComponentes_33ZS()
        {
            return _subComponentes;
        }

        public List<Patente_33ZS> ObtenerPatentes_33ZS()
        {
            List<Patente_33ZS> patentes = new List<Patente_33ZS>();

            foreach (var componente in _subComponentes)
            {
                if (componente is Patente_33ZS patente)
                {
                    patentes.Add(patente);
                }
                else if (componente is Familia_33ZS familia)
                {
                    patentes.AddRange(familia.ObtenerPatentes_33ZS());
                }
            }
            return patentes;
        }
    }
}
