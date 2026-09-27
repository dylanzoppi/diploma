using DAL.Security;
using Servicios.Composite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Mappers.Security
{
    public class PerfilMapper_33ZS
    {
        private readonly PerfilDataAccess_33ZS dataAccess = new PerfilDataAccess_33ZS();

        public List<Patente_33ZS> ObtenerPatentes_33ZS()
        {
            DataTable table = dataAccess.Patentes_33ZS().Tables[0];
            return table.Rows.Cast<DataRow>()
                .Select(row => new Patente_33ZS(Convert.ToInt32(row["ID"]), row["Nombre"].ToString()))
                .ToList();
        }

        private static Dictionary<int, Familia_33ZS> ConstruirFamilias(DataSet data)
        {
            Dictionary<int, Patente_33ZS> patentes = data.Tables[0].Rows.Cast<DataRow>()
                .ToDictionary(row => Convert.ToInt32(row["ID"]),
                    row => new Patente_33ZS(Convert.ToInt32(row["ID"]), row["Nombre"].ToString()));
            Dictionary<int, Familia_33ZS> familias = data.Tables[1].Rows.Cast<DataRow>()
                .ToDictionary(row => Convert.ToInt32(row["ID"]),
                    row => new Familia_33ZS(Convert.ToInt32(row["ID"]), row["Nombre"].ToString()));

            foreach (DataRow row in data.Tables[2].Rows)
            {
                Familia_33ZS familia;
                Patente_33ZS patente;
                if (familias.TryGetValue(Convert.ToInt32(row["FamiliaID"]), out familia) &&
                    patentes.TryGetValue(Convert.ToInt32(row["PatenteID"]), out patente))
                    familia.Agregar_33ZS(patente);
            }
            foreach (DataRow row in data.Tables[3].Rows)
            {
                Familia_33ZS familia;
                Familia_33ZS subfamilia;
                if (familias.TryGetValue(Convert.ToInt32(row["FamiliaID"]), out familia) &&
                    familias.TryGetValue(Convert.ToInt32(row["SubFamiliaID"]), out subfamilia))
                    familia.Agregar_33ZS(subfamilia);
            }
            return familias;
        }

        public List<Familia_33ZS> ObtenerFamilias_33ZS()
        {
            return ConstruirFamilias(dataAccess.Familias_33ZS())
                .Values.ToList();
        }

        public List<Familia_33ZS> ObtenerRoles_33ZS()
        {
            DataSet data = dataAccess.Roles_33ZS();
            Dictionary<int, Familia_33ZS> familias = ConstruirFamilias(data);
            Dictionary<int, Patente_33ZS> patentes = data.Tables[0].Rows.Cast<DataRow>()
                .ToDictionary(row => Convert.ToInt32(row["ID"]),
                    row => new Patente_33ZS(Convert.ToInt32(row["ID"]), row["Nombre"].ToString()));
            Dictionary<int, Familia_33ZS> roles = data.Tables[4].Rows.Cast<DataRow>()
                .ToDictionary(row => Convert.ToInt32(row["ID"]),
                    row => new Familia_33ZS(Convert.ToInt32(row["ID"]), row["Nombre"].ToString()));

            foreach (DataRow row in data.Tables[5].Rows)
            {
                Familia_33ZS rol;
                Patente_33ZS patente;
                if (roles.TryGetValue(Convert.ToInt32(row["RolID"]), out rol) &&
                    patentes.TryGetValue(Convert.ToInt32(row["PatenteID"]), out patente))
                    rol.Agregar_33ZS(patente);
            }
            foreach (DataRow row in data.Tables[6].Rows)
            {
                Familia_33ZS rol;
                Familia_33ZS familia;
                if (roles.TryGetValue(Convert.ToInt32(row["RolID"]), out rol) &&
                    familias.TryGetValue(Convert.ToInt32(row["FamiliaID"]), out familia))
                    rol.Agregar_33ZS(familia);
            }
            return roles.Values.ToList();
        }

        private static DataTable Componentes(List<Componente_33ZS> componentes)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("Tipo", typeof(string));
            foreach (Componente_33ZS componente in componentes)
            {
                if (componente is Patente_33ZS)
                    table.Rows.Add(componente.Id, "P");
                else if (componente is Familia_33ZS)
                    table.Rows.Add(componente.Id, "F");
                else
                    throw new ArgumentException("Componente de perfil no admitido.", nameof(componentes));
            }
            return table;
        }

        public int GuardarFamilia_33ZS(string nombre, List<Componente_33ZS> subComponentes)
        {
            return dataAccess.GuardarFamilia_33ZS(nombre, Componentes(subComponentes));
        }

        public int GuardarRol_33ZS(string nombre, List<Componente_33ZS> subComponentes)
        {
            return dataAccess.GuardarRol_33ZS(nombre, Componentes(subComponentes));
        }

        public void ModificarFamilia_33ZS(int id, string nombre, List<Componente_33ZS> subComponentes)
        {
            dataAccess.ModificarFamilia_33ZS(id, nombre, Componentes(subComponentes));
        }

        public void ModificarRol_33ZS(int id, string nombre, List<Componente_33ZS> subComponentes)
        {
            dataAccess.ModificarRol_33ZS(id, nombre, Componentes(subComponentes));
        }

        public void EliminarFamilia_33ZS(int id) => dataAccess.EliminarFamilia_33ZS(id);
        public void EliminarRol_33ZS(int id) => dataAccess.EliminarRol_33ZS(id);
        public bool FamiliaEnUso_33ZS(int id) => dataAccess.FamiliaEnUso_33ZS(id);
        public bool RolEnUso_33ZS(int id) => dataAccess.RolEnUso_33ZS(id);
    }
}
