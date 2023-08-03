using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tio_Soft.DAL.DBContext;
using Tio_Soft.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Tio_Soft.DAL.Implementacion
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        //Constructor para resivir el contexto de la base de datos
        private readonly Tio_SoftContext _dbContext;
        public GenericRepository(Tio_SoftContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TEntity> Obtener(Expression<Func<TEntity, bool>> filtro)
        {
            try
            {
                TEntity entidad = await _dbContext.Set<TEntity>().FirstAsync(filtro);
                return entidad;
            }
            catch
            {
                throw;
            }
        }

        public async Task<TEntity> Crear(TEntity entidad)
        {
            try
            {
                _dbContext.Set<TEntity>().Add(entidad);
                await _dbContext.SaveChangesAsync();
                return entidad;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Editar(TEntity entidad)
        {
            try
            {
                _dbContext.Update(entidad);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Eliminar(TEntity entidad)
        {
            try
            {
                _dbContext.Remove(entidad);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<IQueryable<TEntity>> Cunsultar(Expression<Func<TEntity, bool>> filtro = null)
        {
            IQueryable <TEntity> queryEntidad = filtro == null ? _dbContext.Set<TEntity>(): _dbContext.Set<TEntity>().Where(filtro);
            return queryEntidad;
        }
    }
}
