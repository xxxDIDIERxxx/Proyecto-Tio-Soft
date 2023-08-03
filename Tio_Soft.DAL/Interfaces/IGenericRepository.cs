using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Linq.Expressions;

namespace Tio_Soft.DAL.Interfaces
{
    //Interfas para todas las entidades de crud
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        //metodo para obtener
        Task<TEntity> Obtener(Expression<Func<TEntity, bool>> filtro);
        //metodo para crear
        Task<TEntity> Crear(TEntity entidad);
        //metodo para editar
        Task<bool> Editar(TEntity entidad);
        //metodo para eliminar
        Task<bool> Eliminar(TEntity entidad);

        Task<IQueryable<TEntity>> Cunsultar(Expression<Func<TEntity, bool>> filtro = null);
    }
}
