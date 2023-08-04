using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tio_Soft.Entity;

namespace Tio_Soft.BLL.Interfaces
{
    public interface IRolService
    {
        //metodo tipo tarea asyncrona 
        Task<List<Rol>> Lista();
    }
}
