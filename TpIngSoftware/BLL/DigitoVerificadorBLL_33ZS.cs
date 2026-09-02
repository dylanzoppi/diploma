using DAL;
using Servicios;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BLL
{
    public class DigitoVerificadorBLL_33ZS
    {
        private readonly DigitoVerificadorDAL_33ZS _dal = new DigitoVerificadorDAL_33ZS();

        private static readonly Dictionary<string, string> _tablas = new Dictionary<string, string>
        {
            { "Usuario",         "DNI" },
            { "Rol",             "ID" },
            { "Patente",         "ID" },
            { "Familia",         "ID" },
            { "Rol_Patente",     "RolID, PatenteID" },
            { "Rol_Familia",     "RolID, FamiliaID" },
            { "Patente_Familia", "PatenteID, FamiliaID" },
            { "FamiliaN",        "FamiliaID, SubFamiliaID" }
        };

        private string HashGrupo_33ZS(IEnumerable<string> valores)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string v in valores)
                if (!string.IsNullOrEmpty(v))
                    sb.Append(v);
            return Encriptador_33ZS.Hash(sb.ToString());
        }

        private string CalcularDVH_33ZS(DataTable dt)
        {
            List<string> hashesPorFila = new List<string>();
            foreach (DataRow row in dt.Rows)
            {
                List<string> fila = new List<string>();
                foreach (DataColumn col in dt.Columns)
                    fila.Add(row[col].ToString());
                hashesPorFila.Add(HashGrupo_33ZS(fila));
            }
            return HashGrupo_33ZS(hashesPorFila);
        }

        private string CalcularDVV_33ZS(DataTable dt)
        {
            List<string> hashesPorColumna = new List<string>();
            foreach (DataColumn col in dt.Columns)
            {
                List<string> columna = new List<string>();
                foreach (DataRow row in dt.Rows)
                    columna.Add(row[col].ToString());
                hashesPorColumna.Add(HashGrupo_33ZS(columna));
            }
            return HashGrupo_33ZS(hashesPorColumna);
        }

        public void GuardarDigitos_33ZS(string tabla)
        {
            string ordenarPor = _tablas[tabla];
            DataTable dt = _dal.ObtenerTabla_33ZS(tabla, ordenarPor);
            _dal.GuardarDV_33ZS(tabla, CalcularDVH_33ZS(dt), CalcularDVV_33ZS(dt));
        }

        public void GenerarTodo_33ZS()
        {
            foreach (KeyValuePair<string, string> t in _tablas)
            {
                DataTable dt = _dal.ObtenerTabla_33ZS(t.Key, t.Value);
                _dal.GuardarDV_33ZS(t.Key, CalcularDVH_33ZS(dt), CalcularDVV_33ZS(dt));
            }
        }

        public List<string> Verificar_33ZS()
        {
            List<string> inconsistentes = new List<string>();

            foreach (KeyValuePair<string, string> t in _tablas)
            {
                DataTable dt = _dal.ObtenerTabla_33ZS(t.Key, t.Value);
                string dvh = CalcularDVH_33ZS(dt);
                string dvv = CalcularDVV_33ZS(dt);

                DataTable guardado = _dal.ObtenerDVGuardado_33ZS(t.Key);
                if (guardado.Rows.Count == 0 ||
                    guardado.Rows[0]["DVH"].ToString() != dvh ||
                    guardado.Rows[0]["DVV"].ToString() != dvv)
                {
                    inconsistentes.Add(t.Key);
                }
            }

            return inconsistentes;
        }
    }
}
