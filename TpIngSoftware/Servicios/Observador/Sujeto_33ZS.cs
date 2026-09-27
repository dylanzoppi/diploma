using System.Collections.Generic;
using System.Linq;

namespace Servicios
{
    public abstract class Sujeto_33ZS
    {
        private readonly List<IObservador_33ZS> _observadores_33ZS = new List<IObservador_33ZS>();

        public void Suscribir_33ZS(IObservador_33ZS observador)
        {
            if (observador != null && !_observadores_33ZS.Contains(observador))
                _observadores_33ZS.Add(observador);
        }

        public void Desuscribir_33ZS(IObservador_33ZS observador)
        {
            _observadores_33ZS.Remove(observador);
        }

        protected void Notificar_33ZS()
        {
            foreach (var observador in _observadores_33ZS.ToList())
                observador.Actualizar_33ZS();
        }
    }
}
