using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tio_Soft.BLL.Interfaces;
using System.Security.Cryptography;

namespace Tio_Soft.BLL.Implementacion
{
    public class UtilidadesService : IUtilidadesService
    {
        public string GenerarClave()
        {
            //retorna una cadena de texto aleatoria 
            string clave = Guid.NewGuid().ToString("N").Substring(0,6);
            return clave;
        }

        public string ConvertirSha256(string texto)
        {
            StringBuilder sb = new StringBuilder();

            //objeto para poder encriptar el texto
            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;

                byte[] result = hash.ComputeHash(enc.GetBytes(texto));

                foreach (byte b in result)
                {
                    sb.Append(b.ToString("x2"));
                }
            }
            return sb.ToString();
        }
    }
}
