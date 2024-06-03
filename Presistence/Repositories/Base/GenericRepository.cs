using Core.Entities.BaseEntities;
using Core.Interfaces.Base;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Presistence.Contexts;
using System.Data;
using System.Linq.Expressions;
using static Dapper.SqlMapper;

namespace Presistence.Repositories.Base
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity, new()
    {
        private readonly ApplicationDbContext _context;
        public DbSet<T> dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            dbSet = context.Set<T>();
        }

        #region StoreProcedure
        public async Task<int> ExecuteAsync(string sql, object param = null, CommandType commandType = CommandType.StoredProcedure, IDbTransaction transaction = null, CancellationToken cancellationToken = default)
        {

            return await _context.Connection.ExecuteAsync(sql, param, transaction, commandType: commandType);
        }

        public async Task<T> ExecuteScalarAsync<T>(string sql, object param = null, CommandType commandType = CommandType.StoredProcedure, IDbTransaction transaction = null, CancellationToken cancellationToken = default)
        {
            return await _context.Connection.ExecuteScalarAsync<T>(sql, param, transaction, commandType: commandType);
        }

        public async Task<IReadOnlyList<T>> QueryAsync<T>(string sql, object param = null, CommandType commandType = CommandType.StoredProcedure, IDbTransaction transaction = null, CancellationToken cancellationToken = default)
        {
            return (await _context.Connection.QueryAsync<T>(sql, param, transaction, commandType: commandType)).AsList();
        }
        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null, CommandType commandType = CommandType.StoredProcedure, IDbTransaction transaction = null, CancellationToken cancellationToken = default)
        {
            return await _context.Connection.QueryFirstOrDefaultAsync<T>(sql, param, transaction, commandType: commandType);
        }
        public async Task<T> QuerySingleAsync<T>(string sql, object param = null, CommandType commandType = CommandType.StoredProcedure, IDbTransaction transaction = null, CancellationToken cancellationToken = default)
        {
            return await _context.Connection.QuerySingleOrDefaultAsync<T>(sql, param, transaction, commandType: commandType);
        }
        public async Task<Tuple<IList<T1>, IList<T2>>> QueryMultipleAsync<T1, T2>(string sql, object param = null, CommandType commandType = CommandType.StoredProcedure, IDbTransaction transaction = null, CancellationToken cancellationToken = default)
        {
            var data = await _context.Connection.QueryMultipleAsync(sql, param, transaction, commandType: commandType);
            return new Tuple<IList<T1>, IList<T2>>(data.Read<T1>().AsList(), data.Read<T2>().AsList());
        }

        #endregion

        #region Get
        public async Task<T?> GetByIdAsync(long Id,
                                           Expression<Func<T, bool>> filter = null,
                                           params Expression<Func<T, object>>[] includes)
        {
            var entity = dbSet.Where(e => !e.IsDeleted &&
                                           e.Id == Id);

            if (filter is not null)
            {
                entity = entity.Where(filter);
            }

            if (includes?.Length > 0)
            {
                foreach (var include in includes)
                {
                    entity = entity.Include(include);
                }
            }

            return await entity.FirstOrDefaultAsync();
        }

        public async Task<bool> Exists(Expression<Func<T, bool>> filter = null)
        {
            var entity = dbSet.Where(e => !e.IsDeleted);

            if (filter is not null)
            {
                return await entity.AnyAsync(filter);
            }

            return await entity.AnyAsync();
        }

        public async Task<TResult?> GetPropertyWithSelectorAsync<TResult>(Expression<Func<T, TResult>> selector,
                                                                          bool disableTracking = true,
                                                                          Expression<Func<T, bool>> filter = null,
                                                                          Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                                                          params Expression<Func<T, object>>[] includes)
        {
            var entity = dbSet.Where(r => !r.IsDeleted);

            if (disableTracking)
            {
                entity = entity.AsNoTracking();
            }

            if (filter is not null)
            {
                entity = entity.Where(filter);
            }

            if (includes?.Length > 0)
            {
                foreach (var include in includes)
                {
                    entity = entity.Include(include);
                }
            }

            if (orderBy is not null)
            {
                entity = orderBy(entity);
            }

            return await entity.Select(selector)
                               .FirstOrDefaultAsync();
        }

        public async Task<IList<TResult>> GetPagedWithSelectorAsync<TResult>(Expression<Func<T, TResult>> selector,
                                                                             int pageIndex,
                                                                             int pageCount,
                                                                             bool disableTracking = true,
                                                                             Expression<Func<T, bool>> filter = null,
                                                                             Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                                                             params Expression<Func<T, object>>[] includes)
        {
            var entity = dbSet.Where(e => !e.IsDeleted);

            if (disableTracking)
            {
                entity = entity.AsNoTracking();
            }

            if (filter is not null)
            {
                entity = entity.Where(filter);
            }

            if (includes?.Length > 0)
            {
                foreach (var include in includes)
                {
                    entity = entity.Include(include);
                }
            }

            if (orderBy is not null)
            {
                return await orderBy(entity).Skip((pageIndex - 1) * pageCount)
                                            .Take(pageCount)
                                            .Select(selector)
                                            .ToListAsync();
            }
            return await entity.Skip((pageIndex - 1) * pageCount)
                               .Take(pageCount)
                               .Select(selector)
                               .ToListAsync();
        }

        public async Task<IList<TResult>> GetAllWithSelectorAsync<TResult>(Expression<Func<T, TResult>> selector,
                                                                           bool disableTracking = true,
                                                                           Expression<Func<T, bool>> filter = null,
                                                                           Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                                                           params Expression<Func<T, object>>[] includes)
        {
            var entity = dbSet.Where(r => !r.IsDeleted);

            if (disableTracking)
            {
                entity = entity.AsNoTracking();
            }

            if (filter is not null)
            {
                entity = entity.Where(filter);
            }

            if (includes?.Length > 0)
            {
                foreach (var include in includes)
                {
                    entity = entity.Include(include);
                }
            }

            if (orderBy is not null)
            {
                entity = orderBy(entity);
            }

            return await entity.Select(selector)
                               .ToListAsync();
        }

        public async Task<IQueryable<TResult>> GetAllAsync<TResult>(Expression<Func<T, TResult>> selector,
                                                                    bool disableTracking = true,
                                                                    Expression<Func<T, bool>> filter = null,
                                                                    Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                                                    params Expression<Func<T, object>>[] includes)
        {
            var entity = dbSet.Where(r => !r.IsDeleted);

            if (disableTracking)
            {
                entity = entity.AsNoTracking();
            }

            if (filter is not null)
            {
                entity = entity.Where(filter);
            }

            if (includes?.Length > 0)
            {
                foreach (var include in includes)
                {
                    entity = entity.Include(include);
                }
            }

            if (orderBy is not null)
            {
                return orderBy(entity).Select(selector);
            }

            return entity.Select(selector);
        }

        public Task<int> GetCountAsync(Expression<Func<T, bool>> filter = null)
        {
            if (filter is not null)
            {
                return dbSet.CountAsync(filter);
            }
            return dbSet.CountAsync(f => !f.IsDeleted);
        }
        #endregion

        #region Insert
        public T Add(T entity)
        {
            try
            {
                dbSet.Add(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return entity;
        }

        public virtual void AddRang(List<T> entity)
        {
            dbSet.AddRange(entity);
        }

        #endregion

        #region Update
        public virtual void Update(T entity)
        {
            dbSet.Update(entity);
        }

        public virtual void UpdateRange(List<T> entity)
        {
            dbSet.UpdateRange(entity);
        }
        #endregion

        #region Delete
        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IList<T> entities)
        {
            dbSet.RemoveRange(entities);
        }
        #endregion
    }
}

