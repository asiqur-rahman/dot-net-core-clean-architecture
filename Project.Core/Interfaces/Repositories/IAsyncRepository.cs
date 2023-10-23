using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace Project.Core.Interfaces.Repositories
{

    public interface IAsyncRepository<T> where T : class
    {
        Task<bool> IsExistAsync(Expression<Func<T, bool>> predicate);
        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);
        Task<long> LongCountAsync();
        Task<long> LongCountAsync(Expression<Func<T, bool>> predicate);

        Task<TR> MaxAsync<TR>(Expression<Func<T, TR>> predicate);
        Task<TR> MaxAsync<TR>(Expression<Func<T, TR>> predicate, Expression<Func<T, bool>> where);
        Task<TR> MinAsync<TR>(Expression<Func<T, TR>> predicate);
        Task<TR> MinAsync<TR>(Expression<Func<T, TR>> predicate, Expression<Func<T, bool>> where);

        Task<T> FindAsync(Expression<Func<T, bool>> predicate);
        Task<T> GetAsync(Expression<Func<T, bool>> predicate);
        Task<T> GetByIdAsync(int id);
        Task<List<T>> GetListBySpecAsync(ISpecification<T> spec);
        Task<List<T>> ListAllAsync();
        Task<IPagedList<T>> PagedListAsync(int page, int size);

        /// <summary>
        /// CreateAsync wihtout savechanges method
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>T</returns>
        Task<T> CreateAsync(T entity);
        /// <summary>
        /// AddAsync new data with savechanges method
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<T> AddAsync(T entity);
        /// <summary>
        /// AddRangeAsync Entities without SaveChanges Methods
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);

        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);

        Task<int> ExecuteCommandAsync(string sqlCommand, params object[] parameters);
        Task<List<string>> ExecuteScalarAsync(string sqlCommand, params object[] parameters);
        Task<TEntity> ExecuteScalarAsync<TEntity>(string sqlCommand, params object[] parameters) where TEntity : class;

        Task<TEntity> ExecuteSingleQueryAsync<TEntity>(string sqlQuery, params object[] parameters) where TEntity : class;
        Task<List<TEntity>> ExecuteQueryAsync<TEntity>(string sqlQuery, params object[] parameters) where TEntity : class;

        Task<int> SaveChangesAsync();
    }
}
