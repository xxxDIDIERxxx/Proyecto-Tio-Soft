using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Tio_Soft.BLL.Interfaces
{
    public interface IUtilidadesService
    {
         //metodo queretorna un codigo para que le usuario se legoe
        string GenerarClave();

        //resive un texto y lo devuelve encriptado Sha256
        string ConvertirSha256(string texto);
    }
}
