using DAL.Security;
using Servicios;
using System.Collections.Generic;
using System.Data;

namespace Mappers.Security
{
    public class UsuarioMapper_33ZS
    {
        private readonly UsuarioDataAccess_33ZS data = new UsuarioDataAccess_33ZS();

        private static List<Usuario_33ZS> Mapear(DataTable table)
        {
            List<Usuario_33ZS> result = new List<Usuario_33ZS>();
            foreach (DataRow row in table.Rows)
                result.Add(new Usuario_33ZS(row));
            return result;
        }

        private static Usuario_33ZS Primero(DataTable table)
        {
            return table.Rows.Count == 0 ? null : new Usuario_33ZS(table.Rows[0]);
        }

        public List<Usuario_33ZS> ObtenerUsuarios_33ZS() => Mapear(data.Todos_33ZS());
        public Usuario_33ZS ObtenerUsuarioPorLogin_33ZS(string login) => Primero(data.PorLogin_33ZS(login));
        public List<Usuario_33ZS> ObtenerUsuariosPorEstado_33ZS(bool activo) => Mapear(data.PorEstado_33ZS(activo));
        public Usuario_33ZS ObtenerUsuario_33ZS(string login) => Primero(data.PorNombre_33ZS(login));
        public Usuario_33ZS ObtenerUsuarioPorDNI_33ZS(string dni) => Primero(data.PorDNI_33ZS(dni));

        public void AgregarUsuario_33ZS(Usuario_33ZS usuario) => data.Agregar_33ZS(
            usuario.DNI_33ZS, usuario.Apellidos_33ZS, usuario.Nombre_33ZS,
            usuario.Login_33ZS, usuario.Password_33ZS, usuario.Rol_33ZS,
            usuario.Email_33ZS, usuario.Bloqueo_33ZS, usuario.Activo_33ZS);

        public void ModificarUsuario_33ZS(Usuario_33ZS usuario) => data.Modificar_33ZS(
            usuario.DNI_33ZS, usuario.Apellidos_33ZS, usuario.Nombre_33ZS,
            usuario.Login_33ZS, usuario.Rol_33ZS, usuario.Email_33ZS,
            usuario.Bloqueo_33ZS, usuario.Activo_33ZS);

        public void BloquearUsuario_33ZS(string dni) => data.Bloquear_33ZS(dni);
        public void DesbloquearUsuario_33ZS(string dni) => data.Desbloquear_33ZS(dni);
        public void CambiarEstadoUsuario_33ZS(string dni, bool activo) => data.CambiarEstado_33ZS(dni, activo);
        public void ActualizarIdioma_33ZS(string dni, string idioma) => data.ActualizarIdioma_33ZS(dni, idioma);
        public void CambiarClave_33ZS(string dni, string passwordHash) => data.CambiarClave_33ZS(dni, passwordHash);
        public int RegistrarIntentoFallido_33ZS(string dni, int maxIntentos) =>
            data.RegistrarIntentoFallido_33ZS(dni, maxIntentos);
        public void ReiniciarIntentos_33ZS(string dni) => data.ReiniciarIntentos_33ZS(dni);
    }
}
