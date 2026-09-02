using Servicios.Composite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DAL
{
    public class PerfilDAL_33ZS
    {
        public List<Patente_33ZS> ObtenerPatentes_33ZS()
        {
            Acceso_33ZS acc = new Acceso_33ZS();
            DataSet ds = acc.ExecuteDataSet_33ZS(
                "SELECT ID, Nombre FROM Patente");

            List<Patente_33ZS> patentes = new List<Patente_33ZS>();
            foreach (DataRow dr in ds.Tables[0].Rows)
                patentes.Add(new Patente_33ZS(Convert.ToInt32(dr["ID"]), dr["Nombre"].ToString()));

            return patentes;
        }

        public List<Familia_33ZS> ObtenerFamilias_33ZS()
        {
            Acceso_33ZS acc = new Acceso_33ZS();

            DataSet dsPatentes = acc.ExecuteDataSet_33ZS("SELECT ID, Nombre FROM Patente");
            Dictionary<int, Patente_33ZS> patentesMap = new Dictionary<int, Patente_33ZS>();
            foreach (DataRow dr in dsPatentes.Tables[0].Rows)
            {
                int id = Convert.ToInt32(dr["ID"]);
                patentesMap[id] = new Patente_33ZS(id, dr["Nombre"].ToString());
            }

            DataSet dsFamilias = acc.ExecuteDataSet_33ZS("SELECT ID, Nombre FROM Familia");
            Dictionary<int, Familia_33ZS> familiasMap = new Dictionary<int, Familia_33ZS>();
            foreach (DataRow dr in dsFamilias.Tables[0].Rows)
            {
                int id = Convert.ToInt32(dr["ID"]);
                familiasMap[id] = new Familia_33ZS(id, dr["Nombre"].ToString());
            }

            DataSet dsPF = acc.ExecuteDataSet_33ZS("SELECT PatenteID, FamiliaID FROM Patente_Familia");
            foreach (DataRow dr in dsPF.Tables[0].Rows)
            {
                int patenteId = Convert.ToInt32(dr["PatenteID"]);
                int familiaId = Convert.ToInt32(dr["FamiliaID"]);

                if (familiasMap.ContainsKey(familiaId) && patentesMap.ContainsKey(patenteId))
                    familiasMap[familiaId].Agregar_33ZS(patentesMap[patenteId]);
            }

            DataSet dsFN = acc.ExecuteDataSet_33ZS("SELECT FamiliaID, SubFamiliaID FROM FamiliaN");
            foreach (DataRow dr in dsFN.Tables[0].Rows)
            {
                int familiaId = Convert.ToInt32(dr["FamiliaID"]);
                int subFamiliaId = Convert.ToInt32(dr["SubFamiliaID"]);

                if (familiasMap.ContainsKey(familiaId) && familiasMap.ContainsKey(subFamiliaId))
                    familiasMap[familiaId].Agregar_33ZS(familiasMap[subFamiliaId]);
            }

            return familiasMap.Values.ToList();
        }

        public int GuardarFamilia_33ZS(string nombre, List<Componente_33ZS> subComponentes)
        {
            Acceso_33ZS acc = new Acceso_33ZS();
            int nuevoId = acc.ObtenerUltimoId_33ZS("SELECT ISNULL(MAX(ID), 0) + 1 FROM Familia");

            var comandos = new List<Tuple<string, SqlParameter[]>>();

            comandos.Add(Tuple.Create(
                "INSERT INTO Familia(ID, Nombre) VALUES (@ID, @Nombre)",
                new SqlParameter[] {
                    new SqlParameter("@ID", nuevoId),
                    new SqlParameter("@Nombre", nombre)
                }));

            foreach (Componente_33ZS componente in subComponentes)
            {
                if (componente is Patente_33ZS)
                {
                    comandos.Add(Tuple.Create(
                        "INSERT INTO Patente_Familia(PatenteID, FamiliaID) VALUES (@PatenteID, @FamiliaID)",
                        new SqlParameter[] {
                            new SqlParameter("@PatenteID", componente.Id),
                            new SqlParameter("@FamiliaID", nuevoId)
                        }));
                }
                else if (componente is Familia_33ZS)
                {
                    comandos.Add(Tuple.Create(
                        "INSERT INTO FamiliaN(FamiliaID, SubFamiliaID) VALUES (@FamiliaID, @SubFamiliaID)",
                        new SqlParameter[] {
                            new SqlParameter("@FamiliaID", nuevoId),
                            new SqlParameter("@SubFamiliaID", componente.Id)
                        }));
                }
            }

            acc.ExecuteNonQueryBatch_33ZS(comandos);
            return nuevoId;
        }

        public List<Familia_33ZS> ObtenerRoles_33ZS()
        {
            Acceso_33ZS acc = new Acceso_33ZS();

            DataSet dsPatentes = acc.ExecuteDataSet_33ZS("SELECT ID, Nombre FROM Patente");
            Dictionary<int, Patente_33ZS> patentesMap = new Dictionary<int, Patente_33ZS>();
            foreach (DataRow dr in dsPatentes.Tables[0].Rows)
            {
                int id = Convert.ToInt32(dr["ID"]);
                patentesMap[id] = new Patente_33ZS(id, dr["Nombre"].ToString());
            }

            DataSet dsFamilias = acc.ExecuteDataSet_33ZS("SELECT ID, Nombre FROM Familia");
            Dictionary<int, Familia_33ZS> familiasMap = new Dictionary<int, Familia_33ZS>();
            foreach (DataRow dr in dsFamilias.Tables[0].Rows)
            {
                int id = Convert.ToInt32(dr["ID"]);
                familiasMap[id] = new Familia_33ZS(id, dr["Nombre"].ToString());
            }

            DataSet dsPF = acc.ExecuteDataSet_33ZS("SELECT PatenteID, FamiliaID FROM Patente_Familia");
            foreach (DataRow dr in dsPF.Tables[0].Rows)
            {
                int patenteId = Convert.ToInt32(dr["PatenteID"]);
                int familiaId = Convert.ToInt32(dr["FamiliaID"]);
                if (familiasMap.ContainsKey(familiaId) && patentesMap.ContainsKey(patenteId))
                    familiasMap[familiaId].Agregar_33ZS(patentesMap[patenteId]);
            }

            DataSet dsFN = acc.ExecuteDataSet_33ZS("SELECT FamiliaID, SubFamiliaID FROM FamiliaN");
            foreach (DataRow dr in dsFN.Tables[0].Rows)
            {
                int familiaId = Convert.ToInt32(dr["FamiliaID"]);
                int subFamiliaId = Convert.ToInt32(dr["SubFamiliaID"]);
                if (familiasMap.ContainsKey(familiaId) && familiasMap.ContainsKey(subFamiliaId))
                    familiasMap[familiaId].Agregar_33ZS(familiasMap[subFamiliaId]);
            }

            DataSet dsRoles = acc.ExecuteDataSet_33ZS("SELECT ID, Nombre FROM Rol");
            Dictionary<int, Familia_33ZS> rolesMap = new Dictionary<int, Familia_33ZS>();
            foreach (DataRow dr in dsRoles.Tables[0].Rows)
            {
                int id = Convert.ToInt32(dr["ID"]);
                rolesMap[id] = new Familia_33ZS(id, dr["Nombre"].ToString());
            }

            DataSet dsRP = acc.ExecuteDataSet_33ZS("SELECT RolID, PatenteID FROM Rol_Patente");
            foreach (DataRow dr in dsRP.Tables[0].Rows)
            {
                int rolId = Convert.ToInt32(dr["RolID"]);
                int patenteId = Convert.ToInt32(dr["PatenteID"]);
                if (rolesMap.ContainsKey(rolId) && patentesMap.ContainsKey(patenteId))
                    rolesMap[rolId].Agregar_33ZS(patentesMap[patenteId]);
            }

            DataSet dsRF = acc.ExecuteDataSet_33ZS("SELECT RolID, FamiliaID FROM Rol_Familia");
            foreach (DataRow dr in dsRF.Tables[0].Rows)
            {
                int rolId = Convert.ToInt32(dr["RolID"]);
                int familiaId = Convert.ToInt32(dr["FamiliaID"]);
                if (rolesMap.ContainsKey(rolId) && familiasMap.ContainsKey(familiaId))
                    rolesMap[rolId].Agregar_33ZS(familiasMap[familiaId]);
            }

            return rolesMap.Values.ToList();
        }

        public int GuardarRol_33ZS(string nombre, List<Componente_33ZS> subComponentes)
        {
            Acceso_33ZS acc = new Acceso_33ZS();
            int nuevoId = acc.ObtenerUltimoId_33ZS("SELECT ISNULL(MAX(ID), 0) + 1 FROM Rol");

            var comandos = new List<Tuple<string, SqlParameter[]>>();

            comandos.Add(Tuple.Create(
                "INSERT INTO Rol(ID, Nombre) VALUES (@ID, @Nombre)",
                new SqlParameter[] {
                    new SqlParameter("@ID", nuevoId),
                    new SqlParameter("@Nombre", nombre)
                }));

            foreach (Componente_33ZS componente in subComponentes)
            {
                if (componente is Patente_33ZS)
                {
                    comandos.Add(Tuple.Create(
                        "INSERT INTO Rol_Patente(RolID, PatenteID) VALUES (@RolID, @PatenteID)",
                        new SqlParameter[] {
                            new SqlParameter("@RolID", nuevoId),
                            new SqlParameter("@PatenteID", componente.Id)
                        }));
                }
                else if (componente is Familia_33ZS)
                {
                    comandos.Add(Tuple.Create(
                        "INSERT INTO Rol_Familia(RolID, FamiliaID) VALUES (@RolID, @FamiliaID)",
                        new SqlParameter[] {
                            new SqlParameter("@RolID", nuevoId),
                            new SqlParameter("@FamiliaID", componente.Id)
                        }));
                }
            }

            acc.ExecuteNonQueryBatch_33ZS(comandos);
            return nuevoId;
        }

        public void EliminarFamilia_33ZS(int id)
        {
            Acceso_33ZS acc = new Acceso_33ZS();

            var comandos = new List<Tuple<string, SqlParameter[]>>();

            comandos.Add(Tuple.Create(
                "DELETE FROM Patente_Familia WHERE FamiliaID = @ID",
                new SqlParameter[] { new SqlParameter("@ID", id) }));

            comandos.Add(Tuple.Create(
                "DELETE FROM FamiliaN WHERE FamiliaID = @ID OR SubFamiliaID = @ID",
                new SqlParameter[] { new SqlParameter("@ID", id) }));

            comandos.Add(Tuple.Create(
                "DELETE FROM Rol_Familia WHERE FamiliaID = @ID",
                new SqlParameter[] { new SqlParameter("@ID", id) }));

            comandos.Add(Tuple.Create(
                "DELETE FROM Familia WHERE ID = @ID",
                new SqlParameter[] { new SqlParameter("@ID", id) }));

            acc.ExecuteNonQueryBatch_33ZS(comandos);
        }

        public void EliminarRol_33ZS(int id)
        {
            Acceso_33ZS acc = new Acceso_33ZS();
            var comandos = new List<Tuple<string, SqlParameter[]>>();

            comandos.Add(Tuple.Create(
                "DELETE FROM Rol_Patente WHERE RolID = @ID",
                new SqlParameter[] { new SqlParameter("@ID", id) }));
            comandos.Add(Tuple.Create(
                "DELETE FROM Rol_Familia WHERE RolID = @ID",
                new SqlParameter[] { new SqlParameter("@ID", id) }));
            comandos.Add(Tuple.Create(
                "DELETE FROM Rol WHERE ID = @ID",
                new SqlParameter[] { new SqlParameter("@ID", id) }));

            acc.ExecuteNonQueryBatch_33ZS(comandos);
        }

        public bool FamiliaEnUso_33ZS(int id)
        {
            Acceso_33ZS acc = new Acceso_33ZS();
            DataSet ds = acc.ExecuteDataSet_33ZS(
                @"SELECT COUNT(*) FROM Rol_Familia rf
                  INNER JOIN Rol r ON rf.RolID = r.ID
                  INNER JOIN Usuario u ON u.Rol = r.Nombre
                  WHERE rf.FamiliaID = @ID",
                new SqlParameter[] { new SqlParameter("@ID", id) });
            return Convert.ToInt32(ds.Tables[0].Rows[0][0]) > 0;
        }

        public bool RolEnUso_33ZS(int id)
        {
            Acceso_33ZS acc = new Acceso_33ZS();
            DataSet ds = acc.ExecuteDataSet_33ZS(
                @"SELECT COUNT(*) FROM Usuario u
                  INNER JOIN Rol r ON u.Rol = r.Nombre
                  WHERE r.ID = @ID",
                new SqlParameter[] { new SqlParameter("@ID", id) });
            return Convert.ToInt32(ds.Tables[0].Rows[0][0]) > 0;
        }

        public void ModificarFamilia_33ZS(int id, string nombre, List<Componente_33ZS> subComponentes)
        {
            Acceso_33ZS acc = new Acceso_33ZS();
            var comandos = new List<Tuple<string, SqlParameter[]>>();

            comandos.Add(Tuple.Create(
                "UPDATE Familia SET Nombre = @Nombre WHERE ID = @ID",
                new SqlParameter[] {
                    new SqlParameter("@Nombre", nombre),
                    new SqlParameter("@ID", id)
                }));

            comandos.Add(Tuple.Create(
                "DELETE FROM Patente_Familia WHERE FamiliaID = @ID",
                new SqlParameter[] { new SqlParameter("@ID", id) }));
            comandos.Add(Tuple.Create(
                "DELETE FROM FamiliaN WHERE FamiliaID = @ID",
                new SqlParameter[] { new SqlParameter("@ID", id) }));

            foreach (Componente_33ZS componente in subComponentes)
            {
                if (componente is Patente_33ZS)
                    comandos.Add(Tuple.Create(
                        "INSERT INTO Patente_Familia(PatenteID, FamiliaID) VALUES (@PatenteID, @FamiliaID)",
                        new SqlParameter[] {
                            new SqlParameter("@PatenteID", componente.Id),
                            new SqlParameter("@FamiliaID", id)
                        }));
                else if (componente is Familia_33ZS)
                    comandos.Add(Tuple.Create(
                        "INSERT INTO FamiliaN(FamiliaID, SubFamiliaID) VALUES (@FamiliaID, @SubFamiliaID)",
                        new SqlParameter[] {
                            new SqlParameter("@FamiliaID", id),
                            new SqlParameter("@SubFamiliaID", componente.Id)
                        }));
            }

            acc.ExecuteNonQueryBatch_33ZS(comandos);
        }

        public void ModificarRol_33ZS(int id, string nombre, List<Componente_33ZS> subComponentes)
        {
            Acceso_33ZS acc = new Acceso_33ZS();
            var comandos = new List<Tuple<string, SqlParameter[]>>();

            comandos.Add(Tuple.Create(
                "UPDATE Rol SET Nombre = @Nombre WHERE ID = @ID",
                new SqlParameter[] {
                    new SqlParameter("@Nombre", nombre),
                    new SqlParameter("@ID", id)
                }));

            comandos.Add(Tuple.Create(
                "DELETE FROM Rol_Patente WHERE RolID = @ID",
                new SqlParameter[] { new SqlParameter("@ID", id) }));
            comandos.Add(Tuple.Create(
                "DELETE FROM Rol_Familia WHERE RolID = @ID",
                new SqlParameter[] { new SqlParameter("@ID", id) }));

            foreach (Componente_33ZS componente in subComponentes)
            {
                if (componente is Patente_33ZS)
                    comandos.Add(Tuple.Create(
                        "INSERT INTO Rol_Patente(RolID, PatenteID) VALUES (@RolID, @PatenteID)",
                        new SqlParameter[] {
                            new SqlParameter("@RolID", id),
                            new SqlParameter("@PatenteID", componente.Id)
                        }));
                else if (componente is Familia_33ZS)
                    comandos.Add(Tuple.Create(
                        "INSERT INTO Rol_Familia(RolID, FamiliaID) VALUES (@RolID, @FamiliaID)",
                        new SqlParameter[] {
                            new SqlParameter("@RolID", id),
                            new SqlParameter("@FamiliaID", componente.Id)
                        }));
            }

            acc.ExecuteNonQueryBatch_33ZS(comandos);
        }
    }
}
