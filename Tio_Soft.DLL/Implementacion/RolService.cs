using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tio_Soft.BLL.Interfaces;
using Tio_Soft.DAL.Interfaces;
using Tio_Soft.Entity;

namespace Tio_Soft.BLL.Implementacion
{
    public class RolService : IRolService
    {
        private readonly IGenericRepository<Rol> _repositorio;

        public RolService(IGenericRepository<Rol> repositorio)
        {
            _repositorio = repositorio;
        }

        //metodo solo devuelve la lista de roles
        public async Task<List<Rol>> Lista()
        {
            IQueryable<Rol> query = await _repositorio.Cunsultar();

            return query.ToList();
        }
    }
}
