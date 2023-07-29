using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Linq.Expressions;

namespace Tio_Soft.DAL.Interfaces
{
    internal interface IGenericRepository<TEntity> where TEntity:class
    {
        Task<TEntity> Obtener(Expression<Func<TEntity, bool>> filtro);
    }
}
