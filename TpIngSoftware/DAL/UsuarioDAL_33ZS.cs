using Servicios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class UsuarioDAL_33ZS
    {
        public UsuarioDAL_33ZS() { }

        public List<Usuario_33ZS> ObtenerUsuarios_33ZS()
        {
            Acceso_33ZS acc = new Acceso_33ZS();

            string command = "SELECT * FROM Usuario";

            DataSet ds = acc.ExecuteDataSet_33ZS(command);

            List<Usuario_33ZS> listaUsuarios = new List<Usuario_33ZS>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                listaUsuarios.Add(new Usuario_33ZS(dr));
            }

            return listaUsuarios;
        }
        public Usuario_33ZS ObtenerUsuarioPorLogin_33ZS(string login)
        {
            Acceso_33ZS acc = new Acceso_33ZS();

            string command = @"SELECT * FROM Usuario 
                       WHERE Login = @Login OR Email = @Login";

            SqlParameter[] parametros =
            {
        new SqlParameter("@Login", login)
    };

            DataSet ds = acc.ExecuteDataSet_33ZS(command, parametros);

            if (ds.Tables[0].Rows.Count == 0)
                return null;

            return new Usuario_33ZS(ds.Tables[0].Rows[0]);
        }

        public void BloquearUsuario_33ZS(string dni)
        {
            Acceso_33ZS acc = new Acceso_33ZS();

            string command = @"UPDATE Usuario
                       SET Bloqueo = 1
                       WHERE DNI = @DNI";

            SqlParameter[] parametros =
            {
        new SqlParameter("@DNI", dni)
    };

            acc.ExecuteNonQuery_33ZS(command, parametros);
        }
        public List<Usuario_33ZS> ObtenerUsuariosPorEstado_33ZS(bool activo)
        {
            Acceso_33ZS acc = new Acceso_33ZS();

            string command = "SELECT * FROM Usuario WHERE Activo = @Activo";

            SqlParameter[] parametros =
            {
                new SqlParameter("@Activo", activo)
            };

            DataSet ds = acc.ExecuteDataSet_33ZS(command, parametros);

            List<Usuario_33ZS> listaUsuarios = new List<Usuario_33ZS>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                listaUsuarios.Add(new Usuario_33ZS(dr));
            }

            return listaUsuarios;
        }

        public Usuario_33ZS ObtenerUsuario_33ZS(string usuarioLogin)
        {
            Acceso_33ZS acc = new Acceso_33ZS();

            string command = "SELECT * FROM Usuario WHERE Login = @Login";

            SqlParameter[] parametros =
            {
                new SqlParameter("@Login", usuarioLogin)
            };

            DataSet ds = acc.ExecuteDataSet_33ZS(command, parametros);

            if (ds.Tables[0].Rows.Count == 0)
                return null;

            return new Usuario_33ZS(ds.Tables[0].Rows[0]);
        }

        public Usuario_33ZS ObtenerUsuarioPorDNI_33ZS(string dni)
        {
            Acceso_33ZS acc = new Acceso_33ZS();

            string command = "SELECT * FROM Usuario WHERE DNI = @DNI";

            SqlParameter[] parametros =
            {
                new SqlParameter("@DNI", dni)
            };

            DataSet ds = acc.ExecuteDataSet_33ZS(command, parametros);

            if (ds.Tables[0].Rows.Count == 0)
                return null;

            return new Usuario_33ZS(ds.Tables[0].Rows[0]);
        }

        public void AgregarUsuario_33ZS(Usuario_33ZS nuevoUsuario)
        {
            Acceso_33ZS acc = new Acceso_33ZS();

            string command = @"INSERT INTO Usuario 
                               (DNI, Apellidos, Nombre, Login, Password, Rol, Email, Bloqueo, Activo)
                               VALUES
                               (@DNI, @Apellidos, @Nombre, @Login, @Password, @Rol, @Email, @Bloqueo, @Activo)";

            SqlParameter[] parametros =
            {
                new SqlParameter("@DNI", nuevoUsuario.DNI_33ZS),
                new SqlParameter("@Apellidos", nuevoUsuario.Apellidos_33ZS),
                new SqlParameter("@Nombre", nuevoUsuario.Nombre_33ZS),
                new SqlParameter("@Login", nuevoUsuario.Login_33ZS),
                new SqlParameter("@Password", nuevoUsuario.Password_33ZS),
                new SqlParameter("@Rol", nuevoUsuario.Rol_33ZS),
                new SqlParameter("@Email", nuevoUsuario.Email_33ZS),
                new SqlParameter("@Bloqueo", nuevoUsuario.Bloqueo_33ZS),
                new SqlParameter("@Activo", nuevoUsuario.Activo_33ZS)
            };

            acc.ExecuteNonQuery_33ZS(command, parametros);
        }

        public void ModificarUsuario_33ZS(Usuario_33ZS usuario)
        {
            Acceso_33ZS acc = new Acceso_33ZS();

            string command = @"UPDATE Usuario
                               SET Apellidos = @Apellidos,
                                   Nombre = @Nombre,
                                   Login = @Login,
                                   Rol = @Rol,
                                   Email = @Email,
                                   Bloqueo = @Bloqueo,
                                   Activo = @Activo
                               WHERE DNI = @DNI";

            SqlParameter[] parametros =
            {
                new SqlParameter("@DNI", usuario.DNI_33ZS),
                new SqlParameter("@Apellidos", usuario.Apellidos_33ZS),
                new SqlParameter("@Nombre", usuario.Nombre_33ZS),
                new SqlParameter("@Login", usuario.Login_33ZS),
                new SqlParameter("@Rol", usuario.Rol_33ZS),
                new SqlParameter("@Email", usuario.Email_33ZS),
                new SqlParameter("@Bloqueo", usuario.Bloqueo_33ZS),
                new SqlParameter("@Activo", usuario.Activo_33ZS)
            };

            acc.ExecuteNonQuery_33ZS(command, parametros);
        }

        public void DesbloquearUsuario_33ZS(string dni)
        {
            Acceso_33ZS acc = new Acceso_33ZS();

            string command = @"UPDATE Usuario
                               SET Bloqueo = 0
                               WHERE DNI = @DNI";

            SqlParameter[] parametros =
            {
                new SqlParameter("@DNI", dni)
            };

            acc.ExecuteNonQuery_33ZS(command, parametros);
        }

        public void CambiarEstadoUsuario_33ZS(string dni, bool activo)
        {
            Acceso_33ZS acc = new Acceso_33ZS();

            string command = @"UPDATE Usuario
                               SET Activo = @Activo
                               WHERE DNI = @DNI";

            SqlParameter[] parametros =
            {
                new SqlParameter("@DNI", dni),
                new SqlParameter("@Activo", activo)
            };

            acc.ExecuteNonQuery_33ZS(command, parametros);
        }

        public void ActualizarIdioma_33ZS(string dni, string idioma)
        {
            Acceso_33ZS acc = new Acceso_33ZS();

            string command = @"UPDATE Usuario
                       SET Idioma = @Idioma
                       WHERE DNI = @DNI";

            SqlParameter[] parametros =
            {
        new SqlParameter("@Idioma", idioma),
        new SqlParameter("@DNI", dni)
    };

            acc.ExecuteNonQuery_33ZS(command, parametros);
        }

        public void CambiarClave_33ZS(string dni, string passwordHash)
        {
            Acceso_33ZS acc = new Acceso_33ZS();

            string command = @"UPDATE Usuario
                       SET Password = @Password
                       WHERE DNI = @DNI";

            SqlParameter[] parametros =
            {
        new SqlParameter("@DNI", dni),
        new SqlParameter("@Password", passwordHash)
    };

            acc.ExecuteNonQuery_33ZS(command, parametros);
        }
    }
}