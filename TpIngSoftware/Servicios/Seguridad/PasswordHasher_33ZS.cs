using System;
using System.Configuration;
using System.Globalization;
using System.Security.Cryptography;

namespace Servicios
{
    public static class PasswordHasher_33ZS
    {
        private const string Version = "PBKDF2-SHA256";
        private const int SaltBytes = 16;
        private const int HashBytes = 32;

        private static int IteracionesConfiguradas_33ZS()
        {
            int iterations;
            if (!int.TryParse(ConfigurationManager.AppSettings["PasswordHashIterations"],
                    NumberStyles.None, CultureInfo.InvariantCulture, out iterations) ||
                iterations < 100000 || iterations > 2000000)
                throw new ConfigurationErrorsException(
                    "Configure PasswordHashIterations con un entero entre 100000 y 2000000.");
            return iterations;
        }

        public static string Crear_33ZS(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("La contraseña es obligatoria.", nameof(password));

            byte[] salt = new byte[SaltBytes];
            using (RandomNumberGenerator random = RandomNumberGenerator.Create())
                random.GetBytes(salt);

            int iterations = IteracionesConfiguradas_33ZS();
            byte[] hash = Derivar(password, salt, iterations, HashBytes);
            return Version + "$" + iterations.ToString(CultureInfo.InvariantCulture) + "$" +
                Convert.ToBase64String(salt) + "$" + Convert.ToBase64String(hash);
        }

        public static bool Verificar_33ZS(string password, string almacenado, out bool actualizar)
        {
            actualizar = false;
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(almacenado))
                return false;

            if (almacenado.StartsWith(Version + "$", StringComparison.Ordinal))
            {
                string[] parts = almacenado.Split('$');
                int iterations;
                if (parts.Length != 4 ||
                    !int.TryParse(parts[1], NumberStyles.None, CultureInfo.InvariantCulture, out iterations) ||
                    iterations < 100000 || iterations > 2000000)
                    return false;

                try
                {
                    byte[] salt = Convert.FromBase64String(parts[2]);
                    byte[] expected = Convert.FromBase64String(parts[3]);
                    if (salt.Length != SaltBytes || expected.Length != HashBytes)
                        return false;
                    byte[] actual = Derivar(password, salt, iterations, expected.Length);
                    bool valid = IgualesEnTiempoConstante(actual, expected);
                    actualizar = valid && iterations < IteracionesConfiguradas_33ZS();
                    return valid;
                }
                catch (FormatException)
                {
                    return false;
                }
            }

            // Compatibilidad con los registros previos, que guardaban SHA-256 hexadecimal.
            if (almacenado.Length != 64)
                return false;
            for (int i = 0; i < almacenado.Length; i++)
                if (!Uri.IsHexDigit(almacenado[i])) return false;
            bool legacyValid = IgualesEnTiempoConstante(
                System.Text.Encoding.ASCII.GetBytes(Encriptador_33ZS.Hash(password)),
                System.Text.Encoding.ASCII.GetBytes(almacenado.ToLowerInvariant()));
            actualizar = legacyValid;
            return legacyValid;
        }

        private static byte[] Derivar(string password, byte[] salt, int iterations, int length)
        {
            using (var derive = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256))
                return derive.GetBytes(length);
        }

        private static bool IgualesEnTiempoConstante(byte[] left, byte[] right)
        {
            if (left.Length != right.Length) return false;
            int difference = 0;
            for (int i = 0; i < left.Length; i++)
                difference |= left[i] ^ right[i];
            return difference == 0;
        }
    }
}
