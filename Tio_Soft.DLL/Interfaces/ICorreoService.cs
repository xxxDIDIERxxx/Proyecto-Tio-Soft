using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tio_Soft.BLL.Interfaces
{
    public interface ICorreoService
    {
        //metodo tipo booleano con parametros...
        Task<bool> EnviarCorreo(string CorreoDestino, string Asunto, string Mensaje);
    }
}
