using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace Servicios
{
    public class Encriptador_33ZS
    {

        public static string Hash(string value)
        {
            if (string.IsNullOrEmpty(value)) {
                return string.Empty;
            } 

            SHA256 sha256 = SHA256.Create(); 
            byte[] inputBytes = Encoding.UTF8.GetBytes(value);
            byte[] hashBytes = sha256.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("x2"));
            }

            return sb.ToString();
        }

           
    }
}
